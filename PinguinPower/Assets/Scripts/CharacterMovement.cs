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
    public Transform glidingDirection;
    public Transform graphics;

    public float glideDrag = 0.5f;
    public float walkDrag = 1f;
    private float jumpDrag = 0f;

    private MovementMode movementMode;
    private MoveDirection moveDirection;
    private TurnDirection turnDirection;

    public float walkSpeed1 = 5f;
    public float walkSpeed2 = 10f;
    public float turnSpeed = 2f;
    public float jumpForce = 500f;

    private bool jumping = false;

    private float currentRotation = 0f;
    private float maxRotation = 45f;

    private bool turningPart = false;

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
        Vector3 velocity = this.rigidBody.velocity;

        //Jumping
        if (jumping && this.IsGrounded())
        {
            jumping = false;
            if (this.movementMode == MovementMode.Glide)
            {
                this.rigidBody.drag = glideDrag;
            }
            else if (this.movementMode == MovementMode.Walk)
            {
                this.rigidBody.drag = walkDrag;
            }
        }
        //print(this.rigidBody.velocity.y);
        //Movementmode & Drag
        if (velocity.y < -0.01f && !jumping)
        {
            
            if (this.movementMode == MovementMode.Walk && IsGrounded())
            {
                this.rigidBody.drag = glideDrag;
                SwitchMovementMode(MovementMode.Glide);
            }
        }
        else if (!jumping)
        {
            if (this.movementMode == MovementMode.Glide && IsGrounded())
            {
                this.rigidBody.drag = walkDrag;
                SwitchMovementMode(MovementMode.Walk);
            }
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
        if (turningPart)
        {
            if (this.movementMode == MovementMode.Glide)
            {
                float smooth = 0.1f;
                Quaternion lookRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(this.rigidBody.velocity), smooth);
                //lookRotation.
                float angle = Quaternion.Angle(lookRotation, transform.rotation);
                //this.currentRotation -= angle;
                this.transform.rotation = new Quaternion(this.transform.rotation.x, lookRotation.y, this.transform.rotation.z, lookRotation.w);
            }
        }
        else
        {
            if (this.movementMode == MovementMode.Glide)
            {
                float smooth = 0.1f;
                Quaternion lookRotation = Quaternion.Lerp(graphics.rotation, Quaternion.LookRotation(this.rigidBody.velocity), smooth);
                //lookRotation.
                float angle = Quaternion.Angle(lookRotation, graphics.rotation);
                //this.currentRotation -= angle;
                this.graphics.rotation = new Quaternion(this.graphics.rotation.x, lookRotation.y, this.graphics.rotation.z, lookRotation.w);
            }
        }
        
        print(this.currentRotation);
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
            glidingDirection.rotation = Quaternion.LookRotation(this.rigidBody.velocity);

            switch (this.turnDirection)
            {
                case TurnDirection.Stop: 
                    break;
                case TurnDirection.Left:
                    if (Mathf.Abs(currentRotation) < maxRotation)
                    {
                        rotation += transform.up * -turnSpeed;
                        currentRotation += -turnSpeed;
                    }
                    sidewaysMovement += -Vector3.right * turnSpeed * 10;
                    break;
                case TurnDirection.Right:
                    if (Mathf.Abs(currentRotation) < maxRotation)
                    {
                        rotation += transform.up * turnSpeed;
                        currentRotation += turnSpeed;
                    }
                    sidewaysMovement += Vector3.right * turnSpeed * 10;
                    break;
            }
            /*
            float angle;
            Vector3 axis;
            Quaternion.LookRotation(this.rigidBody.velocity).ToAngleAxis(out angle, out axis);
            float angleBetween = Vector3.Angle(axis, Vector3.forward);
            float wantedAngle = 90 - angleBetween;
            Quaternion wantedRotation = Quaternion.AngleAxis(90, Vector3.up);

            Transform axisTransform = Transform.Instantiate(this.transform);
            axisTransform.rotation = Quaternion.LookRotation(this.rigidBody.velocity);
            




            float sin = Mathf.Sin(90);
            float cos = Mathf.Cos(90);

            float tx = axis.x;
            float tz = axis.z;
            axis.x = (cos * tx) + (sin * tz);
            axis.z = (cos * tz) - (sin * tx);
            

            Vector3 right = axis * Vector3.forward / Vector3.right;
            print(Quaternion.Angle(this.transform.rotation, Quaternion.LookRotation(this.rigidBody.velocity)));*/

            if (Mathf.Abs(currentRotation) < maxRotation)
            {
                //this.rigidBody.transform.Rotate(rotation);
            }
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
            this.jumping = true;
            this.rigidBody.drag = jumpDrag;
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

    void OnCollisionEnter(Collision coll)
    {
        GameObject other = coll.gameObject;
        switch (other.tag)
        {
            case "TurningPart":
                if (this.movementMode == MovementMode.Glide)
                {
                    turningPart = true;
                }
                break;
            case "StraightPart":
                if (this.movementMode == MovementMode.Glide)
                {
                    turningPart = false;
                }
                break;
        }
    }
}