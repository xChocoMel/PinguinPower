using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    public int FishPerLife = 50;

    public MenuManager menuManager;

    public AudioClip collectFishClip;
    public AudioClip extraLifeClip;
    public AudioClip[] ouchPenguinClips;
    public AudioClip deadClip;
    public AudioClip oefClip;
	public AudioClip checkpointClip;

    private Animator animator;
    public AudioSource audioSource;

    private int lives = 3;
    private int fish = 0;
    private int friends = 0;
	private bool inCannon;
    //private bool canBeDamaged=true;

    private List<Vector3> friendPositions;

    private float collideIcecubeTimer = 0f;

	// Use this for initialization
	void Start () {
        this.animator = this.GetComponentInChildren<Animator>();
        this.friendPositions = new List<Vector3>();
		this.inCannon = false;
        this.LoadSave();
        StartCoroutine(InitUI());
    }

    // Update is called once per frame
    void Update()
    {
        //Timers
        if (this.collideIcecubeTimer > 0)
        {
            this.collideIcecubeTimer -= Time.deltaTime;
        }
    }

    private IEnumerator InitUI()
    {
        yield return new WaitForFixedUpdate();
        menuManager.UpdateFish(fish.ToString());
        menuManager.UpdateFriends(friends.ToString());
        menuManager.UpdateLives(lives.ToString());
    }

    public void LoadSave()
    {
        //Load position from checkpoint
        Vector3 position = this.menuManager.getSaveManager().LoadCheckpoint(Application.loadedLevel);
        if (position != Vector3.zero)
        {
            Debug.Log("Checkpoint loaded successful");
            this.transform.position = position;
        }
        else
        {
            Debug.Log("No Checkpoints found");
        }

        //Load characterdata
        int[] values = this.menuManager.getSaveManager().LoadCharacterdata(Application.loadedLevel);
        if (values != null)
        {
            Debug.Log("Characterdata loaded successfull");
            this.fish = values[0];
            this.lives = values[1];
            this.friends = values[2];
            this.menuManager.UpdateFish(fish.ToString());
            this.menuManager.UpdateLives(lives.ToString());
            this.menuManager.UpdateFriends(friends.ToString());
        }
        else
        {
            Debug.Log("No Characterdata found");
        }

        //Load collected friends
        this.friendPositions = new List<Vector3>();
        Vector3[] positions = this.menuManager.getSaveManager().LoadCollectedFriends(Application.loadedLevel);
        this.SearchAndDestroyFriends(positions);
    }

    //Untested
    private void SearchAndDestroyFriends(Vector3[] positions)
    {
        foreach (Vector3 pos in positions)
        {
            Debug.Log("Position: " + pos);

            Collider[] hitColliders = Physics.OverlapSphere(pos, 1);
            foreach (Collider col in hitColliders)
            {
                if (col.tag == "Friend")
                {
                    Destroy(col.gameObject);
                }
            }
        }
    }

    public int GetLives()
    {
        return lives;
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1f);
        menuManager.ShowGameOverMenu();
    }

    public void Damage()
    {
        //if(canBeDamaged=true)
        //{
        this.animator.SetTrigger("Damage");
        this.audioSource.PlayOneShot(ouchPenguinClips[UnityEngine.Random.Range(0, ouchPenguinClips.Length)]);
        lives--;
        //}
        menuManager.UpdateLives(this.lives.ToString());
        if (lives == 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(1f);
        this.animator.SetTrigger("Dead");
        this.audioSource.PlayOneShot(deadClip);
        StartCoroutine(GameOver());
    }

    private void CollideFish(GameObject fish, int amount)
    {
        // TODO fancy stuff - fish collect
        this.fish += amount;
        audioSource.PlayOneShot(collectFishClip);
        if (this.fish >= this.FishPerLife)
        {
            // TODO fancy stuff - extra live
            this.lives++;
            this.fish -= FishPerLife;
            audioSource.PlayOneShot(extraLifeClip);

            menuManager.UpdateLives(this.lives.ToString());
        }

        menuManager.UpdateFish(this.fish.ToString());
        // TODO fancy destroy?
        Destroy(fish);
    }

    private void CollideFriend(GameObject friend)
    {
        // TODO fancy stuff - friend collect
        this.friends++;
        audioSource.PlayOneShot(extraLifeClip);

        menuManager.UpdateFriends(this.friends.ToString());
        // TODO fancy destroy?
        Friend friendScript = friend.GetComponent<Friend>();
        StartCoroutine(SayYay(friendScript.yayClips[UnityEngine.Random.Range(0, friendScript.yayClips.Length)]));
        Destroy(friend);
    }

    private IEnumerator SayYay(AudioClip clip)
    {
        yield return new WaitForSeconds(0.2f);
        audioSource.PlayOneShot(clip);
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        switch (other.tag)
        {
            case "Icecube":
                this.collideIcecubeTimer = 2f;
                this.audioSource.PlayOneShot(oefClip);
                break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject other = collider.gameObject;
        switch (other.tag)
        {
            case "Fish":
                this.CollideFish(other, 1);
                break;
            case "GoldFish":
                this.CollideFish(other, FishPerLife);
                break;
            case "Friend":
                this.CollideFriend(other);
                break;
            case "Fall":
                this.audioSource.PlayOneShot(deadClip);
                StartCoroutine(GameOver());
                break;
            case "Checkpoint":
				this.audioSource.PlayOneShot(checkpointClip);
				ParticleSystem[] particles = other.GetComponentsInChildren<ParticleSystem> ();
				
				foreach (ParticleSystem p in particles) {
					if (p.tag.Equals("ParticleImpulse")) {
						p.Play();
					}
				}

                this.menuManager.getSaveManager().SaveCheckpoint(Application.loadedLevel, new Vector3(collider.transform.position.x, this.transform.position.y, collider.transform.position.z));

                if (this.friendPositions.Count > 0)
                {
                    this.menuManager.getSaveManager().SaveCollectedFriends(Application.loadedLevel, this.friendPositions.ToArray());
                    this.friendPositions.Clear();
                }
                break;
            case "Snowman":
				StartCoroutine(other.GetComponent<DestroyableObject>().Destroy());
                break;
			case "Barrel":
				StartCoroutine(other.GetComponent<DestroyableObject>().Destroy());
				break;
			case "Cannon":
				if (!this.inCannon)
				{
					Cannon cannon = collider.GetComponent<Cannon>();
					if (cannon.LoadCannonAllowed())
					{
						this.inCannon = true;
						this.transform.parent = cannon.getSpot();
						this.transform.localPosition = Vector3.zero;
						this.transform.localRotation = new Quaternion(0, 0, 0, 0);
						cannon.Load(this.transform);
					}
				}
				break;
			case "CannonGlide":
                CannonGlide cannonGlide = collider.GetComponent<CannonGlide>();
                cannonGlide.Shoot(this.transform);
                break;
        }
    }

    public bool isColliding()
    {
        return (this.collideIcecubeTimer > 0);
    }

	public void DetachCannon()
	{
		this.inCannon = false;
	}
	
	public bool getInCannon()
	{
		return this.inCannon;
	}
}
