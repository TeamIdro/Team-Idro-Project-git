using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu(fileName = "Magia/MagiaSO", menuName = "Magia/MagiaSO")]
public class MagiaSO : ScriptableObject
{
    public GameObject prefabAnimatoriMagia;
    public TipoComportamentoMagia magicBehaviourType;
    [Space]
    public TipoMagia tipoMagia;
    public TipoDiDannoMagia damageType;
    [Header("Lista Combinazioni")]
    public List<ElementoMagiaSO> combinazioneDiElementi;
    [Space(15)]
    [Header("Valori Magia Generici")]
    public float dannoDellaMagia;
    [Space(5)]
    [Range(0f, 10f)]
    public float moltiplicatoreDelDannoDellaMagia;
    [Space(5)]
    [Range(0f, 10f)]
    public float moltiplicatoreResistenzaAllaMagia;
    [Space(5)]
    [Space()]
    [Header("Valori per fisica proiettili")]
    [Max(10)]
    public float forzaDiGravitaPerProiettile = 0;
    public float knockbackForzaEsplosione = 1;
    [Header("Valori per camera shake")]
    public float intensity;
    public float shakeTime;
    [Space()]
    [Header("Valori Per Magia Lanciata")]
    [Range(0, 100)]
    [Tooltip("Velocita del proiettile")]
    public float velocitaDellaMagiaLanciata;
    [Tooltip("Se il proiettile deve rallentare fino a fermarsi")]
    public bool rallentamentoGraduale;
    [Tooltip("Tempo che ci mette il proiettile a fermarsi rallentando gradualmente")]
    [Range(1, 100)]
    public float tempoDecellerazioneMagiaLanciata;
    [Range(0, 100)]
    [Tooltip("Distanza percorsa dal proiettile prima di essere distrutto")]
    public float distanzaMagiaLanciata;
    [Range(1, 1000)]
    [Tooltip("Durata in secondi del proiettile prima di essere distrutto")]
    public float tempoMagiaLanciata = 5;
    [Tooltip("Se la magia deve detonare all'impatto con qualcosa")]
    public bool detonazioneAdImpatto;
    [Tooltip("Inserire prefab dell'esplosione desiderata, obbligatorio se detonazioneAdImpatto � spuntata")]
    public GameObject ExplosionPref;
   
    [Tooltip("I layer con cui il proiettile non collide, pu� comunque infliggere danni ai nemici impostati su layerMaskPerDanneggiaTarget ma infligger� danno solo una volta per ognuno sulla traiettoria e passer� oltre")]
    public LayerMask layerMaskPerIgnoraCollisioni;
    [Tooltip("I layer che possono essere danneggiati dal proiettile")]
    public LayerMask layerMaskPerDanneggiaTarget;

    [Header("Valori Per Magia Stazionaria")]
    public float tickTime;
    public float raggioArea = 0;


    [Space]
    [Header("Lista Effetti Magia")]
    public List<EffettoBaseSO> effettiMagia;


    [Space]
    public GameObject spawnaOggettoAdImpatto;
    public float dannoAreaOggettoAdImpattoSpawnato;



    
    


    public void ApplicaEffetti(EnemyScript nemicoACuiApplicareGliEffetti)
    {
        if (effettiMagia.Count > 0)
        {
            foreach(EffettoBaseSO effetto in effettiMagia)
            {
                effetto.ApplicaEffetto(nemicoACuiApplicareGliEffetti);
            }
        }
    }
    public void TogliEffetti(EnemyScript nemicoACuiTogliereGliEffetti)
    {
        if(effettiMagia.Count > 0)
        {
            foreach (EffettoBaseSO effetto in effettiMagia)
            {
                effetto.TogliEffetto(nemicoACuiTogliereGliEffetti);
            }
        }
    }
    public void TogliEffettiDopoTempo(EnemyScript nemicoACuiTogliereGliEffetti)
    {
        if (effettiMagia.Count > 0)
        {
            foreach (EffettoBaseSO effetto in effettiMagia)
            {
                nemicoACuiTogliereGliEffetti.StartCoroutine(effetto.TogliEffettoDopoDelTempo(nemicoACuiTogliereGliEffetti));
            }
        }
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
