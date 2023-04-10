using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public float jumpForce = 10;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public Transform groundDetector;
    private int count;
    private Rigidbody rb;
    private float movementX;
    private float movementY;
    private int jCount;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count:" + count.ToString();
        if (count >= 6)
        {
            winTextObject.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        // Check if the player is grounded
        // Debug.Log("LayerMask ground is ground: " + LayerMask.GetMask("Ground"));
        //I wasn't able to get the tag to work as it would never reach the case for resetting the jumps
        //modifying the layer as well got it to work
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, 0.5f, 0), 0.1f, LayerMask.GetMask("Ground"));        
        if (isGrounded)
        {
            jCount = 0; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
        }
    }

// use the known OnJump function
    void OnJump()
    {
        // for double jump
        if (jCount < 1)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jCount++;
        }
    }
}