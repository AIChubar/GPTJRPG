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
    
    private int _currentEnemyIndex = -1; //

    private GameObject _currentEnemy;
    
    [SerializeField]
    private UnitHUD _infoHUD;

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
        _infoHUD.UpdateHUD();
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
            _action = Attack();
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
        
        completeActionButton.interactable = true;
        attackButton.interactable = true;
        GameManager.gameManager.OutlineObject(_currentAlly, true);
        
    }

    public void OnCompleteActionButtonPressed()
    {
        if (!_selectingTarget || _currentEnemy == null || _battleState != BattleState.PLAYERTURN)
            return;
        
        attackButton.interactable = false;
        completeActionButton.interactable = false;

        StartCoroutine(_action);
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(1);
        _battleState = BattleState.ENEMYTURN;

        _currentEnemy.GetComponent<Unit>().ChangeCurrentHP(-_currentAlly.GetComponent<Unit>().unitData.damage);

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
        
        int attackedAllyIndex = Random.Range(0, _allies.Count - 1);

        var attackedAlly = _allies[attackedAllyIndex];
        
        GameManager.gameManager.OutlineObject(attackedAlly, true);
        
        yield return new WaitForSeconds(0.5f);
        _battleState = BattleState.PLAYERTURN;

        attackedAlly.GetComponent<Unit>().ChangeCurrentHP(-_currentEnemy.GetComponent<Unit>().unitData.damage);

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
 
    // Update is called once per frame
    void Update()
    {
            if( Input.GetMouseButtonDown(1)){
                Ray ray = _camera.ScreenPointToRay( Input.mousePosition );
                RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll( ray );
                bool gotTarget = false;  
                foreach( RaycastHit2D hit in hits ){  
                    if( hit.transform.gameObject.TryGetComponent(out Unit unit) ) {  
                        _infoHUD.SetHUD(unit.unitData);   
                        gotTarget = true;  
                        break;  
                    }
                }
                if( !gotTarget ) _infoHUD.SetHUD(null);  
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
                    //if( !gotTarget ) StopSelectingTarget(); //If we missed everything, deselect
                }
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
    }
}
