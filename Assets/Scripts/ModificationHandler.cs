using UnityEngine;

public class ModificationHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject modificationOptionPrefab;
    [SerializeField] private Transform optionsContainer;
    [SerializeField] private ModificationIconSet modificationIconSet;
    [SerializeField] private ManualGun manualGun;

    [Header("Stations")]
    [SerializeField] private GameObject squareStation;
    [SerializeField] private GameObject pentagonStation;
    [SerializeField] private GameObject hexagonStation;
    [SerializeField] private GameObject septagonStation;

    [Header("Camera")]
    [SerializeField] private Camera mainCam;

    private readonly int numberOfOptions = 2;

    private void Start()
    {
        SpawnTwoStations();
    }

    public void GenerateModificationOptions()
    {
        // Clear any existing options that might be present firstly
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }

        // Generate new ones now
        for (int i = 0; i < numberOfOptions; i++)
        {
            ModificationData randomModification = GetRandomModification();
            var option = Instantiate(modificationOptionPrefab, optionsContainer);
            option.GetComponent<ModificationOptionUI>().Setup(randomModification);
        }
    }

    private ModificationData GetRandomModification()
    {
        // Secondly let's get a random upgrade type
        // TODO - Implement a feature where you cannot get the same option shown twice
        ModificationType type = (ModificationType)Random.Range(0, System.Enum.GetValues(typeof(ModificationType)).Length);

        // Build the complete UpgradeData structure
        return BuildModificationData(type);
    }

    private ModificationData BuildModificationData(ModificationType type)
    {
        string name = "";
        string description = "";
        Sprite icon = null;

        switch (type)
        {
            case ModificationType.TripleShot:
                name = "Triple Shot";
                description = "Why shoot one ball directly in front of you... when you can shoot that plus an extra two either side? How cool is that!";
                icon = modificationIconSet.tripleShotIcon;
                return new ModificationData(name, description, icon,
                    () => manualGun.EnableModification(name));

            case ModificationType.ExplosiveShot:
                name = "Explosive Shot";
                description = "The game isn't cool enough for you yet? This option causes you to gain a massive headache with screenshake and explosion!";
                icon = modificationIconSet.explosiveShotIcon;
                return new ModificationData(name, description, icon,
                    () => manualGun.EnableModification(name));

            case ModificationType.PiercingShot:
                name = "Piercing Shot";
                description = "Even wonder why the ball you shoot disappears after it hits an enemy? Look no further! Now it can pass through one and hit two!";
                icon = modificationIconSet.piercingShotIcon;
                return new ModificationData(name, description, icon,
                    () => manualGun.EnableModification(name));

            case ModificationType.BouncingShot:
                name = "Bouncing Shot";
                description = "Tired of the game bounds accidentally eating your ball? This modification coats your ball in rubber and allows it to bounce off walls!";
                icon = modificationIconSet.bouncingShotIcon;
                return new ModificationData(name, description, icon,
                    () => manualGun.EnableModification(name));

            case ModificationType.RecoilShot:
                name = "Recoil Shot";
                description = "Tired of getting snuck up on from behind? Well.. do I have the solution for you.. Shoot in front of you and behind you.. AT THE SAME TIME!";
                icon = modificationIconSet.recoilShotIcon;
                return new ModificationData(name, description, icon,
                    () => manualGun.EnableModification(name));

            case ModificationType.RicochetShot:
                name = "Ricochet Shot";
                description = "Those pesky irregular polygons gaining the upper hand? Teach your ball new tricks to automatically hop to a second one right after the first!";
                icon = modificationIconSet.ricochetShotIcon;
                return new ModificationData(name, description, icon,
                    () => manualGun.EnableModification(name));

            default:
                return null;
        }
    }

    private void SpawnTwoStations()
    {
        int stationOne = Random.Range(0, 2);
        int stationTwo;

        GameObject stationOnePrefab;
        GameObject stationTwoPrefab;

        if (stationOne == 0)
        {
            // Can either be square + hexagon OR square + septagon
            stationTwo = Random.Range(2, 4);
            stationOnePrefab = squareStation;
            
            if (stationTwo == 2)
                stationTwoPrefab = hexagonStation;
            else
                stationTwoPrefab = septagonStation;
        }
        else
        {
            // Has to be pentagon and septagon
            stationOnePrefab = pentagonStation;
            stationTwoPrefab = septagonStation;
        }

        // Spawn inside camera bounds ensuring not to overlap
        Vector3 pos1 = GetRandomPositionInCameraView();
        Instantiate(stationOnePrefab, pos1, Quaternion.identity);

        Vector3 pos2 = GetRandomPositionInCameraView();
        Instantiate(stationTwoPrefab, pos2, Quaternion.identity);
    }

    private Vector3 GetRandomPositionInCameraView()
    {
        Vector2 viewportPos = new Vector2
        (
            Random.Range(0.1f, 0.9f),
            Random.Range(0.1f, 0.9f)
        );

        Vector3 worldPos = mainCam.ViewportToWorldPoint
        (
            new Vector3
            (
                viewportPos.x,
                viewportPos.y,
                Mathf.Abs(mainCam.transform.position.z)
            )
        );

        worldPos.z = 0f;
        return worldPos;
    }
}
