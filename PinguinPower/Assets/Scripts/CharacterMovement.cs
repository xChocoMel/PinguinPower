using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Used to manage the playable character (Penguin)
/// </summary>
public class CharacterMovement : MonoBehaviour
{

    //public Text Text1;
    //public Text Text2;

    public AudioClip kickClip;
    public AudioClip boostClip;
    public AudioClip boingClip;
    public AudioClip[] footstepClips;

    private Rigidbody myRigidBody;
    private CapsuleCollider penguinCollider;
    public Transform graphics;

    private Animator animator;
    public AudioSource audioSourceNormal;
    public AudioSource audioSourceGliding;
    public AudioSource audioSourceWalking;

    public Material glidingMaterial;
    public Material walkingMaterial;
    public Texture glidingTexture;
    public Texture walkingTexture;

    public float glideDrag = 0.5f;
    public float walkDrag = 1f;
    private float jumpDrag = 1f;

    public float walkGravity = -9.81f;
    public float glideGravity = -30f;

    private MovementMode movementMode;
    private MoveDirection moveDirection;
    private TurnDirection turnDirection;

    public float walkSpeed1 = 10f;
    public float walkSpeed2 = 10f;
    public float turnSpeed = 2f;
    public float jumpForce = 500f;
    public float constantGlidingForce = 6f;
    private float slowDownSpeed = 0;

    private bool jumping = false;
    private float jumpTimer;

    private bool turningPart = false;

    private float colliderYWalking = 0.8f;
    private float colliderYGliding = 0.44f;
	private bool isKicking;

    // Use this for initialization
    void Start()
    {
        this.Setup();
    }
	public bool IsKicking()
	{
		return isKicking;
	}
    private void Setup()
    {
        this.myRigidBody = this.GetComponent<Rigidbody>();
        this.penguinCollider = this.GetComponent<CapsuleCollider>();
        this.animator = this.GetComponentInChildren<Animator>();

        this.movementMode = MovementMode.Walk;
        this.moveDirection = MoveDirection.Stop;
        this.turnDirection = TurnDirection.Stop;

        this.jumpTimer = 0.3f;
    }

    void FixedUpdate()
    {
        //Movement
        if (this.movementMode == MovementMode.Walk)
        {
            switch (this.moveDirection)
            {
                case MoveDirection.Stop:
                    //if (slowDownSpeed > 0)
                    //{
                    //    slowDownSpeed -= 1;
                    //    if (slowDownSpeed < 0)
                    //    {
                    //        slowDownSpeed = 0;
                    //    }
                    //    this.myRigidBody.AddRelativeForce(Vector3.forward * slowDownSpeed);
                    //}
                    break;
                case MoveDirection.Forward1:
                    this.myRigidBody.AddRelativeForce(Vector3.forward * walkSpeed1);
                    //this.myRigidBody.velocity = new Vector3((transform.forward * walkSpeed1).x, this.myRigidBody.velocity.y, (transform.forward * walkSpeed1).z);
                    break;
            }
        }
        else if (this.movementMode == MovementMode.Glide)
        {
            myRigidBody.AddRelativeForce(Vector3.forward * constantGlidingForce);
        }
    }

    // Update is called once per frame
    void Update()
    {
		//this.animator.SetTrigger("Kick");
        //Jumping
        if (jumping && this.jumpTimer <= 0 && this.IsGrounded())
        {
            jumping = false;
            this.ResetDrag();
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
        if (this.movementMode == MovementMode.Walk)
        {
            if (this.moveDirection == MoveDirection.Stop)
            {
                StartCoroutine(PlayFootsteps());
            }
            this.moveDirection = MoveDirection.Forward1;
            this.animator.SetBool("Walking", true);
        }
    }

    private IEnumerator PlayFootsteps()
    {
        yield return new WaitForSeconds(0.5f);
        if (this.moveDirection != MoveDirection.Stop)
        {
            this.audioSourceWalking.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
            StartCoroutine(PlayFootsteps());
        }
    }

    /// <summary>
    /// Switch to slow forward movement if on fast forward movement
    /// Switch to stop (no forward movement) if on slow forward movement
    /// </summary>
    public void ForwardMovementDown()
    {
        if (this.movementMode == MovementMode.Walk)
        {
            //if (this.moveDirection == MoveDirection.Forward1)
            //{
            //    this.slowDownSpeed = walkSpeed1;
            //}

            this.moveDirection = MoveDirection.Stop;
            this.myRigidBody.velocity = new Vector3(0, this.myRigidBody.velocity.y, 0);
            this.animator.SetBool("Walking", false);
        }
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

            if (this.moveDirection == MoveDirection.Forward1)
            {
                //Calculate new force direction after turning (to prevent walking in the old direction after you the penguin has turned)
                float magnitude = new Vector3(this.myRigidBody.velocity.x, 0, this.myRigidBody.velocity.z).magnitude;
                Vector3 newVelocity = this.transform.forward * magnitude;
                newVelocity.y = this.myRigidBody.velocity.y;
                if (IsGrounded())
                {
                    this.myRigidBody.velocity = newVelocity;
                }
                else
                {
                    this.myRigidBody.velocity = newVelocity;
                }
            }

            //this.myRigidBody.AddRelativeForce(sidewaysMovement);
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
            this.myRigidBody.transform.Rotate(rotation);
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
            this.jumpTimer = 0.3f;
            this.jumping = true;
            this.animator.SetTrigger("Jump");
            this.myRigidBody.velocity = new Vector3(this.myRigidBody.velocity.x, 0, this.myRigidBody.velocity.z);
            this.myRigidBody.drag = jumpDrag;
            this.myRigidBody.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Force);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(this.transform.position, -Vector3.up, 0.2f);
    }

