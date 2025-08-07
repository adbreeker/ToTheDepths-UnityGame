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
        GameMenager gameMenager = FindFirstObjectByType<GameMenager>();
        if(!gameMenager.resolution_16_9)
        {
            leftbound = -1.9f;
            rightbound = 1.9f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
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
        transform.Translate(Vector2.right * sideSpeed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
