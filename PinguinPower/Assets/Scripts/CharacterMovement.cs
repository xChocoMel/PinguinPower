using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Used to manage the playable character (Penguin)
/// </summary>
public class CharacterMovement : MonoBehaviour
{
	private AudioClip kickClip;
	private AudioClip boostClip;
	private AudioClip boingClip;
	private AudioClip[] footstepClips;
	private AudioClip[] woehoeClips;
	private AudioClip jumpClip;

    private Rigidbody myRigidBody;
    private CapsuleCollider penguinCollider;
    private Transform graphics;
    private CharacterManager characterManager;

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
    private float currentSpeed = 0f;
    private TurnDirection turnDirection;
    private float currentTurnSpeed = 0f;

    public float maxWalkSpeed = 20f;
    public float maxTurnSpeed = 3f;
    public float jumpForce = 500f;
    public float constantGlidingForce = 6f;

    private bool jumping = false;
    private float jumpTimer;

    private bool turningPart = false;

    private float colliderYWalking = 0.8f;
    private float colliderYGliding = 0.44f;
	private bool isKicking;
    private bool inWindturbine;

    private int inCannon;
    private float inCannonTimer;

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
		getAudioClips ();
		GameObject p = GameObject.FindGameObjectWithTag("Penguin");
		this.graphics = p.transform.GetChild (0);

        this.myRigidBody = this.GetComponent<Rigidbody>();
        this.penguinCollider = this.GetComponent<CapsuleCollider>();
        this.animator = this.GetComponentInChildren<Animator>();
        this.characterManager = this.GetComponent<CharacterManager>();

        this.movementMode = MovementMode.Walk;

        this.jumpTimer = 0.3f;

        this.inWindturbine = false;
        this.inCannon = 0;
        this.inCannonTimer = 0;

