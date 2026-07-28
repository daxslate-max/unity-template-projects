using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float height;
    private float startingHeight;
    bool rising;

    // Start is called before the first frame update
    void Start()
    {
        startingHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update() {
        if (transform.position.y <= startingHeight) {
            rising = true;
        } else if (transform.position.y > height) {
            rising = false;
        }

        if (rising) {
            transform.Translate(new Vector3(0,1,0) * speed);
        } else {
            transform.Translate(new Vector3(0,-1,0) * speed);
        }

    }

}
