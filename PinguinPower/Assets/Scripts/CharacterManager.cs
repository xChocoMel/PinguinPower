using UnityEngine;
using System.Collections;

/// <summary>
/// Used to manage the playable character (Penguin)
/// </summary>
public class CharacterManager : MonoBehaviour {

    private Rigidbody rigidBody;

    private MovementMode movementMode;
    private MoveDirection moveDirection;
    private TurnDirection turnDirection;
    private float walkSpeed1;
    private float walkSpeed2;
    private float turnSpeed;

	// Use this for initialization
	void Start () {
        
        this.rigidBody = this.gameObject.GetComponent<Rigidbody>();

        this.Setup();
	}

    private void Setup() {
        this.movementMode = MovementMode.Walk;
        this.moveDirection = MoveDirection.Stop;
        this.turnDirection = TurnDirection.Stop;

        this.walkSpeed1 = 10;
        this.walkSpeed2 = 20;
        this.turnSpeed = 5;
    }
	
	// Update is called once per frame
	void Update () {
	
        Vector3 force = Vector3.zero;
        Vector3 rotation = Vector3.zero;

        //Movement
        switch (this.movementMode) {
            case MovementMode.Walk:

                switch (this.moveDirection) {
                    case MoveDirection.Stop: break;
                    case MoveDirection.Forward1: force = transform.forward * walkSpeed1; break;
                    case MoveDirection.Forward2: force = transform.forward * walkSpeed2; break;
                }
                rigidBody.AddRelativeForce(force);
                //this.rigidBody.velocity = force;

                /*
                switch (this.turnDirection)
                {
                    case TurnDirection.Stop: break;
                    case TurnDirection.Left: rotation += -transform.right * 90;  break;
                    case TurnDirection.Right: rotation += transform.right * 90;  break;  
                }

                this.transform.Rotate(rotation);*/
                
                break;

            case MovementMode.Glide: 
                
                break;

            case MovementMode.Swim: break;
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
        Vector3 rotation = Vector3.zero;
        switch (this.turnDirection)
        {
            case TurnDirection.Stop: break;
            case TurnDirection.Left: rotation += transform.up * -2; break;
            case TurnDirection.Right: rotation += transform.up * 2; break;
        }

        this.transform.Rotate(rotation);
    }

    /// <summary>
    /// Jump (walk mode)
    /// Swim up (swim mode)
    /// </summary>
    public void Jump() {
        Debug.Log("--Penguin > Jump");
    }

    public void Kick() {
        Debug.Log("--Penguin > Kick ");
    }
}
