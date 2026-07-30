using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CounterClickChecker : MonoBehaviour
{
    [SerializeField] private CounterLoop counter;
    [SerializeField] private TMP_Text counterLabel;
    [SerializeField] private TMP_Text resultLabel;
    [SerializeField] private Image enemyHealthBar;
    [SerializeField] private Image rabbitHealthBar;
    [SerializeField] private Animator wolfAnimator;
    [SerializeField] private Animator rabbitAnimator;
    [SerializeField] private float perfectDamage = 0.1f;
    [SerializeField] private float goodDamage = 0.05f;
    [SerializeField] private float badDamage = 0.05f;
    [SerializeField] private float resultHoldDuration = 0.5f;
    [SerializeField] private float resultAnimDuration = 0.1f;
    [SerializeField] private float resultMinScale = 0.2f;
    [SerializeField] private float resultMaxScale = 1f;
    [SerializeField] private Color perfectColor = Color.yellow;
    [SerializeField] private Color goodColor = Color.green;
    [SerializeField] private Color badColor = new Color(0.5f, 0f, 0.5f);
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioSource clickSource;
    [SerializeField] private RabbitHeadbop rabbitHeadbop;

    private float resultElapsed;
    private float resultTotalTime;
    private bool resultVisible;
    private bool hasLoadedWinScene;
    private bool hasLoadedDeathScene;

    private void Awake()
    {
        if (counter == null)
        {
            counter = FindFirstObjectByType<CounterLoop>();
        }

        ResolveHealthBars();
        ResolveWolfAnimator();
        ResolveRabbitAnimator();

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
        if (!StartPause.IsGameStarted)
            return;

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

    private void ResolveHealthBars()
    {
        if (enemyHealthBar == null)
        {
            enemyHealthBar = FindImageByName("EnemyHealthBar", "Enemy HP", "EnemyHealth", "enemyHealthBar");
        }

        if (rabbitHealthBar == null)
        {
            rabbitHealthBar = FindImageByName("RabbitHealthBar", "Rabbit HP", "RabbitHealth", "rabbitHealthBar");
        }

        if (enemyHealthBar == null)
        {
            Debug.LogWarning("Enemy health bar image could not be found automatically. Assign it in the inspector.");
        }

        if (rabbitHealthBar == null)
        {
            Debug.LogWarning("Rabbit health bar image could not be found automatically. Assign it in the inspector.");
        }
    }

    private void ResolveWolfAnimator()
    {
        if (wolfAnimator == null)
        {
            wolfAnimator = FindAnimatorByName("WolfEnemy", "Wolf", "EnemyAnimator");
        }

        if (wolfAnimator == null)
        {
            Debug.LogWarning("Wolf animator could not be found automatically. Assign it in the inspector.");
        }
    }

    private void ResolveRabbitAnimator()
    {
        if (rabbitAnimator == null)
        {
            rabbitAnimator = FindAnimatorByName("RabbitPlayer", "Rabbit", "RabbitAnimator");
        }

        if (rabbitAnimator == null)
        {
            Debug.LogWarning("Rabbit animator could not be found automatically. Assign it in the inspector.");
        }
    }

    private Image FindImageByName(params string[] possibleNames)
    {
        foreach (string possibleName in possibleNames)
        {
            GameObject foundObject = GameObject.Find(possibleName);
            if (foundObject != null)
            {
                Image image = foundObject.GetComponent<Image>();
                if (image != null)
                {
                    return image;
                }
            }
        }

        Image[] images = Resources.FindObjectsOfTypeAll<Image>();
        foreach (Image image in images)
        {
            if (image == null || image.gameObject.scene != gameObject.scene)
                continue;

            string nameLower = image.name.ToLowerInvariant();
            foreach (string possibleName in possibleNames)
            {
                if (nameLower.Contains(possibleName.ToLowerInvariant()))
                {
                    return image;
                }
            }
        }

        return null;
    }

    private Animator FindAnimatorByName(params string[] possibleNames)
    {
        foreach (string possibleName in possibleNames)
        {
            GameObject foundObject = GameObject.Find(possibleName);
            if (foundObject != null)
            {
                Animator animator = foundObject.GetComponent<Animator>();
                if (animator != null)
                {
                    return animator;
                }
            }
        }

        Animator[] animators = Resources.FindObjectsOfTypeAll<Animator>();
        foreach (Animator animator in animators)
        {
            if (animator == null || animator.gameObject.scene != gameObject.scene)
                continue;

            string nameLower = animator.name.ToLowerInvariant();
            foreach (string possibleName in possibleNames)
            {
                if (nameLower.Contains(possibleName.ToLowerInvariant()))
                {
                    return animator;
                }
            }
        }

        return null;
    }

    private void TriggerBadAnimation()
    {
        if (wolfAnimator != null)
        {
            wolfAnimator.SetTrigger("Bad");
        }
        else
        {
            Debug.LogWarning("Wolf animator is not assigned, so the bad animation could not be triggered.");
        }
    }

    private void TriggerRabbitAngryAnimation()
    {
        if (rabbitAnimator == null)
        {
            Debug.LogWarning("Rabbit animator is not assigned, so RabbitAngry could not be triggered.");
            return;
        }

        rabbitAnimator.SetTrigger("RabbitAngry");
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
            TriggerRabbitAngryAnimation();
            rabbitHeadbop?.TriggerForwardThrust();
            ApplyHealthChange(enemyHealthBar, perfectDamage);
        }
        else if (currentValue >= 11 && currentValue <= 30)
        {
            Debug.Log("Good!");
            ShowResultText("Good!", goodColor);
            TriggerRabbitAngryAnimation();
            rabbitHeadbop?.TriggerForwardThrust();
            ApplyHealthChange(enemyHealthBar, goodDamage);
        }
        else
        {
            Debug.Log("Bad...");
            ShowResultText("Bad...", badColor);
            rabbitHeadbop?.TriggerBadThrust();
            ApplyHealthChange(rabbitHealthBar, badDamage);
            TriggerBadAnimation();
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

    private void ApplyHealthChange(Image healthBar, float amount)
    {
        if (healthBar == null)
            return;

        float previousFill = healthBar.fillAmount;
        healthBar.fillAmount = Mathf.Clamp01(healthBar.fillAmount - amount);

        Debug.Log($"{healthBar.name} fill changed from {previousFill} to {healthBar.fillAmount}");

        if (healthBar.fillAmount <= 0f)
        {
            Debug.Log($"{healthBar.name} reached zero.");

            if (healthBar == enemyHealthBar && !hasLoadedWinScene)
            {
                hasLoadedWinScene = true;

                if (SceneManager.GetActiveScene().name == "levelWolf")
                {
                    SceneManager.LoadScene("Win2");
                }
                else
                {
                    SceneManager.LoadScene("Win");
                }
            }
            else if (healthBar == rabbitHealthBar && !hasLoadedDeathScene)
            {
                hasLoadedDeathScene = true;
                SceneManager.LoadScene("Death");
            }
        }
    }
}
