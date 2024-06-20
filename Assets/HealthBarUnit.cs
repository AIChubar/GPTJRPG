using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Health Bar that can be attached to a unit.
/// </summary>
public class HealthBarUnit : MonoBehaviour
{
    private Slider slider;

    [HideInInspector]
    private Unit unit;
    
    void Start()
    {
        unit = GetComponentInParent<Unit>();
        slider = GetComponent<Slider>();
        GameEvents.gameEvents.OnUnitHPChanged += GameEvents_OnUnitHPChanged;
        GameEvents.gameEvents.OnUnitKilled += GameEvents_OnUnitKilled;
        slider.maxValue = unit.unitData.maxHP;
        slider.value = unit.unitData.currentHP;
    }

    private void GameEvents_OnUnitHPChanged(Unit u)
    {
        UpdateHealth();
    }
    
    private void GameEvents_OnUnitKilled(Unit u)
    {
        if (unit.Equals(u))
            StartCoroutine(Disappear());
    }
    
    public void UpdateHealth ()
    {
        slider.value = unit.unitData.currentHP;
        
    }

    private IEnumerator Disappear()
    {
        Image[] children = GetComponentsInChildren<Image>();

        for (float t = 0; t < 1; t += Time.deltaTime / 0.1f)
        {
            foreach(Image child in children) {
                var newColor = child.color;
                newColor.a =  Mathf.Lerp(1, 0, t);
                child.color = newColor;
            }
            yield return null;
        }
        GameEvents.gameEvents.OnUnitHPChanged -= GameEvents_OnUnitHPChanged;
        GameEvents.gameEvents.OnUnitKilled -= GameEvents_OnUnitKilled;
        Destroy(gameObject);
    }
    
}
