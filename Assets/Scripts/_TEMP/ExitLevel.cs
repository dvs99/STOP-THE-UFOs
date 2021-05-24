using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    public int FallbackSceneIndex;
    public void exit()
    {
        SceneManager.LoadScene(FallbackSceneIndex, LoadSceneMode.Single);
    }
}
