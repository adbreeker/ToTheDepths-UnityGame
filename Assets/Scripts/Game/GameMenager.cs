using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenager : MonoBehaviour
{
    //game
    public bool resolution_16_9;
    public GameObject piranha, plank, coin, background;
    public Player player;
    Vector3 sp1, sp2, sp3, sp4, sp5;

    //ui
    int score = 0;
    public TextMeshProUGUI scoreText, coinCounterText;
    public GameObject textbox;
    bool newBest = false;
    bool deathpanel_active = false;

    //sound
    public SoundMenager soundMenager;
    public AudioSource backgroundMusic;

    //continue panel
    public GameObject continuePanel;
    public TextMeshProUGUI actualdepth;

    //death panel
    public GameObject deathPanel;
    public TextMeshProUGUI finaldepth;


    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        if (!PlayerPrefs.HasKey("bestDepth"))
        {
            PlayerPrefs.SetInt("bestDepth", 0);
        }
        Time.timeScale = 1;
        coinCounterText.text = PlayerPrefs.GetInt("Coins").ToString();
        CheckResolution();
        if(resolution_16_9)
        {
            sp1 = new Vector3(-2, -6, 0);
            sp2 = new Vector3(-1, -6, 0);
            sp3 = new Vector3(0, -6, 0);
            sp4 = new Vector3(1, -6, 0);
            sp5 = new Vector3(2, -6, 0);
        }
        else
        {
            sp1 = new Vector3(-1.8f, -6, 0);
            sp2 = new Vector3(-0.8f, -6, 0);
            sp3 = new Vector3(0, -6, 0);
            sp4 = new Vector3(0.8f, -6, 0);
            sp5 = new Vector3(1.8f, -6, 0);
        }
        GameStart();
    }

    void CheckResolution()
    {
        if(Screen.currentResolution.height / Screen.currentResolution.width < 2)
        {
            resolution_16_9 = true;
        }
        else
        {
            resolution_16_9 = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!deathpanel_active)
            {
                pause();
            }
        }
    }

    IEnumerator SpawnSprites()
    {
        while (true)
        {
            float waitTime = Random.Range(2f, 3f);
            yield return new WaitForSeconds(waitTime);

            int spawnpoint = Random.Range(1, 6);
            int fishOrPlank = Random.Range(0, 2);
            if (spawnpoint == 1)
            {
                if (fishOrPlank == 0)
                {
                    Instantiate(piranha, sp1, Quaternion.identity);
                }
                else
                {
                    Instantiate(plank, sp1, Quaternion.identity);
                    SpawnCoin(spawnpoint);
                }
            }
            if (spawnpoint == 2)
            {
                if (fishOrPlank == 0)
                {
                    Instantiate(piranha, sp2, Quaternion.identity);
                }
                else
                {
                    Instantiate(plank, sp2, Quaternion.identity);
                    SpawnCoin(spawnpoint);
                }
            }
            if (spawnpoint == 3)
            {
                if (fishOrPlank == 0)
                {
                    Instantiate(piranha, sp3, Quaternion.identity);
                }
                else
                {
                    Instantiate(plank, sp3, Quaternion.identity);
                    SpawnCoin(spawnpoint);
                }
            }
            if (spawnpoint == 4)
            {
                if (fishOrPlank == 0)
                {
                    Instantiate(piranha, sp4, Quaternion.identity);
                }
                else
                {
                    Instantiate(plank, sp4, Quaternion.identity);
                    SpawnCoin(spawnpoint);
                }
            }
            if (spawnpoint == 5)
            {
                if (fishOrPlank == 0)
                {
                    Instantiate(piranha, sp5, Quaternion.identity);
                }
                else
                {
                    Instantiate(plank, sp5, Quaternion.identity);
                    SpawnCoin(spawnpoint);
                }
            }
        }
    }

    void SpawnCoin(int usedSpawnPoint)
    {
        int choice = Random.Range(1, 4);
        if (choice == 3)
        {
            int spawnpoint = Random.Range(1, 6);
            while (spawnpoint == usedSpawnPoint || Mathf.Abs(usedSpawnPoint-spawnpoint) == 1)
            {
                spawnpoint = Random.Range(1, 6);
            }

            if (spawnpoint == 1)
            {
                Instantiate(coin, sp1, Quaternion.identity);
            }
            if (spawnpoint == 2)
            {
                Instantiate(coin, sp2, Quaternion.identity);
            }
            if (spawnpoint == 3)
            {
                Instantiate(coin, sp3, Quaternion.identity);
            }
            if (spawnpoint == 4)
            {
                Instantiate(coin, sp4, Quaternion.identity);
            }
            if (spawnpoint == 5)
            {
                Instantiate(coin, sp5, Quaternion.identity);
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
            if(PlayerPrefs.GetInt("bestDepth") < score)
            {
                newBest = true;
                PlayerPrefs.SetInt("bestDepth", score);
            }
            scoreText.text = "Depth:\n" + score.ToString() + " m";
            Time.timeScale = 1f + ((float)score) / 250f;
        }
    }

    IEnumerator GoingDeeper()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.7f);
            float decrease = 0.0F;
            Color oldColor = background.GetComponent<SpriteRenderer>().color;
            if ((oldColor.r == oldColor.g && oldColor.g == oldColor.b) && oldColor.r > 0.1F)
            {
                decrease = 0.01F;
            }
            if ((oldColor.r == oldColor.g && oldColor.g == oldColor.b) && oldColor.r > 0.2F)
            {
                decrease = 0.05F;
            }
            Color newColor = new Color(oldColor.r - decrease, oldColor.g - decrease, oldColor.b - decrease, oldColor.a);
            background.GetComponent<SpriteRenderer>().color = newColor;
        }
    }

    public void PlayerDeath()
    {
        soundMenager.PlayerDamagedSound();
        Time.timeScale = 0;
        ShowDeathPanel();
        StartCoroutine("deathAnimation");
    }

    void ShowDeathPanel()
    {
        deathPanel.SetActive(true);
        deathpanel_active = true;
        textbox.SetActive(false);
        continuePanel.SetActive(false);
        finaldepth.text = "You died on depth:\n" + score + " m";
        if (newBest)
        {
            finaldepth.text += "\n NEW BEST!";
        }
    }

    IEnumerator deathAnimation()
    {
        Color oldColor = player.GetComponent<SpriteRenderer>().color;
        Color newColor = new Color(1f, 0f, 0f, 1f);
        player.GetComponent<SpriteRenderer>().color = newColor;
        while (oldColor.a > 0f)
        {
            yield return new WaitForSecondsRealtime(0.0001f);
            oldColor = player.GetComponent<SpriteRenderer>().color;
            newColor = new Color(1f, 0f, 0f, oldColor.a - 0.01f);
            player.GetComponent<SpriteRenderer>().color = newColor;
        }
        Time.timeScale = 0;
    }

    public void PlayerRevive()
    {
        StartCoroutine("deathUndo");
    }

    IEnumerator deathUndo()
    {
        Color oldColor = player.GetComponent<SpriteRenderer>().color;
        Color newColor = new Color(1f, 1f, 1f, 0f);
        player.GetComponent<SpriteRenderer>().color = newColor;
        deathPanel.SetActive(false);
        while (oldColor.a < 1f)
        {
            yield return new WaitForSecondsRealtime(0.0001f);
            oldColor = player.GetComponent<SpriteRenderer>().color;
            newColor = new Color(1f, 1f, 1f, oldColor.a + 0.01f);
            player.GetComponent<SpriteRenderer>().color = newColor;
        }
        textbox.SetActive(true);
        player.unfreeze();
        Time.timeScale = 1f + ((float)score) / 250f;
        deathpanel_active = false;
    }


    public void RestartButton()
    {
        soundMenager.ButtonClickSound();
        SceneManager.LoadScene("Game");
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
        actualdepth.text = "Actual Depth:\n" + score + " m \n Best Depth: \n" + PlayerPrefs.GetInt("bestDepth").ToString() + " m";
        player.freeze();
    }

    public void Continue()
    {
        soundMenager.ButtonClickSound();
        backgroundMusic.Play();
        Time.timeScale = 1f + ((float)score) / 250f;
        continuePanel.SetActive(false);
        textbox.SetActive(true);
        player.unfreeze();
    }

    public void GameStart()
    {
        StartCoroutine("SpawnSprites");
        StartCoroutine("ScoreUp");
        StartCoroutine("GoingDeeper");
    }
}
