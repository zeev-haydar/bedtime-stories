using GameObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KalaHand : EnemyObject
{
    public KalaObject kalaObj;

    public override void TakeHit(int damage)
    {
        kalaObj.TakeHit(damage);
    }

    public override void EnemyDeath()
    {
        throw new System.NotImplementedException();
    }
}
