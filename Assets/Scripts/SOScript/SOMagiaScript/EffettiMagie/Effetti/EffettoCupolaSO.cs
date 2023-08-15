using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magia/Effetti/EffettoCupolaSO", menuName = "Magia/Effetti/EffettoCupolaSO")]
public class EffettoCupolaSO : EffettoBaseSO
{
    [Space]
    [Header("Variabili Per Effetto Cupola")]
    [SerializeField] private float raggioCupola;
    [SerializeField] private int dannoCupola;
    [SerializeField] private GameObject cupolaSulPlayer;

    private GameObject tempInstatieted;
    public override void ApplicaEffettoAlMago(MagicController mago)
    {
        Debug.Log("Effetto Mago Applicato a:" + mago);
        float durataEffettoInterno = durataEffetto;
        if(mago.GetComponentInChildren<CupolaDanno>()==null)
        {
            GameObject cupolaIstanziata = Instantiate(cupolaSulPlayer);
            cupolaIstanziata.SetActive(true);
            cupolaIstanziata.transform.SetParent(mago.transform);
            cupolaIstanziata.transform.SetLocalPositionAndRotation(Vector3.zero,Quaternion.identity);
            cupolaIstanziata.GetComponent<CupolaDanno>()?.SetRaggioCupola(raggioCupola);
            cupolaIstanziata.GetComponent<CupolaDanno>()?.SetDannoCupola(dannoCupola);
            Destroy(cupolaIstanziata, durataEffettoInterno);
        }
    }
    public override IEnumerator TogliEffettiAlMagoDopoTempo(MagicController mago)
    {
        yield return new WaitForSeconds(durataEffetto);
        Debug.Log("Effetto Mago tolto a:" + mago);
    }
    public override void ApplicaEffettoANemico(EnemyScript nemico)
    {
        return;
    }

    public override IEnumerator TogliEffettoDopoDelTempoANemico(EnemyScript nemico)
    {
        yield return null;
    }
}
