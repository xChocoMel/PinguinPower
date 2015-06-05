using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CharacterManager : MonoBehaviour {
    public int FishPerLife = 50;

    public MenuManager menuManager;

    private int lives = 3;
    private int fish = 0;
    private int friends = 0;

	// Use this for initialization
	void Start () {
        StartCoroutine(InitUI());
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
			case "Seal":
				lives--;
				
				break;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject other = collider.gameObject;
        switch (other.tag)
        {
            case "Fish":
                this.CollideFish(other);
                break;
            case "Friend":
                this.CollideFriend(other);
                break;
        }
    }

    private void CollideObstacle(GameObject obstacle)
    {
        Destroy(obstacle);
    }

    private void CollideFish(GameObject fish)
    {
        // TODO fancy stuff - fish collect
        this.fish++;
        if (this.fish >= this.FishPerLife)
        {
            // TODO fancy stuff - extra live
            this.lives++;
            this.fish = 0;
            
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

        menuManager.UpdateFriends(this.friends.ToString());
        // TODO fancy destroy?
        Destroy(friend);
    }
}
