using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class CounterClickChecker : MonoBehaviour
{
    [SerializeField] private CounterLoop counter;
    [SerializeField] private TMP_Text counterLabel;
    [SerializeField] private TMP_Text resultLabel;
    [SerializeField] private float resultHoldDuration = 0.5f;
    [SerializeField] private float resultAnimDuration = 0.1f;
    [SerializeField] private float resultMinScale = 0.2f;
    [SerializeField] private float resultMaxScale = 1f;
    [SerializeField] private Color perfectColor = Color.yellow;
    [SerializeField] private Color goodColor = Color.green;
    [SerializeField] private Color badColor = new Color(0.5f, 0f, 0.5f);
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioSource clickSource;

    private float resultElapsed;
    private float resultTotalTime;
    private bool resultVisible;

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

        if (resultLabel == null)
        {
            TMP_Text[] textComponents = GetComponentsInChildren<TMP_Text>();
            foreach (TMP_Text text in textComponents)
            {
                if (text != counterLabel)
                {
                    resultLabel = text;
                    break;
                }
            }
        }

        if (resultLabel != null)
        {
            resultLabel.text = string.Empty;
            resultLabel.rectTransform.pivot = new Vector2(0.5f, 0.5f);
            resultLabel.transform.localScale = Vector3.one * resultMinScale;
        }
    }

    private void Update()
    {
        if (counter != null && counterLabel != null)
        {
            counterLabel.text = counter.CurrentValue.ToString();
        }

        if (resultVisible && resultLabel != null)
        {
            resultElapsed += Time.deltaTime;
            if (resultElapsed >= resultTotalTime)
            {
                resultVisible = false;
                resultLabel.text = string.Empty;
                resultLabel.transform.localScale = Vector3.one * resultMinScale;
            }
            else
            {
                float scale = resultMinScale;
                if (resultElapsed < resultAnimDuration)
                {
                    float t = resultElapsed / resultAnimDuration;
                    scale = Mathf.Lerp(resultMinScale, resultMaxScale, t);
                }
                else if (resultElapsed < resultAnimDuration + resultHoldDuration)
                {
                    scale = resultMaxScale;
                }
                else
                {
                    float t = (resultElapsed - resultAnimDuration - resultHoldDuration) / resultAnimDuration;
                    scale = Mathf.Lerp(resultMaxScale, resultMinScale, t);
                }

                resultLabel.transform.localScale = Vector3.one * scale;
            }
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
            ShowResultText("Perfect!", perfectColor);
        }
        else if (currentValue >= 11 && currentValue <= 30)
        {
            Debug.Log("Good!");
            ShowResultText("Good!", goodColor);
        }
        else
        {
            Debug.Log("Bad...");
            ShowResultText("Bad...", badColor);
        }

        PlayClickSound();
    }

    private void ShowResultText(string result, Color color)
    {
        if (resultLabel == null)
            return;

        resultLabel.text = result;
        resultLabel.color = color;
        resultVisible = true;
        resultElapsed = 0f;
        resultTotalTime = resultAnimDuration * 2f + resultHoldDuration;
        resultLabel.transform.localScale = Vector3.one * resultMinScale;
    }
}
