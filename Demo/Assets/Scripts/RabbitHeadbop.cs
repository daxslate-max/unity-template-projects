using UnityEngine;

public class RabbitHeadbop : MonoBehaviour
{
    [SerializeField] private float bopInterval = 1f;
    [SerializeField] private float squishDuration = 0.12f;
    [SerializeField] private Vector3 squishScale = new Vector3(1.1f, 0.8f, 1f);

    private Vector3 originalScale;
    private float timer;
    private float animationTime;
    private bool isBopping;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
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
    }
}
