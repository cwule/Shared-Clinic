using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockTargets : MonoBehaviour
{
    public List<GameObject>  mockTargets = new List<GameObject>(4);
    private int ind = 0;

    // chance mock pose of body
    public void ChangePose()
    {
        // index that cycles through all elbows, knees, targets
        int i = 0;

        // copy position and rotation for all elbows, knees, targets
        foreach (Transform mockPose in GetComponentInChildren<Transform>())
        {
            mockPose.SetPositionAndRotation(mockTargets[ind].transform.GetChild(i).position, mockTargets[ind].transform.GetChild(i).rotation);
            i++;
        }

        // increment pose
        ind++;
        if (ind == mockTargets.Count)
            ind = 0;
    }
}
