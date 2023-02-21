using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Magia/MagiaSO", menuName = "Magia/MagiaSO")]
public class MagiaSO : ScriptableObject,IMagia
{
    [SerializeField] TipoComportamentoMagia magicBehaviourType;
    [Space]
    [SerializeField] TipoDiDannoMagia damageType;
    [Header("Lista Combinazioni")]
    public List<ElementoMagiaSO> combinazioneDiElementi;
    [Space(15)]
    [Header("Valori Magia")]
    public float dannoDellaMagia;
    [Space(5)]
    [Range(0f, 10f)]
    public float moltiplicatoreDanno;
    [Space(5)]
    [Range(0f, 10f)]
    public float moltiplicatoreResistenza;

    public virtual void Lancia()
    {

    }
    public TipoComportamentoMagia GetBehaviourType() => magicBehaviourType;
    public TipoDiDannoMagia GetDamageType() => damageType;
    private void OnValidate()
    {
        if (combinazioneDiElementi.Count > 3)
        {
            combinazioneDiElementi.Remove(combinazioneDiElementi.Last());
            
        }
    }
    
}

public enum TipoComportamentoMagia
{
    Lanciata,
    Stazionaria,
    Teleport,
}
public enum TipoDiDannoMagia
{
    TargetSingolo,
    AOE
}
internal interface IMagia
{
    public void Lancia();
}