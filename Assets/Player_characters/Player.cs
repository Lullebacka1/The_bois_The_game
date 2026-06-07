using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; 
    public static Player selected;
    private Rigidbody rb;
    private Animator anim;
    
    private float characterSelected = 11f;
    private Vector2 inputMovement;
    private bool shouldJump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();   
        anim = GetComponent<Animator>();

        rb.freezeRotation = true;
        rb.useGravity = true;

        string myName = gameObject.name.ToLower().Trim();
        string parentName = transform.parent != null ? transform.parent.name.ToLower().Trim() : string.Empty;

        if (myName == "texmex" || parentName == "texmex") characterSelected = 10f;
        else if (myName == "bullpog" || parentName == "bullpog") characterSelected = 11f;
        else if (myName == "lordagen" || parentName == "lordagen") characterSelected = 12f;
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        float moveX = 0f;
        float moveZ = 0f;

        if (Keyboard.current.dKey.isPressed) moveX = 1f;
        if (Keyboard.current.aKey.isPressed) moveX = -1f;
        if (Keyboard.current.wKey.isPressed) moveZ = 1f;
        if (Keyboard.current.sKey.isPressed) moveZ = -1f;

        inputMovement = new Vector2(moveX, moveZ).normalized;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && Mathf.Abs(rb.linearVelocity.y) < 0.05f)
        {
            shouldJump = true;
        }

        if (anim != null) 
        {
            anim.SetBool("isWalking", inputMovement.magnitude > 0);
        }
    }

    void FixedUpdate()
{
    // 1. Räkna ut exakt rörelse baserat på WASD-input (Y är alltid 0 här!)
    Vector3 movement = new Vector3(inputMovement.x, 0f, inputMovement.y).normalized;
    Vector3 currentMove = movement * moveSpeed * characterSelected * Time.fixedDeltaTime;

    // 2. Flytta karaktären stabilt längs marken (Fysiken kan inte låsa denna!)
    rb.MovePosition(transform.position + currentMove);

    // 3. Hantera hoppet helt fristående
    if (shouldJump)
    {
        float jumpForce = characterSelected * 2.5f; // Ökad kraft för ett bra hopp
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        shouldJump = false; 
    }

    // 4. Den skottsäkra rotations- och positionsspärren för Blender-modellen
    if (transform.childCount > 0)
    {
        Transform childTransform = transform.GetChild(0);
        childTransform.localPosition = Vector3.zero;
        childTransform.localRotation = Quaternion.identity; 
    }
}
}
