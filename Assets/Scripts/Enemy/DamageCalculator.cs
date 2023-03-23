using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class DamageCalculator
{
    public static int DamageToEnemyCalculator(int damage, TipoMagia enemyWeakness, TipoMagia magicType)
    {
        if(enemyWeakness == magicType)
        {
            return damage*2;
        }

        return damage;
    }
}
