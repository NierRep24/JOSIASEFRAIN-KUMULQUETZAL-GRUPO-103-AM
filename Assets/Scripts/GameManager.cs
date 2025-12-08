using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    [Header("Referencias de juego")]
    public ShipController ship;
    public Transform cargo;

    [Header("Tiempo de reinicio")]
    public float reloadDelay = 2f;

    private bool isGameOver = false;

    private void Start()
    {
        if (ship != null)
        {
            ship.gameStarted = false;

            var rbShip = ship.GetComponent<Rigidbody2D>();
            if (rbShip != null)
            {
                rbShip.gravityScale = 0f;
                rbShip.linearVelocity = Vector2.zero;
            }
        }

        if (cargo != null)
        {
            var rbCargo = cargo.GetComponent<Rigidbody2D>();
            if (rbCargo != null)
            {
                rbCargo.gravityScale = 0f;
                rbCargo.linearVelocity = Vector2.zero;
            }
        }
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

        Debug.Log("DERROTA: La carga se perdió. Reiniciando...");

        RestartAsync();
    }

    public void Victory()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("VICTORIA: Llegaste a la meta. Reiniciando...");

        RestartAsync();
    }

    private async void RestartAsync()
    {
        await Awaitable.WaitForSecondsAsync(reloadDelay);

        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
