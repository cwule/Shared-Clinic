using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeBruceLung : MonoBehaviour
{
    void Start()
    {
        Transform playSpace = GameObject.Find("SharedSpace").transform;
        this.transform.SetParent(playSpace);
       
            this.transform.localPosition = new Vector3(0f, 0.4f, 0f);
            this.transform.localRotation = Quaternion.Euler(0, 0, 0);
            this.transform.localScale = new Vector3(0.002f, 0.002f, 0.002f);

        int qualLvls = QualitySettings.names.Length;
        Debug.Log("Qual levels " + qualLvls.ToString());
        QualitySettings.SetQualityLevel(qualLvls - 1);
    }
}
