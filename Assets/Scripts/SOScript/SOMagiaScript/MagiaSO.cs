using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "Magia/MagiaSO", menuName = "Magia/MagiaSO")]
public class MagiaSO : ScriptableObject,IMagia
{
    public AnimationClip animatorMagia;
    public TipoComportamentoMagia magicBehaviourType;
    [Space]
    public TipoDiDannoMagia damageType;
    [Header("Lista Combinazioni")]
    public List<ElementoMagiaSO> combinazioneDiElementi;
    [Space(15)]
    [Header("Valori Magia Generici")]
    public int dannoDellaMagia;
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
    public float velocitaMagiaLanciata;
    [Tooltip("Se il proiettile deve rallentare fino a fermarsi")]
    public bool rallentamentoGraduale;
    [Tooltip("Tempo che ci mette il proiettile a fermarsi rallentando gradualmente")]
    [Range(1, 100)]
    public float decellerazioneTime;
    [Range(0, 100)]
    [Tooltip("Distanza percorsa dal proiettile prima di essere distrutto")]
    public float distanzaMagiaLanciata;
    [Range(1, 1000)]
    [Tooltip("Durata in secondi del proiettile prima di essere distrutto")]
    public float tempoMagiaLanciata = 5;
    [Tooltip("Se la magia deve detonare all'impatto con qualcosa")]
    public bool detonazioneAdImpatto;
    [Tooltip("Inserire prefab dell'esplosione desiderata, obbligatorio se detonazioneAdImpatto è spuntata")]
    public GameObject ExplosionPref;
    [Tooltip("Se si vuole sparare altro al posto del normale bullet inserire qui il prefab, se lasciato vuoto verrà sparato il prefab bullet")]
    public GameObject AlternativeBullet;    
    [Range(0, 100)]
    public float explosionKnockbackForce = 1;
    [Tooltip("I layer con cui il proiettile non collide, può comunque infliggere danni ai nemici impostati su danneggiaTarget ma infliggerà danno solo una volta per ognuno sulla traiettoria e passerà oltre")]
    public LayerMask ignoraCollisioni;
    [Tooltip("I layer che possono essere danneggiati dal proiettile")]
    public LayerMask danneggiaTarget;

    [Header("Valori Per Magia Stazionaria")]
    public int placeHolder;
    [Header("Valori Per Magia Teleport")]
    public Transform posizioneDelNemicoPiuVicino;


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