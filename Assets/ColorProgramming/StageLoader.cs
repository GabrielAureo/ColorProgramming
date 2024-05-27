using System.Collections;
using System.Text.RegularExpressions;
using ColorProgramming;
using ColorProgramming.Items;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoader : MonoBehaviour
{
    public static int stageIndex;
    public static bool hasPlacedArena;

    private static StageLoader instance;

    public static StageLoader Instance
    {
        get { return instance; }
    }

    private void OnEnable()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private static void CleanUp()
    {
        var agents = FindObjectsOfType<AgentController>();
        var targets = FindObjectsOfType<TargetNodeController>();

        foreach (var agent in agents)
        {
            Destroy(agent.gameObject);
        }

        foreach (var target in targets)
        {
            Destroy(target.gameObject);
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
        hasPlacedArena = false;

    }

    public static void ResetStage()
    {
        CleanUp();

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