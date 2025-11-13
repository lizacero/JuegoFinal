using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private InputManagerSO inputManager;

    [Header("Player")]
    [SerializeField] private Player player;
    private float vidaPlayer;
    private float vidaMax;
    public float VidaPlayer { get => vidaPlayer; set => vidaPlayer = value; }

    [Header("Enemy")]
    [SerializeField] private GameObject enemy;
    [SerializeField] private Transform puntoSpawnEnemy;
    [SerializeField] private float tiempoEntreSpawn = 5f;
    private bool spawnContinuo = true;
    private int maxEnemigos = 5;
    private int enemigosVivos = 0;
    private int enemigosEliminados = 0;
    public int EnemigosEliminados { get => enemigosEliminados; set => enemigosEliminados = value; }

    [Header("HUD")]
    [SerializeField] private MenuGameplay menuGameplay;
    [SerializeField] private Slider vidaSlider;

    [Header("Drops")]
    [SerializeField] private GameObject[] objetosDrop;
    public GameObject[] ObjetosDrop { get => objetosDrop; set => objetosDrop = value; }

    private int[] items = new int[5];
    public int[] Items { get => items; set => items = value; }

    [Header("Interacción")]
    [SerializeField] private GameObject panelInteraccion;
    public GameObject PanelInteraccion { get => panelInteraccion; set => panelInteraccion = value; }
    private bool activar = false;
    private bool ritualActivo = false;
    public bool RitualActivo { get => ritualActivo; set => ritualActivo = value; }

    //[SerializeField] private GameObject altar;
    //private float distancia;
    //private float distanciaInteraccion = 3f;
    //private bool enRango = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        Time.timeScale = 1;
        if (instance == null) instance = this;
        else Destroy(gameObject);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (player == null)
        {
            player = FindAnyObjectByType<Player>();
        }

        enemigosEliminados = 0;
    }

    void Start()
    {
        //StartCoroutine(SpawnEnemy());

        //if (spawnContinuo)
        //{
        //    StartCoroutine(SpawnEnemyContinuo());
        //}

        StartCoroutine(SpawnContinuo());
    }

    // Update is called once per frame
    void Update()
    {

        vidaSlider.value = vidaPlayer;
        if (vidaPlayer <= 0)
        {
            menuGameplay.MostrarPanelLose();
        }
        ObjetosRecolectados();

    }

    public void InicializarVida(float vidaInicial)
    {
        vidaPlayer = vidaInicial;
        vidaMax = vidaInicial;

        if (vidaSlider != null)
        {
            vidaSlider.minValue = 0f;
            vidaSlider.maxValue = vidaMax;
            vidaSlider.value = vidaMax;
        }
    }

    public void EnemigoEliminado()
    {
        enemigosEliminados++;
        enemigosVivos--;
        //Debug.Log(enemigosEliminados);

        if (enemigosEliminados >= 10)
        {
            //menuGameplay.MostrarPanelWin();
        }
    }

    private void SpawnearEnemigo()
    {
        Instantiate(enemy, puntoSpawnEnemy.position, puntoSpawnEnemy.rotation);
        enemigosVivos++;
    }


    private IEnumerator SpawnContinuo()
    {
        //yield return new WaitForSeconds(3f);
        while (spawnContinuo)
        {
            yield return new WaitForSeconds(3f);
            if (enemigosVivos < maxEnemigos)
            {
                SpawnearEnemigo();
            }
        }

        yield return new WaitForSeconds(tiempoEntreSpawn);
    }

    private void ObjetosRecolectados()
    {
        //Debug.Log(items[0] + "," + items[1] + "," + items[2] + "," + items[3] + "," + items[4]);
        if (items[0] >= 1 && items[1] >= 1 && items[2] >= 1 && items[3] >= 1 && items[4] >= 1)
        {
            Debug.Log("Se han recolectado todos los cristales");
            activar = true;
            
        }
    }

    public IEnumerator ActivarRitual()
    {
        if (activar == true)
        {
            Debug.Log("interactuando con el altar");
            ritualActivo = true;
            //interactúa con la piedra
            yield return new WaitForSeconds(10f);
            menuGameplay.MostrarPanelWin();
        }
        else
        {
            Debug.Log("Falta recolectar los cristales");
        }
    }
}
