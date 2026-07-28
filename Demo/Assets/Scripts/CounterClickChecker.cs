using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CounterClickChecker : MonoBehaviour
{
    [SerializeField] private CounterLoop counter;
    [SerializeField] private TMP_Text counterLabel;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioSource clickSource;

    private void Awake()
    {
        if (counter == null)
        {
            counter = FindFirstObjectByType<CounterLoop>();
        }

        if (counterLabel == null)
        {
            counterLabel = GetComponentInChildren<TMP_Text>();
        }

        if (clickSource == null)
        {
            clickSource = GetComponent<AudioSource>();
            if (clickSource == null && clickSound != null)
            {
                clickSource = gameObject.AddComponent<AudioSource>();
                clickSource.playOnAwake = false;
            }
        }
    }

    private void Update()
    {
        if (counter != null && counterLabel != null)
        {
            counterLabel.text = counter.CurrentValue.ToString();
        }

        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            CheckCounter();
        }
    }

    private void PlayClickSound()
    {
        if (clickSound == null)
            return;

        if (clickSource != null)
        {
            clickSource.PlayOneShot(clickSound);
            return;
        }

        AudioSource.PlayClipAtPoint(clickSound, Camera.main != null ? Camera.main.transform.position : transform.position);
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

        if (currentValue <= 10)
        {
            Debug.Log("Perfect!");
        }

        PlayClickSound();
    }
}
