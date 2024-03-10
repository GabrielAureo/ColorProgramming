using System.Collections;
using System.Text.RegularExpressions;
using ColorProgramming;
using ColorProgramming.Items;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{
    public static int stageIndex;

    private static StageLoader instance;

    public static StageLoader Instance
    {
        get { return instance; }
    }

    private void OnEnable()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public static void LoadStage(int stageIndex)
    {

        var loadedStage = FindLoadedStage();

        if (loadedStage != 0)
        {
            SceneManager.UnloadSceneAsync($"Stage {stageIndex}");
        }

        SceneManager.LoadScene("MainScene");
        SceneManager.LoadScene($"Stage {stageIndex}", LoadSceneMode.Additive);

        instance.StartCoroutine(WaitLoad());

    }

    public static void ResetStage()
    {
        var stageIndex = FindLoadedStage();
        LoadStage(stageIndex);
    }

    private static int FindLoadedStage()
    {
        string pattern = @"Stage (\d+)";

        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            var match = Regex.Match(scene.name, pattern);
            if (match.Success)
            {
                if (int.TryParse(match.Groups[1].Value, out int stageNumber))
                {
                    return stageNumber;
                }
            }

        }

        return 0;
    }


    private static IEnumerator WaitLoad()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        GameManager.Instance.StageController.LoadStage();
    }

}