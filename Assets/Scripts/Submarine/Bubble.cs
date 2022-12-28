using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float speed = 2f;
    public float disapearRatio = 0.95f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);


        Vector3 scale = transform.localScale;
        scale = scale * disapearRatio;
        transform.localScale = scale;
        if(transform.localScale.x < 0.01)
        {
            Destroy(gameObject);
        }
    }
}
