using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillUIHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AscensionIconSet ascensionIconSet;
    [SerializeField] private GameObject skillContainer;
    [SerializeField] private Image skillIcon;
    [SerializeField] private TextMeshProUGUI cooldown;
    [SerializeField] private InputActionReference skillActionReference;
    [SerializeField] private PlayerSkillsHandler playerSkillsHandler;

    [Header("Skill Settings")]
    [SerializeField] private float skillCooldown;

    private bool skillActive = false;
    private bool skillReady = false;
    private string skillName = "";

    private void Start()
    {
        skillContainer.SetActive(false);
        cooldown.text = string.Empty;
    }
    private void OnEnable()
    {
        skillActionReference.action.performed += OnSkillPressed;
    }

    private void OnDisable()
    {
        skillActionReference.action.performed -= OnSkillPressed;
    }

    private void ShowSkill()
    {
        skillContainer.SetActive(true);
    }

    public void SetSkill(string name)
    {
        skillName = name;
        skillActive = true;
        skillReady = true;

        switch (name)
        {
            case "Celestial Magnet":
                skillIcon.sprite = ascensionIconSet.magnetIcon;
                skillCooldown = 30f;
                break;

            case "Otherwordly Shield":
                skillIcon.sprite = ascensionIconSet.shieldIcon;
                skillCooldown = 15f;
                break;
        }

        ShowSkill();
    }

    private void OnSkillPressed(InputAction.CallbackContext ctx)
    {
        if (skillActive && skillReady)
            StartCoroutine(UseSkill());
        else
            Debug.Log("Skill not active or not ready yet");
    }

    private IEnumerator UseSkill()
    {
        playerSkillsHandler.CastSkill(skillName);

        Color c = skillIcon.color;

        c.a = 0.2f;
        skillIcon.color = c;

        skillReady = false;

        float timer = skillCooldown;
        while (timer > 0f)
        {
            cooldown.text = Mathf.Ceil(timer).ToString();
            timer -= Time.deltaTime;
            yield return null;
        }

        c.a = 1f;
        skillIcon.color = c;
        cooldown.text = string.Empty;

        skillReady = true;
    }
}
