using Mono.Cecil.Cil;
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
    [SerializeField] [Range(1, 3)] private int m_spellSlot;
    [SerializeField] private GameObject m_basePrefabToShoot;
    [SerializeField, ReadOnly] private FasiDiLancioMagia m_faseCorrente = FasiDiLancioMagia.AspettoComponimentoMagia;
    [SerializeField, ReadOnly] private List<MagiaSO> m_tuttaLaListaDelleMagie;
    [Tooltip("Se spuntato fa si che se la lista degli elementi � piena e provi ad inserirne uno nuovo viene buttato fuori il primo elemento della lista per fare spazio, se non � spuntato una volta che la lista � piena non si potr� pi� aggiungere elementi")]
    [SerializeField] private bool lastInFirstOut = true;
    [Space(15)]
    //[SerializeField]  public int livCatalizzatore = 1;
    //[SerializeField] private List<MagiaSO> m_listaDiQuelloCheIlMagoSa;
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

        
        
        var magicTakenFromFolder = Resources.LoadAll<MagiaSO>("Data/MagiaSO/Combinazione");
        foreach (var magia in magicTakenFromFolder)
        {
            var tempMagia = (MagiaSO)magia;
            Debug.Log(tempMagia);
            m_tuttaLaListaDelleMagie.Add(tempMagia);
        }
        var elementsTakenFromFolder = Resources.LoadAll("Data/MagiaSO/Elementi", typeof(ElementoMagiaSO));
       
        foreach (var item in elementsTakenFromFolder)
        {
            var elementTemp = item as ElementoMagiaSO;
            Debug.Log(item);
            m_elementiDaPrendere.Add(elementTemp);
        }
        for (int i = 0; i < m_tuttaLaListaDelleMagie.Count; i++)
        {
            m_tuttaLaListaDelleMagie.OrderBy(x => x.combinazioneDiElementi);
            m_tuttaLaListaDelleMagie[i].combinazioneDiElementi.OrderBy(x => x.tipoDiMagia);
        }
     
        m_dizionariElementi.Add(m_gamePlayInput.Mage.UsaElementoAcqua, TipoMagia.Acqua);
        m_dizionariElementi.Add(m_gamePlayInput.Mage.UsaElementoTerra, TipoMagia.Terra);
        m_dizionariElementi.Add(m_gamePlayInput.Mage.UsaElementoFuoco, TipoMagia.Fuoco);
        m_dizionariElementi.Add(m_gamePlayInput.Mage.UsaElementoAria, TipoMagia.Vento);

        m_gamePlayInput.Mage.UsaElementoAcqua.performed += AddElement;
        m_gamePlayInput.Mage.UsaElementoTerra.performed += AddElement;
        m_gamePlayInput.Mage.UsaElementoFuoco.performed += AddElement;
        m_gamePlayInput.Mage.UsaElementoAria.performed += AddElement;
        m_gamePlayInput.Mage.Fire.performed += Onfire;
    }

    private void AddElement(InputAction.CallbackContext obj)
    {
        if (lastInFirstOut)
        {
            if (m_listaValoriLancio.Count < m_spellSlot)
            {
                TipoMagia tipoDiMagiaDaAggiungere = m_dizionariElementi.GetValueOrDefault(obj.action);
                ElementoMagiaSO elementoMagia = m_elementiDaPrendere.Find(x => x.tipoDiMagia == tipoDiMagiaDaAggiungere);
                m_listaValoriLancio.Add(elementoMagia);
                UIelementiMagia.AddUI(elementoMagia, m_spellSlot, false);
            }
            else
            {
                TipoMagia tipoDiMagiaDaAggiungere = m_dizionariElementi.GetValueOrDefault(obj.action);
                ElementoMagiaSO elementoMagia = m_elementiDaPrendere.Find(x => x.tipoDiMagia == tipoDiMagiaDaAggiungere);
                m_listaValoriLancio.RemoveAt(0);
                m_listaValoriLancio.Add(elementoMagia);
                UIelementiMagia.AddUI(elementoMagia, m_spellSlot, true);
            }
        }
        else
        {
            if (m_listaValoriLancio.Count < m_spellSlot)
            {
                TipoMagia tipoDiMagiaDaAggiungere = m_dizionariElementi.GetValueOrDefault(obj.action);
                ElementoMagiaSO elementoMagia = m_elementiDaPrendere.Find(x => x.tipoDiMagia == tipoDiMagiaDaAggiungere);
                m_listaValoriLancio.Add(elementoMagia);
                UIelementiMagia.AddUI(elementoMagia, m_spellSlot, false);
            }
            else
            {
                return;
            }
        }
        
      

    }

  

    public void Start()
    {
        
    }

    public void Update()
    {
        if (m_faseCorrente == FasiDiLancioMagia.AspettoComponimentoMagia)
        {
            CheckIfPlayerWantToStartMagic();
        }
        
    }


    private void InizializzaMagia()
    {
        magiaDaInizializzare = m_basePrefabToShoot.GetComponent<Magia>();
        magiaDaInizializzare.magia = m_magiaDaLanciare;
        m_magiaDaLanciare = null;
    }

    private void DoSomethingOnMagicComposed()
    {
        m_listaValoriLancio.OrderBy(x => x.tipoDiMagia);
        List<MagiaSO> listaTutteMagieLocale = new List<MagiaSO>();
        listaTutteMagieLocale = m_tuttaLaListaDelleMagie;
        listaTutteMagieLocale.OrderBy(x => x.name);
        int elementoMultiploRipetizioni = 0;
        ElementoMagiaSO elemMultiplo = ScriptableObject.CreateInstance("MagiaSO")as ElementoMagiaSO;

        for (int i = 0; i < m_listaValoriLancio.Count; i++)
        {
            elementoMultiploRipetizioni = m_listaValoriLancio.FindAll(f => f == m_listaValoriLancio[i]).Count;
            if (elementoMultiploRipetizioni >= 2)
            {
                elemMultiplo = m_listaValoriLancio[i];
                break;
            }
        }
        
        foreach (MagiaSO spell in listaTutteMagieLocale)
        {
            if (spell.combinazioneDiElementi.Count > m_listaValoriLancio.Count)
            {
                continue;
            }

            if (elemMultiplo!= null && elementoMultiploRipetizioni > 1)
            {
                if (spell.combinazioneDiElementi.FindAll(f => f == elemMultiplo).Count == elementoMultiploRipetizioni)
                {
                    if (elementoMultiploRipetizioni == m_listaValoriLancio.Count)
                    {
                        m_magiaDaLanciare = spell;
                        break;
                    }
                    else if (m_listaValoriLancio.Find(f => f != elemMultiplo) == spell.combinazioneDiElementi.Find(f=>f != elemMultiplo))
                    {
                        m_magiaDaLanciare = spell;
                        break;
                    }
                }
            }
            else
            {
                int corrispondenze;
                corrispondenze = 0;
                foreach (ElementoMagiaSO element in m_listaValoriLancio)
                {
                    if (spell.combinazioneDiElementi.Find(f => f == element))
                    {
                        corrispondenze++;
                    }
                }

                if (corrispondenze == m_listaValoriLancio.Count)
                {
                    m_magiaDaLanciare = spell;
                    break;
                }
            }                        
        }

        if (m_magiaDaLanciare == null)
        {
            Debug.LogWarning("Magia non creata");
            ClearElementList();
            UIelementiMagia.ClearUI();
            m_faseCorrente = FasiDiLancioMagia.AspettoComponimentoMagia;
            return;
        }
        m_faseCorrente = FasiDiLancioMagia.AspettandoLancioMagia;
        OnMagicComposed.Invoke();
        
    }


    private IEnumerator ComponiMagia()  //in teoria non serve
    {
        yield return null;

        if (true)
        {
            //bool isMagiaCheStaLanciandoConosciuta = false;
            //int counterCheck = 0;
            //List<MagiaSO> listaTutteMagieLocale = new List<MagiaSO>();
            //listaTutteMagieLocale = m_tuttaLaListaDelleMagie;
            //m_listaValoriLancio.OrderBy(x => x.tipoDiMagia);
            //listaTutteMagieLocale.OrderBy(x => x.name);
            ////m_listaDiQuelloCheIlMagoSa.OrderBy(x => x.name);
            //yield return new WaitForSeconds(timeBeforeShoot);

            //for (int i = 0; i < m_tuttaLaListaDelleMagie.Count; i++)
            //{
            //    for (int j = 0; j < m_listaDiQuelloCheIlMagoSa.Count; j++)
            //    {
            //        //bool isEqual = first.OrderBy(x => x).SequenceEqual(second.OrderBy(x => x));
            //        if (listaTutteMagieLocale[i].combinazioneDiElementi.SequenceEqual(m_listaDiQuelloCheIlMagoSa[j].combinazioneDiElementi))
            //        {
            //            for (int k = 0; k < m_listaValoriLancio.Count; k++)
            //            {
            //                /* TODO BUG: il sistema se si premono combinazioni diverse di tasti da qualsiasi magia essa prender� elementi dalle diverse magie che sta ciclando 
            //                    per poi prendere la l'ultima magia che sta ciclando e creare quella, se non ricordi bug fai test*/
            //                if (m_listaDiQuelloCheIlMagoSa[j].combinazioneDiElementi[k].tipoDiMagia == m_listaValoriLancio[k].tipoDiMagia)
            //                {
            //                    counterCheck++;
            //                }
            //                else
            //                {
            //                    break;
            //                }
            //            }
            //            if (counterCheck <= m_spellSlot && counterCheck != 0)
            //            {
            //                m_magiaDaLanciare = m_listaDiQuelloCheIlMagoSa[j];
            //                Debug.Log("Magia da lanciare: " + m_magiaDaLanciare);
            //                isMagiaCheStaLanciandoConosciuta = true;
            //                counterCheck = 0;
            //                break;
            //            }
            //        }
            //    }
            //}

            //if (isMagiaCheStaLanciandoConosciuta == true)
            //{
            //    m_faseCorrente = FasiDiLancioMagia.MagiaComposta;
            //    counterCheck = 0;
            //}
            //else
            //{
            //    m_listaValoriLancio.Clear();
            //    m_faseCorrente = FasiDiLancioMagia.AspettoComponimentoMagia;
            //    counterCheck = 0;
            //}
            //StopCoroutine(ComponiMagia());
            //yield return null;
        }

    }

    private void CheckIfPlayerWantToStartMagic()
    {
        if (m_listaValoriLancio.Count > 0)
        {
            m_faseCorrente = FasiDiLancioMagia.ComponendoMagia;
        }
    }

    private void Onfire(InputAction.CallbackContext obj)
    {
        Debug.Log("OnFire");
        if (m_listaValoriLancio.Count <= 0)
        {
            return;           
        }
        m_faseCorrente = FasiDiLancioMagia.MagiaComposta;
        DoSomethingOnMagicComposed();
    }

    public void CastSpell()
    {
        m_faseCorrente = FasiDiLancioMagia.LancioMagia;
        if (m_magiaDaLanciare.magicBehaviourType == TipoComportamentoMagia.Lanciata)
        {
            GameObject bullet;

            //var asset = AssetDatabase.FindAssets("Bullet", new[] { "Assets/Prefabs/Object" });
            var magia = Resources.Load("BulletPrefab/Bullet") as GameObject;
            //var magia = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(asset[0]));
            //var magia = Resources.LoadAll("")
            if (m_magiaDaLanciare.AlternativeBullet != null)
            {
                magia = m_magiaDaLanciare.AlternativeBullet;
            }

            if (PlayerCharacterController.playerFacingDirection == PlayerFacing.Destra)
            {
                bullet = Instantiate(magia, gameObject.transform.position + new Vector3(1, 0, 0), gameObject.transform.rotation);
                var animatorPrefabSpawned = Instantiate(m_magiaDaLanciare.prefabAnimatoriMagia, gameObject.transform.position + new Vector3(1, 0, 0), gameObject.transform.rotation);
                animatorPrefabSpawned.transform.position = Vector2.zero;
                animatorPrefabSpawned.transform.SetParent(bullet.transform, false);
                bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(m_magiaDaLanciare.velocitaMagiaLanciata*10, 0));
                if (m_magiaDaLanciare.rallentamentoGraduale)
                {
                    bullet.AddComponent<RallentaProiettile>().decelerationTime = m_magiaDaLanciare.decellerazioneTime;
                }

            }
            else
            {                
                bullet = Instantiate(magia, gameObject.transform.position + new Vector3(-1, 0, 0), gameObject.transform.rotation);
                var animatorPrefabSpawned = Instantiate(m_magiaDaLanciare.prefabAnimatoriMagia, gameObject.transform.position + new Vector3(-1, 0, 0), gameObject.transform.rotation);
                animatorPrefabSpawned.transform.position = Vector2.zero;
                animatorPrefabSpawned.transform.SetParent(bullet.transform, false);
                bullet.transform.localScale = new Vector3(-bullet.transform.localScale.x, bullet.transform.localScale.y, bullet.transform.localScale.z);
                bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(-m_magiaDaLanciare.velocitaMagiaLanciata*10, 0));
                if (m_magiaDaLanciare.rallentamentoGraduale)
                {
                    bullet.AddComponent<RallentaProiettile>().decelerationTime = m_magiaDaLanciare.decellerazioneTime;
                }
            }
          
            if (m_magiaDaLanciare.detonazioneAdImpatto)
            {
                bullet.AddComponent<InstatiateExplosion>().ExplosionPref = m_magiaDaLanciare.ExplosionPref;
                bullet.GetComponent<InstatiateExplosion>().explosionKnockbackForce = m_magiaDaLanciare.explosionKnockbackForce;
                bullet.GetComponent<InstatiateExplosion>().DamageContact = m_magiaDaLanciare.danneggiaTarget;
            }
            if (bullet.GetComponent<Animator>() != null)
            {
               
            }

            if (bullet.GetComponent<CircleCollider2D>() != null)
            { bullet.GetComponent<CircleCollider2D>().enabled = true; }

            bullet.AddComponent<DestroyOnTrigger>().SetLayer(m_magiaDaLanciare.ignoraCollisioni);
            bullet.AddComponent<DestroyOnTrigger>().damage = m_magiaDaLanciare.dannoDellaMagia;
            bullet.AddComponent<DestroyAfterDistance>().MaxDistance = m_magiaDaLanciare.distanzaMagiaLanciata;
            bullet.AddComponent<DestroyAfterTime>().destroyAfterTime(m_magiaDaLanciare.tempoMagiaLanciata);
        }
        
        m_magiaDaLanciare = null;
        ClearElementList();
        UIelementiMagia.ClearUI();
        m_faseCorrente = FasiDiLancioMagia.AspettoComponimentoMagia;

    }

    private void ClearElementList()
    {
        m_listaValoriLancio.Clear();
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

