using UnityEngine;

public class CargoMonitor : MonoBehaviour
{
    public GameManager gameManager;

    private void Update()
    {
        if (gameManager == null || Camera.main == null)
            return;

        // Revisar si la esfera salió de cámara
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);

        // Fuera del rango [0,1] en x o y = fuera de vista
        if (vp.x < 0f || vp.x > 1f || vp.y < 0f || vp.y > 1f)
        {
            gameManager.Defeat();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si toca el suelo -> derrota
        if (collision.collider.CompareTag("Ground"))
        {
            gameManager.Defeat();
        }
    }
}
