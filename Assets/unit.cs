using UnityEngine;
using System;
using Unity.Mathematics;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class unit : MonoBehaviour
{
    public string name = "???";
    public int level = 1;
    public int exp = 0;
    //ui
    public GameObject UIBase;
    public GameObject HPBar;
    public GameObject MPBar;
    //main body
    public GameObject Body;
    //base stats
    public Stats stats = new Stats(10,10,1,1,1,1,1);
    //current equipment
    public Weapon weapon = WeaponManager.unarmed;
    public Armor armor = ArmorManager.none;
    //final stats for some things after combineing equipment and base stats
    //attack is weapon power + STR
    public int attack;
    //defense is armor resistance + VIT
    public int defense;
    //evades is armor dodge + SPD
    public int Evade;
    // Accuracy is weapon hitrate + SPD
    public int Accuracy;
    //crit is weapon critRate + LUC
    public int critRate;
    //DMG dealt by critical hits
    public float critDMG = 1.5f;
    //current HP, MP and living status
    public int HP;
    public int Mp;
    public bool isAlive = true;
    //active stats, after buffs
    public int MaxHP;
    public int MaxMP;
    public int STR;
    public int MAG;
    public int VIT;
    public int SPD;
    public int LUC;
    //action speed counts down to 0, when it hits 0 turn starts, action speed used will be applied to current action speed if it needs to be reset after speed buff
    public int actionSpeed = 1000;
    public int actionSpeedused = 0;
    //
    public BattleManager battleManager;
     
    //target of current ability 
    public unit abilityTarget;
    //used in RNG rolls, like critical hits and dodging
    public System.Random RNG = new System.Random();
    //possible actions
    public enum Action
    {
        noneSet,
        attack
    }
    //determions if an action should target allys or enemies
    //1= 1 enemy target
    //2= 1 ally target
    //3 = self target
    //4 = all enemy targets
    //5 = all ally targets
    public Dictionary<Action, int> actionTargetType = new Dictionary<Action, int>()
    {
        {Action.noneSet, 1},
        {Action.attack, 1},




    };
    
    //next action taken
    public Action action;
    //triggers current action on current target
    public void PerfromAction()
    {
        switch (action)
        {
            case Action.noneSet:
                Debug.Log("No action was set");
                break;


            case Action.attack:

                Strike(abilityTarget );

                break;
        }
    }
    //overload used for multi target ablilities
    public void PerfromAction(bool targetPlayerTeam)
    {
        switch (action)
        {
            case Action.noneSet:
                Debug.Log("No action was set");
                break;


            
        }
    }
    //sets this unit as the target for the unit currently taking turn
    public void SetAsTarget()
    {
        battleManager.currentUnit.abilityTarget = this;
        battleManager.currentUnit.PerfromAction();
    }
    //starts/ends turn
    public virtual void StartTurn()
    {
        //counts down buff/debuff timers if they are active
        if(attackBuffActive || attackDebuffActive)
        {
            attackChangeTimer -= 1;
            if(attackChangeTimer <= 0)
            {
                removeStatChange(BuffType.Attack);
            }
        }
        if(defenseBuffActive || defenseDebuffActive)
        {
            defenseChangeTimer -= 1;
            if(defenseChangeTimer <= 0)
            {
                removeStatChange(BuffType.Defense);
            }
        }
        if(speedBuffActive || speedDebuffActive)
        {
            speedChangeTimer -= 1;
            if(speedChangeTimer <= 0)
            {
                removeStatChange(BuffType.Speed);
            }
        }
        if(critBuffActive || critDebuffActive)
        {
            critChangeTimer -= 1;
            if(critChangeTimer <= 0)
            {
                removeStatChange(BuffType.Crit);
            }
        }
        
    }
    
    public virtual void EndTurn()
    {

        Debug.Log("turn ended");
        battleManager.currentUnit = null;
        battleManager.nextTurn();
    }
    //change equipment
    public void ChangeWeapon(Weapon wep)
    {
        weapon = wep;
        updateStats(false);
    }
    public void ChangeArmor(Armor arm)
    {
        armor = arm; 
        updateStats(false);
    }
    //boost base stats when leveling up
    public void LevelUp(int hp, int mp, int str, int mag, int vit, int spd, int luc)
    {
        stats.LevelUp(hp, mp, str, mag, vit, spd, luc);
        updateStats(true);
    }
    //types of stat buffs/debuffs
    public enum BuffType
    {
        Attack,
        Defense,
        Speed,
        Crit,
        All
    }
    //buff/debuff timers
    int attackChangeTimer = 0;
    int defenseChangeTimer = 0;
    int speedChangeTimer = 0;
    int critChangeTimer = 0;
    //buff checks
    bool attackBuffActive = false;
    bool defenseBuffActive = false;
    bool speedBuffActive = false;
    bool critBuffActive = false;
    //debuffchecks
    bool attackDebuffActive = false;
    bool defenseDebuffActive = false;
    bool speedDebuffActive = false;
    bool critDebuffActive = false;

    //buff and debuff methoeds
    //if appling a buff/debuff already on target the old buff is removed for the new one to be added
    // as a result buffs/debuffs do not stack, buffs will cancel equvalent debuffs and the opposite is also true
    //meothds are ment to take small decimals 

    //applies stat buffs
    public void BuffStat(BuffType stat, float multi,int durration)
    {
        switch (stat)
        {
            case BuffType.Attack:
                if (!attackDebuffActive)
                {
                    Debug.Log("applied attack buff");
                    if (attackBuffActive)
                    {
                        removeStatChange(BuffType.Attack);
                    }
                    attackBuffActive = true;
                    attackChangeTimer = durration;
                    attack = (int)math.round(attack * multi);
                    MAG = (int)math.round(MAG * multi);
                }
                else
                {
                    removeStatChange(BuffType.Attack);
                }
                    break;
            

        
            case BuffType.Defense:
                if (!defenseDebuffActive)
                {
                    Debug.Log("applied defense buff");
                    if (defenseBuffActive)
                    {
                        removeStatChange(BuffType.Defense);
                    }
                    defenseBuffActive = true;
                    defenseChangeTimer = durration;
                    defense = (int)math.round(defense * multi);
                }
                else
                {
                    removeStatChange(BuffType.Defense);
                }
                break;


            case BuffType.Speed:

                if (!speedDebuffActive)
                {
                    Debug.Log("applied speed buff");
                    if (speedBuffActive)
                    {
                        removeStatChange(BuffType.Speed);
                    }
                    speedBuffActive = true;
                    speedChangeTimer = durration;
                    SPD = (int)math.round(SPD * multi);
                    Evade = (int)math.round(Evade + ((multi - 1) * 5));
                    Accuracy = (int)math.round(Accuracy + ((multi - 1) * 10));
                }
                else
                {
                    removeStatChange(BuffType.Speed);
                }
                break;

            case BuffType.Crit:

                if (!critDebuffActive)
                {
                    Debug.Log("applied crit buff");
                    if (critBuffActive)
                    {
                        removeStatChange(BuffType.Crit);
                    }
                    critBuffActive = true;
                    critChangeTimer = durration;
                    critRate = (int)math.round(critRate + multi);
                }
                else
                {
                    removeStatChange(BuffType.Crit);
                }
                break;

            case BuffType.All:
                BuffStat(BuffType.Attack,multi,durration);
                BuffStat(BuffType.Defense,multi,durration);
                BuffStat(BuffType.Speed,multi,durration);
                BuffStat(BuffType.Crit,multi,durration);
                break;

        }
    }
    //applies Debuffs
    public void DebuffStat(BuffType stat, float multi, int durration)
    {
        switch (stat)
        {
            case BuffType.Attack:
                if (!attackBuffActive)
                {
                    Debug.Log("applied attack debuff");
                    if (attackDebuffActive)
                    {
                        removeStatChange(BuffType.Attack);
                    }
                    attackDebuffActive = true;
                    attackChangeTimer = durration;
                    attack = (int)math.round(attack / multi);
                    MAG = (int)math.round(MAG / multi);
                }
                else
                {
                    removeStatChange(BuffType.Attack);
                }
                break;



            case BuffType.Defense:
                if (!defenseBuffActive)
                {
                    Debug.Log("applied defense debuff");
                    if (defenseDebuffActive)
                    {
                        removeStatChange(BuffType.Defense);
                    }
                    defenseDebuffActive = true;
                    defenseChangeTimer = durration;
                    defense = (int)math.round(defense / multi);
                }
                else
                {
                    removeStatChange(BuffType.Defense);
                }
                break;


            case BuffType.Speed:

                if (!speedBuffActive)
                {
                    Debug.Log("applied speed debuff");
                    if (speedDebuffActive)
                    {
                        removeStatChange(BuffType.Speed);
                    }
                    speedDebuffActive = true;
                    speedChangeTimer = durration;
                    SPD = (int)math.round(SPD / multi);
                    Evade = (int)math.round(Evade - ((multi - 1) * 5));
                    Accuracy = (int)math.round(Accuracy - ((multi - 1) * 10));
                }
                else
                {
                    removeStatChange(BuffType.Speed);
                }
                break;

            case BuffType.Crit:

                if (!critBuffActive)
                {
                    Debug.Log("applied crit debuff");
                    if (critDebuffActive)
                    {
                        removeStatChange(BuffType.Crit);
                    }
                    critDebuffActive = true;
                    critChangeTimer = durration;
                    critRate = (int)math.round(critRate - multi);
                }
                else
                {
                    removeStatChange(BuffType.Crit);
                }
                break;

            case BuffType.All:
                DebuffStat(BuffType.Attack, multi, durration);
                DebuffStat(BuffType.Defense, multi, durration);
                DebuffStat(BuffType.Speed, multi, durration);
                DebuffStat(BuffType.Crit, multi, durration);
                break;

        }
    }
    //resets stats for just buffed stat
    public void removeStatChange(BuffType stat)
    {
        switch (stat)
        {
            case BuffType.Attack:
                attack = STR + weapon.power;
                MAG = stats.MAG+weapon.MAG;
                attackBuffActive = false;
                attackDebuffActive = false;
                attackChangeTimer = 0;
                Debug.Log("removed attack buff/debuff");
                break;



            case BuffType.Defense:
                defense = VIT + armor.Resistance;
                defenseBuffActive = false;
                defenseDebuffActive = false;
                defenseChangeTimer = 0;
                Debug.Log("removed defense buff/debuff");
                break;


            case BuffType.Speed:

                SPD = stats.SPD + weapon.SPD + armor.SPD;
                Evade = SPD + armor.Dodge;
                Accuracy = weapon.hitRate + SPD;
                speedChangeTimer = 0;
                speedBuffActive = false;
                speedDebuffActive = false;
                Debug.Log("removed speed buff/debuff");
                break;

            case BuffType.Crit:

                critRate = weapon.critRate + LUC;
                critBuffActive = false;
                critChangeTimer = 0;
                critDebuffActive = false;
                Debug.Log("removed crit buff/debuff");

                break;

            case BuffType.All:
                removeStatChange(BuffType.Attack);
                removeStatChange(BuffType.Defense);
                removeStatChange(BuffType.Speed);
                removeStatChange(BuffType.Crit);
                break;

        }
    }

    //updates stats when weapon is changed or base stats increase, bool determons if HP should be restored or not
    public void updateStats(bool heal)
    {
        MaxHP = stats.MaxHP;
        if(heal)
        {
            HP = MaxHP;
            isAlive = true;
        }
        else if(HP > MaxHP)
        {
            HP = MaxHP;
        }
        MaxMP = stats.MaxMP;
        if (heal)
        {
            Mp = MaxMP;
        }
        else if (Mp > MaxMP)
        {
            Mp = MaxMP;
        }
        STR = stats.STR+weapon.STR+armor.STR;
        MAG=stats.MAG+weapon.MAG+armor.MAG;
        VIT=stats.VIT+weapon.VIT+armor.VIT;
        SPD=stats.SPD+weapon.SPD+armor.SPD;
        LUC=stats.LUC+weapon.LUC+armor.LUC;
        attack = STR + weapon.power;
        defense = VIT + armor.Resistance;
        Evade = SPD + armor.Dodge;
        Accuracy = weapon.hitRate + SPD;
        critRate = weapon.critRate +LUC;
        Debug.Log($"stats reset, HP:{MaxHP},MP:{MaxMP},ATK:{attack},DEF:{defense},EVD{Evade},ACC{Accuracy},SIR:{STR},MAG:{MAG},VIT:{VIT},SPD:{SPD},LUC:{LUC}");
    }

    

    //basic attack, first tries to roll for crit, then dodge then just does damage
    //if critical hit, target defense is bypassed, damage is multiplied by critDMG, and dodge roll is skipped
    

    
    
    public virtual void Strike(unit Target)
    {
        
        int DMG = attack;
        int critRoll = RNG.Next(1,101);
        int dodgeRoll = RNG.Next(1, 101);
        if(critRate - Target.LUC >= critRoll)
        {
            Debug.Log("attakc was a critical hit");
            DMG = (int)math.round(DMG * critDMG);
            Target.TakeDamage(DMG);
        }
        else if (Target.Evade - Accuracy >=dodgeRoll)
        {
            Debug.Log("the attack was dodged");
        }
        else
        {
            Debug.Log("attacked");
            DMG -= Target.defense;
            Target.TakeDamage(DMG);
        }
        
        
    }
    public virtual void TakeDamage(int DMG)
    {

        HP -= DMG;
        if (HP < 0)
        {
            isAlive = false;
            //remvoes buffs and resets action speed
            removeStatChange(BuffType.All);
            battleManager.resetSpeed(this, true);
        }
    }
    public virtual void Heal(int recovery)
    {
        HP += recovery;
        if(HP > MaxHP)
        {
            HP = MaxHP;
        }
    }
    public virtual void revive(bool fullRevive)
    {
        isAlive = true;
        if (fullRevive)
        {
            HP=MaxHP;
        }
        else
        {
            HP = MaxHP/2;
        }
    }
    
}
