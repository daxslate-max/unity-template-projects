using UnityEngine;
using UnityEngine.InputSystem;

public class CounterLoop : MonoBehaviour
{
    [SerializeField] private int minValue = 0;
    [SerializeField] private int maxValue = 100;

    [Tooltip("Numbers counted per second.")]
    [SerializeField] private float countSpeed = 50f;

    public int CurrentValue { get; private set; }

    private bool countingUp = true;
    private float timer;

    private void Start()
    {
        CurrentValue = minValue;
    }

    private void Update()
    {
        if (countSpeed <= 0f)
            return;

        timer += Time.deltaTime;
        float interval = 1f / countSpeed;

        while (timer >= interval)
        {
            timer -= interval;

            if (countingUp)
            {
                CurrentValue++;

                if (CurrentValue >= maxValue)
                {
                    CurrentValue = maxValue;
                    countingUp = false;
                }
            }
            else
            {
                CurrentValue--;

                if (CurrentValue <= minValue)
                {
                    CurrentValue = minValue;
                    countingUp = true;
                }
            }
        }
    }
}
