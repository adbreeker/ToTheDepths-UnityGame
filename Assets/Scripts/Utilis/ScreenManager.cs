using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this instance across scenes
            if(SceneManager.GetActiveScene().name == "Start")
            {
                LoadScene("Menu", ScreenOrientation.Portrait);
            }
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    public void SetOrientation(ScreenOrientation orientation)
    {
        if (orientation == ScreenOrientation.Portrait || orientation == ScreenOrientation.PortraitUpsideDown)
        {
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;
        }
        else
        {
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
        }
        Screen.orientation = ScreenOrientation.AutoRotation;
    }

    public void ResetScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void LoadScene(string sceneName, ScreenOrientation orientation)
    {
        Screen.orientation = orientation;
        SceneManager.LoadScene("Loading");
        StartCoroutine(SmoothLoading(sceneName, orientation));
    }

    IEnumerator SmoothLoading(string sceneName, ScreenOrientation orientation)
    {
        yield return new WaitForSecondsRealtime(0.5f); // Small delay to ensure the loading scene is displayed

        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        loading.allowSceneActivation = false;
        float timePass = 0f;
        while (timePass <= 1f)
        {
            Debug.Log($"Loading scene {sceneName}: {timePass}");
            yield return null;
            timePass += Time.unscaledDeltaTime;

            Camera cam = Camera.main;
            if (cam != null)
            {
                Debug.Log($"Camera found: {cam.name}");
                cam.ResetAspect();
                cam.rect = new Rect(0f, 0f, 1f, 1f);
                Canvas.ForceUpdateCanvases();
            }
        }

        loading.allowSceneActivation = true;
    }
}
