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
    private int enemigosEliminados = 0;
    public int EnemigosEliminados { get => enemigosEliminados; set => enemigosEliminados = value; }

    [Header("HUD")]
    [SerializeField] private MenuGameplay menuGameplay;
    [SerializeField] private Slider vidaSlider;

    [Header("Drops")]
    [SerializeField] private GameObject[] objetosDrop;
    public GameObject[] ObjetosDrop { get => objetosDrop; set => objetosDrop = value; }

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

    }

    // Update is called once per frame
    void Update()
    {

        vidaSlider.value = vidaPlayer;
        if (vidaPlayer <= 0)
        {
            menuGameplay.MostrarPanelLose();
        }
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
        Debug.Log(enemigosEliminados);

        if (enemigosEliminados >= 10)
        {
            menuGameplay.MostrarPanelWin();
        }
    }

    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(3f);
        Instantiate(enemy, puntoSpawnEnemy);
    }
}
