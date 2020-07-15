using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CalibrationVuforia : MonoBehaviour
{
    // the space that is shared between devices and which is set by the imagetarget
    public Transform _sharedSpace;
    public Text errorText;

    // this is for vuforia calibration
    public void CalibrateSpace()
    {
        // make sure that avatar is not attached to calibration object
        if (_sharedSpace.Find("HumanAnatomyCompleteNetSkel(Clone)"))
            _sharedSpace.Find("HumanAnatomyCompleteNetSkel(Clone)").transform.parent = null;
        else if (_sharedSpace.Find("Lung(Clone)"))
            _sharedSpace.Find("Lung(Clone)").transform.parent = null;
        else if (_sharedSpace.Find("Heart(Clone)"))
            _sharedSpace.Find("Heart(Clone)").transform.parent = null;
        Vuforia.VuforiaBehaviour.Instance.enabled = true;
        GetComponent<CalibrateTrackableEventHandler>().enabled = true;
        //this.GetComponentInChildren<Renderer>().enabled = true;
    }
    
    // this is for vuforia calibration
    public void CalibrationDone()
    {
        if (!NetworkClient.active)
        {
            errorText.text = "Host Session first";
            return;
        }
        else if (transform.Find("CalibCube").gameObject.GetComponent<Renderer>().enabled == false)
        {
            errorText.text = "Calibration marker not visible";
            return;
        }
        else if (Vuforia.VuforiaBehaviour.Instance.enabled == false)
        {
            errorText.text = "Calibrate first";
            return;
        }
        errorText.text = "";
        _sharedSpace.SetPositionAndRotation(transform.position, transform.rotation);
        this.transform.Find("CalibCube").GetComponent<Renderer>().enabled = false;
  
        if (GameObject.Find("HumanAnatomyCompleteNetSkel(Clone)/WholeBody"))
        {
            Transform _avatar = GameObject.Find("HumanAnatomyCompleteNetSkel(Clone)/WholeBody").transform.parent;
            _avatar.SetParent(_sharedSpace);
            _avatar.localPosition = new Vector3(0.0f, 0.127f, 0.0f);
            _avatar.localRotation = Quaternion.Euler(-90, 0, 0);
            _avatar.localScale = new Vector3(0.01f, 0.011f, 0.01f);
        }
        else if(GameObject.Find("Lung(Clone)"))
        {
            Transform _avatar = GameObject.Find("Lung(Clone)").transform;
            _avatar.SetParent(_sharedSpace);
            _avatar.transform.localPosition = new Vector3(0.0f, 0.4f, 0.0f);
            _avatar.transform.localRotation = Quaternion.Euler(0, 0, 0);
            _avatar.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (GameObject.Find("Heart(Clone)"))
        {
            Transform _avatar = GameObject.Find("Heart(Clone)").transform;
            _avatar.SetParent(_sharedSpace);
            _avatar.transform.localPosition = new Vector3(-0.173f, 0.981f, 0.201f);
            _avatar.transform.localRotation = Quaternion.Euler(0, 0, 0);
            _avatar.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        Vuforia.VuforiaBehaviour.Instance.enabled = false;
        GetComponent<CalibrateTrackableEventHandler>().enabled = false;
    }

}
