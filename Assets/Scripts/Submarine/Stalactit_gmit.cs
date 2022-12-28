using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalactit_gmit : MonoBehaviour
{
    float speed = 2f;
    Vector2 direction = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 scale = transform.localScale;
        scale.y = Random.Range(0.40f, 1.75f);
        

        if(transform.rotation.z != 0.0f)
        {
            scale.x *= -1;
        }

        transform.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void setDirection(Vector2 dir)
    {
        direction = dir;
    }
}
