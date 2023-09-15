using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffettoBaseSO : ScriptableObject
{
    public float durataEffetto = 0f;
    public Color coloreEffetto;
    /// <summary>
    /// Funzione base per applicare l'effetto al nemico
    /// </summary>
    /// <param name="nemico"></param>
    public abstract void ApplicaEffettoANemico(GameObject nemico,Vector2 position);
    public abstract IEnumerator TogliEffettoDopoDelTempoANemico(GameObject nemico);
    public abstract void TogliEffettoANemico(GameObject nemico);
    public abstract void ApplicaEffettoAlMago(MagicController mago);
    public abstract IEnumerator TogliEffettiAlMagoDopoTempo(MagicController mago);
}
