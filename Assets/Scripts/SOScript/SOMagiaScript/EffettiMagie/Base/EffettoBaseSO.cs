using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EffettoBaseSO : ScriptableObject
{
    public float durataEffetto = 0f;

    /// <summary>
    /// Funzione base per applicare l'effetto al nemico
    /// </summary>
    /// <param name="nemico"></param>
    public abstract void ApplicaEffetto(EnemyScript nemico);
    public abstract IEnumerator TogliEffettoDopoDelTempo(EnemyScript nemico);
}
