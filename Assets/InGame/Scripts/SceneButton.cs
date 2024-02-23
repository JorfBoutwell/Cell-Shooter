using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    [SerializeField] int sceneIndex;
    public void OnPress()
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}
