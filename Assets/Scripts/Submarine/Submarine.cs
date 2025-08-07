using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Submarine : MonoBehaviour
{

    Rigidbody2D rb;
    public SubmarineGameMenager gameMenager;
    bool firstMove = true;
    bool falling = false;
    public GameObject bubble;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("BubbleAnimation");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if(falling)
            {
                rb.linearVelocity = Vector2.zero;
            }
            falling = false;
            
            if(Time.timeScale >= 1)
            {
                rb.gravityScale = -1 / Mathf.Sqrt(Time.timeScale);
            }
            

            if(firstMove)
            {
                firstMove = false;
                unfreeze();
            }
        }
        else
        {
            if(Time.timeScale >= 1)
            {
                rb.gravityScale = 1/Mathf.Sqrt(Time.timeScale);
            }
            falling = true;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            collision.isTrigger = false;
            gameMenager.changeHealth(-3);
            freeze();
        }
        if (collision.gameObject.tag == "Pickable")
        {
            Destroy(collision.gameObject);
            gameMenager.AddCoin();
        }
        if (collision.gameObject.tag == "HealthRestore")
        {
            Destroy(collision.gameObject);
            gameMenager.changeHealth(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            gameMenager.changeHealth(-1);
        }
    }

    public void freeze()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void unfreeze()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

    IEnumerator BubbleAnimation()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            Vector3 bubblePos = transform.position - new Vector3(1.3f, Random.Range(0.1f,0.5f), 0);
            Instantiate(bubble, bubblePos, Quaternion.identity);
        }
    }


}
