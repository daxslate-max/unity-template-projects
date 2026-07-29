using UnityEngine;

public class ClickIndicator : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float resetX = 10f;

    private void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if (transform.position.x <= -resetX)
        {
            transform.position = new Vector3(resetX, transform.position.y, transform.position.z);
        }
    }
}
