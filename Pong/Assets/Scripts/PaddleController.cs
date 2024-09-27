using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;

public class PaddleController : NetworkBehaviour
{
    public float speed = 5f;
    public float lowerBound = -2.11f;
    public float upperbound = 4.2f;
    [Networked] private Vector3 networkedPosition { get; set; }

    // Start is called before the first frame update
    void Start()
    {
       networkedPosition = new Vector3(4.35f, 1.22f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Object.HasInputAuthority)
        {
            
            HandleMovement();
            networkedPosition = transform.position;
        }
        else
        {
            Debug.Log("Networked position" +  networkedPosition);
            transform.position = Vector3.Lerp(transform.position, networkedPosition, Time.deltaTime * 10f);
        }
    }
    void HandleMovement()
    {
        float moveInput = 0f;

        // Check for up and down arrow key input
        if (Input.GetKey(KeyCode.UpArrow))
        {
            
            moveInput = 1f; // Move upward
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            
            moveInput = -1f; // Move downward
        }

        // Update the current position using moveInput and speed
        Vector3 currentPosition = transform.position;
        currentPosition.y += moveInput * speed * Time.deltaTime;

        // Apply the updated position to the paddle
        transform.position = currentPosition;

        networkedPosition = transform.position;


    }
}
