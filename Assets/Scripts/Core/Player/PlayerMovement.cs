using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader inputReader; // get movement inputs
    [SerializeField] private Transform bodyTransform; // change body position
    [SerializeField] private Rigidbody2D rb; // physics
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float turningRate = 250f;

    private Vector2 previousMovementInput;

    // networking needs a little delay to get set up. Don't use on start
    public override void OnNetworkSpawn()
    {
        if(!IsOwner){return;}
        // subscribe to inputReader.OnMove here
        inputReader.OnMoveEvent += HandleMove;
        
    }
    public override void OnNetworkDespawn()
    {
        if(!IsOwner){return;}
        // unsubscribe to inputReader.OnMove Here
        inputReader.OnMoveEvent -= HandleMove;
    }
    void Update()
    {
        if(!IsOwner) return;
        //rotate on z axis
        // because we don't let player strafe left and right, left and right buttons (x axis) are for turning
        float zRotation = previousMovementInput.x * -turningRate * Time.deltaTime; // turning rate is negative to flip player input from vector 2
        bodyTransform.Rotate(0f, 0f, zRotation);
    }
    private void FixedUpdate() { // called every frame of physics engine, should be used to calculate physics (aka, with rigid bodies)
    if(!IsOwner) return;
        //rb.linearVelocityY = bodyTransform.up.y * previousMovementInput.y * movementSpeed;
        rb.linearVelocity = bodyTransform.up * previousMovementInput.y * movementSpeed;
        
    }
    private void HandleMove(Vector2 movementInput){
        previousMovementInput = movementInput;
    }
}
