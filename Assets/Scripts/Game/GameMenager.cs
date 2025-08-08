using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenager : MonoBehaviour
{
    //Instance
    public static GameMenager instance;



    public GameObject piranha, plank, coin, background;
    public Player player;
    public List<Transform> spawnPoints = new List<Transform>();

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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("bestDepth"))
        {
            PlayerPrefs.SetInt("bestDepth", 0);
        }
        Time.timeScale = 1;
        coinCounterText.text = PlayerPrefs.GetInt("Coins").ToString();
        GameStart();
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
        Debug.Log("Fps: " + (1f / Time.deltaTime).ToString("F2"));
    }

    IEnumerator SpawnSprites()
    {
        while (true)
        {
            float waitTime = Random.Range(2f, 3f);
            yield return new WaitForSeconds(waitTime);

            int spawnpoint = Random.Range(0, 5);
            int fishOrPlank = Random.Range(0, 2);
            if (fishOrPlank == 0)
            {
                Instantiate(piranha, spawnPoints[spawnpoint].position, Quaternion.identity);
            }
            else
            {
                Instantiate(plank, spawnPoints[spawnpoint].position, Quaternion.identity, spawnPoints[spawnpoint]);
                SpawnCoin(spawnpoint);
            }
        }
    }

    void SpawnCoin(int usedSpawnPoint)
    {
        int choice = Random.Range(1, 4);
        if (choice == 3)
        {
            int spawnpoint = Random.Range(0, 5);
            while (spawnpoint == usedSpawnPoint || Mathf.Abs(usedSpawnPoint-spawnpoint) == 1)
            {
                spawnpoint = Random.Range(0, 5);
            }

            Instantiate(coin, spawnPoints[spawnpoint].position, Quaternion.identity);
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
        SpriteRenderer backgroundRenderer = background.GetComponent<SpriteRenderer>();
        while (true)
        {
            yield return new WaitForFixedUpdate();
            Color currentColor = backgroundRenderer.color;
            Color newColor = Color.Lerp(currentColor, Color.gray3, 15/255f * Time.fixedDeltaTime);

            backgroundRenderer.color = newColor;
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
