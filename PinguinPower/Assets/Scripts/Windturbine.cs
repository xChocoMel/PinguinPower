using UnityEngine;
using System.Collections;

public class Windturbine : MonoBehaviour {

    public LeverCode Button;
    public float height = 10;

    private GameObject windturbine;
    private ParticleSystem wind;
    private bool playing;

	// Use this for initialization
	void Start () {

        this.windturbine = this.transform.GetChild(0).gameObject;
        this.wind = this.windturbine.GetComponent<ParticleSystem>();

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
        this.wind.startSpeed = height;
        this.wind.startSize = height / 2;

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
