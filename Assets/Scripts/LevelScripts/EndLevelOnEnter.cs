using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Changes the scene when the player interact with the trigger.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class EndLevelOnEnter : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Hero hero))
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            GameManager.gameManager.levelIndex += 1;
            SceneController.LoadScene(1, 1, 1, 1.0f);
        }
    }
}
