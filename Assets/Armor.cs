using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class Armor : MonoBehaviour
{
    //base defense
    public int Resistance;
    //base dodge chance
    public int Dodge;
    //boosts to base stats
    public int STR;
    public int MAG;
    public int VIT;
    public int SPD;
    public int LUC;

    public Armor(int res, int dog)
    {
        Resistance = res;
        Dodge = dog;
        STR = 0;
        MAG = 0;
        VIT = 0;
        SPD = 0;
        LUC = 0;
    }
    public Armor(int res, int dog, int str, int mag, int vit, int spd, int luc)
    {
        Resistance = res;
        Dodge = dog;
        STR = str;
        MAG = mag;
        VIT = vit;
        SPD = spd;
        LUC = luc;
    }
}
