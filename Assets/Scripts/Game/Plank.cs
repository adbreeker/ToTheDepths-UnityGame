using UnityEngine;

public class Plank : MonoBehaviour
{
    public float speed = 2f;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
