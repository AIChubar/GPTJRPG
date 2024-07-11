using UnityEngine;

/// <summary>
/// Script for objects that should not be destroyed when the scene is switched.
/// </summary>
public class DontDestroy : MonoBehaviour
{
    [HideInInspector]
    public string objectID;
    private void Awake()
    {
        objectID = name + transform.position;
    }
    void Start()
    {
        for (int i = 0; i < FindObjectsOfType<DontDestroy>().Length; i++)
        {
            if (FindObjectsOfType<DontDestroy>()[i] != this)
            {
                if (FindObjectsOfType<DontDestroy>()[i].objectID == objectID)
                {
                    Destroy(gameObject);
                }
            }
        }
        DontDestroyOnLoad(gameObject);
    }
}
