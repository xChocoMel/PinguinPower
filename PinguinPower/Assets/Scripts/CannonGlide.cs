using UnityEngine;
using System.Collections;

public class CannonGlide : MonoBehaviour {

    public float cannonforce = 2000;
    private float timer;
    private Transform Spot;
    private GameObject ParticleSystem;

	// Use this for initialization
	void Start () {

        this.Spot = this.transform.GetChild(1);
        this.ParticleSystem = this.transform.GetChild(2).gameObject;
        this.ParticleSystem.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if (this.ParticleSystem.activeSelf)
        {
            this.timer -= Time.deltaTime;

            if (this.timer < 0)
            {
                this.ParticleSystem.SetActive(false);
            }
        }
	}

    public void Shoot(Transform penguin)
    {
        Debug.Log("Cannon fire!");
        this.timer = 0.2f;

        penguin.position = this.Spot.position;
        penguin.rotation = this.Spot.rotation;
        penguin.GetComponent<Rigidbody>().velocity = Vector3.zero;
        penguin.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * this.cannonforce);

        this.ParticleSystem.gameObject.SetActive(true);
    }
}
