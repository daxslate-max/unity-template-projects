using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    [SerializeField] private float delayBeforeReturn = 2f;

    private void Start()
    {
        Invoke(nameof(ReturnToMainScene), delayBeforeReturn);
    }

    private void ReturnToMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
