using System.Linq;
using UnityEngine;

public class AscensionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject ascensionOptionPrefab;
    [SerializeField] private Transform optionsContainer;
    [SerializeField] private AscensionIconSet ascensionIconSet;
    [SerializeField] private SkillUIHandler skillHandler;

    private readonly int numberOfOptions = 2;

    public void GenerateAscensionOptions()
    {
        // Clear any existing options that might be present firstly
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }

        // Get list of all possible mods
        var allAscensions = System.Enum.GetValues(typeof(AscensionType)).Cast<AscensionType>().ToList();

        // Shuffle the list to randomize when we pick X amount from it later
        allAscensions = allAscensions.OrderBy(x => Random.value).ToList();

        // Generate new ones now
        for (int i = 0; i < numberOfOptions; i++)
        {
            AscensionData ascension = BuildAscensionData(allAscensions[i]);
            var option = Instantiate(ascensionOptionPrefab, optionsContainer);
            option.GetComponent<AscensionOptionUI>().Setup(ascension);
        }
    }

    private AscensionData BuildAscensionData(AscensionType type)
    {
        string name = "";
        string description = "";
        Sprite icon = null;

        switch (type)
        {
            case AscensionType.Magnet:
                name = "Celestial Magnet";
                description = "Tired of manually collecting all that bothersome experience? This gift from the stars does it for you! Effects may vary.";
                icon = ascensionIconSet.magnetIcon;
                return new AscensionData(name, description, icon,
                    () => skillHandler.SetSkill(name));

            case AscensionType.Shield:
                name = "Otherwordly Shield";
                description = "Unable to outrun the swarm of irregular shapes? This technology provides you with a little breathing room to escape.";
                icon = ascensionIconSet.shieldIcon;
                return new AscensionData(name, description, icon,
                    () => skillHandler.SetSkill(name));

            default:
                return null;
        }
    }
}
