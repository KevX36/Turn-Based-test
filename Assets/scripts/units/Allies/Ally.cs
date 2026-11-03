using UnityEngine;

public class Ally : unit
{
    //used for level ups
    public int[] EXPForLevel = new int[50];
    //increases stat on level
    public int[] HPBoostOnLevel = new int[50];
    public int[] MPBoostOnLevel = new int[50];
    public int[] STRBoostOnLevel = new int[50];
    public int[] MAGBoostOnLevel = new int[50];
    public int[] VITBoostOnLevel = new int[50];
    public int[] SPDBoostOnLevel = new int[50];
    public int[] LUCBoostOnLevel = new int[50];
    

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
    
}
