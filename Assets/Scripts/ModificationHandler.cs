using UnityEngine;

public class ModificationHandler : MonoBehaviour
{
    [Header("Images")]
    [SerializeField] private Sprite squareImage;
    [SerializeField] private Sprite pentagonImage;
    [SerializeField] private Sprite hexagonImage;
    [SerializeField] private Sprite septagonImage;

    private void Start()
    {
        SpawnTwoStations();
    }

    private void SpawnTwoStations()
    {
        // 0 & 2
        // 0 & 3
        // 1 & 3

        // Spawn prefab with images
    }
}
