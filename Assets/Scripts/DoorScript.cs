using System.Collections;
using UnityEngine;

/// <summary>
/// Script attached to a door object. Can be called when the door should be opened.
/// </summary>
public class DoorScript : MonoBehaviour
{
    [Header("Transparent sprite mask")]
    [SerializeField]
    private SpriteMask mask;
    [Header("Door collider")]
    [SerializeField]
    private Collider2D col;
    
    void Start()
    {
        col.enabled = false;
    }

    public void OpenDoor()
    {
        StartCoroutine(DoorOpening());
    }
    
    private IEnumerator DoorOpening()
    {
        col.enabled = false;
        for (float t = 0; t < 1; t += Time.deltaTime / 2)
        {
            mask.transform.Translate(Time.deltaTime / 2, 0, 0);
            yield return null;
        }

        col.enabled = true;
    }
}
