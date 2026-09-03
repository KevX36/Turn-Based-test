using UnityEngine;

public class Weapon : MonoBehaviour
{
    //base damage
    public int power;
    //base crit rate
    public int critRate;
    //base offs of hitting
    public int hitRate;
    // boosts to base stats
    public int STR;
    public int MAG;
    public int VIT;
    public int SPD;
    public int LUC;

    public Weapon (int pow,int crit,int hit)
    {
        power = pow;
        critRate = crit;
        hitRate = hit;
        STR = 0;
        MAG = 0;
        VIT = 0;
        SPD = 0;
        LUC = 0;
    }
    public Weapon(int pow, int crit,int hit, int str, int mag, int vit, int spd, int luc)
    {
        power = pow;
        critRate = crit;
        hitRate = hit;
        STR = str;
        MAG = mag;
        VIT = vit;
        SPD = spd;
        LUC = luc;
    }
}

