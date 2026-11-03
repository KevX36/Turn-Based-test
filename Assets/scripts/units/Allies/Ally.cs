using UnityEngine;

public class Ally : unit
{
    //used for level ups
    public int[] EXPForLevel = { 20,40,80,160,320,640,1280,2560,5120,10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000, 10000 };
    //increases stat on level
    public int[] HPBoostOnLevel = new int[50];
    public int[] MPBoostOnLevel = new int[50];
    public int[] STRBoostOnLevel = new int[50];
    public int[] MAGBoostOnLevel = new int[50];
    public int[] VITBoostOnLevel = new int[50];
    public int[] SPDBoostOnLevel = new int[50];
    public int[] LUCBoostOnLevel = new int[50];
    
    public void setBaseStats()
    {
        stats = new Stats(HPBoostOnLevel[0], MPBoostOnLevel[0], STRBoostOnLevel[0], MAGBoostOnLevel[0], VITBoostOnLevel[0], SPDBoostOnLevel[0], LUCBoostOnLevel[0]);
        
    }
    
    private void Awake()
    {
        
    }
    public override void StartTurn()
    {
        base.StartTurn();
        battleManager.menu.SetActive(true);
        abilityTarget = null;
    }
    public override void EndTurn()
    {

        base.EndTurn();

    }
    public void GainEXP(int expGained)
    {
        exp += expGained;
        if(level < 50)
        {
            checkForLevelUp();
        }
    }
    public void checkForLevelUp()
    {
        if (exp >= EXPForLevel[level])
        {
            LevelUp(HPBoostOnLevel[level], MAGBoostOnLevel[level], STRBoostOnLevel[level], MAGBoostOnLevel[level], VITBoostOnLevel[level], SPDBoostOnLevel[level], LUCBoostOnLevel[level]);
            checkForLevelUp();
        }
    }
    //boost base stats when leveling up
    public void LevelUp(int hp, int mp, int str, int mag, int vit, int spd, int luc)
    {
        stats.LevelUp(hp, mp, str, mag, vit, spd, luc);
        level++;
        updateStats(true);
    }
}
