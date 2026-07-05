using System;
using Unity.Netcode;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    public event EventHandler OnClientScoreIncreased;
    public event EventHandler OnHostScoreIncreased;
    public event EventHandler OnCollided;

    public static Ball Instance { get; private set; }

    private float intialBallSpeed = 10f;
    private Vector3 direction;
    private float ballSpeed;
    private Vector3[] possibleDirections = new Vector3[]
    {
        Vector3.up + Vector3.left,
        Vector3.down + Vector3.right,
        Vector3.up + Vector3.right,
        Vector3.down + Vector3.left,
        Vector3.left,
        Vector3.right,

    };

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnGamePlaying += GameManager_OnGamePlaying;
        ballSpeed = intialBallSpeed;
    }

    private void GameManager_OnGamePlaying(object sender, EventArgs e)
    {
        if (IsServer)
        {
            ChooseInitialDirection();
            MoveBallClientRpc(direction);
        }
    }

    public override void OnNetworkSpawn()
    {
        
    }

    private void Update()
    {
        if (IsServer)
        {
            if (GameManager.Instance.IsGamePlaying())
            {
                transform.position += direction * ballSpeed * Time.deltaTime;
                SyncBallPositionClientRpc(transform.position);
            }
        }
    }

    [ClientRpc]
    private void SyncBallPositionClientRpc(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
    private void ChooseInitialDirection()
    {
        Vector3 newdirection = possibleDirections[UnityEngine. Random.Range(0, possibleDirections.Length)];
        direction = newdirection.normalized;
    }

    [ClientRpc]
    private void MoveBallClientRpc(Vector3 newDirection)
    {
        direction = newDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return; // Ensure only the server handles collisions       
        int layer = collision.gameObject.layer;
        string layerName = LayerMask.LayerToName(layer);

        switch (layerName)
        {
            case "Host":
            case "Client":
                HandlePaddleCollision(collision, layerName);
                PlayOnCollidedSoundClientRpc();
                ballSpeed++;
                break;

            case "Host Boundary":
                OnClientScoreIncreased?.Invoke(this, EventArgs.Empty);
                RespawnBall();
                break;

            case "Client Boundary":
                OnHostScoreIncreased?.Invoke(this, EventArgs.Empty);
                RespawnBall();
                break;

            case "Reflecting Boundary":
                direction = Vector3.Reflect(direction, collision.contacts[0].normal);
                PlayOnCollidedSoundClientRpc();
                break;

            default:
                Debug.LogWarning($"Unhandled collision with layer: {layerName}");
                break;
        }
        SyncBallDirectionClientRpc(direction.normalized);
        direction = direction.normalized; // Enforce immediate update for the server

    }

    private void HandlePaddleCollision(Collision2D collision, string paddleType)
    {
        // Get the paddle's collider bounds
        Bounds paddleBounds = collision.collider.bounds;

        // Determine the collision point
        Vector3 collisionPoint = collision.contacts[0].point;

        // Calculate the relative position of the collision point on the paddle
        float relativeHitPosition = (collisionPoint.y - paddleBounds.center.y) / paddleBounds.extents.y;

        // Define the maximum angle for reflection
        float maxReflectionAngle = 75f; // Adjust as needed

        // Calculate the reflection angle based on the relative hit position
        float reflectionAngle = relativeHitPosition * maxReflectionAngle;

        // Convert the reflection angle to a direction vector
        float reflectionAngleRadians = reflectionAngle * Mathf.Deg2Rad;
        Vector3 newDirection = new Vector3(
            paddleType == "Host" ? Mathf.Cos(reflectionAngleRadians) : -Mathf.Cos(reflectionAngleRadians),
            Mathf.Sin(reflectionAngleRadians),
            0
        );

        // Normalize the direction to maintain consistent speed
        direction = newDirection.normalized;

        // Debugging
        Debug.Log($"Collision with {paddleType} at relative position: {relativeHitPosition}, angle: {reflectionAngle}");
    }


    private void RespawnBall()
    {
        Debug.Log("Respawning");
        SetBallToInitialPosition();
        ChooseInitialDirection();
        MoveBallClientRpc(direction);
    }

    private void SetBallToInitialPosition()
    {
        transform.position = Vector3.zero;
        ballSpeed = intialBallSpeed; // Reset ball speed
       // numberOfBounces = 1; // Reset bounces
    }

    [ClientRpc]
    private void SyncBallDirectionClientRpc(Vector3 newdirection)
    {
        direction = newdirection;
    }

    [ClientRpc]
    private void PlayOnCollidedSoundClientRpc()
    {
        OnCollided?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        Debug.Log("Ball Destroyed");
    }
}
