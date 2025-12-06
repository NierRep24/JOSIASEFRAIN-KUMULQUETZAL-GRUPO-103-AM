using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [Header("Referencias de juego")]
    public ShipController ship;
    public Transform cargo;

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
        if (defeatPanel != null) defeatPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }
    public void StartGame()
    {
        Debug.Log("Juego iniciado desde GameManager");
        if (ship != null)
        {
            ship.gameStarted = true;

            var rbShip = ship.GetComponent<Rigidbody2D>();
            if (rbShip != null)
            {
                rbShip.gravityScale = 0.4f;
                rbShip.linearVelocity = Vector2.zero;
            }
        }
        if (cargo != null)
        {
            var rbCargo = cargo.GetComponent<Rigidbody2D>();
            if (rbCargo != null)
            {
                rbCargo.gravityScale = 0.4f;
                rbCargo.linearVelocity = Vector2.zero;
            }
        }
    }
    public void Defeat()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("DERROTA: La carga se perdió.");
        if (defeatPanel != null)
            defeatPanel.SetActive(true);

        if (defeatText != null)
            defeatText.text = "YA PERDISTE LOL";

        StopShipAndCargo();
        ReloadLevelAsync();
    }
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
    private async void ReloadLevelAsync()
    {
        await Awaitable.WaitForSecondsAsync(reloadDelay);

        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
