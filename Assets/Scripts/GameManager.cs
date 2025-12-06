using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [Header("Referencias de juego")]
    public ShipController ship;   // arrastra aquí la Nave
    public Transform cargo;       // arrastra aquí la esfera roja

    [Header("UI Derrota")]
    public GameObject defeatPanel;
    public TextMeshProUGUI defeatText;

    [Header("UI Victoria")]
    public GameObject victoryPanel;
    public TextMeshProUGUI victoryText;

    [Header("Tiempo de reinicio")]
    public float reloadDelay = 2f;

    private bool isGameOver = false;

    private void Start()
    {
        // Asegurarnos de que los paneles empiecen ocultos
        if (defeatPanel != null) defeatPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    // 🔹 Este es el método que te falta: lo llama ShipController cuando presionas SPACE por primera vez
    public void StartGame()
    {
        Debug.Log("Juego iniciado desde GameManager");

        // Activar nave
        if (ship != null)
        {
            ship.gameStarted = true;

            var rbShip = ship.GetComponent<Rigidbody2D>();
            if (rbShip != null)
            {
                rbShip.gravityScale = 0.4f;   // gravedad suave
                rbShip.linearVelocity = Vector2.zero;
            }
        }

        // Activar carga
        if (cargo != null)
        {
            var rbCargo = cargo.GetComponent<Rigidbody2D>();
            if (rbCargo != null)
            {
                rbCargo.gravityScale = 0.4f;  // empieza a caer junto con la nave
                rbCargo.linearVelocity = Vector2.zero;
            }
        }
    }

    // 🔹 Derrota: la llama CargoMonitor cuando la bola toca el suelo o sale de cámara
    public void Defeat()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("DERROTA: La carga se perdió.");

        // Mostrar panel de derrota
        if (defeatPanel != null)
            defeatPanel.SetActive(true);

        if (defeatText != null)
            defeatText.text = "YA PERDISTE LOL";

        StopShipAndCargo();

        // Reinicio async (RT-02, sin corrutinas)
        ReloadLevelAsync();
    }

    // 🔹 Victoria: la llama GoalArea cuando nave y carga están en la meta
    public void Victory()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("VICTORIA: Nave y carga llegaron a la META.");

        StopShipAndCargo();

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        if (victoryText != null)
            victoryText.text = "¡GANASTE!";
    }

    private void StopShipAndCargo()
    {
        if (ship != null)
        {
            ship.enabled = false;
            var rbShip = ship.GetComponent<Rigidbody2D>();
            if (rbShip != null) rbShip.linearVelocity = Vector2.zero;
        }

        if (cargo != null)
        {
            var rbCargo = cargo.GetComponent<Rigidbody2D>();
            if (rbCargo != null) rbCargo.linearVelocity = Vector2.zero;
        }
    }

    // 🔹 Versión async del reinicio (cumple RT-02)
    private async void ReloadLevelAsync()
    {
        await Awaitable.WaitForSecondsAsync(reloadDelay);

        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
