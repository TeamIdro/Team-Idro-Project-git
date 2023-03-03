using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Magia/MagiaSO", menuName = "Magia/MagiaSO")]
public class MagiaSO : ScriptableObject,IMagia
{
    public AnimatorController animatorMagia;
    public TipoComportamentoMagia magicBehaviourType;
    [Space]
    public TipoDiDannoMagia damageType;
    [Header("Lista Combinazioni")]
    public List<ElementoMagiaSO> combinazioneDiElementi;
    [Space(15)]
    [Header("Valori Magia Generici")]
    public float dannoDellaMagia;
    [Space(5)]
    [Range(0f, 10f)]
    public float moltiplicatoreDanno;
    [Space(5)]
    [Range(0f, 10f)]
    public float moltiplicatoreResistenza;
    [Space(5)]
    [Header("Valori Per Magia Lanciata")]
    [Range(0, 100)]
    [Tooltip("Velocità del proiettile")]
    public float velocitàMagiaLanciata;
    [Tooltip("Se il proiettile deve rallentare fino a fermarsi")]
    public bool rallentamentoGraduale;
    [Tooltip("Velocità di decellerazione, viene usata solo se rallentamento graduale è spuntato")]
    [Range(0, 100)]
    public float decellerazione;
    [Range(0, 100)]
    [Tooltip("Distanza percorsa dal proiettile prima di essere distrutto")]
    public float distanzaMagiaLanciata;
    [Range(0, 1000)]
    [Tooltip("Durata in secondi del proiettile prima di essere distrutto")]
    public float tempoMagiaLanciata = 5;
    [Tooltip("Se la magia deve detonare all'impatto con qualcosa")]
    public bool detonazioneAdImpatto;
    [Tooltip("I layer con cui il proiettile non collide")]
    public LayerMask ignoraCollisioni;
    [Tooltip("I layer che possono essere danneggiati dal proiettile")]
    public LayerMask danneggiaTarget;
    [Tooltip("Se il colpo deve passare oltre i bersagli danneggiabili, se vero il colpo danneggerà il bersaglio una volta e passerà oltre danneggiando qualunque bersaglio valido sulla traiettoria")]
    public bool colpoPerforante;

    [Header("Valori Per Magia Stazionaria")]
    public int placeHolder;
    [Header("Valori Per Magia Teleport")]
    public Transform posizioneDelNemicoPiùVicino;


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