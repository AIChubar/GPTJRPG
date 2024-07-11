using System.Collections;
using UnityEngine;

/// <summary>
/// Script attached to a door object. Called when the door should be opened.
/// </summary>
public class DoorScript : MonoBehaviour
{
    /// <summary>
    /// Transparent sprite mask.
    /// </summary>
    [SerializeField]
    private SpriteMask mask;
    /// <summary>
    /// Door Collider.
    /// </summary>
    [SerializeField]
    private Collider2D col;
    
    void Start()
    {
        col.enabled = false;
    }

    /// <summary>
    /// Opens the door, making it available to interact with. Called when the corresponding quest is completed.
    /// </summary>
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
