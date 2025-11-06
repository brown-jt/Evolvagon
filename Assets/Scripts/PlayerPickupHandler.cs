using UnityEngine;

public class PlayerPickupHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Experience"))
        {
            ExperiencePickup exp = other.GetComponent<ExperiencePickup>();
            if (exp != null)
            {
                exp.Collect(transform);
            }
        }
    }
}
