using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Script for manipulating scenes, smooth scenes changing.
/// </summary>
public class SceneController : MonoBehaviour
{
    [Tooltip("Image that is going to be expanded to the whole screen during scene transition")]
    [SerializeField]
    private Image fader;
    
    private static SceneController instance;

    private GameObject _map;

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
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 1)
            _map = GameObject.FindWithTag("Map");
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static void LoadScene(int index, float closingDuration = 1f, float openingDuration = 1f, float waitTime = 0.4f, bool additive = false, bool toUnload = false)
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
        Time.timeScale = 1.0f;
        fader.gameObject.SetActive(true);
        for (float t = 0; t < 1; t += Time.deltaTime / closingDuration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, t));
            yield return null;
        }
        
        yield return new WaitForSeconds(waitTime/2f);

        if (additive)
        {
            SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
            _map.SetActive(false);
        }
        else
        {
            if (toUnload)
            {
                SceneManager.UnloadSceneAsync(index);
                _map.SetActive(true);

            }
            else
                SceneManager.LoadSceneAsync(index);
        }
        

        yield return new WaitForSeconds(waitTime/2f);


        for (float t = 0; t < 1; t += Time.deltaTime / openingDuration)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, t));
            yield return null;
        }
        
        if (toUnload)
        {
            GameManager.gameManager.SceneUnloaded();
        }

        fader.gameObject.SetActive(false);
        if (currentSceneIndex >= 1)
            GameManager.gameManager.transitioning = false;

        currentSceneIndex = index;
    }
    
    
}
