using UnityEngine;
using UnityEngine.EventSystems;


public class Player : MonoBehaviour, Daniable
{
    [SerializeField] private InputManagerSO inputManager;
    private CharacterController controller;
    [SerializeField] private Animator anim;

    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float factorGravedad;
    [SerializeField] private Transform camara;
    private Vector3 direccionMovimiento;
    private Vector3 direccionInput;
    private Vector3 velocidadVertical;

    [Header("Sistema de combate")]
    [SerializeField] private float vidaPlayer;
    [SerializeField] private float distanciaDisparo;
    [SerializeField] private float danioDisparo;
    [SerializeField] private ParticleSystem particulas;
    private AudioSource sonidoDisparo;

    [Header("GameState")]
    [SerializeField] private MenuGameplay menuGameplay;

    private bool mouseSobreUI = false;

    private void OnEnable()
    {
        inputManager.OnMover += Mover;
        inputManager.OnDisparar += Disparar;
        inputManager.OnRecargar += Recargar;
    }

    private void OnDisable()
    {
        // Desuscribirse de los eventos antes de que se destruya el objeto
        if (inputManager != null)
        {
            inputManager.OnMover -= Mover;
            inputManager.OnDisparar -= Disparar;
            inputManager.OnRecargar -= Recargar;
        }
    }

    void Start()
    {
        
        controller = GetComponent<CharacterController>();
        sonidoDisparo = GetComponent<AudioSource>();
        anim.SetBool("canShoot", true);
        anim.SetBool("canReload", true);
        
        if (GameManager.instance != null)
        {
            GameManager.instance.InicializarVida(vidaPlayer);
        }
    }

    void Update()
    {
        // Verificar el estado de UI en Update() (se ejecuta antes de los callbacks)
        if (EventSystem.current != null)
        {
            mouseSobreUI = EventSystem.current.IsPointerOverGameObject();
        }

        AplicarGravedad();
        Moviendo();
        
    }


    private void Mover(Vector2 ctx)
    {
        direccionInput = new Vector3(ctx.x,0,ctx.y);
        //Debug.Log(direccionInput);
    }

    private void Disparar()
    {
        if (mouseSobreUI)
        {
            return; // Si está sobre UI, no disparar
        }

        if (Time.timeScale == 0)
        {
            return; // Si está pausado, no disparar
        }


        Debug.Log("Disparando");
        anim.SetTrigger("shooting");
        particulas.Play();
        sonidoDisparo.Play();
;        if (Physics.Raycast(camara.position, camara.forward, out RaycastHit hitInfo, distanciaDisparo))
        {
            if (hitInfo.transform.TryGetComponent(out Daniable sistemaDanho))
            {
                if (!hitInfo.transform.CompareTag("Player"))
                {
                    sistemaDanho.RecibirDanio(danioDisparo);
                }
            }
        }
    }


    private void Recargar()
    {
        Debug.Log("Recargando");
        anim.SetTrigger("reloading");
    }


    private void AplicarGravedad()
    {
        velocidadVertical.y += factorGravedad * Time.deltaTime;
        controller.Move(velocidadVertical*Time.deltaTime);
    }

    private void Moviendo()
    {
        Vector3 direccionFrente = camara.forward;
        direccionFrente.y = 0;
        direccionFrente = direccionFrente.normalized;

        Vector3 direccionDerecha = camara.right;
        direccionDerecha.y = 0;
        direccionDerecha = direccionDerecha.normalized;

        direccionMovimiento = direccionFrente*direccionInput.z + direccionDerecha*direccionInput.x;
        controller.Move(direccionMovimiento*velocidadMovimiento*Time.deltaTime);

        RotarHaciaDestino(direccionFrente);

        if (direccionInput.sqrMagnitude > 0)
        {
            anim.SetBool("walking", true);
            
        }
        else
        {
            anim.SetBool("walking", false);
        }
        Debug.DrawRay(camara.position, camara.forward * distanciaDisparo, Color.yellow);
    }

    private void RotarHaciaDestino(Vector3 destino)
    {
        Quaternion rot = Quaternion.LookRotation(destino);
        transform.rotation = rot;
    }

    public void RecibirDanio(float danio)
    {
        
        vidaPlayer -= danio;
        GameManager.instance.VidaPlayer = vidaPlayer;
        if (vidaPlayer <= 0)
        {   
            GameManager.instance.PanelObjetivo.SetActive(false);
            Destroy(GetComponent<CharacterController>());
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Item1"))
        {
            GameManager.instance.Items[0]++;
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.CompareTag("Item2"))
        {
            GameManager.instance.Items[1]++;
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.CompareTag("Item3"))
        {
            GameManager.instance.Items[2]++;
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.CompareTag("Item4"))
        {
            GameManager.instance.Items[3]++;
            Destroy(hit.gameObject);
        }
        else if (hit.gameObject.CompareTag("Item5"))
        {
            GameManager.instance.Items[4]++;
            Destroy(hit.gameObject);
        }

    }


}
