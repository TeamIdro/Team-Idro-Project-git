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
        return;
    }
    public override IEnumerator TogliEffettiAlMagoDopoTempo(MagicController mago)
    {
        yield return null;
    }
    public override void ApplicaEffettoANemico(GameObject target)
    {
        float durataEffettoInterno = durataEffetto;
        GameObject cupolaIstanziata = Instantiate(cupolaSulPlayer);
        cupolaIstanziata.SetActive(true);
        cupolaIstanziata.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        cupolaIstanziata.GetComponent<CupolaDanno>()?.SetRaggioCupola(raggioCupola);
        cupolaIstanziata.GetComponent<CupolaDanno>()?.SetDannoCupola(dannoCupola);
        Destroy(cupolaIstanziata, durataEffettoInterno);
    }

    public override IEnumerator TogliEffettoDopoDelTempoANemico(GameObject target)
    {
        yield return new WaitForSeconds(durataEffetto);
        Debug.Log("Effetto Mago tolto a:" + target);
    }
}
