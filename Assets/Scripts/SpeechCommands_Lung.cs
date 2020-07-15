using UnityEngine;

public class SpeechCommands_Lung : MonoBehaviour
{
    public GameObject _lung, _heart;

    private ManageScenes _sceneManager;

    private void Start()
    {
        _sceneManager = GameObject.Find("SceneManager").transform.GetComponent<ManageScenes>();
    }


    public void ChangeScene()
    {
        _sceneManager.ChangeScene(_lung, _heart);
    }
}
