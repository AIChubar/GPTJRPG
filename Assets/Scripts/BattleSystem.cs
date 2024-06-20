using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BattleSystem : MonoBehaviour
{
    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOST }
    
    public enum ActionState { ATTACK, DEFEND, ABILITY, NONE }
    
    private ActionState _actionState = ActionState.NONE;
    
    private BattleState _battleState;

    private bool waitingForEnemyToSelect = false;
    
    private bool waitingForAllyToSelect = false;

    private int _remainingAllies;

    [HideInInspector]
    public GameObject allyTauntCaster = null;

    private int _remainingEnemies;

    private int _currentAllyIndex;
    
    private int _currentEnemyIndex = -1;

    private GameObject _currentTarget;

    public GameObject GetCurrentTarget() { return _currentTarget;}
    
    private GameObject _currentEnemy;
    
    [SerializeField]
    private InfoHUD infoHUD;

    private int roundIndex = 1;

    private GameObject _currentAlly;

    private List<GameObject> _allies = new List<GameObject>();
    
    public List<GameObject> GetAllies() { return _allies;}
    
    private List<GameObject> _enemies = new List<GameObject>();    
    
    public List<GameObject> GetEnemies() { return _enemies;}
    
    private IEnumerator _action;
    
    [SerializeField] private Button completeActionButton;

    [SerializeField] private Button attackButton;
    
    [SerializeField] private Button defendButton;
    
    [SerializeField] private Button abilityButton;

    [SerializeField]
    private GridSystem gridSystem;

    [SerializeField]
    private Camera _camera;

    private void Start()
    {
        GameManager.gameManager.battleSystem = this;
        GameEvents.gameEvents.OnUnitHPChanged += GameEvents_OnUnitHPChanged;
        GameEvents.gameEvents.OnUnitKilled += GameEvents_OnUnitKilled;
        _battleState = BattleState.START;
        
        foreach (var allyCell in gridSystem.allyGridObjects)
        {
            if (allyCell.objectHolding is not null && !allyCell.objectHolding.Equals(null))
            {
                _allies.Add(allyCell.objectHolding);
            }
        }

        foreach (var enemyCell in gridSystem.enemyGridObjects)
        {
            if (enemyCell.objectHolding is not null && !enemyCell.objectHolding.Equals(null))
            {
                _enemies.Add(enemyCell.objectHolding);
            }
        }
        abilityButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = _allies[0].GetComponent<Unit>().abilitySprite;
        _remainingAllies = _allies.Count;
        _remainingEnemies = _enemies.Count;
        StartCoroutine(BeginBattle());
    }

    private void GameEvents_OnUnitHPChanged(Unit unit)
    {
        /*if (unit.unitData.currentHP <= 0)
            infoHUD.SetHUD(null);
        else
            infoHUD.UpdateHUD();*/
    }
    
    private void GameEvents_OnUnitKilled(Unit unit)
    {
        
    }

    private bool CheckIfBattleEnds()
    {
        for (int j = _allies.Count - 1; j >= 0; j--)
        {
            if (!_allies[j] || _allies[j].IsDestroyed() || _allies[j].GetComponent<Unit>().isBeingDestroyed)
            {
                _allies.RemoveAt(j);
                _remainingAllies--;
                GameManager.gameManager.hero.allyGroup.units.RemoveAt(j);
            }
        }

        for (int j = _enemies.Count - 1; j >= 0; j--)
        {
            if (!_enemies[j] || _enemies[j].IsDestroyed() || _enemies[j].GetComponent<Unit>().isBeingDestroyed)
            {
                _enemies.RemoveAt(j);
                _remainingEnemies--;
            }
        }

        if (_remainingEnemies <= 0 )
        {
            _battleState = BattleState.WIN;
            GameEvents.gameEvents.OnUnitHPChanged -= GameEvents_OnUnitHPChanged;
            GameEvents.gameEvents.OnUnitKilled -= GameEvents_OnUnitKilled;
            GameManager.gameManager.battleResult = _battleState;
            SceneController.UnloadScene(2, 0.5f, 0.5f, 0.5f);
            return true;
        }
        else if (_remainingAllies <= 0)
        {
            _battleState = BattleState.LOST;
            GameEvents.gameEvents.OnUnitHPChanged -= GameEvents_OnUnitHPChanged;
            GameEvents.gameEvents.OnUnitKilled -= GameEvents_OnUnitKilled;
            GameManager.gameManager.battleResult = _battleState;
            SceneController.UnloadScene(2, 0.5f, 0.5f, 0.5f);
            return true;
        }

        return false;
    }


    private IEnumerator BeginBattle()
    {
        yield return new WaitForSeconds(1);
        GameManager.gameManager.battleJournal.AddActionDescription("Combat start.\n");
        
        _battleState = BattleState.PLAYERTURN;
        _currentAllyIndex = 0;
        _currentAlly = _allies[_currentAllyIndex];
        GameManager.gameManager.battleJournal.AddActionDescription("Round " + roundIndex  + ".\n");
        GameManager.gameManager.OutlineObject(_currentAlly, true);
        _currentAlly.GetComponent<Unit>().TurnStartUpdate(abilityButton.gameObject);

    }
    
    public void OnAttackButtonPressed()
    {
        if (_battleState != BattleState.PLAYERTURN || _actionState == ActionState.ATTACK)
            return;
        StopSelectingTarget();
        waitingForEnemyToSelect = true;
        if (_actionState != ActionState.ATTACK)
        {
            _actionState = ActionState.ATTACK;
            _action = PlayerAttack();
        }
    }
    
    public void OnDefendButtonPressed()
    {
        if (_battleState != BattleState.PLAYERTURN  || _actionState == ActionState.DEFEND)
            return;
        StopSelectingTarget();

        _action = PlayerDefend();
        _actionState = ActionState.DEFEND;
    }
    
    public void OnAbilityButtonPressed()
    {
        if (_battleState != BattleState.PLAYERTURN  || _actionState == ActionState.ABILITY)
            return;
        StopSelectingTarget();
        var currentAllyUnit = _currentAlly.GetComponent<Unit>();
        switch (currentAllyUnit.unitType)
        {
            case Unit.UnitType.striker:
                waitingForEnemyToSelect = true;
                break;
            case Unit.UnitType.balancer:
                waitingForAllyToSelect = true;
                break;
            case Unit.UnitType.trickster:
                waitingForEnemyToSelect = true;
                break;
            case Unit.UnitType.sniper:
                waitingForEnemyToSelect = true;
                break;
            case Unit.UnitType.fighter:
            case Unit.UnitType.guardian:
            case Unit.UnitType.tank:
            case Unit.UnitType.fortress:
            case Unit.UnitType.berserker:
            case Unit.UnitType.savage:
            default:
                break;
        }
        _action = PlayerAbility();
        _actionState = ActionState.ABILITY;
    }

    private void SelectNextAllyUnit()
    {
        if (_battleState != BattleState.PLAYERTURN)
            return;
        if (_currentAlly  is not null && !_currentAlly.Equals(null))
        {
            GameManager.gameManager.OutlineObject(_currentAlly, false);
            
        }

        int initialAllyIndex = _currentAllyIndex;
        
        
        if (CheckIfBattleEnds())
            return;
        int i = 0;
        do
        {
            i++;
            _currentAllyIndex = (_currentAllyIndex + 1) % (_allies.Count);
            _currentAlly = _allies[_currentAllyIndex];
            if (i >= 5)
            {
                return;
            }
        } while (_currentAlly is null || _currentAlly.Equals(null) ||_currentAlly.GetComponent<Unit>().isBeingDestroyed);

        if (initialAllyIndex > _currentAllyIndex)
        {
            roundIndex++;
            GameManager.gameManager.battleJournal.AddActionDescription("\n" + "Round " + roundIndex  + ".\n");
        }
        
        EnableInput();
        GameManager.gameManager.OutlineObject(_currentAlly, true);
        _currentAlly.GetComponent<Unit>().TurnStartUpdate(abilityButton.gameObject);
    }
    

    public void OnCompleteActionButtonPressed()
    {
        if (_actionState == ActionState.NONE || ((waitingForEnemyToSelect || waitingForAllyToSelect) && (_currentTarget is null || _currentTarget.Equals(null)) || _battleState != BattleState.PLAYERTURN))
            return;
        DisableInput();

        StartCoroutine(_action);
    }

    private IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(1f);
        Attack(_currentAlly, _currentTarget);
        yield return PassTurnToEnemy();
    }
    
    private IEnumerator PlayerDefend()
    {
        yield return new WaitForSeconds(1f);
        Defend(_currentAlly);
        
        yield return AbilityIndicationAnimations(GameManager.gameManager.armouredSprite, new []{_currentAlly.transform});
        yield return PassTurnToEnemy();

    }
    private IEnumerator PlayerAbility()
    {
        yield return new WaitForSeconds(1f);
        var currentAllyUnit = _currentAlly.GetComponent<Unit>();
        yield return currentAllyUnit.UnitAbility();
        yield return PassTurnToEnemy();

    }

    private IEnumerator PassTurnToEnemy()
    {
        _battleState = BattleState.ENEMYTURN;
        yield return new WaitForSeconds(0.5f);
        GameManager.gameManager.OutlineObject(_currentAlly, false);
        StopSelectingTarget();
        yield return new WaitForSeconds(1.0f);
        EnemyAction();
    }
    public IEnumerator AbilityIndicationAnimations(Sprite sprite, Transform[] targets)
    {
        List<AbilityIndication> abilityIndications = new List<AbilityIndication>();
        foreach (var target in targets)
        {
            var abilityIndication = Instantiate(GameManager.gameManager.abilityIndicationPrefab,
                target.position, Quaternion.identity, target).GetComponent<AbilityIndication>();
            abilityIndication.SetAbilityIndication(sprite);
            abilityIndication.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
            abilityIndications.Add(abilityIndication);
        }
        yield return new WaitForSeconds(0.2f);

        for (float t = 0; t < 1; t += Time.deltaTime / 0.5f)
        {
            foreach (var ai in abilityIndications)
            {
                ai.SetVisibility(Mathf.SmoothStep(0, 1, t));
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        for (float t = 0; t < 1; t += Time.deltaTime / 0.5f)
        {
            foreach (var ai in abilityIndications)
            {
                ai.SetVisibility(Mathf.SmoothStep(1, 0, t));
            }
            yield return null;
        }

        foreach (var ai in abilityIndications)
        {
            Destroy(ai.gameObject);
        }
        yield return new WaitForSeconds(0.5f);
    }
    
    
    private IEnumerator EnemyAttack()
    {
        
        GameManager.gameManager.OutlineObject(_currentEnemy, true);
        
        yield return new WaitForSeconds(0.6f);


        var currentEnemyunit = _currentEnemy.GetComponent<Unit>();
        
        if (currentEnemyunit.stunned > 0)
        {
            currentEnemyunit.SkipTurn();
        }
        else if (currentEnemyunit.attacksRandomAlly > 0)
        {
            
            int attackedEnemyIndex;
            do
            {
                attackedEnemyIndex = Random.Range(0, _enemies.Count);
            } while (attackedEnemyIndex == _currentEnemyIndex);

            var attackedEnemy = _enemies[attackedEnemyIndex];
            GameManager.gameManager.OutlineObject(attackedEnemy, true);
            yield return new WaitForSeconds(0.5f);
            Attack(_currentEnemy, attackedEnemy);
            GameManager.gameManager.OutlineObject(attackedEnemy, false);
            currentEnemyunit.attacksRandomAlly--;
            
        }
        else if (allyTauntCaster is not null && !allyTauntCaster.Equals(null))
        {
            GameManager.gameManager.OutlineObject(allyTauntCaster, true);
            yield return new WaitForSeconds(0.5f);
            Attack(_currentEnemy, allyTauntCaster);
            GameManager.gameManager.OutlineObject(allyTauntCaster, false);
        }
        else
        {
            for (int i = 0; i < _allies.Count; i++)
            {
                if (!_allies[i] || _allies[i].IsDestroyed() || _allies[i].GetComponent<Unit>().isBeingDestroyed)
                    _allies.RemoveAt(i);
            }
        
            int attackedAllyIndex = Random.Range(0, _allies.Count);
            
            var attackedAlly = _allies[attackedAllyIndex];
            GameManager.gameManager.OutlineObject(attackedAlly, true);
            yield return new WaitForSeconds(0.5f);
            Attack(_currentEnemy, attackedAlly);
            GameManager.gameManager.OutlineObject(attackedAlly, false);
        }
        _battleState = BattleState.PLAYERTURN;
        GameManager.gameManager.OutlineObject(_currentEnemy, false);
        StopSelectingTarget();
        
        yield return new WaitForSeconds(1.5f);
         
        SelectNextAllyUnit();
        
    }

    private void StopSelectingTarget()
    {
        waitingForEnemyToSelect = false;
        waitingForAllyToSelect = false;
        _actionState = ActionState.NONE;
        if (_currentTarget is null || _currentTarget.Equals(null))
            return;
        GameManager.gameManager.OutlineObject(_currentTarget, false);
        _currentTarget = null;
    }
        
    private void SelectNewTarget(GameObject target)
    {
        if (_currentTarget is not null && !_currentTarget.Equals(null))
        {
            GameManager.gameManager.OutlineObject(target, false);
        }

        _currentTarget = target;
        GameManager.gameManager.OutlineObject(target, true);
    }

    private void Attack(GameObject attacking, GameObject attacked)
    {
        var attackingUnit = attacking.GetComponent<Unit>();
        var attackedUnit = attacked.GetComponent<Unit>();
        attackedUnit.BeingAttacked(attackingUnit);
        
    }

    private void Defend(GameObject defending)
    {
        var defendingUnit = defending.GetComponent<Unit>();
        defendingUnit.ArmourUp();
    }
    private void EnemyAction()
    {
        if (_battleState != BattleState.ENEMYTURN)
            return;
        
        
        if (CheckIfBattleEnds())
            return;
        int i = 0;
        do
        {
            i++;
            _currentEnemyIndex = (_currentEnemyIndex + 1) % (_enemies.Count);

            _currentEnemy = _enemies[_currentEnemyIndex];
            if (i >= 5)
            {
                return;
            }
        } while (_currentEnemy.IsDestroyed());


        StartCoroutine(EnemyAttack());
    }

    public void DisableInput()
    {
        attackButton.interactable = false;
        completeActionButton.interactable = false;
        defendButton.interactable = false;
        abilityButton.interactable = false;
    }
    public void EnableInput()
    {
        attackButton.interactable = true;
        completeActionButton.interactable = true;
        defendButton.interactable = true;
        abilityButton.interactable = true;

    }
    // Update is called once per frame
    void Update()
    {
            if( !GameManager.gameManager.transitioning){
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                eventData.position =  Input.mousePosition;
                List<RaycastResult> uiRaycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll( eventData, uiRaycastResults );
                
                Ray ray = _camera.ScreenPointToRay( Input.mousePosition );
                RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll( ray );
                bool gotTarget = false;  
                foreach( RaycastHit2D hit in hits ){  
                    if( hit.transform.gameObject.TryGetComponent(out Unit unit) )
                    {
                        if (unit.isBeingDestroyed)
                            break; 
                        infoHUD.SetHUD(unit.unitData, unit.GetComponent<SpriteRenderer>().sprite);   
                        gotTarget = true;  
                        break;  
                    }
                }
                if (!gotTarget)
                    foreach (var result in uiRaycastResults)
                    {
                        if (result.gameObject.transform.gameObject.TryGetComponent(out ObjectForInfoHUD obj))
                        {
                            if (obj.abilityButton)
                            {
                                var currentClass = GameManager.gameManager.classesDescriptions[_currentAlly.GetComponent<Unit>().unitType];
                                obj.headerRow = currentClass.className;
                                obj.firstRow = currentClass.abilityDescription;
                                obj.secondRow = currentClass.abilityCooldown;
                            }
                            infoHUD.SetHUD(obj);  
                            gotTarget = true;  
                            break;  
                        }
                    }
                if( !gotTarget ) infoHUD.ResetHUD();  
            }
            if (_actionState == ActionState.ATTACK)
            {
                if (EventSystem.current.currentSelectedGameObject != attackButton.gameObject)
                    EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
            }
            else if (_actionState == ActionState.DEFEND)
            {
                if (EventSystem.current.currentSelectedGameObject != defendButton.gameObject)
                    EventSystem.current.SetSelectedGameObject(defendButton.gameObject);
            }
            else if (_actionState == ActionState.ABILITY)
            {
                if (EventSystem.current.currentSelectedGameObject != abilityButton.gameObject)
                    EventSystem.current.SetSelectedGameObject(abilityButton.gameObject);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
            }

            if (waitingForEnemyToSelect)
            {
                Ray ray = _camera.ScreenPointToRay( Input.mousePosition );
                if( Input.GetMouseButtonDown(0)){
                    RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll( ray );
                    //bool gotTarget = false;  
                    foreach( RaycastHit2D hit in hits ){ 
                        if( hit.transform.gameObject.layer == 3 && !hit.transform.gameObject.GetComponent<Unit>().isBeingDestroyed) {
                            SelectNewTarget(hit.transform.gameObject);    
                            break; 
                        }
                    }
                }
            }
            else if (waitingForAllyToSelect)
            {
                Ray ray = _camera.ScreenPointToRay( Input.mousePosition );
                if( Input.GetMouseButtonDown(0)){
                    RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll( ray );
                    //bool gotTarget = false;  
                    foreach( RaycastHit2D hit in hits ){ 
                        if( hit.transform.gameObject.layer == 6 && !hit.transform.gameObject.GetComponent<Unit>().isBeingDestroyed) {
                            SelectNewTarget(hit.transform.gameObject);    
                            break; 
                        }
                    }
                }
            }
    }
}
