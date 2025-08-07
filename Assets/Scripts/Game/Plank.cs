using UnityEngine;

public class Plank : MonoBehaviour
{
    float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        GameMenager gameMenager = FindFirstObjectByType<GameMenager>();
        if(!gameMenager.resolution_16_9)
        {
            Vector3 scale = transform.localScale;
            scale.x = 0.45f;
            transform.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
