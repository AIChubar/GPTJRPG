using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// Script that controls the text that floats above the object.
/// </summary>
public class FloatingText : MonoBehaviour
{
    public float textSpeed = 4f;

    public float lifeTime = 0.4f;

    private TextMeshPro _textObj;
    
    /// <summary>
    /// Launches the Text object.
    /// </summary>
    /// <param name="hpDif">Number to be displayed/>.</param>
    public void LaunchText(int hpDif)
    {
        _textObj = gameObject.GetComponent<TextMeshPro>();
        _textObj.text = Math.Abs(hpDif).ToString();
        _textObj.color =  new Color(0.9f, 0.0f, 0.1f, 0f);
        StartCoroutine(ObjectLife(lifeTime, hpDif));
    }
    
    private IEnumerator ObjectLife(float duration, int hpDif)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        if (hpDif < 0)
            _textObj.color = new Color(0.9f, 0.0f, 0.1f, 1f);
        else if (hpDif > 0)
            _textObj.color = new Color(0.2f, 0.9f, 0.2f, 1f);
        else
        {
            _textObj.color = Color.white;
        }
        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            transform.position += new Vector3(0, textSpeed) * Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }


}
