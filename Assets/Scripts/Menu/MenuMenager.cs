using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuMenager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI coinCounterText, submarineButtonText;
    public SoundMenager soundMenager;

    public GameObject instructionsPanel;

    bool quitTried = false;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        if (!PlayerPrefs.HasKey("Coins"))
        {
            PlayerPrefs.SetInt("Coins", 0);
        }

        coinCounterText.text = PlayerPrefs.GetInt("Coins").ToString();

        if (!PlayerPrefs.HasKey("Submarine"))
        {
            PlayerPrefs.SetInt("Submarine", 0);
        }

        if(PlayerPrefs.GetInt("Submarine") == 0)
        {
            submarineButtonText.text = "Spend 100 <color=yellow>●</color> to buy a submarine";
        }
        else
        {
            submarineButtonText.text = "Submarine";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!quitTried)
            {
                quitTried = true;
                _ShowAndroidToastMessage("Click one more time to exit application");
                StartCoroutine("CountQuitTry");
            }
            else
            {
                Application.Quit();
            }
            
        }
    }

    IEnumerator CountQuitTry()
    {
        yield return new WaitForSecondsRealtime(3f);
        quitTried = false;
    }

    public void LoadGame()
    {
        soundMenager.ButtonClickSound();
        SceneManager.LoadScene("Game");
    }

    public void Submarine()
    {
        if(PlayerPrefs.GetInt("Submarine") == 1)
        {
            soundMenager.ButtonClickSound();
            SceneManager.LoadScene("Submarine");
        }
        else
        {
            if(PlayerPrefs.GetInt("Coins") >= 100)
            {
                soundMenager.PurchaseSound();
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - 100);
                coinCounterText.text = PlayerPrefs.GetInt("Coins").ToString();
                submarineButtonText.text = "Submarine";
                PlayerPrefs.SetInt("Submarine", 1);
            }
            else
            {
                soundMenager.ButtonClickSound();
                _ShowAndroidToastMessage("You don't have enough coins");
            }
        }
    }

    public void ShowInstructions()
    {
        soundMenager.ButtonClickSound();
        instructionsPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        soundMenager.ButtonClickSound();
        instructionsPanel.SetActive(false);
    }

    /// <param name="message">Message string to show in the toast.</param>
    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
}
