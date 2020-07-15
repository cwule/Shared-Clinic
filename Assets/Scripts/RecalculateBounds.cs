using System.Collections.Generic;
using UnityEngine;

public class RecalculateBounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SkinnedMeshRenderer[] allMeshRends= gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer meshRend in allMeshRends)
        {
            Debug.Log("Blip");
                Mesh mesh = meshRend.sharedMesh;
                mesh.RecalculateBounds();
        }
       
    }

}
