using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterManager : MonoBehaviour {
    public int FishPerLife = 50;

    public MenuManager menuManager;

    public AudioClip collectFishClip;
    public AudioClip extraLifeClip;
    public AudioClip ouchPenguinClip;

    private Animator animator;
    public AudioSource audioSource;

    private int lives = 3;
    private int fish = 0;
    private int friends = 0;
	//private bool canBeDamaged=true;

	// Use this for initialization
	void Start () {
        this.animator = this.GetComponentInChildren<Animator>();
        StartCoroutine(InitUI());
	}
	public int GetLives()
	{
		return lives;
	}
    private IEnumerator InitUI()
    {
        yield return new WaitForFixedUpdate();
        menuManager.UpdateFish(fish.ToString());
        menuManager.UpdateFriends(friends.ToString());
        menuManager.UpdateLives(lives.ToString());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;
        switch (other.tag)
        {
            case "Obstacle":
                this.CollideObstacle(other);
                break;
			case "Icicle":
                this.Damage();
                break;
			/*
			case "Seal":
				if(this.GetComponent<CharacterMovement>().IsKicking ()==false)
				{
                	this.Damage();
				}
				break;*/
        }
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
        this.audioSource.PlayOneShot(ouchPenguinClip);
        lives--;
		//}
        menuManager.UpdateLives(this.lives.ToString());
        if (lives == 0) {
			StartCoroutine (Die ());
		}  
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(1f);
        this.animator.SetTrigger("Dead");
        StartCoroutine(GameOver());
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
                StartCoroutine(GameOver());
                break;
            case "Gate":
                int newLevel = Application.loadedLevel + 1;
                if (newLevel > 2)
                {
                    newLevel = 1;
                }
                Application.LoadLevel(newLevel);
                break;
        }
    }

    private void CollideObstacle(GameObject obstacle)
    {
        Destroy(obstacle);
        this.animator.SetTrigger("Damage");
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
            this.fish = 0;
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
        StartCoroutine(SayThankYou(friendScript.thankYouClip));
        Destroy(friend);
    }

    private IEnumerator SayThankYou(AudioClip clip)
    {
        yield return new WaitForSeconds(0.2f);
        audioSource.PlayOneShot(clip);
    }
 
}
