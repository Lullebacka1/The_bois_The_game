using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float groundCheckDistance = 1.1f;
    [SerializeField] private float moveSpeed = 5f; // Bas-hastighet
    public static Player selected;
    private Rigidbody rb;
    private Animator anim;
    
    private float characterSelected = 1f;
    private Vector2 inputMovement;
    private bool shouldJump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();   
        anim = GetComponent<Animator>();

        // Lås rotationen så att karaktären inte ramlar omkull av fysiken
        rb.freezeRotation = true;

        string myName = gameObject.name.ToLower().Trim();
        string parentName = transform.parent != null ? transform.parent.name.ToLower().Trim() : string.Empty;

        if (myName == "texmex" || parentName == "texmex") characterSelected = 10f;
        else if (myName == "bullpog" || parentName == "bullpog") characterSelected = 11f;
        else if (myName == "lordagen" || parentName == "lordagen") characterSelected = 12f;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        // 1. Hantera Input i Update (så att inga knapptryck missas)
        float moveX = 0f;
        float moveZ = 0f;

        if (Keyboard.current.dKey.isPressed) moveX = 1f;
        if (Keyboard.current.aKey.isPressed) moveX = -1f;
        if (Keyboard.current.wKey.isPressed) moveZ = 1f;
        if (Keyboard.current.sKey.isPressed) moveZ = -1f;

        inputMovement = new Vector2(moveX, moveZ).normalized;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && IsGrounded())
        {
            shouldJump = true;
        }

        // Sätt animationen baserat på om vi har input
        if (anim != null) 
        {
            anim.SetBool("isWalking", inputMovement.magnitude > 0);
        }
    }

    void FixedUpdate()
    {
        // 2. Hantera all Fysik och Rigidbody i FixedUpdate

        // Räkna ut önskad hastighet på X- och Z-axeln (ingen Time.deltaTime behövs här!)
        float targetVelocityX = inputMovement.x * moveSpeed * characterSelected;
        float targetVelocityZ = inputMovement.y * moveSpeed * characterSelected;

        // Behåll Rigidbody-komponentens nuvarande Y-hastighet (så faller vi och hoppar naturligt)
        float currentYVelocity = rb.linearVelocity.y; // Använd rb.velocity.y om du har en äldre Unity-version

        // Sätt den nya hastigheten direkt
        rb.linearVelocity = new Vector3(targetVelocityX, currentYVelocity, targetVelocityZ);

        // Hantera hoppet
        if (shouldJump)
        {
            float jumpForce = characterSelected * 0.5f;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            shouldJump = false; // Återställ hopp-flaggan
        }
    }

    private bool IsGrounded()
    {
        Vector3 rayStart = transform.position + Vector3.up * 0.2f;
        return Physics.Raycast(rayStart, Vector3.down, groundCheckDistance);
    }
}
