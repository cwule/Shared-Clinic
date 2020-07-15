using UnityEngine;
using UnityEngine.Networking;

public class ManageScenes : NetworkBehaviour
{

    public void ChangeScene(GameObject netprefab, GameObject newSceneObj)
    {
        var newPrefab = Instantiate(newSceneObj, new Vector3(0, 0, 0), Quaternion.identity);
        DestroyOldPrefab(netprefab);
        SpawnNewPrefab(newPrefab);
    }

    public void SpawnNewPrefab(GameObject prefabType)
    {
        NetworkServer.Spawn(prefabType);
    }

    public void DestroyOldPrefab(GameObject prefabType)
    {
        NetworkServer.Destroy(prefabType);
    }
}
