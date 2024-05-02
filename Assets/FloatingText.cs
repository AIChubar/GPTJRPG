using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float textSpeed = 4f;

    public float lifeTime = 0.4f;
    // Start is called before the first frame update


    public void LaunchText(int hpDif)
    {
        var textObj = gameObject.GetComponent<TextMeshPro>();
        textObj.text = Math.Abs(hpDif).ToString();
        if (hpDif < 0)
            textObj.color = new Color(0.9f, 0.0f, 0.1f, 1f);
        else if (hpDif > 0)
            textObj.color = new Color(0.2f, 0.9f, 0.2f, 1f);
        StartCoroutine(DestroyDelay(lifeTime));
    }
    
    private IEnumerator DestroyDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(0, textSpeed) * Time.deltaTime;
    }
}
