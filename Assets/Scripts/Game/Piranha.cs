using UnityEngine;

public class Piranha : MonoBehaviour
{
    float speed = 2f;
    float sideSpeed;
    float leftbound = -2.2f;
    float rightbound = 2.2f;
    // Start is called before the first frame update
    void Start()
    {
        sideSpeed = Random.Range(0.5f, 1.5f);
        if(Random.Range(0,2) == 0)
        {
            sideSpeed *= -1;
        }
        float screenWidth = Camera.main.orthographicSize * 2 / Screen.height * Screen.width;
        leftbound = -screenWidth / 2 + 0.4f;
        rightbound = screenWidth / 2 - 0.4f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime);
        if (transform.position.x >= rightbound)
        {
            sideSpeed = Mathf.Abs(sideSpeed) * -1;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * -1;
            transform.localScale = scale;
        }
        if (transform.position.x <= leftbound)
        {
            sideSpeed = Mathf.Abs(sideSpeed);

        }

        if (sideSpeed < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * -1;
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        transform.Translate(Vector2.right * sideSpeed * Time.fixedDeltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
