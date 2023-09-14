using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MagicController : MonoBehaviour, ISubscriber
{
    [SerializeField] private float timeBeforeShoot;
    [SerializeField] private GameObject m_UIPrefab;
    [SerializeField][Range(1, 3)] private int m_spellSlot;
    [SerializeField] private GameObject m_basePrefabToShootForCombination;
    [SerializeField, ReadOnly] private FasiDiLancioMagia m_faseCorrente = FasiDiLancioMagia.AspettoComponimentoMagia;
    [SerializeField] private List<MagiaSO> listaMagieDisponibili;
    [Tooltip("Se spuntato fa si che se la lista degli elementi � piena e provi ad inserirne uno nuovo viene buttato fuori il primo elemento della lista per fare spazio, se non � spuntato una volta che la lista � piena non si potr� pi� aggiungere elementi")]
    [SerializeField] private bool lastInFirstOut = true;
    [Space(15)]
    //[SerializeField]  public int livCatalizzatore = 1;
    //[SerializeField] private List<MagiaSO> m_listaDiQuelloCheIlMagoSa;
    [SerializeField] private List<ElementoMagiaSO> m_elementiDaPrendere;
    public UnityEvent OnMagicComposed;
    public UnityEvent OnMagicCasted;

    private Dictionary<MagiaSO, float> cooldowns = new Dictionary<MagiaSO, float>();


    private GamePlayInputActions m_gamePlayInput;
    private Dictionary<InputAction, TipoMagia> m_dizionariElementi = new Dictionary<InputAction, TipoMagia>();
    private List<ElementoMagiaSO> m_listaValoriLancio = new List<ElementoMagiaSO>();
    private MagiaSO m_magiaDaLanciare;
    private Magia magiaComponent;
    private UIElementiMagia UIelementiMagia;
    private int m_facingDirectionForLeftAndRight = 0;
    private int m_facingDirectionForUpAndDown = 0;
    private bool magicIsBlocked = false;
    private int linePoints = 0;
    private AudioSource m_playerAudioSource;
    private void Awake()
    {
        UIelementiMagia = m_UIPrefab.GetComponent<UIElementiMagia>();
        m_gamePlayInput = new GamePlayInputActions();
        m_playerAudioSource = gameObject.GetComponent<AudioSource>();

        var elementsTakenFromFolder = Resources.LoadAll("Data/MagiaSO/Elementi", typeof(ElementoMagiaSO));

        foreach (var item in elementsTakenFromFolder)
        {
            var elementTemp = item as ElementoMagiaSO;
            m_elementiDaPrendere.Add(elementTemp);
        }
        for (int i = 0; i < listaMagieDisponibili.Count; i++)
        {
            listaMagieDisponibili.OrderBy(x => x.combinazioneDiElementi);
            listaMagieDisponibili[i].combinazioneDiElementi.OrderBy(x => x.tipoDiMagia);
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
        if (magicIsBlocked == true) return;
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

    public void Update()
    {
        UpdateCooldowns();
        if (m_faseCorrente == FasiDiLancioMagia.AspettoComponimentoMagia)
        {
            CheckIfPlayerWantToStartMagic();
        }
    }


    private void DoSomethingOnMagicComposed()
    {
        m_listaValoriLancio.OrderBy(x => x.tipoDiMagia);
        List<MagiaSO> listaTutteMagieLocale = new List<MagiaSO>();
        listaTutteMagieLocale = listaMagieDisponibili;
        listaTutteMagieLocale.OrderBy(x => x.name);
        int elementoMultiploRipetizioni = 0;
        ElementoMagiaSO elemMultiplo = ScriptableObject.CreateInstance("MagiaSO") as ElementoMagiaSO;

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

            if (elemMultiplo != null && elementoMultiploRipetizioni > 1)
            {
                if (spell.combinazioneDiElementi.FindAll(f => f == elemMultiplo).Count == elementoMultiploRipetizioni)
                {
                    if (elementoMultiploRipetizioni == m_listaValoriLancio.Count)
                    {
                        m_magiaDaLanciare = spell;
                        break;
                    }
                    else if (m_listaValoriLancio.Find(f => f != elemMultiplo) == spell.combinazioneDiElementi.Find(f => f != elemMultiplo))
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
        if (m_listaValoriLancio.Count <= 0)
        {
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
        if (!IsOnCooldown(m_magiaDaLanciare))
        {
            Debug.Log("COLPO BASE LANCIATO");
            CastMagiaLanciata(magia);
            float cooldownTime = m_magiaDaLanciare.tempoDiAttesaDellaMagia; // Imposta il tempo di cooldown desiderato
            SetCooldown(m_magiaDaLanciare, cooldownTime);
            
        }
    }

    public void CastCombinationSpell()
    {
        ClearElementList();
        UIelementiMagia.ClearUI();
        if (m_magiaDaLanciare != null)
        {
            if (!IsOnCooldown(m_magiaDaLanciare))
            {
                m_faseCorrente = FasiDiLancioMagia.LancioMagia;
                var magia = Resources.Load("BulletPrefab/Bullet_For_Combination") as GameObject;
                m_magiaDaLanciare.ApplicaEffettoAMago(this);
                m_magiaDaLanciare.TogliEffettoAMago(this);
                m_magiaDaLanciare.PlayCastingSound(m_playerAudioSource);
                float cooldownTime = m_magiaDaLanciare.tempoDiAttesaDellaMagia; // Imposta il tempo di cooldown desiderato
                SetCooldown(m_magiaDaLanciare, cooldownTime);
                if (m_magiaDaLanciare.magicBehaviourType == TipoComportamentoMagia.Lanciata)
                {
                    CastMagiaLanciata(magia);
                }
                else if (m_magiaDaLanciare.magicBehaviourType == TipoComportamentoMagia.Stazionaria)
                {
                    CastMagiaStazionaria(magia);
                }
                else if (m_magiaDaLanciare.magicBehaviourType == TipoComportamentoMagia.LineCast)
                {
                    CastMagiaLineCast(magia);
                }

                m_magiaDaLanciare = null;
                m_faseCorrente = FasiDiLancioMagia.AspettoComponimentoMagia;

            }
        }
        else { return; }

    }




    private void CastMagiaLineCast(GameObject magia)
    {
        GameObject bullet = IstanziaMagiaEPrendiIlComponent(magia);
        CheckIfThereIsParticleAndGetIt(bullet);
        StaticMagicInitialize(magia);
        Vector2 firePointPosition = gameObject.transform.position;
        Vector2 endPosition;
        Debug.Log(PlayerCharacterController.playerFacingDirections);
        if (PlayerCharacterController.playerFacingDirections == PlayerFacingDirections.Right)
        {
            endPosition = firePointPosition + (Vector2)gameObject.transform.right * m_magiaDaLanciare.lunghezzaLineCast;
        }
        else
        {
            endPosition = firePointPosition + (Vector2)gameObject.transform.right * (-m_magiaDaLanciare.lunghezzaLineCast);
        }
        RaycastHit2D hit = Physics2D.Linecast(firePointPosition, endPosition, m_magiaDaLanciare.layerMaskPerDanneggiaTarget);
        Debug.DrawLine(firePointPosition, endPosition, Color.red);
        LineRenderer lr = bullet.GetComponentInChildren<LineRenderer>();
        lr.enabled = true;
        Debug.Log(hit.collider);
        linePoints = lr.positionCount - 1;
        var distance = hit.distance;
        if (distance == 0)
        {
            distance = m_magiaDaLanciare.lunghezzaLineCast;
        }
        for (int i = 0; i < linePoints; i++)
        {

            Vector3 pos = lr.GetPosition(i);
            float t = (float)i / (linePoints - 1);  // Valore normalizzato tra 0 e 1
            pos.x = (distance / linePoints * i) + UnityEngine.Random.Range(-.4f, .4f);
            if (PlayerCharacterController.playerFacingDirections == PlayerFacingDirections.Right)
                pos.x = -pos.x;
            pos.y += UnityEngine.Random.Range(-m_magiaDaLanciare.YNoise, m_magiaDaLanciare.YNoise);
            pos.z = 0;
            Debug.Log("settato all'indice: " + i + "\n con valore: " + pos);
            lr.SetPosition(i, pos);
        }
        if (distance < m_magiaDaLanciare.lunghezzaLineCast)
        {

            var finalPosition = new Vector2((PlayerCharacterController.playerFacingDirections == PlayerFacingDirections.Left) ? distance : -distance, 0);

            lr.SetPosition(linePoints, finalPosition);
            if (m_magiaDaLanciare.explosionPref != null)
            {
                var obj = Instantiate(m_magiaDaLanciare.explosionPref, (Vector3)hit.point, Quaternion.identity);
                Destroy(obj, 6);
            }
        }
        else
        {
            var finalPosition = new Vector2((PlayerCharacterController.playerFacingDirections == PlayerFacingDirections.Left) ? m_magiaDaLanciare.lunghezzaLineCast : -m_magiaDaLanciare.lunghezzaLineCast, 0);
            lr.SetPosition(linePoints, finalPosition);
            if (m_magiaDaLanciare.explosionPref != null)
            {
                var obj = Instantiate(m_magiaDaLanciare.explosionPref, endPosition, Quaternion.identity);
                Destroy(obj, 6);
            }
        }
        StartCoroutine(DisableLine(bullet));
    }




    public IEnumerator DisableLine(GameObject bullet)
    {
        LineRenderer lr = bullet.GetComponentInChildren<LineRenderer>();
        yield return new WaitForSeconds(0.2f);
        lr.enabled = false;
        for (int i = 1; i < linePoints; i++)
        {
            var pos = lr.GetPosition(i);
            pos.x = i;
            pos.y = 0f;
            lr.SetPosition(i, pos);
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
            if (direction.y < 0) { bullet.transform.Rotate(0, 0, 90); }
            else if (direction.y > 0) { bullet.transform.Rotate(0, 0, -90); }
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
            magiaComponent.explosionPref = m_magiaDaLanciare.explosionPref;
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
        if (message is StopOnOpenPauseMessage)
        {
            magicIsBlocked = true;
        }
        else if (message is StartOnClosedPauseMessage)
        {
            magicIsBlocked = false;
        }
    }
    public void AggiungiMagiaAllaLista(MagiaSO magiaSO)
    {
        if (listaMagieDisponibili.Find(x => x == magiaSO) == null)
        {
            cooldowns.Clear();
            listaMagieDisponibili.Add(magiaSO);
            foreach (MagiaSO magiaSOf in listaMagieDisponibili)
            {
                cooldowns[magiaSOf] = 0f;
            }
        }
    }
    private bool IsOnCooldown(MagiaSO magiaSO)
    {
        return cooldowns.ContainsKey(magiaSO) && cooldowns[magiaSO] > 0f;
    }

    private void SetCooldown(MagiaSO magiaSO, float cooldownTime)
    {
        cooldowns[magiaSO] = cooldownTime;
    }

    private void UpdateCooldowns()
    {
        foreach (MagiaSO magiaSO in cooldowns.Keys.ToList())
        {
            if (cooldowns[magiaSO] > 0f)
            {
                cooldowns[magiaSO] -= Time.deltaTime;
                if (cooldowns[magiaSO] < 0f)
                {
                    cooldowns[magiaSO] = 0f;
                }
            }
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