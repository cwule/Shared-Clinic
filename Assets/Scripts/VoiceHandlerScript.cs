using System;
using System.Text;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.Networking;

using System.Collections.Generic;
using UnityEngine.UI;

public class VoiceHandlerScript : MonoBehaviour
{
    // public fields
    public NetworkManager netManager;
    private const bool debug = true;
    public CalibrationVuforia _calibMark;
    public Text _speechText, _connectText, _bodyText, _lungText, _heartText, _errorText;
    public GameObject _displayPanel;
    public ManageScenes _sceneManager;
    public GameObject _lung, _heart, _shark;

    // pseudo public fields
    [HideInInspector]
    public GameObject _avatar;

    // private fields
    [SerializeField]
    private GrammarRecognizer grammarRecognizer;
    private GameObject _avatarNet;
    private bool _trackingOn = true;
    private bool isNetworked;
    

    // Start is called before the first frame update
    void Start()
    {
        if (PhraseRecognitionSystem.isSupported)
        {
            Debug.Log("phrase recognition supported"); 
   
            grammarRecognizer = new GrammarRecognizer(System.IO.Path.Combine(Application.streamingAssetsPath, "SRGS", "grammar.xml"), ConfidenceLevel.Low);
            grammarRecognizer.OnPhraseRecognized += GrammarRecognizer_OnPhraseRecognized;

            grammarRecognizer.Start();  
        }

        if (netManager)
        {
            isNetworked = true;
            Debug.Log("networking supported");
        } else
        {
            isNetworked = false;
            Debug.Log("networking not supported");
        }
        
    }

    private void GrammarRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    { 
        SemanticMeaning[] meanings = args.semanticMeanings;

        List<string> parameters = new List<string>();
        String action = "";

        foreach (SemanticMeaning meaning in meanings)
        { 
            // Debug.Log($"meaning {meaning.key}, {meaning.values[0]}");
            if (meaning.key == "Action")
            {
                action = meaning.values[0];
            } else if (meaning.key == "Parameters")
            {
                parameters.AddRange(meaning.values);
            }
        }

        if (debug)
        {
            String partsString = String.Join(", ", parameters.ToArray());

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0}{1}", args.text, Environment.NewLine);
            builder.AppendFormat("Confidence: {0}\tDuration: {1}{2}", args.confidence, args.phraseDuration.TotalSeconds, Environment.NewLine);
            builder.AppendFormat("Action: {0}\tParts: {1}{2}", action, partsString, Environment.NewLine);
            Debug.Log(builder.ToString());
        }

        if (action == "show")
        {
            //setModelActive(parameters, false);
            _speechText.text = "Show " + parameters[0]; 
            GameObject avatar = getWholeBody().parent.gameObject;
            avatar.GetComponent<NetworkRenderManager>().setModelActive(parameters, true);
            //Debug.Log("NI found " + getWholeBody().transform.parent.gameObject.GetComponent<NetworkIdentity>().ToString());

        }
        else if (action == "hide")
        {
            _speechText.text = "Hide " + parameters[0];
            GameObject avatar = getWholeBody().parent.gameObject;
            //avatar.GetComponent<NetworkRenderManager>().RpcDisableSkin();
            avatar.GetComponent<NetworkRenderManager>().setModelActive(parameters, false);
            //setModelActive(parameters, false);
            //Debug.Log("NI found " + getWholeBody().transform.parent.gameObject.GetComponent<NetworkIdentity>().ToString());
        }
        //else if (action == "connect")
        //{
        //    _speechText.text = "Connect";
        //    Connect2Server();
        //}
        //else if (action == "disconnect")
        //{
        //    _speechText.text = "Disconnect";
        //    DisconnectServer();
        //}
        else if (action == "host session")
        {
            _speechText.text = "Host session";
            HostSession();
        }
        else if (action == "change parent")
        {
            Transform avatar = getWholeBody().parent.transform;
            avatar.parent = avatar.parent == Camera.main.transform ? null : Camera.main.transform;
        }
        else if (action == "next pose")
        {
            Transform wholeBody = getWholeBody();
            wholeBody.parent.transform.Find("IKTargets").GetComponent<MockTargets>().ChangePose();
        }
        else if (action == "calibrate")
        {
            _speechText.text = "Calibrate";
            _calibMark.CalibrateSpace();
            
        }
        else if (action == "calibration done")
        {
                _speechText.text = "Calibration done";
                _calibMark.CalibrationDone();
        }
        else if (action == "toggle display")
        {
            _speechText.text = "Toggle Display";
            bool dispState = _displayPanel.activeSelf ? false : true;
            _displayPanel.SetActive(dispState);

        }
        else if (action == "study lung")
        {
            _errorText.text = "";
            _bodyText.gameObject.SetActive(false);
            _lungText.gameObject.SetActive(true);
            _speechText.text = "Study Lung";
            GameObject avatar = getWholeBody().parent.gameObject;
            _sceneManager.ChangeScene(avatar, _lung);
        }
        else if (action == "study heart")
        {
            if (!getLung())
            {
                _errorText.text = "Study lung first";
                return;
            }
            _errorText.text = "";
            _lungText.gameObject.SetActive(false);
            _heartText.gameObject.SetActive(true);
            _speechText.text = "Study Heart";
            _sceneManager.ChangeScene(getLung().gameObject, _heart);
        }
        else if (action == "almost done")
        {
            if (!getHeart())
            {
                _errorText.text = "Study heart first";
                return;
            }
            _errorText.text = "";
            _displayPanel.SetActive(false);
            _sceneManager.ChangeScene(getHeart().gameObject, _shark);
        }
    }

    private void OnApplicationQuit()
    {
        if (grammarRecognizer != null && grammarRecognizer.IsRunning)
        {
            grammarRecognizer.OnPhraseRecognized -= GrammarRecognizer_OnPhraseRecognized;
            grammarRecognizer.Stop();
        }
    }

    /// <summary>gets the WholeBody Transform depending on networking/tracking status. Needs to be changed depending on Avatar used.</summary>
    private Transform getWholeBody()
    {
        return GameObject.Find("HumanAnatomyCompleteNetSkel(Clone)/WholeBody").transform;
    }
    
    private Transform getLung()
    {
        if (GameObject.Find("Lung(Clone)"))
            return GameObject.Find("Lung(Clone)").transform;
        else
            return null;
    }
    
    private Transform getHeart()
    {
        if (GameObject.Find("Heart(Clone)"))
            return GameObject.Find("Heart(Clone)").transform;
        else
            return null;
    }

    private void Connect2Server()
    {
        if (isNetworked && !NetworkClient.active && !NetworkServer.active)
            netManager.StartClient();
    }

    private void DisconnectServer()
    {
        if (isNetworked && NetworkClient.active || NetworkServer.active)
        {
            netManager.StopServer();
        }
    }

    private void HostSession()
    {
        _errorText.text = "";
        if (isNetworked && !NetworkClient.active && !NetworkServer.active)
            netManager.StartHost();
            _calibMark.GetComponent<CalibrateTrackableEventHandler>().enabled = true;
    }

    private void Update()
    {
        if (NetworkClient.active)
        {
            _connectText.text = "Connected";
            _connectText.color = Color.green;
                }
        else
        {
            _connectText.text = "Disconnected";
            _connectText.color = Color.red;
        }
    }

}


