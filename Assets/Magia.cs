using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magia : MonoBehaviour
{

    public MagiaSO magia;
    public bool isCasted = false;
    public Animator animator;
    private PlayerFacing playerFacingOnInstance;


    private SpriteRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        InitializeMagic();
    }

    private void InitializeMagic()
    {
        playerFacingOnInstance = PlayerCharacterController.playerFacingDirection;
        animator.runtimeAnimatorController = magia.animatorMagia;
      
    }

    // Update is called once per frame
    void Update()
    {
        LanciaMagia();
    }
    public void LanciaMagia()
    {
        switch(magia.GetBehaviourType())
        {
            case TipoComportamentoMagia.Stazionaria:
                LancioMagiaStazionaria();
                break;

            case TipoComportamentoMagia.Lanciata:
                LancioMagiaLanciata();
                break;
            case TipoComportamentoMagia.Teleport:
                LancioMagiaTeleport();
                break;

        }
    }

    private void LancioMagiaTeleport()
    {
        throw new NotImplementedException();
    }

    private void LancioMagiaLanciata()
    {
        if (!isCasted) return;
        if (playerFacingOnInstance == PlayerFacing.Sinistra)
        {
            transform.Translate(Vector2.left * Time.deltaTime * magia.velocit‡MagiaLanciata);
            renderer.flipX= true;
        }
        else
        {
            transform.Translate(Vector2.right * Time.deltaTime * magia.velocit‡MagiaLanciata);
            renderer.flipX= false;
        }
        StartCoroutine(DistruggiDopoSecondi(1f));

    }

    private IEnumerator DistruggiDopoSecondi(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void LancioMagiaStazionaria()
    {
        throw new NotImplementedException();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isCasted = false;
        if(magia.GetBehaviourType() == TipoComportamentoMagia.Lanciata)
        {
            if (collision.collider != false)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnDisable()
    {
        magia = null;
    }
}
