using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu(fileName = "Magia/TipoMagia", menuName = "Magia/TipoMagia")]
public class MagiaSO : ScriptableObject
{
    public GameObject prefabParticleMagia;
    public TipoComportamentoMagia magicBehaviourType;
    [Space]
    public TipoMagia tipoMagia;
    public TipoDiDannoMagia damageType;
    [Space, Range(0, 100)] public float tempoDiAttesaDellaMagia; 

    [Header("Lista Combinazioni")]
    public List<ElementoMagiaSO> combinazioneDiElementi;
    [Space(15)]
    [Header("Valori Magia Generici")]
    public float dannoDellaMagia;
    [Space()]

    [Header("Suoni Per Magia")]
    public AudioClip suonoLancioMagia;
    public AudioClip suonoImpattoMagia;


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
    [ControlVisibility("magicBehaviourType == TipoComportamentoMagia.Lanciata")] public float velocitaDellaMagiaLanciata;
    [Tooltip("Se il proiettile deve rallentare fino a fermarsi")]
    [ControlVisibility("magicBehaviourType == TipoComportamentoMagia.Lanciata")] public bool rallentamentoGraduale;
    [Tooltip("Tempo che ci mette il proiettile a fermarsi rallentando gradualmente")]
    [Range(1, 100)]
    [ControlVisibility("magicBehaviourType == TipoComportamentoMagia.Lanciata")] public float tempoDecellerazioneMagiaLanciata;
    [Range(0, 100)]
    [Tooltip("Distanza percorsa dal proiettile prima di essere distrutto")]
    [ControlVisibility("magicBehaviourType == TipoComportamentoMagia.Lanciata")] public float distanzaMagiaLanciata;
    [Range(1, 1000)]
    [Tooltip("Durata in secondi del proiettile prima di essere distrutto")]
    [ControlVisibility("magicBehaviourType == TipoComportamentoMagia.Lanciata")] public float tempoMagiaLanciata = 5;
    [Tooltip("Se la magia deve detonare all'impatto con qualcosa")]
    public bool detonazioneAdImpatto;
    public bool staccaFiglioAllEsplosione;
    [Tooltip("Inserire prefab dell'esplosione desiderata, obbligatorio se detonazioneAdImpatto � spuntata")]
    public GameObject explosionPref;
   
    [Tooltip("I layer con cui il proiettile non collide, pu� comunque infliggere danni ai nemici impostati su layerMaskPerDanneggiaTarget ma infligger� danno solo una volta per ognuno sulla traiettoria e passer� oltre")]
    public LayerMask layerMaskPerIgnoraCollisioni;
    [Tooltip("I layer che possono essere danneggiati dal proiettile")]
    public LayerMask layerMaskPerDanneggiaTarget;

    [Header("Valori Per Magia Stazionaria")]
    [ControlVisibility("magicBehaviourType == TipoComportamentoMagia.Stazionaria")]
    public float tickTime;
    [ControlVisibility("magicBehaviourType == TipoComportamentoMagia.Stazionaria")]
    public float raggioArea = 0;
    [Header("Valori Per Magia Linecast")]
    [ControlVisibility("magicBehaviourType == TipoComportamentoMagia.LineCast")]
    public int conditionalVariable;
    [ControlVisibility("magicBehaviourType == TipoComportamentoMagia.LineCast")]
    public float lunghezzaLineCast = 0;
    [ControlVisibility("magicBehaviourType == TipoComportamentoMagia.LineCast")]public float XNoise;
    [ControlVisibility("magicBehaviourType == TipoComportamentoMagia.LineCast")] public float YNoise;
    [Space]
    [Header("Lista Effetti Magia OnHit")]
    public List<EffettoBaseSO> effettiMagiaQuandoColpito;

    [Space]
    [Header("Lista Effetti Magia Per Il Mago")]
    public List<EffettoBaseSO> effettiMagiaPerIlMago;


    [Space]
    public GameObject spawnaOggettoAdImpatto;
    public float dannoAreaOggettoAdImpattoSpawnato;


    public void PlayCastingSound(AudioSource audioSource)
    {
        if(suonoLancioMagia != null && audioSource != null)
        {
            audioSource.clip = suonoLancioMagia;
            audioSource.Play();
        }

    }

    public void ApplicaEffettoAMago(MagicController objectMago)
    {
        if(effettiMagiaPerIlMago.Count > 0)
        {
            foreach(EffettoBaseSO effetto in effettiMagiaPerIlMago)
            {
                effetto.ApplicaEffettoAlMago(objectMago);
            }
        }
    }
    public void TogliEffettoAMago(MagicController objectMago)
    {
        if (effettiMagiaPerIlMago.Count > 0)
        {
            foreach (EffettoBaseSO effetto in effettiMagiaPerIlMago)
            {
                objectMago.StartCoroutine(effetto.TogliEffettiAlMagoDopoTempo(objectMago));
            }
        }
    }

    public void ApplicaEffettiATarget(GameObject nemicoACuiApplicareGliEffetti,Vector2 position)
    {
        if (effettiMagiaQuandoColpito.Count > 0)
        {
            foreach(EffettoBaseSO effetto in effettiMagiaQuandoColpito)
            {
                effetto.ApplicaEffettoANemico(nemicoACuiApplicareGliEffetti, position);
            }
        }
    }
    public void TogliEffettiDopoTempoAlTarget(GameObject nemicoACuiTogliereGliEffetti)
    {
        if (effettiMagiaQuandoColpito.Count > 0)
        {
            foreach (EffettoBaseSO effetto in effettiMagiaQuandoColpito)
            {
                MonoBehaviour monoBehaviour = nemicoACuiTogliereGliEffetti.GetComponent<MonoBehaviour>();
                if (monoBehaviour != null)
                {
                    monoBehaviour.StartCoroutine(effetto.TogliEffettoDopoDelTempoANemico(nemicoACuiTogliereGliEffetti));
                }
                else
                {
                    return;
                }
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
    public bool HasEffect(EffettoBaseSO effettoDaCercare)
    {
        
        foreach (var effetto in effettiMagiaQuandoColpito)
        {
            if(effetto.GetType() == effettoDaCercare.GetType())
            {
                return true;
            }
        }
        return false;
    }
}

public enum TipoComportamentoMagia
{
    Lanciata,
    Stazionaria,
    LineCast,
}
public enum TipoDiDannoMagia
{
    TargetSingolo,
    AOE
}
