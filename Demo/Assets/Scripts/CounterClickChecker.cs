using UnityEngine;
using UnityEngine.InputSystem;

public class CounterClickChecker : MonoBehaviour
{
    [SerializeField] private CounterLoop counter;

    private void Awake()
    {
        if (counter == null)
        {
            counter = FindFirstObjectByType<CounterLoop>();
        }
    }

    private void Update()
    {
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckCounter();
        }
    }

    private void CheckCounter()
    {
        if (counter == null)
        {
            Debug.LogError("CounterLoop could not be found.");
            return;
        }

        int currentValue = counter.CurrentValue;
        int distanceFrom100 = Mathf.Abs(100 - currentValue);

        Debug.Log(
            $"Clicked at {currentValue}. Distance from 100: {distanceFrom100}"
        );

        if (currentValue == 100)
        {
            Debug.Log("Perfect!");
        }
    }
}
