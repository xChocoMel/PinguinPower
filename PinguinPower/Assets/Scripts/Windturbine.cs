using UnityEngine;
using System.Collections;

public class Windturbine : MonoBehaviour {

    public float height = 10;

    private ParticleSystem wind;

	// Use this for initialization
	void Start () {

        this.wind = this.transform.GetChild(0).GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {

        //Update Particle System
        this.wind.startSpeed = height;
        this.wind.startSize = height / 2;
	}
}
