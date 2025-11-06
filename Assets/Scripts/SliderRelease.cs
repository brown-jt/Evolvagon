using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderRelease : MonoBehaviour, IPointerUpHandler
{
    [Header("Slider Reference")]
    [SerializeField] private Slider slider;

    // Action to call when releasing
    public System.Action<float> OnRelease; 

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("POINTER UP");
        OnRelease?.Invoke(slider.value);
    }
}
