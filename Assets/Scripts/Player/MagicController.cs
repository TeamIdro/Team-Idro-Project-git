using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MagicController : MonoBehaviour, ISubscriber
{
    [SerializeField] private float timeBeforeShoot;
    [SerializeField] private GameObject m_UIPrefab;
    [SerializeField] [Range(1, 3)] private int m_spellSlot;
    [SerializeField] private GameObject m_basePrefabToShootForCombination;
    [SerializeField, ReadOnly] private FasiDiLancioMagia m_faseCorrente = FasiDiLancioMagia.AspettoComponimentoMagia;
    [SerializeField, ReadOnly] private List<MagiaSO> magieDiLivelloUno;
    [SerializeField, ReadOnly] private List<MagiaSO> magieDiLivelloDue;
    [SerializeField, ReadOnly] private List<MagiaSO> magieDiLivelloTre;
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
    private int linePoints = 1;
    
    private void Awake()
    {
        UIelementiMagia = m_UIPrefab.GetComponent<UIElementiMagia>();
        m_gamePlayInput = new GamePlayInputActions();
        
        
        var magicTakenFromFolderLv1 = Resources.LoadAll<MagiaSO>("Data/MagiaSO/Combinazione/LV1");
        var magicTakenFromFolderLv2 = Resources.LoadAll<MagiaSO>("Data/MagiaSO/Combinazione/LV2");
        var magicTakenFromFolderLv3 = Resources.LoadAll<MagiaSO>("Data/MagiaSO/Combinazione/LV3");
        foreach (var magia in magicTakenFromFolderLv1)
        {
            var tempMagia = (MagiaSO)magia;
            magieDiLivelloUno.Add(tempMagia);
        }
        foreach (var magia in magicTakenFromFolderLv2)
        {
            var tempMagia = (MagiaSO)magia;
            magieDiLivelloDue.Add(tempMagia);
        }
        foreach (var magia in magicTakenFromFolderLv3)
        {
            var tempMagia = (MagiaSO)magia;
            magieDiLivelloTre.Add(tempMagia);
        }
        var elementsTakenFromFolder = Resources.LoadAll("Data/MagiaSO/Elementi", typeof(ElementoMagiaSO));
       
        foreach (var item in elementsTakenFromFolder)
        {
            var elementTemp = item as ElementoMagiaSO;
            // Debug.Log(item);
            m_elementiDaPrendere.Add(elementTemp);
        }
        for (int i = 0; i < magieDiLivelloUno.Count; i++)
        {
            magieDiLivelloUno.OrderBy(x => x.combinazioneDiElementi);
            magieDiLivelloUno[i].combinazioneDiElementi.OrderBy(x => x.tipoDiMagia);
        }
        for (int i = 0; i < magieDiLivelloDue.Count; i++)
        {
            magieDiLivelloDue.OrderBy(x => x.combinazioneDiElementi);
            magieDiLivelloDue[i].combinazioneDiElementi.OrderBy(x => x.tipoDiMagia);
        }
        for (int i = 0; i < magieDiLivelloTre.Count; i++)
        {
            magieDiLivelloTre.OrderBy(x => x.combinazioneDiElementi);
            magieDiLivelloTre[i].combinazioneDiElementi.OrderBy(x => x.tipoDiMagia);
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


    private void DoSomethingOnMagicComposed()
    {
        m_listaValoriLancio.OrderBy(x => x.tipoDiMagia);
        List<MagiaSO> listaTutteMagieLocale = new List<MagiaSO>();
        listaTutteMagieLocale = magieDiLivelloUno.Concat(magieDiLivelloDue).Concat(magieDiLivelloTre).ToList();
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
        
        if (m_listaValoriLancio.Count <= 0) {
            CastZeroCombinationSpell();
        }
        else
        {
            m_faseCorrente = FasiDiLancioMagia.MagiaComposta;
            DoSomethingOnMagicComposed();
            CastCombinationSpell();
        }
    }

    private void CastZeroCombinationSpell()
    {
        var magia = Resources.Load("BulletPrefab/Bullet_No_Combination") as GameObject;
        m_magiaDaLanciare = Resources.Load("Data/MagiaSO/Colpo_Base") as MagiaSO;

        Debug.Log("COLPO BASE LANCIATO");
        CastMagiaLanciata(magia);
    }
        
    public void CastCombinationSpell()
    {
        m_faseCorrente = FasiDiLancioMagia.LancioMagia;
        var magia = Resources.Load("BulletPrefab/Bullet_For_Combination") as GameObject;
        m_magiaDaLanciare.ApplicaEffettoAMago(this);
        m_magiaDaLanciare.TogliEffettoAMago(this);
        if (m_magiaDaLanciare.magicBehaviourType == TipoComportamentoMagia.Lanciata)
        {
            CastMagiaLanciata(magia);
        }
        else if(m_magiaDaLanciare.magicBehaviourType == TipoComportamentoMagia.Stazionaria)
        {
            CastMagiaStazionaria(magia);
        }
        else if(m_magiaDaLanciare.magicBehaviourType == TipoComportamentoMagia.LineCast)
        {
            CastMagiaLineCast();
        }

        m_magiaDaLanciare = null;
        ClearElementList();
        UIelementiMagia.ClearUI();
        m_faseCorrente = FasiDiLancioMagia.AspettoComponimentoMagia;

    }




    private void CastMagiaLineCast()
    {
        Vector2 firePointPosition = gameObject.transform.position;
        Vector2 endPosition;
        if (PlayerCharacterController.playerFacingDirections == PlayerFacingDirections.Right)
        {
            endPosition = firePointPosition + (Vector2)gameObject.transform.right * m_magiaDaLanciare.lunghezzaLineCast;
        }
        else
        {
            endPosition = firePointPosition - (Vector2)gameObject.transform.right * m_magiaDaLanciare.lunghezzaLineCast;
        }
        RaycastHit2D hit = Physics2D.Linecast(firePointPosition, endPosition,m_magiaDaLanciare.layerMaskPerDanneggiaTarget);
        Debug.DrawLine(firePointPosition, endPosition,Color.red);
        Debug.Log(hit.collider);
        LineRenderer lr = m_basePrefabToShootForCombination.AddComponent<LineRenderer>();
        lr.enabled = true;
        var distance = hit.distance;
        if(distance == 0)
        {
            distance = m_magiaDaLanciare.lunghezzaLineCast;
        }
        for (int i = 0; i < linePoints; i++)
        {
            var pos = lr.GetPosition(i);

            //Debug.Log(rayEnd.x);
            pos.x = (distance / linePoints * i) + UnityEngine.Random.Range(-.4f, .4f);
            pos.y += UnityEngine.Random.Range(-.4f, .4f);

            lr.SetPosition(i, pos);
        }
        if (distance < m_magiaDaLanciare.lunghezzaLineCast)
        {
            lr.SetPosition(linePoints, new Vector2(distance, 0f));
        }
        else
        {
            lr.SetPosition(linePoints, new Vector2(m_magiaDaLanciare.lunghezzaLineCast, 0f));

        }
    }

    private void CastMagiaStazionaria(GameObject magia)
    {
        GameObject bullet = IstanziaMagiaEPrendiIlComponent(magia);
        CheckIfThereIsParticleAndGetIt(bullet);
        StaticMagicInitialize(bullet);
        OnMagicCasted.Invoke();
    }

   
    private void CastMagiaLanciata(GameObject magia)
    {
        GameObject bullet = IstanziaMagiaEPrendiIlComponent(magia);
        CheckForDirectionToGo(bullet);
        CheckIfThereIsParticleAndGetIt(bullet);
        ThrowedMagicInitialize(bullet);
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
    private void CheckIfThereIsParticleAndGetIt(GameObject bullet)
    {
        GameObject animatorPrefabSpawned;
        if (m_magiaDaLanciare.prefabParticleMagia != null)
        {
            animatorPrefabSpawned = Instantiate(m_magiaDaLanciare.prefabParticleMagia, gameObject.transform.position + new Vector3(-1, 0, 0), gameObject.transform.rotation);
            animatorPrefabSpawned.transform.position = Vector2.zero;
            animatorPrefabSpawned.transform.SetParent(bullet.transform, false);
            if (animatorPrefabSpawned.GetComponent<ParticleSystem>() != null)
            {
                ParticleSystem ps = animatorPrefabSpawned.GetComponent<ParticleSystem>();
                ps.Play();
            }
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
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, m_magiaDaLanciare.velocitaDellaMagiaLanciata * direction.y * 10));
            if(direction.y < 0) { bullet.transform.Rotate(0, 0, 90); }
            else if(direction.y > 0) { bullet.transform.Rotate(0, 0, -90); }
            return;
        }
        else if (direction.x != 0)
        {
            bullet.GetComponent<Rigidbody2D>().AddForce(new Vector2(m_magiaDaLanciare.velocitaDellaMagiaLanciata * direction.x * 10, 0));
            //if (direction.x < 0) { bullet.transform.Rotate(0, 0, -90); }
             if (direction.x > 0) { bullet.transform.Rotate(0, 0, 180); }
            return;
        }


    }

    private void StaticMagicInitialize(GameObject bullet)
    {
        if (bullet.GetComponent<CircleCollider2D>() != null)
        { bullet.GetComponent<CircleCollider2D>().enabled = true; }
        magiaComponent.magia = m_magiaDaLanciare;
        magiaComponent.SetIgnoreLayer(m_magiaDaLanciare.layerMaskPerIgnoraCollisioni);
        magiaComponent.DestroyAfterTime(m_magiaDaLanciare.tempoMagiaLanciata);
        magiaComponent.SetDamageLayer(m_magiaDaLanciare.layerMaskPerDanneggiaTarget);
    }
    private void ThrowedMagicInitialize(GameObject bullet)
    {
        if (m_magiaDaLanciare.rallentamentoGraduale is not true)
        {
            magiaComponent.decelerationTime = m_magiaDaLanciare.tempoDecellerazioneMagiaLanciata;
        }
        magiaComponent.magia = m_magiaDaLanciare;
        if (m_magiaDaLanciare.detonazioneAdImpatto is true)
        {
            magiaComponent.ExplosionPref = m_magiaDaLanciare.ExplosionPref;
            magiaComponent.explosionKnockbackForce = m_magiaDaLanciare.knockbackForzaEsplosione;
            magiaComponent.damageMask = m_magiaDaLanciare.layerMaskPerDanneggiaTarget;
        }
        if (m_magiaDaLanciare.distanzaMagiaLanciata is not 0)
        {
            magiaComponent.MaxDistance = m_magiaDaLanciare.distanzaMagiaLanciata;
        }
        if (bullet.GetComponent<CircleCollider2D>() != null)
        { bullet.GetComponent<CircleCollider2D>().enabled = true; }

        magiaComponent.SetIgnoreLayer(m_magiaDaLanciare.layerMaskPerIgnoraCollisioni);
        magiaComponent.DestroyAfterTime(m_magiaDaLanciare.tempoMagiaLanciata);
        magiaComponent.SetDamageLayer(m_magiaDaLanciare.layerMaskPerDanneggiaTarget);
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









