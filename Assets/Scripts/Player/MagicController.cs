using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MagicController : MonoBehaviour
{
    [SerializeField] private float timeBeforeShoot;
    [SerializeField] private GameObject m_UIPrefab;
    [SerializeField] private int m_spellSlot;
    [SerializeField] private GameObject m_basePrefabToShoot;
    [SerializeField, ReadOnly] private FasiDiLancioMagia m_faseCorrente = FasiDiLancioMagia.AspettoComponimentoMagia;
    [SerializeField,ReadOnly] private List<MagiaSO> m_tuttaLaListaDelleMagie;
    [Space(15)]
    [SerializeField] private List<MagiaSO> m_listaDiQuelloCheIlMagoSa;
    [SerializeField] private List<ElementoMagiaSO> m_elementiDaPrendere;
    public UnityEvent OnMagicComposed;

    private GamePlayInputActions m_gamePlayInput;
    private Dictionary<InputAction, TipoMagia> m_dizionariElementi = new Dictionary<InputAction, TipoMagia>();
    private List<ElementoMagiaSO> m_listaValoriLancio = new List<ElementoMagiaSO>();
    private MagiaSO m_magiaDaLanciare;
    private Magia magiaDaInizializzare;
    UIElementiMagia UIelementiMagia;
    private void Awake()
    {
        UIelementiMagia = m_UIPrefab.GetComponent<UIElementiMagia>();
        m_gamePlayInput = new GamePlayInputActions();
        var assets = AssetDatabase.FindAssets("t:MagiaSO", new[] { "Assets/Data/MagiaSO/Combinazione" });
        var element = AssetDatabase.FindAssets("t:ElementoMagiaSO", new[] { "Assets/Data/MagiaSO/Elementi" });
        foreach (var item in assets)
        {
            var magie = AssetDatabase.LoadAssetAtPath<MagiaSO>(AssetDatabase.GUIDToAssetPath(item));
            Debug.Log(magie);
            m_tuttaLaListaDelleMagie.Add(magie);
        }
        foreach (var item in element)
        {
            var elementoDaAggiungere = AssetDatabase.LoadAssetAtPath<ElementoMagiaSO>(AssetDatabase.GUIDToAssetPath(item));
            m_elementiDaPrendere.Add(elementoDaAggiungere);
        }
        for (int i = 0; i < m_tuttaLaListaDelleMagie.Count; i++)
        {
            m_tuttaLaListaDelleMagie.OrderBy(x => x.combinazioneDiElementi);
            m_tuttaLaListaDelleMagie[i].combinazioneDiElementi.OrderBy(x => x.tipoDiMagia);
        }
        for (int i = 0; i < m_listaDiQuelloCheIlMagoSa.Count; i++)
        {
            m_listaDiQuelloCheIlMagoSa.OrderBy(x => x.combinazioneDiElementi);
            m_listaDiQuelloCheIlMagoSa[i].combinazioneDiElementi.OrderBy(x=> x.tipoDiMagia);
        }
        m_dizionariElementi.Add(m_gamePlayInput.Mage.UsaElementoAcqua, TipoMagia.Acqua);
        m_dizionariElementi.Add(m_gamePlayInput.Mage.UsaElementoTerra, TipoMagia.Terra);
        m_dizionariElementi.Add(m_gamePlayInput.Mage.UsaElementoFuoco, TipoMagia.Fuoco);
        m_dizionariElementi.Add(m_gamePlayInput.Mage.UsaElementoAria, TipoMagia.Vento);
        m_dizionariElementi.Add(m_gamePlayInput.Mage.UsaElementoFulmine, TipoMagia.Fulmine);

        m_gamePlayInput.Mage.UsaElementoAcqua.performed += AddElement;
        m_gamePlayInput.Mage.UsaElementoTerra.performed += AddElement;
        m_gamePlayInput.Mage.UsaElementoFuoco.performed += AddElement;
        m_gamePlayInput.Mage.UsaElementoAria.performed += AddElement;
        m_gamePlayInput.Mage.UsaElementoFulmine.performed += AddElement;
    }

    private void AddElement(InputAction.CallbackContext obj)
    {
        if (m_listaValoriLancio.Count < m_spellSlot)
        {
            TipoMagia tipoDiMagiaDaAggiungere = m_dizionariElementi.GetValueOrDefault(obj.action);
            ElementoMagiaSO elementoMagia = m_elementiDaPrendere.Find(x => x.tipoDiMagia == tipoDiMagiaDaAggiungere);
            m_listaValoriLancio.Add(elementoMagia);
            UIelementiMagia.AddUI(elementoMagia);
        }
        else
        {
            return;
        }
      

    }

  

    public void Start()
    {
        
    }

    public void Update()
    {
        CheckStatusMagia();
    }

    private void CheckStatusMagia()
    {
        switch(m_faseCorrente)
        {
            case FasiDiLancioMagia.AspettoComponimentoMagia:
                CheckIfPlayerWantToStartMagic();
                break;

            case FasiDiLancioMagia.ComponendoMagia:
                StartCoroutine(CheckIfPlayerKnowsTheMagic());
                break;

            case FasiDiLancioMagia.MagiaComposta:
                DoSomethingOnMagicComposed();
                break;

            case FasiDiLancioMagia.AspettandoLancioMagia:
                WaitForMagicToShoot();
                break;

            case FasiDiLancioMagia.LancioMagia:
                ThrowMagic();
                break;

        }    
    }

  
    private void ThrowMagic()
    {
        Instantiate(m_basePrefabToShoot,transform.position,Quaternion.identity);
        magiaDaInizializzare.isCasted = true;
        magiaDaInizializzare = null;
        m_faseCorrente = FasiDiLancioMagia.AspettoComponimentoMagia;
        m_listaValoriLancio.Clear();
        UIelementiMagia.ClearUI();

    }

    private void WaitForMagicToShoot()
    {
        InizializzaMagia();
        m_faseCorrente = FasiDiLancioMagia.LancioMagia;
    }

    private void InizializzaMagia()
    {
        magiaDaInizializzare = m_basePrefabToShoot.GetComponent<Magia>();
        magiaDaInizializzare.magia = m_magiaDaLanciare;
        m_magiaDaLanciare = null;
    }

    private void DoSomethingOnMagicComposed()
    {
        OnMagicComposed.Invoke();
        m_faseCorrente = FasiDiLancioMagia.AspettandoLancioMagia;
    }


    private IEnumerator CheckIfPlayerKnowsTheMagic()
    {
        bool isMagiaCheStaLanciandoConosciuta = false;
        int counterCheck = 0;
        List<MagiaSO> listaTutteMagieLocale = new List<MagiaSO>();
        listaTutteMagieLocale = m_tuttaLaListaDelleMagie;
        m_listaValoriLancio.OrderBy(x => x.tipoDiMagia);
        listaTutteMagieLocale.OrderBy(x => x.name);
        m_listaDiQuelloCheIlMagoSa.OrderBy(x => x.name);
        yield return new WaitForSeconds(timeBeforeShoot);

        for (int i = 0; i < m_tuttaLaListaDelleMagie.Count; i++)
        {
            for (int j = 0; j < m_listaDiQuelloCheIlMagoSa.Count; j++)
            {
                //bool isEqual = first.OrderBy(x => x).SequenceEqual(second.OrderBy(x => x));
                if (listaTutteMagieLocale[i].combinazioneDiElementi.SequenceEqual(m_listaDiQuelloCheIlMagoSa[j].combinazioneDiElementi))
                {
                    for (int k = 0; k < m_listaValoriLancio.Count; k++)
                    {
                        /* TODO BUG: il sistema se si premono combinazioni diverse di tasti da qualsiasi magia essa prenderà elementi dalle diverse magie che sta ciclando 
                            per poi prendere la l'ultima magia che sta ciclando e creare quella, se non ricordi bug fai test*/
                        if (m_listaDiQuelloCheIlMagoSa[j].combinazioneDiElementi[k].tipoDiMagia == m_listaValoriLancio[k].tipoDiMagia)
                        {
                            counterCheck++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (counterCheck <= m_spellSlot && counterCheck != 0)
                    {
                        m_magiaDaLanciare = m_listaDiQuelloCheIlMagoSa[j];
                        Debug.Log("Magia da lanciare: " + m_magiaDaLanciare);
                        isMagiaCheStaLanciandoConosciuta = true;
                        counterCheck = 0;
                        break;
                    }
                }
            }
        }
       
        if (isMagiaCheStaLanciandoConosciuta == true)
        {
            m_faseCorrente = FasiDiLancioMagia.MagiaComposta;
            counterCheck = 0;
        }
        else
        {
            m_listaValoriLancio.Clear();
            m_faseCorrente = FasiDiLancioMagia.AspettoComponimentoMagia;
            counterCheck = 0;
        }
        StopCoroutine(CheckIfPlayerKnowsTheMagic());
        yield return null;
    }

    private void CheckIfPlayerWantToStartMagic()
    {
        if (m_listaValoriLancio.Count == m_spellSlot)
        {
            m_faseCorrente = FasiDiLancioMagia.ComponendoMagia;
        }
    }
       
    private void OnEnable() => m_gamePlayInput.Mage.Enable();
        
}
public enum FasiDiLancioMagia
{
    AspettoComponimentoMagia,
    ComponendoMagia,
    MagiaComposta,
    AspettandoLancioMagia,
    LancioMagia,

}

