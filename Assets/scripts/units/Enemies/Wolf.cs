using UnityEngine;

public class Wolf : Enemy
{
    public Wolf()
    {
        stats = new Stats(30,10,4,2,3,5,2);
        exp = 5;
        avilableActions.Add(Action.attack);
        actionOdds.Add(1);
    }
}
