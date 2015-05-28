using UnityEngine;
using System.Collections;

/// <summary>
/// Used to manage the playable character (Penguin)
/// </summary>
public class CharacterManager : MonoBehaviour {

    private Rigidbody rigidBody;
    private Collider penguinCollider;

    private MovementMode movementMode;
    private MoveDirection moveDirection;
    private TurnDirection turnDirection;
    private float walkSpeed1;
    private float walkSpeed2;
    private float turnSpeed;
    private float jumpForce;
    private float jumpTimer;

	// Use this for initialization
	void Start () {
        
        this.rigidBody = this.gameObject.GetComponent<Rigidbody>();
        this.penguinCollider = this.gameObject.GetComponent<CapsuleCollider>();

        this.Setup();
	}

    private void Setup() {
        this.movementMode = MovementMode.Walk;
        this.moveDirection = MoveDirection.Stop;
        this.turnDirection = TurnDirection.Stop;

        this.walkSpeed1 = 5;
        this.walkSpeed2 = 10;
        this.turnSpeed = 2;
        this.jumpForce = 250;
        this.jumpTimer = 0.3f;
    }
	
	// Update is called once per frame
	void Update () {
	
        Vector3 force = Vector3.zero;
        Vector3 rotation = Vector3.zero;

        //Movement
        switch (this.movementMode) {
            case MovementMode.Walk:

                switch (this.turnDirection)
                {
                    case TurnDirection.Stop: break;
                    case TurnDirection.Left: rotation = rotation = new Vector3(0, -turnSpeed, 0); break;
                    case TurnDirection.Right: rotation = rotation = new Vector3(0, turnSpeed, 0); break;  
                }

                this.rigidBody.transform.Rotate(rotation);

                //Reset turndirection (to have players hold the arrow keys instead of pressing)
                this.turnDirection = TurnDirection.Stop;

                switch (this.moveDirection) {
                    case MoveDirection.Stop: break;
                    case MoveDirection.Forward1: force = Vector3.forward * walkSpeed1; break;
                    case MoveDirection.Forward2: force = Vector3.forward * walkSpeed2; break;
                }

                this.rigidBody.AddRelativeForce(force);
                
                break;

            case MovementMode.Glide: break;

            case MovementMode.Swim: break;
        }



        //Timers
        if (this.jumpTimer > 0) {
            this.jumpTimer -= Time.deltaTime;
        }
	}

    /// <summary>
    /// Switch to slow forward movement if stopped.
    /// Switch to fast forward movement if on slow forward movement
    /// </summary>
    public void ForwardMovementUp() {
        Debug.Log("--Penguin > Forwardmovement++");
        switch (this.moveDirection) {
            case MoveDirection.Stop: this.moveDirection = MoveDirection.Forward1; break;
            case MoveDirection.Forward1: this.moveDirection = MoveDirection.Forward2; break;
        }
    }

    /// <summary>
    /// Switch to slow forward movement if on fast forward movement
    /// Switch to stop (no forward movement) if on slow forward movement
    /// </summary>
    public void ForwardMovementDown()
    {
        Debug.Log("--Penguin > Forwardmovement--");
        switch (this.moveDirection) {
            case MoveDirection.Forward1: this.moveDirection = MoveDirection.Stop; break;
            case MoveDirection.Forward2: this.moveDirection = MoveDirection.Forward1; break;
        }
    }

    /// <summary>
    /// Turn left or right
    /// </summary>
    public void Turn(TurnDirection turnDirection) {
        Debug.Log("--Penguin > Turn " + turnDirection.ToString());
        this.turnDirection = turnDirection;
    }

    /// <summary>
    /// Jump (walk mode)
    /// Swim up (swim mode)
    /// </summary>
    public void Jump() {
        Debug.Log("--Penguin > Jump");
        if (this.IsGrounded() && this.jumpTimer <= 0) {
            this.rigidBody.velocity = new Vector3(this.rigidBody.velocity.x, 0, this.rigidBody.velocity.z);
            this.jumpTimer = 0.1f;
            this.rigidBody.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Force);
        }
    }

    private bool IsGrounded() {
        return Physics.Raycast(this.transform.position, -Vector3.up, 1.01f);
    }

    public void Kick() {
        Debug.Log("--Penguin > Kick ");
    }
}
