using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.UI;
using TMPro;


public class BattleManager : MonoBehaviour
{
    //all units in the fight
    public List<unit> fighters = new List<unit>();
    // list of possable enemy incounters
    public List<Enemy[]> encounters = new List<Enemy[]>();
    //all enemies in the fight, max of 5
    public Enemy[] enemies = new Enemy[5];
    //sets what all fighters action speed starts at
    public int defaultActionSpeed = 1000;
    //unit currently acting
    public unit currentUnit;
    //battle menus
    public GameObject menu;
    public GameObject[] allyTargetButtons = new GameObject[4];
    public GameObject[] enemyTargetButtons = new GameObject[5];
    //text for buttons
    public TextMeshProUGUI[] allyTargetButtonsText = new TextMeshProUGUI[4];
    public TextMeshProUGUI[] enemyTargetButtonsText = new TextMeshProUGUI[5];
    // positions for party ui
    public GameObject[] partyUIPositions = new GameObject[4];
    
    //positions for party members
    public GameObject[] partyPositions = new GameObject[4];
    //poaitions for enemies
    public GameObject[] enemyPositions = new GameObject[5];
    private void Awake()
    {
        battleStart();
    }


    //shows target buttons
    public void showEnemyTargets()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i] != null && enemies[i].isAlive)
            {
                enemyTargetButtons[i].SetActive(true);
            }
        }
    }
    public void ShowAllyTargets()
    {
        for (int i = 0; i < PartyManager.activeParty.Length; i++)
        {
            if (PartyManager.activeParty[i] != null)
            {
                allyTargetButtons[i].SetActive(true);
            }
        }
    }
    public void HideTargetButtons()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            enemyTargetButtons[i].SetActive(false);
        }
        for (int i = 0; i < PartyManager.activeParty.Length; i++)
        {
            allyTargetButtons[i].SetActive(false);
        }
    }
    //sets targets

    public void TargetEnemy1()
    {
        enemies[0].SetAsTarget();
        currentUnit.PerfromAction();
    }
    public void TargetEnemy2()
    {
        enemies[1].SetAsTarget();
        currentUnit.PerfromAction();
    }
    public void TargetEnemy3()
    {
        enemies[2].SetAsTarget();
        currentUnit.PerfromAction();
    }
    public void TargetEnemy4()
    {
        enemies[3].SetAsTarget();
        currentUnit.PerfromAction();
    }
    public void TargetEnemy5()
    {
        enemies[4].SetAsTarget();
        currentUnit.PerfromAction();
    }
    public void TargetAlly1()
    {
        PartyManager.activeParty[0].SetAsTarget();
        currentUnit.PerfromAction();
    }
    public void TargetAlly2()
    {
        PartyManager.activeParty[0].SetAsTarget();
        currentUnit.PerfromAction();
    }
    public void TargetAlly3()
    {
        PartyManager.activeParty[0].SetAsTarget();
        currentUnit.PerfromAction();
    }
    public void TargetAlly4()
    {
        PartyManager.activeParty[0].SetAsTarget();
        currentUnit.PerfromAction();
    }
    //sets the next action
    public void NextActionAttack()
    {
        SetNextAction(unit.Action.attack,true);
    }
    
    public void SetNextAction(unit.Action nextAction,bool targetsEnemy)
    {
        currentUnit.action = nextAction;
        if(targetsEnemy )
        {
            showEnemyTargets();
        }
        else
        {
            ShowAllyTargets();
        }

    }
    //battle cheaks
    public void EndBattle()
    {
        currentUnit = null;
        //all allies gain EXP from enemies
        for (int i = 0; i < enemies.Length; i++)
        {
            if( enemies[i] != null )
            {
                for (int j = 0; j < PartyManager.allies.Count; j++)
                {
                    PartyManager.allies[j].GainEXP(enemies[i].exp);
                }
            }
        }




        

    }
    
    public void battleStart()
    {
        for(int i = 0; i < PartyManager.activeParty.Length; i++)
        { 
            if(PartyManager.activeParty[i] != null)
            {
                fighters.Add(PartyManager.activeParty[i]);
                allyTargetButtonsText[i].text = PartyManager.activeParty[i].name;
                PartyManager.activeParty[i].UIBase.transform.position= partyUIPositions[i].transform.position;
                PartyManager.activeParty[i].Body.transform.position = partyPositions[i].transform.position;
            }
        }
        for(int i = 0;i<enemies.Length; i++)
        {
            if(enemies[i] != null)
            {
                fighters.Add(enemies[i]);
                
                enemyTargetButtonsText[i].text=enemies[i].name;
                enemies[i].Body.transform.position=enemyPositions[i].transform.position;
                enemies[i].Body.transform.position = new Vector3 (enemyPositions[i].transform.position.x, enemyPositions[i].transform.position.y +3, enemyPositions[i].transform.position.z);
            }
        }
        for (int i = 0; i < fighters.Count; i++)
        {
            fighters[i].updateStats(false);
            resetSpeed(fighters[i], true);
        }
        HideTargetButtons();
        nextTurn();
    }
    //trun order, turns start when "action speed" hits 0 or less, speed can be reset to keep action speed already used if speed gets buffed, or reset it if turn is taken
    public void resetSpeed(unit Unit,bool TurnHappened)
    {
        Unit.actionSpeed= (int)math.round(defaultActionSpeed / Unit.SPD);
        if (TurnHappened)
        {
            Unit.actionSpeedused = 0;
        }
        else
        {
            Unit.actionSpeed -= Unit.actionSpeedused;
        }
    }
    public void nextTurn()
    {
        bool turnNotStarted = true;
        while (turnNotStarted)
        {
            for (int i = 0;i<fighters.Count;i++)
            {
                if (fighters[i].isAlive)
                {
                    //when actionspeed = 0
                    fighters[i].actionSpeed -= 1;
                    fighters[i].actionSpeedused += 1;
                    if (fighters[i].actionSpeed <= 0)
                    {
                        turnNotStarted = false;
                        fighters[i].StartTurn();
                        resetSpeed(fighters[i], true);
                        currentUnit = fighters[i];
                    }
                }
                //removes buffs/debuffs and action speed if dead (as a backup for if that fails when taking damage), passive effects whle dead could be checked here
                else
                {
                    fighters[i].removeStatChange(unit.BuffType.All);
                    resetSpeed(fighters[i], true);
                }
                
            }
        }
    }
    
}
