using UnityEngine;
using UnityEngine.Networking;

public class PrefabSpawner : NetworkBehaviour
{
    public void SpawnNewPrefab(GameObject prefabType)
    {
        NetworkServer.Spawn(prefabType);
    }

    public void DestroyOldPrefab(GameObject prefabType)
    {
        NetworkServer.Destroy(prefabType);
    }
}
