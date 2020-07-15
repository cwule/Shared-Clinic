using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkRenderManager : NetworkBehaviour
{
    public Transform wholeBody;

    // sets visibility of Skin, Skeleton, etc. modelName can have hierarchy with / as separator 
    public void setModelActive(List<string> modelNames, bool status)
    {
        foreach (string modelName in modelNames)
        {

            if (status)
            {
                RpcClientEnable(modelName);
            }
            else
            {
                RpcClientDisable(modelName);
            }
        }

    }

    // executes function on all Clients
    [ClientRpc]
    void RpcClientEnable(string modelName)
    {
        Transform mainTransform = wholeBody.Find(modelName).transform;
        // when activating check that parent is active.
        activateParent(mainTransform.parent);
        mainTransform.gameObject.SetActive(true);
        setActiveAllChildren(mainTransform, true);

    }

    [ClientRpc]
    void RpcClientDisable(string modelName)
    {
        Transform mainTransform = wholeBody.Find(modelName).transform;
        mainTransform.gameObject.SetActive(false);
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

    ///// <summary>gets the WholeBody Transform depending on networking/tracking status. Needs to be changed depending on Avatar used.</summary>
    //private Transform getWholeBody()
    //{
    //    return GameObject.Find("HumanAnatomyCompleteNetSkel(Clone)/WholeBody").transform;
    //}
}
