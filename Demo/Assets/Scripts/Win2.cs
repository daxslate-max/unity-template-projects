using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win2 : MonoBehaviour
{
    private const float DelayInSeconds = 2f;

    void Start()
    {
        StartCoroutine(LoadLevelAfterDelay());
    }

    private IEnumerator LoadLevelAfterDelay()
    {
        yield return new WaitForSeconds(DelayInSeconds);
        SceneManager.LoadScene("levelBear");
    }
}
