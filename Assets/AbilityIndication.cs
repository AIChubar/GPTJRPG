using System;
using UnityEngine;

public class AbilityIndication : MonoBehaviour
{
    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public void SetAbilityIndication(Sprite sprite)
    {
        _sr.sprite = sprite;
    }

    public void SetVisibility(float a)
    {
        if (_sr is not null)
            _sr.color= new Color(1, 1, 1, a);
    }
}
