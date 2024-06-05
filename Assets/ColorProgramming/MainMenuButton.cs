using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public void LoadStage(int stageIndex)
    {
        StageLoader.LoadStage(stageIndex);
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

}
