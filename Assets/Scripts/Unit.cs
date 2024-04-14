using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData unitData;
    
    public void SetData(UnitData ud)
    {
        unitData = ud;
        if (!ud.friendly)
        {
            gameObject.layer = 3;
        }
    }

    public void ChangeCurrentHP(int hp)
    {
        unitData.currentHP += hp;
        GameEvents.gameEvents.UnitHPChanged(this);
        if (unitData.currentHP <= 0)
        {
            unitData.currentHP = 0;
            GameEvents.gameEvents.UnitKilled(this);
            StartCoroutine(DeathAnimation(0.5f));
        }
    }
    
    private IEnumerator DeathAnimation(float duration)
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        var sprite = GetComponent<SpriteRenderer>();

        for (float t = 0; t < 1; t += Time.deltaTime/duration)
        {
            sprite.color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, t));
            yield return null;
        }
        Destroy(gameObject);

        //healthSystem.OnHealthChanged -= HealthSystem_OnHealthChanged;
    }
}
