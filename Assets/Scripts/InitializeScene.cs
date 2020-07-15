using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform playSpace = GameObject.Find("SharedSpace").transform;
        this.transform.SetParent(playSpace);
        if (this.name == "Lung(Clone)")
        {
            this.transform.localPosition = new Vector3(0.0f, 0.4f, 0.0f);
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (this.name == "Heart(Clone)")
        {
            this.transform.localPosition = new Vector3(-0.173f, 0.981f, 0.201f);
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (this.name == "Shark(Clone)")
        {
            this.transform.localPosition = new Vector3(0f, 0f, 0f);
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
            this.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        int qualLvls = QualitySettings.names.Length;
        Debug.Log("Qual levels " +  qualLvls.ToString());
        QualitySettings.SetQualityLevel(qualLvls-1);
    }

}
