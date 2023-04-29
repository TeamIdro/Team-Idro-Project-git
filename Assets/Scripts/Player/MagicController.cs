using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MagicController : MonoBehaviour, ISubscriber
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
    public UnityEvent OnMagicCasted;

    private GamePlayInputActions m_gamePlayInput;
    private Dictionary<InputAction, TipoMagia> m_dizionariElementi = new Dictionary<InputAction, TipoMagia>();
    private List<ElementoMagiaSO> m_listaValoriLancio = new List<ElementoMagiaSO>();
    private MagiaSO m_magiaDaLanciare;
    private Magia magiaComponent;
    private UIElementiMagia UIelementiMagia;
    private int m_facingDirectionForLeftAndRight = 0;
    private int m_facingDirectionForUpAndDown = 0;
    private bool magicIsBlocked = false;

    private void Awake()
    {
        UIelementiMagia = m_UIPrefab.GetComponent<UIElementiMagia>();
        m_gamePlayInput = new GamePlayInputActions();
        
        
        var magicTakenFromFolder = Resources.LoadAll<MagiaSO>("Data/MagiaSO/Combinazione");
        foreach (var magia in magicTakenFromFolder)
        {
            var tempMagia = (MagiaSO)magia;
            // Debug.Log(tempMagia);
            m_tuttaLaListaDelleMagie.Add(tempMagia);
        }
        var elementsTakenFromFolder = Resources.LoadAll("Data/MagiaSO/Elementi", typeof(ElementoMagiaSO));
       
        foreach (var item in elementsTakenFromFolder)
        {
            var elementTemp = item as ElementoMagiaSO;
            // Debug.Log(item);
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
        if(magicIsBlocked == true) return;
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
            // Debug.LogWarning("Magia non creata");
            ClearElementList();
            UIelementiMagia.ClearUI();
            m_faseCorrente = FasiDiLancioMagia.AspettoComponimentoMagia;
            return;
        }
        m_faseCorrente = FasiDiLancioMagia.AspettandoLancioMagia;
        OnMagicComposed.Invoke();
        
    }

    
    
    private void CheckIfPlayerWantToStartMagic()
    {
        if (m_listaValoriLancio.Count > 0) { m_faseCorrente = FasiDiLancioMagia.ComponendoMagia; }
       
    }

    private void Onfire(InputAction.CallbackContext obj)
    {
        if (m_listaValoriLancio.Count <= 0) { return; }
        m_faseCorrente = FasiDiLancioMagia.MagiaComposta;
        DoSomethingOnMagicComposed();
    }

    public void CastSpell()
    {
        m_faseCorrente = FasiDiLancioMagia.LancioMagia;
        if (m_magiaDaLanciare.magicBehaviourType == TipoComportamentoMagia.Lanciata)
        {
            CastMagiaLanciata();
        }
        else if(m_magiaDaLanciare.magicBehaviourType == TipoComportamentoMagia.Stazionaria)
        {
            CastMagiaStazionaria();
        }
        else if(m_magiaDaLanciare.magicBehaviourType == TipoComportamentoMagia.Teleport)
        {
            //TODO?: CastMagiaTeleport();
        }
        
        m_magiaDaLanciare = null;
        ClearElementList();
        UIelementiMagia.ClearUI();
        m_faseCorrente = FasiDiLancioMagia.AspettoComponimentoMagia;

    }

    private void CastMagiaStazionaria()
    {
        var magia = Resources.Load("BulletPrefab/Bullet") as GameObject;
        GameObject bullet = IstanziaMagiaEPrendiIlComponent(magia);
        CheckIfThereIsAnimatorAndGetIt(bullet);
        GenericMagicInitialize(bullet);
        StaticMagicInitialize(bullet);
        OnMagicCasted.Invoke();
    }

   
    private void CastMagiaLanciata()
    {
        var magia = Resources.Load("BulletPrefab/Bullet") as GameObject;
        if (m_magiaDaLanciare.AlternativeBullet != null)
        {
            magia = m_magiaDaLanciare.AlternativeBullet;
        }
        GameObject bullet = IstanziaMagiaEPrendiIlComponent(magia);
        CheckForDirectionToGo(bullet);
        CheckIfThereIsAnimatorAndGetIt(bullet);
        GenericMagicInitialize(bullet);
        OnMagicCasted.Invoke();
    }

    

    

  

    private void ClearElementList()
    {
        m_listaValoriLancio.Clear();
    }

    private void OnEnable() => m_gamePlayInput.Mage.Enable();









    //FUNZIONI PER AVER IL CODICE PIU PULITO

    /// <summary>
    /// Cerca e setta l'animator nell'object che verrà istanziato solo se lo contiene
    /// </summary>
    /// <param name="bullet"></param>
    private void CheckIfThereIsAnimatorAndGetIt(GameObject bullet)
    {
        GameObject animatorPrefabSpawned;
        if (m_magiaDaLanciare.prefabAnimatoriMagia != null)
        {
            animatorPrefabSpawned = Instantiate(m_magiaDaLanciare.prefabAnimatoriMagia, gameObject.transform.position + new Vector3(-1, 0, 0), gameObject.transform.rotation);
            animatorPrefabSpawned.transform.position = Vector2.zero;
            animatorPrefabSpawned.transform.SetParent(bullet.transform, false);
        }
    }
    /// <summary>
    /// Metodo in cui viene creata l'object su cui andra la magia
    /// </summary>
    /// <param name="magia"></param>
    /// <returns></returns>
    private GameObject IstanziaMagiaEPrendiIlComponent(GameObject magia)
    {
        GameObject bullet = Instantiate(magia, gameObject.transform.position + new Vector3(m_facingDirectionForLeftAndRight, 0, 0), gameObject.transform.rotation);
        if (bullet.GetComponent<Magia>() != null) { magiaComponent = bullet.GetComponent<Magia>(); }
        else
        {
            bullet.AddComponent<Magia>();
            magiaComponent = bullet.GetComponent<Magia>();
        }

        return bullet;
    }
    /// <summary>
    /// Setta le direzioni che il proiettile deve prendere salvandosele poi in float locali
    /// </summary>
    private void CheckForDirectionToGo(GameObject bullet)
    {

            switch (PlayerCharacterController.playerFacingDirections)
            {
                case PlayerFacingDirections.Left:
                    AddForceToMagicBasedOnDirection(bullet, Vector2.left);
                    break;
                case PlayerFacingDirections.Right:
                    AddForceToMagicBasedOnDirection(bullet, Vector2.right);
                    break;
                case PlayerFacingDirections.Up:
                    AddForceToMagicBasedOnDirection(bullet, Vector2.up);
                    break;
                case PlayerFacingDirections.Down:
                    AddForceToMagicBasedOnDirection(bullet, Vector2.down);
                    break;
            case PlayerFacingDirections.ZeroForLookUpandDown:
                AddForceToMagicBasedOnDirection(bullet, new Vector2(bullet.GetComponent<Rigidbody2D>().velocity.x, 0));
                break;
            default:
                    break;
            }
       
    }
    private void AddForceToMagicBasedOnDirection(GameObject bullet, Vector2 direction)
    {
        bullet.transform.localScale = new Vector3(-bullet.transform.localScale.x, bullet.transform.localScale.y, bullet.transform.localScale.z);
        if (direction.y != 0)
        {
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, m_magiaDaLanciare.velocitaMagiaLanciata * direction.y * 10));
            if(direction.y < 0) { bullet.transform.Rotate(0, 0, 90); }
            else if(direction.y > 0) { bullet.transform.Rotate(0, 0, -90); }
            return;
        }
        else if (direction.x != 0)
        {
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(m_magiaDaLanciare.velocitaMagiaLanciata * direction.x * 10, 0));
            //if (direction.x < 0) { bullet.transform.Rotate(0, 0, -90); }
             if (direction.x > 0) { bullet.transform.Rotate(0, 0, 180); }
            return;
        }


    }
    /// <summary>
    /// Inizializza l'object magia usando le variabili dello scriptableObject MagiaSO
    /// </summary>
    /// <param name="bullet"></param>
    private void GenericMagicInitialize(GameObject bullet)
    {
        if (m_magiaDaLanciare.rallentamentoGraduale is true)
        {
            magiaComponent.decelerationTime = m_magiaDaLanciare.decellerazioneTime;
        }
        magiaComponent.magia = m_magiaDaLanciare;
        if (m_magiaDaLanciare.detonazioneAdImpatto is true)
        {
            magiaComponent.ExplosionPref = m_magiaDaLanciare.ExplosionPref;
            magiaComponent.explosionKnockbackForce = m_magiaDaLanciare.explosionKnockbackForce;
            magiaComponent.damageMask = m_magiaDaLanciare.danneggiaTarget;
        }
        if(m_magiaDaLanciare.distanzaMagiaLanciata is not 0)
        {
            magiaComponent.MaxDistance = m_magiaDaLanciare.distanzaMagiaLanciata;
        }


        
    }
    private void StaticMagicInitialize(GameObject bullet)
    {
        if (bullet.GetComponent<CircleCollider2D>() != null)
        { bullet.GetComponent<CircleCollider2D>().enabled = true; }

        magiaComponent.SetIgnoreLayer(m_magiaDaLanciare.ignoraCollisioni);
        magiaComponent.DestroyAfterTime(m_magiaDaLanciare.tempoMagiaLanciata);
        magiaComponent.SetDamageLayer(m_magiaDaLanciare.danneggiaTarget);
    }


    public void OnPublish(IMessage message)
    {
        if(message is StopOnOpenPauseMessage)
        {
            magicIsBlocked = true;
        }
        else if(message is StartOnClosedPauseMessage)
        {
            magicIsBlocked = false;
        }
    }
}
public enum FasiDiLancioMagia
{
    AspettoComponimentoMagia,
    ComponendoMagia,
    MagiaComposta,
    AspettandoLancioMagia,
    LancioMagia,

}









