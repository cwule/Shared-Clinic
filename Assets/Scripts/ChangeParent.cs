using UnityEngine;

public class ChangeParent : MonoBehaviour
{
    //this is calibration done for manual calibration
    public void CalibrateSpace()
    {
        // make sure that avatar is not attached to calibration object
        if (GameObject.Find("HumanAnatomyCompleteNetSkel(Clone)"))
            GameObject.Find("HumanAnatomyCompleteNetSkel(Clone)").transform.parent = null;

        // make sure calibration object visible and active
        this.GetComponentInChildren<Renderer>().enabled = true;
        this.GetComponent<Microsoft.MixedReality.Toolkit.UI.BoundingBox>().enabled = true;

        if (this.transform.parent == null)
        {
            this.transform.parent = Camera.main.transform;
            this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            this.transform.parent = null;
            this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
     //this is calibration done for manual calibration
    public void CalibrationDone()
    {
        this.transform.parent = null;
        this.GetComponentInChildren<Renderer>().enabled = false;
        this.GetComponent<Microsoft.MixedReality.Toolkit.UI.BoundingBox>().enabled = false;
        Transform _avatar = GameObject.Find("HumanAnatomyCompleteNetSkel(Clone)/WholeBody").transform.parent;
        _avatar.SetParent(transform);
    }

}
