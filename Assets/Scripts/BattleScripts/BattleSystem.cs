using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// Script that manages the Battle. Contains data about the Units participating in a Battle and references to UI elements.
/// </summary>
public class BattleSystem : MonoBehaviour
{
    /// <summary>
    /// State of the current Battle.
    /// </summary>
    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOST }

    private enum ActionState { ATTACK, DEFEND, ABILITY, NONE }
    
    private ActionState _actionState = ActionState.NONE;
    
    private BattleState _battleState;

    private bool waitingForEnemyToSelect = false;
    
    private bool waitingForAllyToSelect = false;

    private int _remainingAllies;

    /// <summary>
    /// If not null, contains the Unit Object that currently forcing enemies to attack it.
    /// </summary>
    [HideInInspector]
    [UnityEngine.Tooltip("If not null, contains the Unit Object that currently forcing enemies to attack it.")]
    public GameObject allyTauntCaster = null;

    private int _remainingEnemies;

    private int _currentAllyIndex;
    
    private int _currentEnemyIndex = -1;

    private GameObject _currentTarget;
    /// <summary>
    /// Returns currently selected target.
    /// </summary>
    public GameObject GetCurrentTarget() { return _currentTarget;}
    
    private GameObject _currentEnemy;
    
    [SerializeField]
    private InfoHUD infoHUD;

    private int roundIndex = 1;

    private GameObject _currentAlly;

    private List<GameObject> _allies = new List<GameObject>();
    
    /// <summary>
    /// Get the list of ally Units.
    /// </summary>
    public List<GameObject> GetAllies() { return _allies;}
    
    private List<GameObject> _enemies = new List<GameObject>();    
    
    /// <summary>
    /// Get the list of enemy Units.
    /// </summary>
    public List<GameObject> GetEnemies() { return _enemies;}
    
    private IEnumerator _action;
    
    [SerializeField] private Button attackButton;
    
    [SerializeField] private Button defendButton;
    
    [SerializeField] private Button abilityButton;
    
    [SerializeField]
    private GridSystem gridSystem;

    [SerializeField]
    private Camera _camera;
    
    
    /// <summary>
    /// Sound of the current Unit Class Ability.
    /// </summary>
    [HideInInspector]
    [UnityEngine.Tooltip("Sound of the current Unit Class Ability.")]
    public Sound CurrentSound;

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
    
    /// <summary>
    /// Called when attack button is pressed. Waits for the target to be attacked. Assigns the <see cref="PlayerAttack"/> as an <see cref="_action"/>.
    /// </summary>
    public void OnAttackButtonPressed()
    {
        AudioManager.instance.Play(GameManager.gameManager.ButtonClick);
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
    /// <summary>
    /// Called when defend button is pressed. Assigns the <see cref="PlayerDefend"/> as an <see cref="_action"/>.
    /// </summary>
    public void OnDefendButtonPressed()
    {
        AudioManager.instance.Play(GameManager.gameManager.ButtonClick);
        if (_battleState != BattleState.PLAYERTURN  || _actionState == ActionState.DEFEND)
            return;
        StopSelectingTarget();

        _action = PlayerDefend();
        _actionState = ActionState.DEFEND;
        
        DisableInput();

        StartCoroutine(_action);
    }
    /// <summary>
    /// Called when defend button is pressed. Assigns the <see cref="PlayerAbility"/> as an <see cref="_action"/>.
    /// </summary>
    public void OnAbilityButtonPressed()
    {
        AudioManager.instance.Play(GameManager.gameManager.ButtonClick);
        if (_battleState != BattleState.PLAYERTURN  || _actionState == ActionState.ABILITY)
            return;
        StopSelectingTarget();
        var currentAllyUnit = _currentAlly.GetComponent<Unit>();
        _action = PlayerAbility();
        _actionState = ActionState.ABILITY;

        switch (currentAllyUnit.unitType)
        {
            case Unit.UnitType.sorcerer:
                CurrentSound = GameManager.gameManager.SorcererSound;
                waitingForEnemyToSelect = true;
                break;
            case Unit.UnitType.healer:
                CurrentSound = GameManager.gameManager.HealerSound;
                waitingForAllyToSelect = true;
                break;
            case Unit.UnitType.trickster:
                CurrentSound = GameManager.gameManager.TricksterSound;
                waitingForEnemyToSelect = true;
                break;
            case Unit.UnitType.marksman:
                waitingForEnemyToSelect = true;
                CurrentSound = GameManager.gameManager.MarksmanSound;
                break;
            case Unit.UnitType.fighter:
            case Unit.UnitType.paladin:
                DisableInput();
                CurrentSound = GameManager.gameManager.PaladinSound;
                StartCoroutine(_action);
                break;
            case Unit.UnitType.protector:
                DisableInput();
                CurrentSound = GameManager.gameManager.ProtectorSound;
                StartCoroutine(_action);
                break;
            case Unit.UnitType.bastion:
                DisableInput();
                CurrentSound = GameManager.gameManager.BastionSound;
                StartCoroutine(_action);
                break;
            case Unit.UnitType.berserker:
                DisableInput();
                CurrentSound = GameManager.gameManager.BerserkerSound;
                StartCoroutine(_action);
                break;
            case Unit.UnitType.shaman:
                DisableInput();
                CurrentSound = GameManager.gameManager.ShamanSound;
                StartCoroutine(_action);
                break;
            default:
                break;
        }
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
    


    private IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(1.5f);
        Attack(_currentAlly, _currentTarget);
        CurrentSound = GameManager.gameManager.AttackSound;
        yield return AbilityIndicationAnimations(GameManager.gameManager.attackSprite ,new []{_currentTarget.transform});
        yield return PassTurnToEnemy();
        _action = null;
    }
    
    private IEnumerator PlayerDefend()
    {
        yield return new WaitForSeconds(1.5f);
        Defend(_currentAlly);
        CurrentSound = GameManager.gameManager.DefendSound;
        yield return AbilityIndicationAnimations(GameManager.gameManager.armouredSprite, new []{_currentAlly.transform});
        yield return PassTurnToEnemy();
        _action = null;

    }
    private IEnumerator PlayerAbility()
    {
        yield return new WaitForSeconds(1.5f);
        var currentAllyUnit = _currentAlly.GetComponent<Unit>();
        yield return currentAllyUnit.UnitAbility();
        yield return PassTurnToEnemy();
        _action = null;

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
    
    private IEnumerator EnemyAttackAnimation(GameObject attacking, GameObject attacked)
    {
        GameManager.gameManager.OutlineObject(attacked, true);
        CurrentSound = GameManager.gameManager.AttackSound;
        yield return AbilityIndicationAnimations(GameManager.gameManager.attackSprite , new []{attacked.transform});
        Attack(attacking, attacked);
        GameManager.gameManager.OutlineObject(attacked, false);
    }
    /// <summary>
    /// Sets up the Ability Indication for the used Class Ability.
    /// </summary>
    /// <param name="sprite">Sprite for the <see cref="AbilityIndication"/>.</param>
    /// <param name="targets">Targets for the current Unit Ability.</param>
    public IEnumerator AbilityIndicationAnimations(Sprite sprite ,Transform[] targets)
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
        AudioManager.instance.Play(CurrentSound);
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
        else if (currentEnemyunit.attacksRandomAlly > 0 && _enemies.Count > 1)
        {
            
            int attackedEnemyIndex;
            do
            {
                attackedEnemyIndex = Random.Range(0, _enemies.Count);
            } while (attackedEnemyIndex == _currentEnemyIndex);

            var attackedEnemy = _enemies[attackedEnemyIndex];
            yield return EnemyAttackAnimation(_currentEnemy, attackedEnemy);
            currentEnemyunit.attacksRandomAlly--;
            
        }
        else if (allyTauntCaster is not null && !allyTauntCaster.Equals(null))
        {
            yield return EnemyAttackAnimation(_currentEnemy, allyTauntCaster);
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
            yield return EnemyAttackAnimation(_currentEnemy, attackedAlly);
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
            GameManager.gameManager.OutlineObject(_currentTarget, false);
        }

        waitingForAllyToSelect = false;
        waitingForEnemyToSelect = false;
        _currentTarget = target;
        GameManager.gameManager.OutlineObject(_currentTarget, true);
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

    /// <summary>
    /// Disables all Unit Buttons.
    /// </summary>
    public void DisableInput()
    {
        attackButton.interactable = false;
        defendButton.interactable = false;
        abilityButton.interactable = false;
    }
    /// <summary>
    /// Enables all Unit Buttons.
    /// </summary>
    public void EnableInput()
    {
        attackButton.interactable = true;
        defendButton.interactable = true;
        abilityButton.interactable = _currentAlly.GetComponent<Unit>().unitType != Unit.UnitType.fighter;

    }
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
                                if (GameManager.gameManager == null || GameManager.gameManager.Equals(null) || _currentAlly == null || _currentAlly.Equals(null))
                                {
                                    return;
                                }
                                var currentClass = GameManager.gameManager.classesDescriptions[_currentAlly.GetComponent<Unit>().unitType];
                                obj.firstRow = currentClass.className;
                                obj.secondRow = currentClass.abilityDescription;
                                obj.thirdRow = currentClass.abilityCooldown;
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
                            DisableInput();
                            StartCoroutine(_action);
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
                            DisableInput();
                            StartCoroutine(_action);
                            break; 
                        }
                    }
                }
            }
    }
}
