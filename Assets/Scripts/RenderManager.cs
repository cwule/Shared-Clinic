using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RenderManager : NetworkBehaviour
{
    // sets visibility of Skin, Skeleton, etc. modelName can have hierarchy with / as separator 
    [ClientRpc]
    public void RpcsetModelActive(GameObject avatar, string[] modelNames, bool status)
    {
        Debug.Log("Run on client");
        //Transform wholeBody = getWholeBody();
        Transform wholeBody = avatar.transform.Find("WholeBody").transform; ;

        foreach (string modelName in modelNames)
        {
            Transform mainTransform = wholeBody.Find(modelName).transform;

            if (status)
            {
                // when activating check that parent is active.
                activateParent(mainTransform.parent);
            }

            // finally activate main object and children or deactivate main object.
            mainTransform.gameObject.SetActive(status);
            if (status)
            {
                setActiveAllChildren(mainTransform, status);
            }
        }

    }

    private void activateParent(Transform parent)
    {
        // recursively activate parents
        if (parent.name == "WholeBody")
        {
            return; // break recusrion when reaching top level
        }
        activateParent(parent.parent);
        // if not active, activate parent but deactivate all children
        if (!parent.gameObject.activeSelf)
        {
            parent.gameObject.SetActive(true);
            setActiveAllChildren(parent, false);
        }
    }

    private void setActiveAllChildren(Transform parent, bool status)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(status);
        }
    }

    private Transform getWholeBody()
    {
        return GameObject.Find("HumanAnatomyCompleteNetSkel(Clone)/WholeBody").transform;
    }
}
