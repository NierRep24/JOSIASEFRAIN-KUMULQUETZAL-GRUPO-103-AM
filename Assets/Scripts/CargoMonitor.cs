using UnityEngine;

public class CargoMonitor : MonoBehaviour
{
    public GameManager gameManager;

    private void Update()
    {
        if (gameManager == null || Camera.main == null)
            return;
        Vector3 vp = Camera.main.WorldToViewportPoint(transform.position);
        if (vp.x < 0f || vp.x > 1f || vp.y < 0f || vp.y > 1f)
        {
            gameManager.Defeat();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            gameManager.Defeat();
        }
    }
}
