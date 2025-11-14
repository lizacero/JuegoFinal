using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Enemy : MonoBehaviour, Daniable
{
    private NavMeshAgent agent;
    private Player target;
    private Animator anim;
    private Rigidbody rb;
    [SerializeField] private Transform puntoDrop;
    //private bool delay;

    [Header("Sistema de combate")]
    [SerializeField] private Transform puntoAtaque;
    [SerializeField] private float radioAtaque;
    [SerializeField] private float danioAtaque;
    private float vidaEnemigo = 20;
    private AudioSource ataque;
    

    [Header("Sistema de caída")]
    private bool enSuelo = false;
    private bool haIniciadoBusqueda = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        ataque = GetComponent<AudioSource>();

        agent.enabled = false;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        target = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {

        if (enSuelo && !haIniciadoBusqueda)
        {
            IniciarBusqueda();
        }
        else if (haIniciadoBusqueda)
        {
            PerseguirPlayer();
        }
    }

    private void IniciarBusqueda()
    {
        if (target == null)
            target = FindAnyObjectByType<Player>();

        agent.enabled = true;

        if (agent.isOnNavMesh && target != null)
        {
            agent.SetDestination(target.transform.position);
            haIniciadoBusqueda = true;
        }
    }

    private void PerseguirPlayer()
    {
        if (target == null || !agent.enabled) return;

        anim.SetBool("walking", true);
        agent.SetDestination(target.transform.position);

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            EnfocarOnjetivo();
            LanzarAtaque();
        }
    }

    private void EnfocarOnjetivo()
    {
        Vector3 direccionAObjetivo = (target.transform.position - transform.position).normalized;
        direccionAObjetivo.y = 0;
        Quaternion rotacionAObjetivo = Quaternion.LookRotation(direccionAObjetivo);
        transform.rotation = rotacionAObjetivo;
    }

    private void LanzarAtaque()
    {
        agent.isStopped = true;
        anim.SetBool("attacking", true);
    }

    private void Atacar()  //Se llama en el evento de la animación
    {
        ataque.Play();
        Collider[] colliderTocados = Physics.OverlapSphere(puntoAtaque.position, radioAtaque);
        foreach (Collider coll in colliderTocados)
        {
            if (coll.CompareTag("Player"))
            {
                if (coll.TryGetComponent(out Daniable danhable))
                {
                    danhable.RecibirDanio(danioAtaque);
                }
            }
        }
    }

    private void FinDeAtaque()  //Se llama en el evento de la animación
    {
        agent.isStopped = false;
        anim.SetBool("attacking", false);
    }

    public void RecibirDanio(float danio)
    {
        vidaEnemigo -= danio;
        if (vidaEnemigo <= 0)
        {
            //elay =true;
            agent.isStopped = true;
            Destroy(GetComponent<CapsuleCollider>());
            anim.SetTrigger("died");

            if (GameManager.instance != null)
            {
                GameManager.instance.EnemigoEliminado();
            }
            
            int random = UnityEngine.Random.Range(0, 5);
            Instantiate(GameManager.instance.ObjetosDrop[random],puntoDrop.position,Quaternion.identity);
            StartCoroutine(DelayMuerte());

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Detectar cuando toca el suelo
        if (!enSuelo && collision.gameObject.layer == LayerMask.NameToLayer("Suelo"))
        {
            enSuelo = true;

            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoAtaque.position, radioAtaque);
    }

    private IEnumerator DelayMuerte()
    {
        Debug.Log("Entré a la corrutina");
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
