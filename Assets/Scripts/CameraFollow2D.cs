using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform target;                 // aquí va la Nave

    [Header("Suavizado")]
    public float smoothTime = 0.2f;          // tiempo de suavizado (más alto = más lento)
    public Vector2 offset = new Vector2(0f, 0f);

    [Header("Ejes a seguir")]
    public bool followX = true;
    public bool followY = true;

    [Header("Esperar a que inicie el juego")]
    public bool waitForGameStart = true;

    private Vector3 currentVelocity = Vector3.zero;
    private ShipController shipController;

    private void Start()
    {
        if (target != null)
        {
            shipController = target.GetComponent<ShipController>();
        }
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Si queremos esperar a que el juego empiece
        if (waitForGameStart && shipController != null && !shipController.gameStarted)
            return;

        // Posición deseada de la cámara
        Vector3 desiredPosition = transform.position;

        if (followX)
            desiredPosition.x = target.position.x + offset.x;

        if (followY)
            desiredPosition.y = target.position.y + offset.y;

        // Aseguramos que la cámara siempre esté a -10 en Z
        desiredPosition.z = -10f;

        // Movimiento suave con SmoothDamp
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref currentVelocity,
            smoothTime
        );
    }
}
