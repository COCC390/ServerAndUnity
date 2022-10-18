using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scenes
{
    Login,
    GameHome
}

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance = new ScenesManager();
    public void ChangeScene()
    {
        SceneManager.LoadScene("GameHome");
    }
}
