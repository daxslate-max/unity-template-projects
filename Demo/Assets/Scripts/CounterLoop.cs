using UnityEngine;
using UnityEngine.InputSystem;

public class CounterLoop : MonoBehaviour
{
    [SerializeField] private int minValue = 0;
    [SerializeField] private int maxValue = 100;

    [Tooltip("Seconds to reach maxValue when counting up.")]
    [SerializeField] private float upDuration = 0.5f;

    [Tooltip("Seconds to reach minValue when counting down.")]
    [SerializeField] private float downDuration = 0.5f;

    public int CurrentValue { get; private set; }

    private bool countingUp = true;
    private float phaseTimer;

    private void Start()
    {
        CurrentValue = minValue;
        phaseTimer = 0f;
    }

    private void Update()
    {
        if (upDuration <= 0f || downDuration <= 0f)
            return;

        phaseTimer += Time.deltaTime;

        if (countingUp)
        {
            float progress = phaseTimer / upDuration;
            if (progress >= 1f)
            {
                CurrentValue = maxValue;
                countingUp = false;
                phaseTimer -= upDuration;
            }
            else
            {
                CurrentValue = Mathf.RoundToInt(Mathf.Lerp(minValue, maxValue, progress));
            }
        }
        else
        {
            float progress = phaseTimer / downDuration;
            if (progress >= 1f)
            {
                CurrentValue = minValue;
                countingUp = true;
                phaseTimer -= downDuration;
            }
            else
            {
                CurrentValue = Mathf.RoundToInt(Mathf.Lerp(maxValue, minValue, progress));
            }
        }
    }
}
