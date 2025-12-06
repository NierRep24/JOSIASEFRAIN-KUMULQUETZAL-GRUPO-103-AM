using UnityEngine;

public class GoalArea : MonoBehaviour
{
    public GameManager gameManager;

    private bool shipInside = false;
    private bool cargoInside = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ShipController>() != null)
        {
            shipInside = true;
        }
        if (other.CompareTag("Cargo") || other.name == "Carga")
        {
            cargoInside = true;
        }

        CheckVictory();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<ShipController>() != null)
        {
            shipInside = false;
        }

        if (other.CompareTag("Cargo") || other.name == "Carga")
        {
            cargoInside = false;
        }
    }

    private void CheckVictory()
    {
        if (gameManager == null) return;
        if (shipInside && cargoInside)
        {
            gameManager.Victory();
        }
    }
}