        Physics.gravity = Vector3.up * walkGravity;
    }

	private void getAudioClips() {
		AudioClip[] audio = Resources.LoadAll<AudioClip>("Sounds");
		footstepClips = Resources.LoadAll<AudioClip>("Sounds/Footsteps");
		woehoeClips = new AudioClip[2];
		
		int i = 0;
		
		foreach (AudioClip a in audio) {
			if (a.name.Equals("Kick")) {
				kickClip = a;
			} else if (a.name.Equals("Swoosh")) {
				boostClip = a;
			} else if (a.name.Equals("Boin")) {
				boingClip = a;
			} else if (a.name.Equals("jump")) {
				jumpClip = a;
			} else if (a.name.Contains("pinguin_happy_speed")) {
				woehoeClips[i] = a;
				i++;
			}
		}
	}

    void FixedUpdate()
    {
        //Movement
        if (this.movementMode == MovementMode.Walk)
        {
            this.myRigidBody.AddRelativeForce(Vector3.forward * currentSpeed);
        }
        else if (this.movementMode == MovementMode.Glide)
        {
            myRigidBody.AddRelativeForce(Vector3.forward * constantGlidingForce);
        }

        //Windturbine
        if (this.inWindturbine)
        {
            Debug.Log("Wind boost");
            this.myRigidBody.AddRelativeForce(Vector3.up * 50);
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

        // Audio gliding
        if (this.movementMode == MovementMode.Glide)
        {
            bool grounded = this.IsGroundedGliding();
            if (audioSourceGliding.isPlaying && !grounded)
            {
                audioSourceGliding.Stop();
            }
            else if (!audioSourceGliding.isPlaying && grounded)
            {
                audioSourceGliding.Play();
            }
        }

        //Cannon
        if (this.inCannon == 2)
        {
            this.inCannonTimer -= Time.deltaTime;
            if (this.inCannonTimer < 0)
            {
                this.inCannon = 3;
            }
        }
        else if (this.inCannon == 3)
        {
            if (this.IsGroundedCannon())
            {
                this.inCannon = 4;
            }
        }
        else if (this.inCannon == 4)
        {
            Quaternion defaultRotation = new Quaternion(0, this.transform.rotation.y, 0, 0);
            if (this.transform.rotation != defaultRotation)
            {
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, defaultRotation, 0.1f);
            }
            else
            {
                this.inCannon = 0;
            }
        }

        Debug.Log("Cannon: " + this.inCannon);
    }

    private bool IsGroundedGliding()
    {
        return Physics.Raycast(this.transform.position, -Vector3.up, 0.5f);
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
    //public void ForwardMovementUp()
    //{
    //    if (this.movementMode == MovementMode.Walk)
    //    {
    //        if (this.moveDirection == MoveDirection.Stop)
    //        {
    //            StartCoroutine(PlayFootsteps());
    //        }
    //        this.moveDirection = MoveDirection.Forward1;
    //        this.animator.SetBool("Walking", true);
    //    }
    //}

    /// <summary>
    /// Set currentSpeed to speedPercentage of maxSpeed
    /// </summary>
    /// <param name="speedPercentage">Value between 0 and 1</param>
    public void MoveForward(float speedPercentage) {
        if (this.movementMode == MovementMode.Walk)
        {
            if (speedPercentage > 1)
            {
                speedPercentage = 1;
            }
            if (this.inCannon != 0)
            {
                return;
            }

            if (this.currentSpeed == 0f)
            {
                StartCoroutine(PlayFootsteps());
            }
            this.currentSpeed = this.maxWalkSpeed * speedPercentage;
            this.animator.SetBool("Walking", true);
            if (speedPercentage == 0)
            {
                this.animator.SetBool("Walking", false);
            }
        }
    }

    private IEnumerator PlayFootsteps()
    {
        yield return new WaitForSeconds(0.5f);
        if (this.movementMode == MovementMode.Walk && this.currentSpeed != 0f)
        {
            if (!jumping)
            {
                this.audioSourceWalking.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]);
            }
            StartCoroutine(PlayFootsteps());
        }
    }

    /// <summary>
    /// Switch to slow forward movement if on fast forward movement
    /// Switch to stop (no forward movement) if on slow forward movement
    /// </summary>
    //public void ForwardMovementDown()
    //{
    //    if (this.movementMode == MovementMode.Walk)
    //    {
    //        //if (this.moveDirection == MoveDirection.Forward1)
    //        //{
    //        //    this.slowDownSpeed = walkSpeed1;
    //        //}

    //        this.moveDirection = MoveDirection.Stop;
    //        this.myRigidBody.velocity = new Vector3(0, this.myRigidBody.velocity.y, 0);
    //        this.animator.SetBool("Walking", false);
    //    }
    //}

    /// <summary>
    /// Turn left or right
    /// </summary>
    //public void Turn(TurnDirection turnDirection)
    //{
    //    this.turnDirection = turnDirection;
    //    Vector3 rotation = Vector3.zero;
    //    Vector3 sidewaysMovement = Vector3.zero;

    //    if (this.movementMode == MovementMode.Walk)
    //    {
    //        switch (this.turnDirection)
    //        {
    //            case TurnDirection.Stop: break;
    //            case TurnDirection.Left:
    //                rotation += transform.up * -maxTurnSpeed;
    //                if (this.currentSpeed != 0f)
    //                {
    //                    sidewaysMovement += -Vector3.right * maxTurnSpeed * 10;
    //                }
    //                break;
    //            case TurnDirection.Right:
    //                rotation += transform.up * maxTurnSpeed;
    //                if (this.currentSpeed != 0f)
    //                {
    //                    sidewaysMovement += Vector3.right * maxTurnSpeed * 10;
    //                }
    //                break;
    //        }

    //        this.myRigidBody.transform.Rotate(rotation);

    //        if (this.currentSpeed > 0f)
    //        {
    //            //Calculate new force direction after turning (to prevent walking in the old direction after you the penguin has turned)
    //            float magnitude = new Vector3(this.myRigidBody.velocity.x, 0, this.myRigidBody.velocity.z).magnitude;
    //            Vector3 newVelocity = this.transform.forward * magnitude;
    //            newVelocity.y = this.myRigidBody.velocity.y;
    //            if (IsGrounded())
    //            {
    //                this.myRigidBody.velocity = newVelocity;
    //            }
    //            else
    //            {
    //                this.myRigidBody.velocity = newVelocity;
    //            }
    //        }

    //        //this.myRigidBody.AddRelativeForce(sidewaysMovement);
    //    }
    //    else if (this.movementMode == MovementMode.Glide)
    //    {
    //        switch (this.turnDirection)
    //        {
    //            case TurnDirection.Stop: 
    //                break;
    //            case TurnDirection.Left:
    //                rotation += transform.up * -maxTurnSpeed;
    //                sidewaysMovement += -Vector3.right * maxTurnSpeed * 10;
    //                break;
    //            case TurnDirection.Right:
    //                rotation += transform.up * maxTurnSpeed;
    //                sidewaysMovement += Vector3.right * maxTurnSpeed * 10;
    //                break;
    //        }
    //        this.myRigidBody.transform.Rotate(rotation);
    //        this.myRigidBody.AddRelativeForce(sidewaysMovement);
    //    }

    //    this.turnDirection = TurnDirection.Stop;
    //}

    /// <summary>
    /// Set turnSpeed to speedPercentage of maxTurnSpeed
    /// </summary>
    /// <param name="speedPercentage">Value between 0 and 1</param>
    public void Turn(TurnDirection turnDirection, float speedPercentage)
    {
        if (this.inCannon != 0)
        {
            return;
        }

        if (speedPercentage > 1)
        {
            speedPercentage = 1;
        }

        this.turnDirection = turnDirection;
        this.currentTurnSpeed = speedPercentage * maxTurnSpeed;
        Vector3 rotation = Vector3.zero;
        Vector3 sidewaysMovement = Vector3.zero;

        if (this.movementMode == MovementMode.Walk)
        {
            switch (this.turnDirection)
            {
                case TurnDirection.Stop: break;
                case TurnDirection.Left:
                    rotation += transform.up * -currentTurnSpeed;
                    if (this.currentSpeed != 0f)
                    {
                        sidewaysMovement += -Vector3.right * -currentTurnSpeed * 10;
                    }
                    break;
                case TurnDirection.Right:
                    rotation += transform.up * currentTurnSpeed;
                    if (this.currentSpeed != 0f)
                    {
                        sidewaysMovement += Vector3.right * currentTurnSpeed * 10;
                    }
                    break;
            }

            this.myRigidBody.transform.Rotate(rotation);
            //print(speedPercentage);
            if (this.currentSpeed <= 0f && turnDirection != TurnDirection.Stop)
            {
                //this.myRigidBody.AddRelativeForce(Vector3.forward * (maxTurnSpeed / 2) * 10);
                this.animator.SetBool("Walking", true);
            }
            else if (turnDirection == TurnDirection.Stop)
            {
                this.animator.SetBool("Walking", false);
            }

            if (this.currentSpeed > 0f)
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
                    rotation += transform.up * -currentTurnSpeed;
                    sidewaysMovement += -Vector3.right * currentTurnSpeed * 10;
                    break;
                case TurnDirection.Right:
                    rotation += transform.up * maxTurnSpeed;
                    sidewaysMovement += Vector3.right * currentTurnSpeed * 10;
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
            this.myRigidBody.AddRelativeForce(new Vector3(0, jumpForce, jumpForce), ForceMode.Force);
            this.audioSourceNormal.PlayOneShot(woehoeClips[Random.Range(0, woehoeClips.Length)]);
            this.audioSourceWalking.PlayOneShot(jumpClip);
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(this.transform.position, -Vector3.up, 0.2f);
    }

    private bool IsGroundedCannon()
    {
        if (Physics.Raycast(this.transform.position, -Vector3.up, 1f) || Physics.Raycast(this.transform.position, Vector3.forward, 1f))
        {
            return true;
        }
        return false;
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
            if (this.currentSpeed > 0f)
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
        }

        DetermineMovementMode(other);
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject other = collider.gameObject;
        switch (other.tag)
        {
            case "SpeedBoost":
                this.audioSourceNormal.PlayOneShot(boostClip);
                this.audioSourceWalking.PlayOneShot(woehoeClips[Random.Range(0, woehoeClips.Length)]);
                break;
            case "Trampoline":
                this.audioSourceNormal.PlayOneShot(boingClip);
                this.audioSourceWalking.PlayOneShot(woehoeClips[Random.Range(0, woehoeClips.Length)]);
                break;
            case "Windturbine":
                this.inWindturbine = true;
                break;
            case "Cannon":
                if (this.inCannon == 0)
                {
                    Cannon cannon = collider.GetComponent<Cannon>();
                    if (cannon.LoadCannonAllowed())
                    {
                        this.transform.parent = cannon.getSpot();
                        this.transform.localPosition = Vector3.zero;
                        this.transform.localRotation = new Quaternion(0, 0, 0, 0);
                        this.myRigidBody.useGravity = false;
                        this.myRigidBody.velocity = Vector3.zero;
                        this.MoveForward(0f);
                        this.inCannon = 1;
                        cannon.Load(this.transform);
                    }
                }
                break;
        }
    }
    
    void OnCollisionStay(Collision coll)
    {
        GameObject other = coll.gameObject;
        DetermineMovementMode(other);        
    }

    private IEnumerator SetStraightPart()
    {
        yield return new WaitForSeconds(2);
        this.turningPart = false;
    }

    private void DetermineMovementMode(GameObject other)
    {
        // Switching gliding/walking
        if (other.name.Contains("Terrain"))
        {
            Terrain terrain = (Terrain)other.GetComponent<Terrain>();
            int mainTexture = TerrainSurface.GetMainTexture(this.transform.position, terrain);
            string texturename = terrain.terrainData.splatPrototypes[mainTexture].texture.name;
            if (texturename.Contains(glidingTexture.name))
            {
                this.SwitchMovementMode(MovementMode.Glide);
                this.turningPart = true;
                StartCoroutine(SetStraightPart());
            }
            else if (texturename.Contains(walkingTexture.name))
            {
                this.SwitchMovementMode(MovementMode.Walk);
            }
        }
        else if (IsGrounded())
        {
            Renderer r = other.GetComponent<Renderer>();
            if (r == null)
            {
                r = other.GetComponentInChildren<Renderer>();
            }
            if (r != null)
            {
                string materialname = r.material.name;
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

    public bool isRotating()
    {
        return this.movementMode == MovementMode.Glide && this.turningPart && (this.myRigidBody.velocity.z < 1f);
    }

    //void OnParticleCollision (GameObject other)
    //{
    //    if (other.tag == "Wind")
    //    {
    //        this.myRigidBody.AddRelativeForce(Vector3.up * 300);
    //    }
    //}

    void OnTriggerExit(Collider collider)
    {
        switch (collider.tag)
        {
            case "Windturbine":
                this.inWindturbine = false;
                break;
        }
    }

    public void DetachCannon()
    {
        this.inCannon = 2;
        this.inCannonTimer = 1;
    }
}