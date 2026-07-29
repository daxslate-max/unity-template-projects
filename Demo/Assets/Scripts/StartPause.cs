using UnityEngine;

public class StartPause : MonoBehaviour
{
    public static bool IsGameStarted { get; private set; }

    private void Awake()
    {
        IsGameStarted = false;
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (!IsGameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            IsGameStarted = true;
            Time.timeScale = 1f;
        }
    }
}
