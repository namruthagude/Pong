using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    private float speed = 5;
    private float maxYdis = 5f;
    private float minYdis = -4f;
    private float yDis = 0;
    private Vector3 hostPosition = new Vector3(-8.5f,0,0);
    private Vector3 clientPosition = new Vector3(8.5f,0,0);

    private void Start()
    {
        //Checking whether the paddle is owner
        if (IsOwner)
        {
            if (IsServer) //If paddle is host
            {
                //Setting Host paddle to host position
                transform.position = hostPosition;
                gameObject.layer = 9; //setting layer as host
            }
            else if (IsClient) //If paddle is server
            {
                //Setting client paddle to client position
                transform.position = clientPosition;
                gameObject.layer = 10; // setting layer as client
            }
        }
        else
        {
            if (IsServer)
            {
                gameObject.layer = 10;// Setting layer as host
            }
            else if (IsClient)
            {
                gameObject.layer = 9; //Setting layer as client
            }
        }
    }

    private void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.UpArrow)) //Move upwards
        {
            
            yDis = Time.deltaTime * speed;
            if (transform.position.y + yDis <= maxYdis) // current Y position less than maximum Y position
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + yDis, transform.position.z);
            }
        }
        if (Input.GetKey(KeyCode.DownArrow)) //Move downwards
        {
            
            yDis = Time.deltaTime * speed;
            if(transform.position.y - yDis >= minYdis) // Current Y position greater than minimum Y position
            {
                transform.position = new Vector3(transform.position.x, transform.position.y - yDis, transform.position.z);
            }
        }
    }
}
