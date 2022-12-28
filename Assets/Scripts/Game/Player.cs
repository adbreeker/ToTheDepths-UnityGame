using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    float dirX;
    public GameMenager gameMenager;
    public GameObject bubble;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine("BubbleAnimation");
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(Input.acceleration.x) > 0.01f)
        {
            dirX = Input.acceleration.x * 10;
        }
        else
        {
            dirX = 0;
        }
        
        if(gameMenager.resolution_16_9)
        {
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, -2.4f, 2.4f), transform.position.y);
        }
        else
        {
            transform.position = new Vector2(Mathf.Clamp(transform.position.x, -2.1f, 2.1f), transform.position.y);
        }
        
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Obstacle")
        {
            collision.isTrigger = false;
            gameMenager.PlayerDeath();
            freeze();
        }
        if(collision.gameObject.tag == "Pickable")
        {
            Destroy(collision.gameObject);
            gameMenager.AddCoin();
        }
    }

    public void freeze()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void unfreeze()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
    }

    IEnumerator BubbleAnimation()
    {

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.5f,1f));
            for(int i=0; i<= Random.Range(0,3); i++)
            {
                Vector3 bubblePos = transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(0.1f, 0.7f), 0);
                GameObject pom = Instantiate(bubble, bubblePos, Quaternion.Euler(0, 0, -90));
                pom.GetComponent<Bubble>().disapearRatio = 0.985f;
            }
        }
    }
}
