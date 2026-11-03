using UnityEngine;

public class Stats
{
    public Stats(int hp,int mp,int str,int mag,int vit, int spd, int luc)
    {
        MaxHP = hp; 
        MaxMP = mp;
        STR = str;
        MAG = mag;
        VIT = vit;
        SPD = spd;
        LUC = luc;
    }
    public int MaxHP {get; private set;}
    public int MaxMP { get; private set; }
    public int STR { get; private set; }
    public int MAG { get; private set; }
    public int VIT { get; private set; }
    public int SPD { get; private set; }
    public int LUC { get; private set; }
    public void LevelUp(int hp, int mp,int str, int mag, int vit, int spd, int luc)
    {
        MaxHP += hp;
        MaxMP += mp;
        STR += str;
        MAG += mag;
        VIT += vit;
        SPD += spd;
        LUC += luc;
    }
}
