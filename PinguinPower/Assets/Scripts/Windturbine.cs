using UnityEngine;
using System.Collections;

public class Windturbine : MonoBehaviour {

    public LeverCode Button;
    public float height = 10;
    public float radius = 1;

    private GameObject windturbine;
    private ParticleSystem wind;
    private BoxCollider boxCollider;
    private bool playing;

	// Use this for initialization
	void Start () {

        this.windturbine = this.transform.GetChild(0).gameObject;
        this.wind = this.windturbine.GetComponent<ParticleSystem>();
        this.boxCollider = this.GetComponent<BoxCollider>();

        if (this.Button == null)
        {
            this.playing = false;
        }
        else
        {
            this.playing = true;
        }
	}
	
	// Update is called once per frame
	void Update () {

        //Update Particle System
        //this.wind.gravityModifier = -height / 10;
        this.wind.startSpeed = height;
        this.wind.startSize = radius / 2;
        this.boxCollider.center = new Vector3(0, height / 2, 0);
        this.boxCollider.size = new Vector3(radius * 2, height, radius * 2);

        if (this.Button != null)
        {
            bool enabled = this.Button.LeverEnabled();
            if (enabled && !this.playing)
            {
                this.wind.Play();
                this.playing = true;
            }
            else if (!enabled && this.playing)
            {
                this.wind.Stop();
                this.playing = false;
            }
        }
	}
}
