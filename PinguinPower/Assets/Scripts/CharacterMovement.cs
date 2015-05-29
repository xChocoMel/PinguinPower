using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Used to manage the playable character (Penguin)
/// </summary>
public class CharacterMovement : MonoBehaviour
{

    public Text Text1;
    public Text Text2;

    public Rigidbody rigidBody;
    public Collider penguinCollider;

    public float glideDrag = 0.5f;
    public float walkDrag = 1f;

    private MovementMode movementMode;
    private MoveDirection moveDirection;
    private TurnDirection turnDirection;

    public float walkSpeed1 = 5f;
    public float walkSpeed2 = 10f;
    public float turnSpeed = 2f;
    public float jumpForce = 500f;

    private bool jumping;

    // Use this for initialization
    void Start()
    {
        this.Setup();
    }

    private void Setup()
    {
        this.movementMode = MovementMode.Walk;
        this.moveDirection = MoveDirection.Stop;
        this.turnDirection = TurnDirection.Stop;
    }

    // Update is called once per frame
    void Update()
    {
        print(this.rigidBody.velocity.y);

        //Movementmode & Drag
        if (Mathf.Abs(this.rigidBody.velocity.y) > 0.01f && !jumping)
        {
            this.rigidBody.drag = glideDrag;
            if (this.movementMode == MovementMode.Walk && IsGrounded())
            {
                SwitchMovementMode(MovementMode.Glide);
            }
        }
        else
        {
            this.rigidBody.drag = walkDrag;

            SwitchMovementMode(MovementMode.Walk);
        }

        //Movement
        if (this.movementMode == MovementMode.Walk)
        {
            Vector3 force = Vector3.zero;
            switch (this.moveDirection)
            {
                case MoveDirection.Stop: break;
                case MoveDirection.Forward1: force = Vector3.forward * walkSpeed1; break;
                case MoveDirection.Forward2: force = Vector3.forward * walkSpeed2; break;
            }
            rigidBody.AddRelativeForce(force);
        }

        //Lookat falling direction
        if (this.movementMode == MovementMode.Glide && this.turnDirection == TurnDirection.Stop)
        {
            this.transform.rotation = new Quaternion(this.transform.rotation.x, Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(this.rigidBody.velocity), 0.1f).y, this.transform.rotation.z, this.transform.rotation.w);
        }
    }

    /// <summary>
    /// Switch to slow forward movement if stopped.
    /// Switch to fast forward movement if on slow forward movement
    /// </summary>
    public void ForwardMovementUp()
    {
        Debug.Log("--Penguin > Forwardmovement++");
        switch (this.moveDirection)
        {
            case MoveDirection.Stop: this.moveDirection = MoveDirection.Forward1; break;
            case MoveDirection.Forward1: this.moveDirection = MoveDirection.Forward2; break;
        }

        this.Text2.text = this.moveDirection.ToString();
    }

    /// <summary>
    /// Switch to slow forward movement if on fast forward movement
    /// Switch to stop (no forward movement) if on slow forward movement
    /// </summary>
    public void ForwardMovementDown()
    {
        Debug.Log("--Penguin > Forwardmovement--");
        switch (this.moveDirection)
        {
            case MoveDirection.Forward1: this.moveDirection = MoveDirection.Stop; break;
            case MoveDirection.Forward2: this.moveDirection = MoveDirection.Forward1; break;
        }

        this.Text2.text = this.moveDirection.ToString();
    }

    /// <summary>
    /// Turn left or right
    /// </summary>
    public void Turn(TurnDirection turnDirection)
    {
        Debug.Log("--Penguin > Turn " + turnDirection.ToString());

        this.turnDirection = turnDirection;
        Vector3 rotation = Vector3.zero;
        Vector3 sidewaysMovement = Vector3.zero;

        if (this.movementMode == MovementMode.Walk)
        {
            switch (this.turnDirection)
            {
                case TurnDirection.Stop: break;
                case TurnDirection.Left:
                    rotation += transform.up * -turnSpeed;
                    if (this.moveDirection != MoveDirection.Stop)
                    {
                        sidewaysMovement += -Vector3.right * turnSpeed * 10;
                    }
                    break;
                case TurnDirection.Right:
                    rotation += transform.up * turnSpeed;
                    if (this.moveDirection != MoveDirection.Stop)
                    {
                        sidewaysMovement += Vector3.right * turnSpeed * 10;
                    }
                    break;
            }

            this.rigidBody.transform.Rotate(rotation);
            this.rigidBody.AddRelativeForce(sidewaysMovement);
        }
        else if (this.movementMode == MovementMode.Glide)
        {

            switch (this.turnDirection)
            {
                case TurnDirection.Stop: 
                    break;
                case TurnDirection.Left:
                    rotation += transform.up * -turnSpeed;
                    sidewaysMovement += -Vector3.right * turnSpeed * 10;
                    break;
                case TurnDirection.Right:
                    rotation += transform.up * turnSpeed;
                    sidewaysMovement += Vector3.right * turnSpeed * 10;
                    break;
            }

            this.rigidBody.transform.Rotate(rotation);
            this.rigidBody.AddRelativeForce(sidewaysMovement);
        }

        this.turnDirection = TurnDirection.Stop;
    }

    /// <summary>
    /// Jump (walk mode)
    /// Swim up (swim mode)
    /// </summary>
    public void Jump()
    {
        if (this.movementMode == MovementMode.Walk && this.IsGrounded() && !jumping)
        {
            Debug.Log("--Penguin > Jump");
            this.rigidBody.velocity = new Vector3(this.rigidBody.velocity.x, 0, this.rigidBody.velocity.z);
            this.jumping = true;
            this.rigidBody.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Force);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(this.transform.position, -Vector3.up, 1.1f);
    }

    public void Kick()
    {
        Debug.Log("--Penguin > Kick ");
    }

    public void SwitchMovementMode(MovementMode m)
    {
        this.movementMode = m;

        this.Text1.text = this.movementMode.ToString();

        //if (m == MovementMode.Glide)
        //{
        //    this.moveDirection = MoveDirection.Stop;
        //}
    }
}