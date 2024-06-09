using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    public enum UnitType { fighter, striker, guardian, tank, fortress, balancer, trickster, berserker, sniper, savage }

    public UnitType unitType;
    
    public JSONReader.UnitJSON unitData;

    [HideInInspector] public int  armouredUp = 0;
    
    [HideInInspector] public int  stunned = 0;
    
    [HideInInspector] public int  attacksRandomAlly = 0;
    
    [HideInInspector] public int  elusive = 0;

    [HideInInspector] public bool skillUsedLastTurn = false;

    [HideInInspector] public int skillCD = -1;

    [HideInInspector] public int currentSkillCD = 0;

    [HideInInspector] public Sprite abilitySprite;

    [HideInInspector] public bool isBeingDestroyed = false;

    [HideInInspector] public bool isBoss = false;

    public float deathTime = 1.5f;

    public void TurnStartUpdate(GameObject abilityButton)
    {
        if (abilityButton is not null)
        {
            abilityButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = abilitySprite;
            if (currentSkillCD > 0)
            {
                abilityButton.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = currentSkillCD.ToString();
            }
            else
            {
                abilityButton.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
            }
            if (currentSkillCD > 0 || unitType is UnitType.fighter)
            {
                abilityButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                abilityButton.GetComponent<Button>().interactable = true;
                currentSkillCD--;
            }
        }

        if (unitType is UnitType.tank)
        { 
            if (GameManager.gameManager.battleSystem.allyTauntCaster is not null && GameManager.gameManager.battleSystem.allyTauntCaster == gameObject)
            {
                GameManager.gameManager.battleSystem.allyTauntCaster = null;
            }
        }
        else if (unitType is UnitType.savage)
        {
            if (skillUsedLastTurn)
            {
                foreach (var ally in GameManager.gameManager.battleSystem.GetAllies())
                {
                    ally.GetComponent<Unit>().elusive--;
                }
            }
        }
        
        if (armouredUp > 0)
        {
            armouredUp--;
            if (armouredUp == 0)
            {
                string actionDescription = "The " + unitData.artisticName + " double armour bonus expires. ";
                GameManager.gameManager.battleJournal.AddActionDescription(actionDescription);
                unitData.armour /= 2;
            }
        }

        if (skillUsedLastTurn)
            skillUsedLastTurn = false;
        
        if (currentSkillCD > 0)
        {
            currentSkillCD--;
        }
    }

    public void ArmourUp()
    {
        if (armouredUp == 0)
        {
            unitData.armour *= 2;
            string actionDescription = "The " + unitData.artisticName + " doubles their armour for one turn. ";
            GameManager.gameManager.battleJournal.AddActionDescription(actionDescription);
        }
        else
        {
            string actionDescription = "The " + unitData.artisticName + " doubles their armour for one additional turn. ";
            GameManager.gameManager.battleJournal.AddActionDescription(actionDescription);
        }
        armouredUp++;

        
    }


    public IEnumerator UnitAbility()
    {
        Transform[] targets = {transform};
        Sprite spriteIndication = abilitySprite;
        var currentTarget = GameManager.gameManager.battleSystem.GetCurrentTarget();
        var allies = GameManager.gameManager.battleSystem.GetAllies();
        var enemies = GameManager.gameManager.battleSystem.GetEnemies();

        switch (unitType)
        {
            case UnitType.striker:
                if (skillCD == -1)
                    skillCD = 1;
                targets = new[] {currentTarget.transform };
                currentTarget.GetComponent<Unit>().stunned++;
                GameManager.gameManager.battleJournal.AddActionDescription(unitData.artisticName + " stuns " + currentTarget.GetComponent<Unit>().unitData.artisticName + " for one turn.");
                break;
            case UnitType.balancer:
                if (skillCD == -1)
                    skillCD = 1;
                targets = new[] { currentTarget.transform };
                currentTarget.GetComponent<Unit>().BeingHealed(0.20f, this);
                break;
            case UnitType.trickster:
                if (skillCD == -1)
                    skillCD = 3;
                targets = new[] { currentTarget.transform };
                currentTarget.GetComponent<Unit>().attacksRandomAlly++;
                GameManager.gameManager.battleJournal.AddActionDescription(unitData.artisticName + " makes " + currentTarget.GetComponent<Unit>().unitData.artisticName + " attacks their ally for one turn.");
                break;
            case UnitType.sniper:
                if (skillCD == -1)
                    skillCD = 1;
                GameManager.gameManager.battleJournal.AddActionDescription(unitData.artisticName + " attack this turn will ignore armour.");
                currentTarget.GetComponent<Unit>().BeingAttacked(this, true);
                targets = new[] { currentTarget.transform };
                break;
            case UnitType.guardian:
                if (skillCD == -1)
                    skillCD = 4;
                targets = allies.Select(go => go.transform).ToArray();
                GameManager.gameManager.battleJournal.AddActionDescription(unitData.artisticName + " casts mass heal for 20% of the maximum health on all ally units.");
                foreach (var ally in allies)
                {
                    ally.GetComponent<Unit>().BeingHealed(0.15f, this);
                }
                break;
            case UnitType.tank:
                if (skillCD == -1)
                    skillCD = 3;
                GameManager.gameManager.battleJournal.AddActionDescription(unitData.artisticName + " taunts all enemy units until the next turn.");
                GameManager.gameManager.battleSystem.allyTauntCaster = gameObject;
                targets = new[] { transform };
                break;
            case UnitType.fortress:
                if (skillCD == -1)
                    skillCD = 3;
                GameManager.gameManager.battleJournal.AddActionDescription(unitData.artisticName + " armour up every ally unit for two turns.");
                foreach (var ally in allies)
                {
                    ally.GetComponent<Unit>().ArmourUp();
                    ally.GetComponent<Unit>().ArmourUp();
                }
                targets = allies.Select(go => go.transform).ToArray();
                break;
            case UnitType.berserker:
                if (skillCD == -1)
                    skillCD = 2;
                GameManager.gameManager.battleJournal.AddActionDescription(unitData.artisticName + " do mass attack against all enemies, but also hurting oneself.");
                foreach (var enemy in enemies)
                {
                    enemy.GetComponent<Unit>().BeingAttacked(this);
                }
                BeingAttacked(this);
                targets = enemies.Select(go => go.transform).Concat(new Transform[] { transform }).ToArray();
                break;
            case UnitType.savage:
                if (skillCD == -1)
                    skillCD = 4;
                GameManager.gameManager.battleJournal.AddActionDescription(unitData.artisticName + " applies 50% evasion buff on all ally units for one round.");
                foreach (var ally in allies)
                {
                    ally.GetComponent<Unit>().elusive++;
                }
                targets = allies.Select(go => go.transform).ToArray();
                break;
            case UnitType.fighter:
            default:
                break;
        }

        currentSkillCD = skillCD;
        skillUsedLastTurn = true;
        yield return GameManager.gameManager.battleSystem.AbilityIndicationAnimations(spriteIndication, targets);
    }

    private void BeingHealed(float fraction, Unit healer)
    {
        var healedAmount = (int) (fraction * unitData.maxHP);
        if (healedAmount + unitData.currentHP > unitData.maxHP)
            healedAmount = unitData.maxHP - unitData.currentHP;
        string actionDescription = "The " + healer.unitData.artisticName + " heals " + unitData.artisticName + " for " + 
                                   healedAmount + " health points.";
        GameManager.gameManager.battleJournal.AddActionDescription(actionDescription);
        ChangeCurrentHP(healedAmount);
    }

    public void SetData(JSONReader.UnitJSON ud)
    {
        unitData = ud;
        if (!ud.friendly)
        {
            gameObject.layer = 3;
        }
        else
        {
            gameObject.layer = 6;
        }
        unitType = (UnitType)Enum.Parse( typeof(UnitType), ud.unitType );
        abilitySprite = GameManager.gameManager.GetSpriteAbility(ud);
    }

    public void BeingAttacked(Unit attackingUnit, bool ignoreArmour = false)
    {
        if (elusive > 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                GameManager.gameManager.battleJournal.AddActionDescription("The " + attackingUnit.unitData.artisticName + " attempts to attack " +
                                                                           unitData.artisticName + " but misses.");
                ChangeCurrentHP(0);
                return;
            }
        }
        var receivedDamage = ignoreArmour ? attackingUnit.unitData.damage : CalculateDamage(attackingUnit, this);
        string actionDescription = "The " + attackingUnit.unitData.artisticName + " do " +
                                   receivedDamage + " damage to " +
                                   unitData.artisticName + ".";
        GameManager.gameManager.battleJournal.AddActionDescription(actionDescription);
        
        ChangeCurrentHP(-receivedDamage);

        if (unitData.friendly && unitType is UnitType.fighter)
        {
            var damageBack = CalculateDamage(this, attackingUnit);
            
            actionDescription = "The " + unitData.artisticName + " do " +
                                damageBack + " damage back to " +
                                       attackingUnit. unitData.artisticName + ".";
            GameManager.gameManager.battleJournal.AddActionDescription(actionDescription);
        
            attackingUnit.ChangeCurrentHP(-damageBack);
        }
    }

    private int CalculateDamage(Unit attackingUnit, Unit attackedUnit)
    {
        int currentArmour = attackedUnit.armouredUp > 0 ? attackedUnit.unitData.armour * 2 : attackedUnit.unitData.armour;
        int receivedDamage;
        if (attackedUnit.unitData.armour <= 0.5f * attackingUnit.unitData.damage)
            receivedDamage = attackingUnit.unitData.damage - currentArmour;
        else if (currentArmour >= 0.5f * attackingUnit.unitData.damage && currentArmour < attackingUnit.unitData.damage)
            receivedDamage = (int)(0.5f * attackingUnit.unitData.damage - 0.5f * (currentArmour - 0.5f * attackingUnit.unitData.damage) + 1);
        else 
            receivedDamage = (int)(0.25f * attackingUnit.unitData.damage) + 1;
        return receivedDamage;
    }
    public void ChangeCurrentHP(int hp)
    {
        unitData.currentHP += hp;
        GameEvents.gameEvents.UnitHPChanged(this);
        SpawnDamageText(hp);
        if (unitData.currentHP <= 0)
        {
            if (unitType is UnitType.tank)
            { 
                if (GameManager.gameManager.battleSystem.allyTauntCaster is not null && GameManager.gameManager.battleSystem.allyTauntCaster == gameObject)
                {
                    GameManager.gameManager.battleSystem.allyTauntCaster = null;
                }
            }
            GameManager.gameManager.battleJournal.AddActionDescription(unitData.artisticName + " perishes.");
            unitData.currentHP = 0;
            StartCoroutine(DeathAnimation(deathTime));
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
        isBeingDestroyed = true;
        gameObject.GetComponent<Collider2D>().enabled = false;
        var sprite = GetComponent<SpriteRenderer>();
        yield return new WaitForSeconds(duration / 2f);

        for (float t = 0; t < 1; t += Time.deltaTime/(duration/2f))
        {
            sprite.color = new Color(1, 1, 1, Mathf.SmoothStep(1, 0, t));
            yield return null;
        }
        GameEvents.gameEvents.UnitKilled(this);
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }

    public void SkipTurn()
    {
        stunned--;
        GameManager.gameManager.battleJournal.AddActionDescription(unitData.artisticName + " is being stunned for this turn.");
    }
}
