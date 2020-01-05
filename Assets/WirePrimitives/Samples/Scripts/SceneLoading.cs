using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour {

    [SerializeField]
    private Toggle sceneSelectionToggle;
    [SerializeField]
    private Toggle sceneGimbalToggle;

    void Start ()
    {
        if (SceneManager.GetActiveScene().name == "Selection")
        {
            sceneSelectionToggle.isOn = true;
        }
        else
        {
            sceneGimbalToggle.isOn = true;
        }
        sceneSelectionToggle.onValueChanged.AddListener(ToggleSceneSelection);
        sceneGimbalToggle.onValueChanged.AddListener(ToggleSceneGimbal);
    }
	
	void ToggleSceneSelection(bool isOn)
    {
        if (isOn)
        {
            SceneManager.LoadScene("Selection");
        }
	
	}

    void ToggleSceneGimbal(bool isOn)
    {
        if (isOn)
        {
            SceneManager.LoadScene("Gimbal");
        }

    }
}
