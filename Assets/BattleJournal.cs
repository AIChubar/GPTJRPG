using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleJournal : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI journalText;
    [SerializeField] private Button journalButton;
    [SerializeField] private ScrollRect scrollRect;
    private void Start()
    {
        journalText.text = "";
        gameObject.SetActive(false);
    }

    public void AddActionDescription(string action)
    {
        journalText.text += action + "\n";
    }

    public void OnJournalOpened()
    {
        StartCoroutine(ForceScrollDown());
        journalButton.interactable = false;
        Time.timeScale = 0f;
    }
    public void OnJournalClosed()
    {
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
