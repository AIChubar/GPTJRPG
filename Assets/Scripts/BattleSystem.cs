using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleSystem : MonoBehaviour
{
    public enum BattleState { START, PLAYERTURN, ENEMYTURN, WIN, LOST }
    
    private BattleState _battleState;

    private bool _selectingTarget = false;

    private int _remainingAllies;

    private int _remainingEnemies;

    private int _currentAllyIndex;
    
    private int _currentEnemyIndex = -1;

    private GameObject _currentEnemy;
    
    [SerializeField]
    private UnitHUD infoHUD;
    
    [SerializeField]
    private BattleJournal battleJournal;

    private GameObject _currentAlly;

    private List<GameObject> _allies = new List<GameObject>();
    
    private List<GameObject> _enemies = new List<GameObject>();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
    
    private IEnumerator _action;
    
    [SerializeField] private Button completeActionButton;

    [SerializeField] private Button attackButton;

    [SerializeField]
    private GridSystem gridSystem;

    [SerializeField]
    private Camera _camera;

    private void Start()
    {
        GameEvents.gameEvents.OnUnitHPChanged += GameEvents_OnUnitHPChanged;
        GameEvents.gameEvents.OnUnitKilled += GameEvents_OnUnitKilled;
        _battleState = BattleState.START;

        foreach (var allyCell in gridSystem.allyGridObjects)
        {
            if (allyCell.objectHolding != null)
            {
                _allies.Add(allyCell.objectHolding);
            }
        }

        foreach (var enemyCell in gridSystem.enemyGridObjects)
        {
            if (enemyCell.objectHolding != null)
            {
                _enemies.Add(enemyCell.objectHolding);
            }
        }

        _remainingAllies = _allies.Count;
        _remainingEnemies = _enemies.Count;
        StartCoroutine(BeginBattle());
    }

    private void GameEvents_OnUnitHPChanged(Unit unit)
    {
        if (unit.unitData.currentHP <= 0)
            infoHUD.SetHUD(null);
        else
            infoHUD.UpdateHUD();
    }
    
    private void GameEvents_OnUnitKilled(Unit unit)
    {
        
        if (unit.unitData.friendly)
        {
            _remainingAllies--;
        }
        else
        {
            _remainingEnemies--;
        }

        if (_remainingEnemies <= 0 )
        {
            _battleState = BattleState.WIN;
            GameEvents.gameEvents.OnUnitHPChanged -= GameEvents_OnUnitHPChanged;
            GameEvents.gameEvents.OnUnitKilled -= GameEvents_OnUnitKilled;
            GameManager.gameManager.battleResult = _battleState;
            SceneController.UnloadScene(2, 0.5f, 0.5f, 0.5f);
        }
        else if (_remainingAllies <= 0)
        {
            _battleState = BattleState.LOST;
            GameEvents.gameEvents.OnUnitHPChanged -= GameEvents_OnUnitHPChanged;
            GameEvents.gameEvents.OnUnitKilled -= GameEvents_OnUnitKilled;
            GameManager.gameManager.battleResult = _battleState;
            SceneController.UnloadScene(2, 0.5f, 0.5f, 0.5f);
        }
    }

    private void CheckIfBattleEnds(bool friendly)
    {
        
    }

    private IEnumerator BeginBattle()
    {
        battleJournal.AddActionDescription("Combat start.\n");
        yield return new WaitForSeconds(1);
        _battleState = BattleState.PLAYERTURN;
         
        _currentAlly = _allies[0];
        GameManager.gameManager.OutlineObject(_currentAlly, true);
    }
    
    public void OnAttackButtonPressed()
    {
        if (_battleState != BattleState.PLAYERTURN)
            return;

        if (!_selectingTarget)
        {
            GameObject firstSelectedEnemy = null;
            foreach (var enemy in gridSystem.enemyGridObjects)
            {
                if (enemy.objectHolding != null)
                {
                    firstSelectedEnemy = enemy.objectHolding;
                    break;
                }
            }
            if (firstSelectedEnemy == null)
                return;
            _selectingTarget = true;
            _action = PlayerAttack();
        }
    }

    private void SelectNextAllyUnit()
    {
        if (_battleState != BattleState.PLAYERTURN)
            return;
        
        if (_currentAlly  != null)
        {
            GameManager.gameManager.OutlineObject(_currentAlly, false);
        }

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
        } while (_currentAlly == null);
        
        EnableInput();
        GameManager.gameManager.OutlineObject(_currentAlly, true);
        
    }

    public void OnCompleteActionButtonPressed()
    {
        if (!_selectingTarget || _currentEnemy == null || _battleState != BattleState.PLAYERTURN)
            return;
        
        DisableInput();

        StartCoroutine(_action);
    }

    private IEnumerator PlayerAttack()
    {
        yield return new WaitForSeconds(1);
        _battleState = BattleState.ENEMYTURN;
        Attack(_currentAlly, _currentEnemy);

        yield return new WaitForSeconds(0.5f);
        GameManager.gameManager.OutlineObject(_currentAlly, false);
        StopSelectingTarget();
        
        yield return new WaitForSeconds(1f);

        EnemyAction();
    }
    private IEnumerator EnemyAttack()
    {
        
        GameManager.gameManager.OutlineObject(_currentEnemy, true);
        
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < _allies.Count; i++)
        {
            if (!_allies[i])
                _allies.RemoveAt(i);
        }
        
        int attackedAllyIndex = Random.Range(0, _allies.Count);

        var attackedAlly = _allies[attackedAllyIndex];
        
        GameManager.gameManager.OutlineObject(attackedAlly, true);
        
        yield return new WaitForSeconds(0.5f);
        _battleState = BattleState.PLAYERTURN;
        Attack(_currentEnemy, attackedAlly);
        
        
        
        GameManager.gameManager.OutlineObject(attackedAlly, false);
         
        StopSelectingTarget();
        
        yield return new WaitForSeconds(1f);
         
        SelectNextAllyUnit();
        
    }

    private void StopSelectingTarget()
    {
        if (_currentEnemy == null)
            return;
        GameManager.gameManager.OutlineObject(_currentEnemy, false);
        _selectingTarget = false;
        _currentEnemy = null;
    }
        
    private void SelectNewEnemy(GameObject enemy)
    {
        if (_currentEnemy != null)
        {
            GameManager.gameManager.OutlineObject(_currentEnemy, false);
        }

        _currentEnemy = enemy;
        GameManager.gameManager.OutlineObject(_currentEnemy, true);
    }

    private void Attack(GameObject attacking, GameObject attacked)
    {
        var attackingUnit = attacking.GetComponent<Unit>();
        var attackedUnit = attacked.GetComponent<Unit>();
        bool killed = attackedUnit.ChangeCurrentHP(-attackingUnit.unitData.damage);
        string actionDescription = "The " + attackingUnit.unitData.artisticName + " do " +
                                   attackingUnit.unitData.damage.ToString() + " damage to " +
                                   attackedUnit.unitData.artisticName + ".";
        battleJournal.AddActionDescription(actionDescription);
        if (killed)
            battleJournal.AddActionDescription(attackedUnit.unitData.artisticName + " perishes.");
    }


    private void EnemyAction()
    {
        if (_battleState != BattleState.ENEMYTURN)
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
    }
    public void EnableInput()
    {
        attackButton.interactable = true;
        completeActionButton.interactable = true;
    }
    // Update is called once per frame
    void Update()
    {
            if( Input.GetMouseButtonDown(1)){
                Ray ray = _camera.ScreenPointToRay( Input.mousePosition );
                RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll( ray );
                bool gotTarget = false;  
                foreach( RaycastHit2D hit in hits ){  
                    if( hit.transform.gameObject.TryGetComponent(out Unit unit) ) {  
                        infoHUD.SetHUD(unit.unitData, unit.GetComponent<SpriteRenderer>().sprite);   
                        gotTarget = true;  
                        break;  
                    }
                }
                if( !gotTarget ) infoHUD.SetHUD(null);  
            }
            if (_selectingTarget)
            {
                if (EventSystem.current.currentSelectedGameObject != attackButton.gameObject)
                    EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
            
                Ray ray = _camera.ScreenPointToRay( Input.mousePosition );
                if( Input.GetMouseButtonDown(0)){
                    RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll( ray );
                    //bool gotTarget = false;  
                    foreach( RaycastHit2D hit in hits ){ 
                        if( hit.transform.gameObject.layer == 3 ) {
                            SelectNewEnemy(hit.transform.gameObject);    
                            //gotTarget = true; 
                            break; 
                        }
                    }
                }
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
    }
}
