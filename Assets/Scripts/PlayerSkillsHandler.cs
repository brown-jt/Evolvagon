using System.Collections;
using UnityEngine;

public class PlayerSkillsHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStatsHandler playerStats;
    [SerializeField] private GameObject shieldEffect;

    [Header("SFX")]
    [SerializeField] private AudioClip magnetSfx;
    [SerializeField] private AudioClip shieldSfx;

    [Header("Skill Settings")]
    [SerializeField] private float shieldDuration = 3f;

    private void Start()
    {
        shieldEffect.SetActive(false);
    }

    public void CastSkill(string name)
    {
        switch (name)
        {
            case "Celestial Magnet":
                Magnet(); break;

            case "Otherwordly Shield":
                Shield(); break;
        }
    }

    private void Magnet()
    {
        AudioManager.Instance.PlaySFX(magnetSfx);

        // Find all objects with tag "Experience"
        GameObject[] experienceObjects = GameObject.FindGameObjectsWithTag("Experience");

        foreach (GameObject expObj in experienceObjects)
        {
            // Get the ExperiencePickup component
            ExperiencePickup pickup = expObj.GetComponent<ExperiencePickup>();
            if (pickup != null)
            {
                pickup.Collect(transform);
            }
        }
    }

    private void Shield()
    {
        AudioManager.Instance.PlaySFX(shieldSfx);
        StartCoroutine(ShieldCoroutine());
    }

    private IEnumerator ShieldCoroutine()
    {
        playerStats.SetImmune(true);
        shieldEffect.SetActive(true);

        yield return new WaitForSeconds(shieldDuration);

        playerStats.SetImmune(false);
        shieldEffect.SetActive(false);
    }
}
