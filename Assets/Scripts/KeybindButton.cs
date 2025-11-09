using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class KeybindButton : MonoBehaviour
{
    [Header("Input Action")]
    [SerializeField] private InputActionReference actionReference;

    [Header("Composite Settings (leave empty for normal binding)")]
    [SerializeField] private string compositeName = "";   // e.g. "MOVE"
    [SerializeField] private string partName = "";        // e.g. "up", "down", "left", "right"

    [Header("UI")]
    [SerializeField] private TMP_Text bindingNameText;
    [SerializeField] private TMP_Text keyText;
    [SerializeField] private Button button;

    private int bindingIndex = -1;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    // Track all KeybindButton instances
    private static readonly List<KeybindButton> allButtons = new List<KeybindButton>();

    private void Awake()
    {
        allButtons.Add(this);
        button.onClick.AddListener(StartRebind);
    }

    private void OnDestroy()
    {
        allButtons.Remove(this);
    }

    private void Start()
    {
        bindingNameText.text = BuildDisplayName();
        FindBindingIndex();
        UpdateKeyDisplay();
    }

    private string BuildDisplayName()
    {
        if (!string.IsNullOrEmpty(partName))
            return char.ToUpper(partName[0]) + partName.Substring(1);
        return actionReference.action.name;
    }

    private void FindBindingIndex()
    {
        var action = actionReference.action;

        if (string.IsNullOrEmpty(compositeName))
        {
            // Normal binding
            for (int i = 0; i < action.bindings.Count; i++)
            {
                if (!action.bindings[i].isComposite && !action.bindings[i].isPartOfComposite)
                {
                    bindingIndex = i;
                    return;
                }
            }

            Debug.LogError($"No regular binding found on action '{action.name}'.");
            bindingIndex = -1;
            return;
        }

        // Composite binding
        int compositeIndex = -1;
        for (int i = 0; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            if (binding.isComposite && string.Equals(binding.name, compositeName, System.StringComparison.OrdinalIgnoreCase))
            {
                compositeIndex = i;
                break;
            }
        }

        if (compositeIndex == -1)
        {
            Debug.LogError($"Composite '{compositeName}' not found on '{action.name}'.");
            bindingIndex = -1;
            return;
        }

        for (int i = compositeIndex + 1; i < action.bindings.Count; i++)
        {
            var binding = action.bindings[i];
            if (!binding.isPartOfComposite) break;

            if (string.Equals(binding.name, partName, System.StringComparison.OrdinalIgnoreCase))
            {
                bindingIndex = i;
                return;
            }
        }

        Debug.LogError($"Part '{partName}' not found inside composite '{compositeName}' on '{action.name}'.");
        bindingIndex = -1;
    }

    private void UpdateKeyDisplay()
    {
        if (bindingIndex < 0)
        {
            keyText.text = "";
            return;
        }

        var binding = actionReference.action.bindings[bindingIndex];

        keyText.text = string.IsNullOrEmpty(binding.effectivePath)
            ? ""
            : InputControlPath.ToHumanReadableString(binding.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void RefreshBindingUI()
    {
        FindBindingIndex();
        UpdateKeyDisplay();
    }

    private void StartRebind()
    {
        if (bindingIndex < 0)
        {
            Debug.LogError("Invalid binding index.");
            return;
        }

        keyText.text = "...";
        actionReference.action.Disable();

        rebindingOperation = actionReference.action
            .PerformInteractiveRebinding(bindingIndex)
            .WithCancelingThrough("<Mouse>/leftButton")
            .OnMatchWaitForAnother(0.1f)
            .OnApplyBinding((op, newPath) =>
            {
                // Remove duplicate key from other actions (including composites)
                RemoveDuplicateBindings(newPath);

                // Apply the new binding
                actionReference.action.ApplyBindingOverride(bindingIndex, newPath);
                UpdateKeyDisplay();
            })
            .OnComplete(op => FinishRebind())
            .OnCancel(op => FinishRebind());

        rebindingOperation.Start();
    }

    private void FinishRebind()
    {
        rebindingOperation.Dispose();
        actionReference.action.Enable();
        UpdateKeyDisplay();
    }

    /// <summary>
    /// Removes the key from any other action or composite part, and updates their UI.
    /// </summary>
    private void RemoveDuplicateBindings(string newPath)
    {
        foreach (var button in allButtons)
        {
            if (button == this) continue;

            var action = button.actionReference.action;
            bool changed = false;

            for (int i = 0; i < action.bindings.Count; i++)
            {
                var binding = action.bindings[i];

                // Use overridePath if set, otherwise fallback to default path
                string currentPath = string.IsNullOrEmpty(binding.overridePath) ? binding.path : binding.overridePath;

                if (!string.IsNullOrEmpty(currentPath) && currentPath == newPath)
                {
                    action.ApplyBindingOverride(i, ""); // Clear the binding
                    changed = true;
                }
            }

            if (changed)
            {
                button.RefreshBindingUI();
            }
        }
    }
}
