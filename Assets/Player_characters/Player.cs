using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            addForce(Vector3.up * jumpForce);
        }
        if (Input.GetKeyDown(KeyCode.w))
        {
          addForce(Vector3.forward * speed);
        }
        if (Input.GetKeyDown(KeyCode.s))
        {
            addForce(Vector3.back * speed);
        }
        if (Input.GetKeyDown(KeyCode.a))
        {
            addForce(Vector3.left * speed);
        }
        if (Input.GetKeyDown(KeyCode.d))
        {
            addForce(Vector3.right * speed);
        }

    }
}
