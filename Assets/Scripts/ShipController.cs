using UnityEngine;
using UnityEngine.InputSystem;   // 👈 para InputAction.CallbackContext

public class ShipController : MonoBehaviour
{
    [Header("Fuerzas principales")]
    public float thrustForce = 2f;      // RF: empuje vertical
    public float lateralForce = 5f;     // RF: movimiento lateral (antes horizontalSpeed)

    [Header("Límites de velocidad")]
    public float maxRiseSpeed = 1.5f;   // velocidad máxima al subir
    public float maxFallSpeed = 2.0f;   // velocidad máxima al caer

    [Header("Drag según estado")]
    public float dragWithoutThrust = 2.5f;  // drag cuando NO hay propulsión
    public float dragWithThrust = 0.7f;     // drag cuando SÍ hay propulsión

    [Header("Feedback visual del propulsor")]
    public Renderer thrusterRenderer;       // RF: Renderer del cubito
    public Color activeColor = Color.yellow; // RF: color cuando está activo
    public Color idleColor = Color.white;    // color cuando está apagado

    [Header("Estado del juego")]
    public bool gameStarted = false;

    [Header("Referencia al GameManager")]
    public GameManager gameManager;

    // ---- internos ----
    private Rigidbody2D rb;
    private float steerInput = 0f;      // entrada horizontal (A/D)
    private bool isThrusting = false;   // si el propulsor está activo
    private SpriteRenderer thrusterSprite; // para cambiar color si es SpriteRenderer

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (thrusterRenderer != null)
        {
            // Intentar obtener SpriteRenderer para usar .color
            thrusterSprite = thrusterRenderer.GetComponent<SpriteRenderer>();
        }
    }

    private void Start()
    {
        // La nave empieza en el centro, sin gravedad ni movimiento
        transform.position = Vector3.zero;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;

        SetThrusterColor(idleColor);
    }

    private void Update()
    {
        // 🔸 Inicio del juego con el primer Space (modo clásico)
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            if (gameManager != null)
                gameManager.StartGame();
            else
                Debug.LogWarning("GameManager no asignado en ShipController");
        }

        if (!gameStarted)
            return;

        // 🔸 INPUT CLÁSICO (sigue funcionando)
        //  A / D o flechas → movimiento lateral
        steerInput = Input.GetAxisRaw("Horizontal");

        // Space presionado → propulsor activo
        isThrusting = Input.GetKey(KeyCode.Space);

        // Actualizar color del cubo según propulsor
        UpdateThrusterVisual();
    }

    private void FixedUpdate()
    {
        if (!gameStarted)
            return;

        // --- Movimiento horizontal ---
        Vector2 velocity = rb.linearVelocity;
        velocity.x = steerInput * lateralForce;
        rb.linearVelocity = velocity;

        // --- Drag según si hay propulsión ---
        rb.linearDamping = isThrusting ? dragWithThrust : dragWithoutThrust;

        // --- Empuje vertical con AddForce controlado ---
        if (isThrusting)
        {
            if (rb.linearVelocity.y < maxRiseSpeed)
            {
                rb.AddForce(Vector2.up * thrustForce, ForceMode2D.Force);
            }
        }

        // Limitar velocidad hacia arriba y abajo
        Vector2 vel = rb.linearVelocity;
        vel.y = Mathf.Clamp(vel.y, -maxFallSpeed, maxRiseSpeed);
        rb.linearVelocity = vel;
    }

    // =========================================================
    // MÉTODOS QUE PIDE LA HOJA (para PlayerInput + Unity Events)
    // =========================================================

    // RF: llamado por el PlayerInput para el botón de propulsión (Space)
    public void OnThrust(InputAction.CallbackContext context)
    {
        if (!gameStarted)
        {
            // primer input también puede iniciar el juego
            if (context.started && gameManager != null)
            {
                gameManager.StartGame();
            }
        }

        // started = se presiona, canceled = se suelta
        if (context.started)
        {
            isThrusting = true;
        }
        else if (context.canceled)
        {
            isThrusting = false;
        }
    }

    // RF: llamado por el PlayerInput para el eje horizontal (A/D)
    public void OnSteer(InputAction.CallbackContext context)
    {
        // Leemos un float (-1 a 1)
        steerInput = context.ReadValue<float>();
    }

    // =========================================================

    private void UpdateThrusterVisual()
    {
        SetThrusterColor(isThrusting ? activeColor : idleColor);
    }

    private void SetThrusterColor(Color c)
    {
        if (thrusterSprite != null)
        {
            thrusterSprite.color = c;
        }
        else if (thrusterRenderer != null && thrusterRenderer.material != null)
        {
            thrusterRenderer.material.color = c;
        }
    }
}