    public void Kick()
    {
		this.animator.SetTrigger("Kick");
        StartCoroutine(PlayKickClip());
		if(!isKicking)
		{
			StartCoroutine(Kicking());
		}
    }

    private IEnumerator PlayKickClip()
    {
        yield return new WaitForSeconds(0.2f);
        this.audioSourceNormal.PlayOneShot(kickClip);
    }

	private IEnumerator Kicking()
	{
		isKicking = true;
		yield return new WaitForSeconds (1.0F);
		isKicking = false;
	}

    public void SwitchMovementMode(MovementMode m)
    {
        // Play animations
        if (this.movementMode == MovementMode.Walk && m == MovementMode.Glide)
        {
            animator.SetTrigger("StartGliding");
            animator.SetBool("Walking", false);
            audioSourceGliding.Play();
        }
        else if (this.movementMode == MovementMode.Glide && m == MovementMode.Walk)
        {
            print(" stop gliding");
            animator.SetTrigger("StopGliding");
            if (this.moveDirection == MoveDirection.Forward1 || this.moveDirection == MoveDirection.Forward2)
            {
                animator.SetBool("Walking", true);
            }
            audioSourceGliding.Stop();
        }
        
        this.movementMode = m;

        //this.Text1.text = this.movementMode.ToString();

        if (m == MovementMode.Walk)
        {
            transform.rotation = graphics.rotation;
            graphics.localRotation = Quaternion.identity;

            // Change collider
            this.penguinCollider.center = new Vector3(this.penguinCollider.center.x, colliderYWalking, this.penguinCollider.center.z);
            this.penguinCollider.direction = 1;

            // Change gravity
            Physics.gravity = Vector3.up * walkGravity;
        }
        else if (m == MovementMode.Glide)
        {
            // Change collider
            this.penguinCollider.center = new Vector3(this.penguinCollider.center.x, colliderYGliding, this.penguinCollider.center.z);
            this.penguinCollider.direction = 2;

            // Change gravity
            Physics.gravity = Vector3.up * glideGravity;
        }

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
            case "SpeedBoost":
                this.audioSourceNormal.PlayOneShot(boostClip);
                //TODO
                break;
            case "Trampoline":
                this.audioSourceNormal.PlayOneShot(boingClip);
                //TODO
                break;
        }
        
        // Switching gliding/walking
        if (other.name.Contains("Terrain"))
        {
            Terrain terrain = (Terrain)other.GetComponent<Terrain>();
            int mainTexture = TerrainSurface.GetMainTexture(this.transform.position, terrain);
            string texturename = terrain.terrainData.splatPrototypes[mainTexture].texture.name;
            print(texturename + " - " + walkingTexture.name);
            if (texturename.Contains(glidingTexture.name))
            {
                this.SwitchMovementMode(MovementMode.Glide);
            }
            else if (texturename.Contains(walkingTexture.name))
            {
                print("go walking");
                this.SwitchMovementMode(MovementMode.Walk);
            }
        }
        else if (IsGrounded())
        {
            string materialname = other.GetComponentInChildren<Renderer>().material.name;
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
    
    void OnCollisionStay(Collision coll)
    {
        GameObject other = coll.gameObject;
        
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
        else if (IsGrounded())
        {
            string materialname = other.GetComponentInChildren<Renderer>().material.name;
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