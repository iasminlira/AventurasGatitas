using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    public GameObject[] buildingPrefabs;
    public Transform player;
    public float spawnDistance = 30f;
    public float buildingWidth = 10f;

    public int backgroundRows = 2;          // Quantas fileiras de fundo
    public float backgroundZOffset = -10f;  // Posição Z da primeira fileira de fundo
    public float backgroundYOffset = -1f;   // Diferença de altura entre as fileiras
    public float backgroundSpacingZ = -10f; // Distância entre fileiras de fundo

    private float lastSpawnX = 0f;

    void Start()
    {
        lastSpawnX = Mathf.Floor(player.position.x / buildingWidth) * buildingWidth;
        SpawnInitialBuildings();
    }

    void Update()
    {
        while (player.position.x + spawnDistance > lastSpawnX)
        {
            SpawnBuilding();
        }
    }

    void SpawnInitialBuildings()
{
    int buildingsBehind = 2;
    int buildingsAhead = 5;

    float playerX = Mathf.Floor(player.position.x / buildingWidth) * buildingWidth;
    float startX = playerX - buildingsBehind * buildingWidth;

    // Garante prédio embaixo do jogador
    SpawnSingleBuilding(playerX, 0f, 0f);
    for (int row = 1; row <= backgroundRows; row++)
    {
        float zOffset = backgroundZOffset + (row - 1) * backgroundSpacingZ;
        float yOffset = backgroundYOffset * row;
        SpawnSingleBuilding(playerX, yOffset, zOffset);
    }

    for (int i = 0; i < buildingsBehind + buildingsAhead + 1; i++)
    {
        float spawnX = startX + i * buildingWidth;

        // Evita duplicar prédio na mesma posição do jogador
        if (Mathf.Approximately(spawnX, playerX)) continue;

        SpawnSingleBuilding(spawnX, 0f, 0f);

        for (int row = 1; row <= backgroundRows; row++)
        {
            float zOffset = backgroundZOffset + (row - 1) * backgroundSpacingZ;
            float yOffset = backgroundYOffset * row;
            SpawnSingleBuilding(spawnX, yOffset, zOffset);
        }
    }

    lastSpawnX = startX + (buildingsBehind + buildingsAhead + 1) * buildingWidth;
}

    void SpawnBuilding()
    {
        // Jogável
        SpawnSingleBuilding(lastSpawnX, 0f, 0f);

        // Fundo
        for (int row = 1; row <= backgroundRows; row++)
        {
            float zOffset = backgroundZOffset + (row - 1) * backgroundSpacingZ;
            float yOffset = backgroundYOffset * row;
            SpawnSingleBuilding(lastSpawnX, yOffset, zOffset);
        }

        lastSpawnX += buildingWidth;
    }

    void SpawnSingleBuilding(float x, float yOffset, float zOffset)
    {
        int index = Random.Range(0, buildingPrefabs.Length);
        Vector3 position = new Vector3(x, yOffset, zOffset);
        Quaternion rotation = Quaternion.Euler(0, 180f, 0);
        Instantiate(buildingPrefabs[index], position, rotation);
    }
}
