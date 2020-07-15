using UnityEngine;
using UnityEngine.Networking;

public class SharkInstantiate : NetworkBehaviour
{
    [ClientRpc]
    public void RpcInstantiateShark(GameObject shark)
    {
        Instantiate(shark);
    }
}
