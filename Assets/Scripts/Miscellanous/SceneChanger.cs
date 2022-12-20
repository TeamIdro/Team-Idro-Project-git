using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeSceneViaEvent(SOScena scenaSO)
    {
        if (SceneManager.GetActiveScene().IsValid())
        {
            SceneManager.LoadScene(scenaSO.sceneReference);
        }
        GameManager.Instance.SetCurrentState(scenaSO.currentStateInScene);
    }
}
