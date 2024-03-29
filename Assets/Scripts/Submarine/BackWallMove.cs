using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackWallMove : MonoBehaviour
{
    float speed = 1f;
    public Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        
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
}
