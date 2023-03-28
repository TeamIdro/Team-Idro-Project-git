using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class DamageCalculator
{
    public static float DamageToEnemyCalculator(float damage, TipoMagia enemyWeakness, TipoMagia magicType)
    {
        switch(WeaknessOrStrenghtCheck(magicType, enemyWeakness))
        {
            case 0:
                return damage;
            case 1:
                return damage*1.5f;
            case 2:
                return damage*0.5f;
        }

        return damage;
    }

    private static int WeaknessOrStrenghtCheck(TipoMagia magicType, TipoMagia enemyWeakness)
    {
        if(magicType == TipoMagia.Acqua
            || magicType == TipoMagia.Terra
            || magicType == TipoMagia.Vento
            || magicType == TipoMagia.Fuoco)
        {
            if(magicType == TipoMagia.Fuoco)
            {
                if(enemyWeakness == TipoMagia.Vento)
                {
                    return 1;
                }
                else if(enemyWeakness == TipoMagia.Acqua)
                {
                    return 2;
                }
            }
            else if(magicType == TipoMagia.Terra)
            {
                if(enemyWeakness == TipoMagia.Acqua)
                {
                    return 1;
                }
                else if(enemyWeakness == TipoMagia.Vento)
                {
                    return 2;
                }
            }
            else if(magicType == TipoMagia.Acqua)
            {
                if(enemyWeakness == TipoMagia.Fuoco)
                {
                    return 1;
                }
                else if(enemyWeakness == TipoMagia.Terra)
                {
                    return 2;
                }
            }
            else if(magicType == TipoMagia.Vento)
            {
                if(enemyWeakness == TipoMagia.Terra)
                {
                    return 1;
                }
                else if(enemyWeakness == TipoMagia.Fuoco)
                {
                    return 2;
                }
            }

            return 0;
        }
        else if(magicType == TipoMagia.Tempesta
            || magicType == TipoMagia.Vapore
            || magicType == TipoMagia.Ghiaccio
            || magicType == TipoMagia.Erba
            || magicType == TipoMagia.Magma
            || magicType == TipoMagia.Veleno)
        {
            if(magicType == TipoMagia.Tempesta)
            {
                if(enemyWeakness == TipoMagia.Veleno)
                {
                    return 1;
                }
                else if(enemyWeakness == TipoMagia.Vapore)
                {
                    return 2;
                }
            }
            else if(magicType == TipoMagia.Vapore)
            {
                if(enemyWeakness == TipoMagia.Tempesta)
                {
                    return 1;
                }
                else if(enemyWeakness == TipoMagia.Magma)
                {
                    return 2;
                }
            }
            else if(magicType == TipoMagia.Ghiaccio)
            {
                if(enemyWeakness == TipoMagia.Magma)
                {
                    return 1;
                }
                else if(enemyWeakness == TipoMagia.Erba)
                {
                    return 2;
                }
            }
            else if(magicType == TipoMagia.Erba)
            {
                if(enemyWeakness == TipoMagia.Ghiaccio)
                {
                    return 1;
                }
                else if(enemyWeakness == TipoMagia.Veleno)
                {
                    return 2;
                }
            }
            else if(magicType == TipoMagia.Magma)
            {
                if(enemyWeakness == TipoMagia.Vapore)
                {
                    return 1;
                }
                else if(enemyWeakness == TipoMagia.Ghiaccio)
                {
                    return 2;
                }
            }
            if(magicType == TipoMagia.Veleno)
            {
                if(enemyWeakness == TipoMagia.Erba)
                {
                    return 1;
                }
                else if(enemyWeakness == TipoMagia.Tempesta)
                {
                    return 2;
                }
            }

            return 0;
        }

        return 0;
    }
}
