using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float groundCheckDistance = 1.1f;
    public static Player selected;
    private Rigidbody rb;
    private Animator anim;
    
    private float characterSelected = 1f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();   
        string myName = gameObject.name.ToLower().Trim();
        string parentName = transform.parent != null ? transform.parent.name.ToLower().Trim() : string.Empty;
        anim = GetComponent<Animator>();

        if (myName == "texmex" || parentName == "texmex")
        {
            characterSelected = 10f;
        }
        else if (myName == "bullpog" || parentName == "bullpog")
        {
            characterSelected = 11f;
        }
        else if (myName == "lordagen" || parentName == "lordagen")
        {
            characterSelected = 12f;
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        bool isMoving = false;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && IsGrounded())
        {
            Vector3 jump = new Vector3(0, 1, 0) * characterSelected * 0.5f;
            rb.AddForce(jump, ForceMode.Impulse);
        }

        if (Keyboard.current.dKey.isPressed) 
        {
            Vector3 right = new Vector3(5, 0, 0) * characterSelected * Time.deltaTime;
            rb.AddForce(right, ForceMode.VelocityChange);
            isMoving = true;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            Vector3 left = new Vector3(-5, 0, 0) * characterSelected * Time.deltaTime;
            rb.AddForce(left, ForceMode.VelocityChange);
            isMoving = true;
        }

        if (Keyboard.current.wKey.isPressed)
        {
            Vector3 forward = new Vector3(0, 0, 5) * characterSelected * Time.deltaTime;
            rb.AddForce(forward, ForceMode.VelocityChange);
            isMoving = true;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            Vector3 back = new Vector3(0, 0, -5) * characterSelected * Time.deltaTime;
            rb.AddForce(back, ForceMode.VelocityChange);
            isMoving = true;
        }

        if (anim != null) 
        {
            anim.SetBool("isWalking", isMoving);
        }
    }

    private bool IsGrounded()
{
    Vector3 rayStart = transform.position + Vector3.up * 0.2f;
    return Physics.Raycast(rayStart, Vector3.down, groundCheckDistance);
}
}