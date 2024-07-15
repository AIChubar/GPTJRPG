using System;
using UnityEngine;
/// <summary>
/// Script that controls object used to indicate who was affected by the <see cref="Unit"/> Ability.
/// </summary>
public class AbilityIndication : MonoBehaviour
{
    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }
    /// <summary>
    ///  Set the sprite for the current Ability Indication.
    /// </summary>
    /// <param name="sprite">Sprite to be set.</param>
    public void SetAbilityIndication(Sprite sprite)
    {
        _sr.sprite = sprite;
    }
    /// <summary>
    /// Sets new alpha value for the current Ability Indication.
    /// </summary>
    /// <param name="a">Alpha value to be set.</param>
    public void SetVisibility(float a)
    {
        if (_sr is not null)
            _sr.color= new Color(1, 1, 1, a);
    }
}
