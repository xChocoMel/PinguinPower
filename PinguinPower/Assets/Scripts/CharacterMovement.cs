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

    private Rigidbody myRigidBody;
    private CapsuleCollider penguinCollider;
    public Transform graphics;

    public Material glidingMaterial;
    public Material walkingMaterial;
    public Texture glidingTexture;
    public Texture walkingTexture;

    public float glideDrag = 0.5f;
    public float walkDrag = 1f;
    private float jumpDrag = 1f;

    private MovementMode movementMode;
    private MoveDirection moveDirection;
    private TurnDirection turnDirection;

    public float walkSpeed1 = 5f;
    public float walkSpeed2 = 10f;
    public float turnSpeed = 2f;
    public float jumpForce = 500f;

    private bool jumping = false;
    private float jumpTimer;

    private bool turningPart = false;

    private float colliderYWalking = 0.8f;
    private float colliderYGliding = 0.44f;

    // Use this for initialization
    void Start()
    {
        this.Setup();
    }

    private void Setup()
    {
        this.myRigidBody = this.GetComponent<Rigidbody>();
        this.penguinCollider = this.GetComponent<CapsuleCollider>();

        this.movementMode = MovementMode.Walk;
        this.moveDirection = MoveDirection.Stop;
        this.turnDirection = TurnDirection.Stop;

        this.jumpTimer = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = this.myRigidBody.velocity;

        //Jumping
        if (jumping && this.IsGrounded() && this.jumpTimer <= 0)
        {
            jumping = false;
            this.ResetDrag();
        }
        //print(this.rigidBody.velocity.y);
        //Movementmode & Drag
        /*
        if (velocity.y < -0.1f && !jumping)
        {
            
            if (this.movementMode == MovementMode.Walk && IsGrounded())
            {
                //this.myRigidBody.drag = glideDrag;
                //SwitchMovementMode(MovementMode.Glide);
            }
        }
        else if (!jumping)
        {
            if (this.movementMode == MovementMode.Glide && IsGrounded())
            {
                //this.myRigidBody.drag = walkDrag;
                //SwitchMovementMode(MovementMode.Walk);
            }
        }*/

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
            myRigidBody.AddRelativeForce(force);
        }

        float smooth = 0.2f;
        //Lookat falling direction
        if (turningPart)
        {
            if (this.movementMode == MovementMode.Glide)
            {
                // Turn transform
                Quaternion lookRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(this.myRigidBody.velocity), smooth);
                this.transform.rotation = new Quaternion(this.transform.rotation.x, lookRotation.y, this.transform.rotation.z, lookRotation.w);
            }
        }
        else
        {
            if (this.movementMode == MovementMode.Glide)
            {
                // Turn graphics
                Quaternion lookRotation = Quaternion.Lerp(graphics.rotation, Quaternion.LookRotation(this.myRigidBody.velocity), smooth);
                this.graphics.rotation = new Quaternion(this.graphics.rotation.x, lookRotation.y, this.graphics.rotation.z, lookRotation.w);
            }
        }

        //Timers
        if (this.jumpTimer > 0)
        {
            this.jumpTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Reset drag depending on MovementMode
    /// </summary>
    private void ResetDrag()
    {
        if (this.movementMode == MovementMode.Glide)
        {
            this.myRigidBody.drag = glideDrag;
        }
        else if (this.movementMode == MovementMode.Walk)
        {
            this.myRigidBody.drag = walkDrag;
        }
    }

    /// <summary>
    /// Switch to slow forward movement if stopped.
    /// Switch to fast forward movement if on slow forward movement
    /// </summary>
    public void ForwardMovementUp()
    {
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

            this.myRigidBody.transform.Rotate(rotation);
            this.myRigidBody.AddRelativeForce(sidewaysMovement);
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
            this.myRigidBody.AddRelativeForce(sidewaysMovement);
        }

        this.turnDirection = TurnDirection.Stop;
    }

    /// <summary>
    /// Jump (walk mode)
    /// Swim up (swim mode)
    /// </summary>
    public void Jump()
    {
        if (this.movementMode == MovementMode.Walk && !jumping)
        {
            this.jumping = true;
            this.myRigidBody.drag = jumpDrag;
            this.myRigidBody.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Force);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(this.transform.position, -Vector3.up, 1.1f);
    }

    public void Kick()
    {
    }

    public void SwitchMovementMode(MovementMode m)
    {
        this.movementMode = m;

        this.Text1.text = this.movementMode.ToString();

        if (m == MovementMode.Walk)
        {
            transform.rotation = graphics.rotation;
            graphics.localRotation = Quaternion.identity;

            // Change collider
            this.penguinCollider.center = new Vector3(this.penguinCollider.center.x, colliderYWalking, this.penguinCollider.center.z);
            this.penguinCollider.direction = 1;
        }
        else if (m == MovementMode.Glide)
        {
            // Change collider
            this.penguinCollider.center = new Vector3(this.penguinCollider.center.x, colliderYGliding, this.penguinCollider.center.z);
            this.penguinCollider.direction = 2;
        }

        /*0.8 > lopen op y-axis
0.44 > glijden op z-axis
^y collider*/

        this.ResetDrag();
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
                    transform.rotation = graphics.rotation;
                    graphics.localRotation = Quaternion.identity;
                }
                break;
            case "StraightPart":
                if (this.movementMode == MovementMode.Glide)
                {
                    turningPart = false;
                }
                break;
        }
        
        // Switching gliding/walking
        if (other.name.Contains("Terrain"))
        {
            Terrain terrain = (Terrain)other.GetComponent<Terrain>();
            int mainTexture = TerrainSurface.GetMainTexture(this.transform.position, terrain);
            string texturename = terrain.terrainData.splatPrototypes[mainTexture].texture.name;
            if (texturename.Contains(glidingTexture.name))
            {
                this.SwitchMovementMode(MovementMode.Glide);
            }
            else if (texturename.Contains(walkingTexture.name))
            {
                this.SwitchMovementMode(MovementMode.Walk);
            }
        }
        else
        {
            string materialname = other.GetComponent<Renderer>().material.name;
            if (materialname.Contains(glidingMaterial.name))
            {
                this.SwitchMovementMode(MovementMode.Glide);
            }
            else if (materialname.Contains(walkingMaterial.name))
            {
                this.SwitchMovementMode(MovementMode.Walk);
            }
        }
    }
}