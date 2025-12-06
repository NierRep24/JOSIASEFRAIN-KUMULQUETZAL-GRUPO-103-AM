using UnityEngine;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    [Header("Fuerzas principales")]
    public float thrustForce = 2f;
    public float lateralForce = 5f;

    [Header("Límites de velocidad")]
    public float maxRiseSpeed = 1.5f;
    public float maxFallSpeed = 2.0f;

    [Header("Drag según estado")]
    public float dragWithoutThrust = 2.5f;
    public float dragWithThrust = 0.7f;

    [Header("Feedback visual del propulsor")]
    public Renderer thrusterRenderer;
    public Color activeColor = Color.yellow;
    public Color idleColor = Color.white;

    [Header("Estado del juego")]
    public bool gameStarted = false;

    [Header("Referencia al GameManager")]
    public GameManager gameManager;
    private Rigidbody2D rb;
    private float steerInput = 0f;
    private bool isThrusting = false;
    private SpriteRenderer thrusterSprite;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (thrusterRenderer != null)
        {
            thrusterSprite = thrusterRenderer.GetComponent<SpriteRenderer>();
        }
    }

    private void Start()
    {
        transform.position = Vector3.zero;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;

        SetThrusterColor(idleColor);
    }

    private void Update()
    {
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            if (gameManager != null)
                gameManager.StartGame();
            else
                Debug.LogWarning("GameManager no asignado en ShipController");
        }

        if (!gameStarted)
            return;
        steerInput = Input.GetAxisRaw("Horizontal");
        isThrusting = Input.GetKey(KeyCode.Space);
        UpdateThrusterVisual();
    }

    private void FixedUpdate()
    {
        if (!gameStarted)
            return;
        Vector2 velocity = rb.linearVelocity;
        velocity.x = steerInput * lateralForce;
        rb.linearVelocity = velocity;
        rb.linearDamping = isThrusting ? dragWithThrust : dragWithoutThrust;
        if (isThrusting)
        {
            if (rb.linearVelocity.y < maxRiseSpeed)
            {
                rb.AddForce(Vector2.up * thrustForce, ForceMode2D.Force);
            }
        }
        Vector2 vel = rb.linearVelocity;
        vel.y = Mathf.Clamp(vel.y, -maxFallSpeed, maxRiseSpeed);
        rb.linearVelocity = vel;
    }
    public void OnThrust(InputAction.CallbackContext context)
    {
        if (!gameStarted)
        {
            if (context.started && gameManager != null)
            {
                gameManager.StartGame();
            }
        }
        if (context.started)
        {
            isThrusting = true;
        }
        else if (context.canceled)
        {
            isThrusting = false;
        }
    }
    public void OnSteer(InputAction.CallbackContext context)
    {
        steerInput = context.ReadValue<float>();
    }
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
