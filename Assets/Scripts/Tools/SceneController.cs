using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script for manipulating scenes, smooth scenes changing.
/// </summary>
public class SceneController : MonoBehaviour
{
    /// <summary>
    /// Image that is going to be expanded to the whole screen during scene transition.
    /// </summary>
    [SerializeField]
    private Image fader;
    
    private static SceneController instance;

    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound LevelMusic;
    
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound MenuMusic;
    
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound BattleMusic;

    private int currentSceneIndex = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            fader.rectTransform.sizeDelta = new Vector2(Screen.width + 2000, Screen.height + 2000);
            fader.gameObject.SetActive(false);
        }
        
    }
    
    void Start()
    {
        AudioManager.instance.Play(MenuMusic);
    }
    /// <summary>
    /// Method that adds or removes an outline from the given GameObject.
    /// </summary>
    /// <param name="index">Scene of the index to be loaded.</param>
    /// <param name="closingDuration">Duration of the fading in animation.</param>
    /// <param name="openingDuration">Duration of the fading out animation.</param>
    /// <param name="waitTime">Duration when the screen is fully faded.</param>
    /// <param name="additive">If the additive scene is loaded</param>
    /// <param name="toUnload">If the additive scene is unloaded.</param>
    public static void LoadScene(int index, float closingDuration = 1f, float openingDuration = 1f, float waitTime = 1.0f, bool additive = false, bool toUnload = false)
    {
        instance.StartCoroutine(instance.FadeScene(index, closingDuration, openingDuration, waitTime, additive, toUnload));
    }
    
    public static void UnloadScene(int index, float closingDuration = 1f, float openingDuration = 1f, float waitTime = 0.4f, bool additive = false, bool toUnload = true)
    {
        instance.StartCoroutine(instance.FadeScene(index, closingDuration, openingDuration, waitTime, additive, toUnload));
    }

    private IEnumerator FadeScene(int index, float closingDuration, float openingDuration, float waitTime, bool additive, bool toUnload)
    {
        if (currentSceneIndex >= 1)
            GameManager.gameManager.transitioning = true;
        Time.timeScale = 0.0f;
        fader.gameObject.SetActive(true);
        
        AudioManager.instance.Stop(MenuMusic);
        AudioManager.instance.Stop(LevelMusic);
        AudioManager.instance.Stop(BattleMusic);
        for (float t = 0; t < 1; t += Time.unscaledDeltaTime / closingDuration)
        {
            fader.color = new Color(0, 0, 0, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        fader.color = Color.black;
        yield return new WaitForSecondsRealtime(0.1f);
        
        if (index == 0)
            Destroy(GameManager.gameManager.gameObject);

        if (additive)
        {
            GameManager.gameManager.SceneAdditive();

            SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        }
        else
        {
            if (toUnload)
            {
                SceneManager.UnloadSceneAsync(index);
                GameManager.gameManager.SceneUnloaded();
            }
            else
            {
                SceneManager.LoadSceneAsync(index);
                
            }
        }
        currentSceneIndex = index;
        
        yield return new WaitForSecondsRealtime(waitTime);
        
        if (index == 0)
            AudioManager.instance.Play(MenuMusic);
        else if (index == 1)
            AudioManager.instance.Play(LevelMusic);
        else if (index == 2)
            AudioManager.instance.Play(LevelMusic);
        for (int i = 0; i < 10; i++)
            yield return null;
        for (float t = 0; t < 1; t += Time.unscaledDeltaTime / openingDuration)
        {
            fader.color = new Color(0, 0, 0, Mathf.SmoothStep(1, 0, t));
            yield return null;
        }
        Time.timeScale = 1.0f;
        
        if (currentSceneIndex >= 1 && GameManager.gameManager != null)
            GameManager.gameManager.transitioning = false;
        
        fader.gameObject.SetActive(false);
        if (GameManager.gameManager == null || GameManager.gameManager.Equals(null))
            yield break;
        if (index == 1)
            GameManager.gameManager.SceneLoaded();
        if (toUnload)
        {
            GameManager.gameManager.SceneFinishedUnloading();
            
        }
        if (index == 1)
            GameManager.gameManager.SceneFinishedLoading();

    }
    
    
}
