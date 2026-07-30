using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    [SerializeField] private float delayBeforeReturn = 2f;
    [SerializeField] private string nextSceneName = "levelWolf";
    [SerializeField] private string alternateNextSceneName = "levelBear";
    [SerializeField] private string winSceneName = "Win";
    [SerializeField] private string win2SceneName = "Win2";

    private void Start()
    {
        Invoke(nameof(GoToWolfLevel), delayBeforeReturn);
    }

    private void GoToWolfLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        string targetSceneName = nextSceneName;

        if (currentSceneName == "levelWolf")
        {
            targetSceneName = "levelBear";
        }
        else if (currentSceneName == "levelBear")
        {
            targetSceneName = "levelWolf";
        }
        else if (currentSceneName == "Win" || currentSceneName == "Win2")
        {
            targetSceneName = "levelWolf";
        }
        else
        {
            targetSceneName = "levelWolf";
        }

        if (Application.CanStreamedLevelBeLoaded(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
            return;
        }

        int sceneIndex = SceneUtility.GetBuildIndexByScenePath($"Assets/Scenes/{targetSceneName}.unity");
        if (sceneIndex >= 0)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError($"Could not find scene '{targetSceneName}' in the build settings.");
        }
    }
}
