using UnityEngine;
using System.Collections;

public class icicleScript : MonoBehaviour {
	
	private FallingIcicle script;
	 
	// Use this for initialization
	void Start () {
		script = GetComponentInChildren<FallingIcicle> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
		if (script != null) {
			script.StartFalling ();
		}
	}

	public void DestroyThis() {
		ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem> ();

		foreach (ParticleSystem p in particles) {
			if (p.tag.Equals("ParticleImpulse")) {
				p.Play();
			} else if (p.tag.Equals("ParticleConstant")) {
				p.Stop();
			}
		}

		Destroy (this.gameObject, 2);
	}
}
