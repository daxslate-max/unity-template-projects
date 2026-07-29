using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    [SerializeField] private float delayBeforeReturn = 2f;
    [SerializeField] private string nextSceneName = "levelWolf";

    private void Start()
    {
        Invoke(nameof(GoToWolfLevel), delayBeforeReturn);
    }

    private void GoToWolfLevel()
    {
        if (Application.CanStreamedLevelBeLoaded(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        int sceneIndex = SceneUtility.GetBuildIndexByScenePath($"Assets/Scenes/{nextSceneName}.unity");
        if (sceneIndex >= 0)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError($"Could not find scene '{nextSceneName}' in the build settings.");
        }
    }
}
