using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public JSONReader.UnitJSON unitData;
    
    public void SetData(JSONReader.UnitJSON ud)
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
        SpawnDamageText(hp);
        if (unitData.currentHP <= 0)
        {
            unitData.currentHP = 0;
            GameEvents.gameEvents.UnitHPChanged(this);
            GameEvents.gameEvents.UnitKilled(this);
            StartCoroutine(DeathAnimation(0.5f));
        }
    }

    //works only for receiving damage
    private void SpawnDamageText(int hp)
    {
        var obj = Instantiate(GameManager.gameManager.floatingTextPrefab, transform.position + new Vector3(0f, 0.7f), Quaternion.identity, transform);
        obj.GetComponent<FloatingText>().LaunchText(hp);
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
