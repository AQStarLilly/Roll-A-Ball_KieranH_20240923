using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public float jumpForce = 7f;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Rigidbody rb;
    private int count;
    private int totalCount;
    private float movementX;
    private float movementY;
    private Vector3 startPoint;

    public LayerMask groundLayer;
    public float raycastDistance = 0.6f;

    private bool isGrounded;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        totalCount = GameObject.FindGameObjectsWithTag("PickUp").Length;
        SetCountText();
        winTextObject.SetActive(false);
        startPoint = transform.position;  //store initial position
    }    

    /*void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if(count >= 8)
        {
            winTextObject.SetActive(true);
        }
    } */

    void SetCountText()
    {
        countText.text = $"{count}/{totalCount}";
        if(count >= totalCount)
        {
            winTextObject.SetActive(true);
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, raycastDistance, groundLayer))
            isGrounded = true;
        else
            isGrounded = false;

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);   //get movement input

        //Get the camera's forward and right directions
        Vector3 cameraForward = Camera.main.transform.forward;  
        Vector3 cameraRight = Camera.main.transform.right;

        //Ignore vertical components of the camera direction
        cameraForward.y = 0;
        cameraRight.y = 0;

        //Normalize to ensure consistent movement
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 adjustedMovement = cameraRight * movement.x + cameraForward * movement.z;  //Adjust movement to align with camera
        rb.AddForce(adjustedMovement * speed);  //Apply movement force to the Rigidbody
    }

    public void ResetPosition()
    {
        transform.position = startPoint;  //move player back to start position      
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }        
    }
}
