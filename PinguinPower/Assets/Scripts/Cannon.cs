using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

    public float cannonforce = 1000;
    public float loadtimer = 2;

    public LeverCode Button;

    private bool activated;

    private bool loaded;
    private float timer;
    private Transform penguin;
    private Transform Spot;
    private GameObject ParticleSystem;

	// Use this for initialization
	void Start () {

        this.Spot = this.transform.GetChild(1).GetChild(0);
        this.ParticleSystem = this.transform.GetChild(1).GetChild(1).gameObject;
        this.ParticleSystem.gameObject.SetActive(false);
        this.activated = false;

        this.loaded = false;
        this.timer = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (this.Button != null)
        {
            if (this.Button.LeverEnabled())
            {
                this.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
            }
            else
            {
                this.transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            }
        }

        if (this.loaded)
        {
            this.timer -= Time.deltaTime;

            if (this.timer < 0)
            {
                this.Shoot();
            }
        }
        else if (this.ParticleSystem.activeSelf)
        {
            this.timer -= Time.deltaTime;

            if (this.timer < -0.2f)
            {
                this.ParticleSystem.SetActive(false);
            }
        }
	}

    public void Load(Transform penguin)
    {
        Debug.Log("Cannon is loaded...");
        this.penguin = penguin;
        this.penguin.GetComponent<Rigidbody>().useGravity = false;
        this.penguin.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.penguin.GetComponent<CharacterMovement>().MoveForward(0f);
        this.penguin.GetComponent<CharacterManager>().setInCannon();
        this.timer = this.loadtimer;
        this.loaded = true;
    }

    private void Shoot()
    {
        Debug.Log("Cannon fire!");
        this.loaded = false;

        this.Spot.DetachChildren();
        this.penguin.GetComponent<Rigidbody>().useGravity = true;
        this.penguin.GetComponent<CharacterManager>().DetachCannon();
        this.penguin.GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * this.cannonforce);
        var vector = Vector3.zero;
        vector.y = this.penguin.rotation.y;
        this.penguin.transform.rotation = Quaternion.Euler(vector);
        this.ParticleSystem.gameObject.SetActive(true);
    }

    public Transform getSpot()
    {
        return this.Spot;
    }

    public bool LoadCannonAllowed()
    {
        if (this.Button != null)
        {
            if (!this.Button.LeverEnabled())
            {
                return false;
            }
        }
        return true;
    }
}
