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
    private float vidaEnemigo = 60;
    [SerializeField] private AudioSource respiracion;
    [SerializeField] private AudioSource ataque;
    private float distanciaReal;


    [Header("Sistema de caída")]
    private bool enSuelo = false;
    private bool haIniciadoBusqueda = false;

    //Prepara el agente para la caída inicial y busca al jugador disponible
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        agent.enabled = false;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        target = FindAnyObjectByType<Player>();
    }

    //Supervisa si el enemigo ya tocó suelo para perseguir al jugador.
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
        //Debug.Log("Distancia real" + distanciaReal);
    }

    // Activa el NavMeshAgent y fija como destino al jugador una vez que el enemigo aterriza.
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

    //Avanza hacia el jugador mientras exista un path válido y decide cuándo pasar al modo ataque.
    private void PerseguirPlayer()
    {
        if (target == null || !agent.enabled) return;

        anim.SetBool("walking", true);
        agent.SetDestination(target.transform.position);

        if (agent.pathPending)
            return;

        if (!agent.hasPath)
            return;

        distanciaReal = Vector3.Distance(transform.position, target.transform.position);

        if (distanciaReal <= 2.5f)
        {
            EnfocarOnjetivo();
            LanzarAtaque();
        }
    }

    //Gira al enemigo para mirar al jugador manteniendo el eje vertical estabilizado.
    private void EnfocarOnjetivo()
    {
        Vector3 direccionAObjetivo = (target.transform.position - transform.position).normalized;
        direccionAObjetivo.y = 0;
        Quaternion rotacionAObjetivo = Quaternion.LookRotation(direccionAObjetivo);
        transform.rotation = rotacionAObjetivo;
    }

    //Detiene el desplazamiento y activa la animación de ataque cuando está en rango.
    private void LanzarAtaque()
    {
        agent.isStopped = true;
        anim.SetBool("attacking", true);
    }

    //Reproduce el sonido y aplica daño al jugador dentro del radio de golpe.
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

    //Reanuda el movimiento del agente y sale del estado de ataque.
    private void FinDeAtaque()  //Se llama en el evento de la animación
    {
        agent.isStopped = false;
        anim.SetBool("attacking", false);
    }

    //Resta vida, lanza la secuencia de muerte al llegar a cero y genera el drop correspondiente.
    public void RecibirDanio(float danio)
    {
        vidaEnemigo -= danio;
        if (vidaEnemigo <= 0)
        {
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

    //Detecta el contacto con el suelo para desactivar la física y habilitar el NavMeshAgent.
    private void OnCollisionEnter(Collision collision)
    {
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

    //Corrutina que espera un tiempo antes de destruir el enemigo.
    private IEnumerator DelayMuerte()
    {
        Debug.Log("Entré a la corrutina");
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }

    //Detecta el contacto con el suelo para desactivar la física y habilitar el NavMeshAgent.
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(puntoAtaque.position, radioAtaque);
    }
}
