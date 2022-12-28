using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SubmarineGameMenager : MonoBehaviour
{
    //game
    public Submarine submarine;
    public GameObject stalactit_gmit, coin, toolkit;
    public Transform sp1, sp2, spCoin;
    int health = 3;
    

    //ui
    int score = 0;
    public TextMeshProUGUI scoreText, coinCounterText;
    public GameObject textbox;
    bool newBest = false;
    public GameObject healthBar;
    public Sprite h0, h1, h2, h3;
    bool deathpanel_active = false;




    //sound
    public SoundMenager soundMenager;
    public AudioSource backgroundMusic;

    //continue panel
    public GameObject continuePanel;
    public TextMeshProUGUI actualdistance;

    //death panel
    public GameObject deathPanel;
    public TextMeshProUGUI finaldistance;


    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeRight;
        if (!PlayerPrefs.HasKey("bestDistance"))
        {
            PlayerPrefs.SetInt("bestDistance", 0);
        }
        Time.timeScale = 1;
        coinCounterText.text = PlayerPrefs.GetInt("Coins").ToString();
        GameStart();
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!deathpanel_active)
            {
                pause();
            }
        }
    }

    IEnumerator SpawnSprites()
    {
        while (true)
        {
            yield return new WaitForSeconds(2.5f);

            int spawnpoint = Random.Range(1, 3);
            int spawnCoin = Random.Range(0, 30);
            if (spawnpoint == 1)
            {
                GameObject pom = Instantiate(stalactit_gmit, sp1.position, Quaternion.identity);
                pom.GetComponent<Stalactit_gmit>().setDirection(Vector2.left);
            }
            if(spawnpoint == 2)
            {
                GameObject pom =Instantiate(stalactit_gmit, sp2.position, Quaternion.Euler(0.0f, 0.0f, 180.0f));
                pom.GetComponent<Stalactit_gmit>().setDirection(Vector2.right);
            }
            if(spawnCoin < 7)
            {
                Instantiate(coin, spCoin.position, Quaternion.Euler(0.0f, 0.0f, 90.0f));
            }
            if(spawnCoin == 8)
            {
                Instantiate(toolkit, spCoin.position, Quaternion.identity);
            }
        }
    }

    public void AddCoin()
    {
        soundMenager.PickUpSound();
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1);
        coinCounterText.text = PlayerPrefs.GetInt("Coins").ToString();
    }

    IEnumerator ScoreUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            score++;
            if (PlayerPrefs.GetInt("bestDistance") < score)
            {
                newBest = true;
                PlayerPrefs.SetInt("bestDistance", score);
            }
            scoreText.text = "Distance:\n" + score.ToString() + " m";
            CalculateTimeScale();
        }
    }

    public void changeHealth(int diff)
    {
        if(diff > 0)
        {
            soundMenager.RepairSound();
        }
        if(diff == -1)
        {
            soundMenager.FrictionSound();
        }
        if(health + diff  <= 3 && health + diff >= 0)
        {
            health = health + diff;
        }
        else
        {
            if(health + diff < 0)
            {
                health = 0;
            }
            if(health + diff > 3)
            {
                health = 3;
            }
        }
        string healthtype = "h" + health;
        healthBar.GetComponent<Image>().sprite = this.GetType().GetField(healthtype).GetValue(this) as Sprite;
        if (health == 0)
        {
            SubmarineCrash();
        }
    }

    public void SubmarineCrash()
    {
        soundMenager.SubmarineCrashSound();
        Time.timeScale = 0;
        deathPanel.SetActive(true);
        deathpanel_active = true;
        textbox.SetActive(false);
        continuePanel.SetActive(false);
        finaldistance.text = "You died on distance:\n" + score + " m";
        if (newBest)
        {
            finaldistance.text += "\n NEW BEST!";
        }
        StartCoroutine("crashAnimation");
    }

    IEnumerator crashAnimation()
    {
        Color oldColor = submarine.GetComponent<SpriteRenderer>().color;
        Color newColor = new Color(1f, 0f, 0f, 1f);
        submarine.GetComponent<SpriteRenderer>().color = newColor;
        while (oldColor.a > 0f)
        {
            yield return new WaitForSecondsRealtime(0.0001f);
            oldColor = submarine.GetComponent<SpriteRenderer>().color;
            newColor = new Color(1f, 0f, 0f, oldColor.a - 0.01f);
            submarine.GetComponent<SpriteRenderer>().color = newColor;
        }
        Time.timeScale = 0;
    }

    public void SubmarineUncrash()
    {
        StartCoroutine("crashUndo");
    }

    IEnumerator crashUndo()
    {
        Color oldColor = submarine.GetComponent<SpriteRenderer>().color;
        Color newColor = new Color(1f, 1f, 1f, 0f);
        submarine.GetComponent<SpriteRenderer>().color = newColor;
        deathPanel.SetActive(false);
        while (oldColor.a < 1f)
        {
            yield return new WaitForSecondsRealtime(0.0001f);
            oldColor = submarine.GetComponent<SpriteRenderer>().color;
            newColor = new Color(1f, 1f, 1f, oldColor.a + 0.01f);
            submarine.GetComponent<SpriteRenderer>().color = newColor;
        }
        textbox.SetActive(true);
        submarine.unfreeze();
        changeHealth(3);
        CalculateTimeScale();
        deathpanel_active = false;

    }

    public void RestartButton()
    {
        soundMenager.ButtonClickSound();
        SceneManager.LoadScene("Submarine");
    }

    public void BackToMenuButton()
    {
        soundMenager.ButtonClickSound();
        SceneManager.LoadScene("Menu");
    }

    public void pause()
    {
        backgroundMusic.Pause();
        Time.timeScale = 0;
        continuePanel.SetActive(true);
        textbox.SetActive(false);
        actualdistance.text = "Actual Distance:\n" + score + " m \n Best Distance: \n" + PlayerPrefs.GetInt("bestDistance").ToString() + " m";
        submarine.freeze();
    }

    public void Continue()
    {
        soundMenager.ButtonClickSound();
        backgroundMusic.Play();
        CalculateTimeScale();
        continuePanel.SetActive(false);
        textbox.SetActive(true);
        submarine.unfreeze();
    }


    void CalculateTimeScale()
    {
        if(Time.timeScale < 2f)
        {
            Time.timeScale = 1f + ((float)score) / 300f;
        }
        else
        {
            Time.timeScale = 2f;
        }
    }
    public void GameStart()
    {
        StartCoroutine("SpawnSprites");
        StartCoroutine("ScoreUp");
    }
}
