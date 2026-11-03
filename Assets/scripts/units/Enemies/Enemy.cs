using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Enemy : unit
{
    
    public List<Action> avilableActions = new List<Action>();
    public List<int> actionOdds = new List<int>();
    public List<int> actionRollCheck;
    public int actionRNG = 0;
    private void Awake()
    {
        

        for (int i = 0; i < actionOdds.Count; i++)
        {
            actionRNG += actionOdds[i];
        }

        //this will be cheaked when rolling for current action
        actionRollCheck = actionOdds;

        for (int i = 0; i < actionOdds.Count; i++)
        {
            for (int j = 0; j < i; j++)
            {
                actionRollCheck[i] += actionOdds[j];
            }
        }
        
    }
    
    public override void StartTurn()
    {
        base.StartTurn();
        //selects random action
        if (avilableActions.Count == 1)
        {
            action= avilableActions[0];
        }
        else
        {
            int actionRoll = RNG.Next(0, actionRNG);
            for (int i = 0; i < actionRollCheck.Count; i++)
            {
                if (actionRollCheck[i] >= actionRoll)
                {
                    action = avilableActions[i];
                }
            }
        }
        ActionTargetCheck();

    }
    //checks what type of ablity targeting type
    public void ActionTargetCheck()
    {
        if (actionTargetType[action] == 1)
        {
            setTargetPlayer();
        }
        else if (actionTargetType[action] == 2)
        {
            TargetRandomEnemyAlly();
        }
        else if (actionTargetType[action] == 3)
        {
            SetAsTarget();
        }
        else if (actionTargetType[action] == 4)
        {
            PerfromAction(true);
        }
        else
        {
            PerfromAction(false);
        }
    }

    //selects random target from avilable player party
    public void setTargetPlayer()
    {
        int totalTargets = 0;
        for (int i = 0; i < PartyManager.activeParty.Length; i++)
        {
            if (PartyManager.activeParty[i] != null)
            {
                totalTargets++;
            }
        }
        int targetRoll = RNG.Next(0, totalTargets);
        if (targetRoll == 0)
        {
            battleManager.TargetAlly1();
        }
        else if (targetRoll == 1)
        {
            battleManager.TargetAlly2();
        }
        else if (targetRoll == 2)
        {
            battleManager.TargetAlly3();
        }
        else
        {
            battleManager.TargetAlly4();
        }
        PerfromAction();
    }
    //selects target from random ally enemy
    public void TargetRandomEnemyAlly()
    {
        int totalTargets = 0;
        for (int i = 0; i < battleManager.enemies.Length; i++)
        {
            if (battleManager.enemies[i] != null)
            {
                totalTargets++;
            }
        }
        int targetRoll = RNG.Next(0, totalTargets);
        if (targetRoll == 0)
        {
            battleManager.TargetAlly1();
        }
        else if (targetRoll == 1)
        {
            battleManager.TargetAlly2();
        }
        else if (targetRoll == 2)
        {
            battleManager.TargetEnemy3();
        }
        else if(targetRoll == 3)
        {
            battleManager.TargetEnemy4();
        }
        else
        {
            battleManager.TargetEnemy5();
        }
        PerfromAction();
    }
}
