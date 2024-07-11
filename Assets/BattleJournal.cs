using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Battle Journal logic and controls.
/// </summary>
public class BattleJournal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI journalText;
    [SerializeField] private TextMeshProUGUI journalHUD;
    [SerializeField] private Button journalButton;
    [SerializeField] private ScrollRect scrollRect;
    private void Awake()
    {
        GameManager.gameManager.battleJournal = this;
        journalText.text = "";
        gameObject.SetActive(false);
    }

    public void AddActionDescription(string action)
    {
        journalText.text += action + "\n";
        journalHUD.text = journalText.text.Replace("\n\n", "\n");
    }

    public void OnJournalOpened()
    {
        AudioManager.instance.Play(GameManager.gameManager.ButtonClick);
        StartCoroutine(ForceScrollDown());
        journalButton.interactable = false;
        Time.timeScale = 0f;
    }
    public void OnJournalClosed()
    {
        AudioManager.instance.Play(GameManager.gameManager.ButtonClick);
        journalButton.interactable = true;
        Time.timeScale = 1f;
    }
    IEnumerator ForceScrollDown () {
        yield return new WaitForEndOfFrame ();
        Canvas.ForceUpdateCanvases ();
        scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases ();
    }
}
