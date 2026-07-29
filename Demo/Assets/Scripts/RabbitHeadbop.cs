using UnityEngine;

public class RabbitHeadbop : MonoBehaviour
{
    [SerializeField] private float bopInterval = 1f;
    [SerializeField] private float squishDuration = 0.12f;
    [SerializeField] private Vector3 squishScale = new Vector3(1.1f, 0.8f, 1f);

    [Header("Thrust")]
    [SerializeField] private Transform badThrustTarget;
    [SerializeField] private float thrustDistance = 0.2f;
    [SerializeField] private float thrustDuration = 0.08f;
    [SerializeField] private float thrustHoldDuration = 0.05f;

    private Vector3 originalScale;
    private float timer;
    private float animationTime;
    private bool isBopping;

    private bool isThrusting;
    private float thrustElapsed;
    private float thrustTotalTime;
    private float thrustDirection;
    private Transform thrustTarget;
    private Vector3 thrustOriginalLocalPosition;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (!StartPause.IsGameStarted)
            return;

        timer += Time.deltaTime;

        if (!isBopping && timer >= bopInterval)
        {
            timer -= bopInterval;
            isBopping = true;
            animationTime = 0f;
        }

        if (isBopping)
        {
            animationTime += Time.deltaTime;
            float halfDuration = squishDuration * 0.5f;

            if (animationTime < halfDuration)
            {
                float t = animationTime / halfDuration;
                transform.localScale = Vector3.Lerp(originalScale, squishScale, t);
            }
            else if (animationTime < squishDuration)
            {
                float t = (animationTime - halfDuration) / halfDuration;
                transform.localScale = Vector3.Lerp(squishScale, originalScale, t);
            }
            else
            {
                transform.localScale = originalScale;
                isBopping = false;
            }
        }

        if (isThrusting)
        {
            UpdateThrust();
        }
    }

    private void UpdateThrust()
    {
        if (thrustTarget == null)
        {
            isThrusting = false;
            return;
        }

        thrustElapsed += Time.deltaTime;

        if (thrustElapsed >= thrustTotalTime)
        {
            thrustTarget.localPosition = thrustOriginalLocalPosition;
            isThrusting = false;
            return;
        }

        float offset;
        if (thrustElapsed < thrustDuration)
        {
            float t = thrustElapsed / thrustDuration;
            offset = Mathf.Lerp(0f, thrustDistance * thrustDirection, t);
        }
        else if (thrustElapsed < thrustDuration + thrustHoldDuration)
        {
            offset = thrustDistance * thrustDirection;
        }
        else
        {
            float t = (thrustElapsed - thrustDuration - thrustHoldDuration) / thrustDuration;
            offset = Mathf.Lerp(thrustDistance * thrustDirection, 0f, t);
        }

        Vector3 localPosition = thrustOriginalLocalPosition + new Vector3(offset, 0f, 0f);
        thrustTarget.localPosition = localPosition;
    }

    private void StartThrust(Transform target, float direction)
    {
        if (target == null)
            return;

        if (isThrusting && thrustTarget != null)
        {
            thrustTarget.localPosition = thrustOriginalLocalPosition;
        }

        thrustTarget = target;
        thrustOriginalLocalPosition = thrustTarget.localPosition;
        thrustDirection = direction;
        thrustElapsed = 0f;
        thrustTotalTime = thrustDuration * 2f + thrustHoldDuration;
        isThrusting = true;
    }

    public void TriggerForwardThrust()
    {
        StartThrust(transform, 1f);
    }

    public void TriggerBadThrust()
    {
        StartThrust(badThrustTarget != null ? badThrustTarget : transform, -1f);
    }
}
