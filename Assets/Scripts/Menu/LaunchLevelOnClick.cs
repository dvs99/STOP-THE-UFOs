using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchLevelOnClick : MonoBehaviour
{
    public int LevelIndex;

    public void Launch()
    {
        SceneManager.LoadScene(LevelIndex, LoadSceneMode.Single);
    }
}
