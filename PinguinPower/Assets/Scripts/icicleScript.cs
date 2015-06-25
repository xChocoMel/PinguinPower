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
		GetComponentInChildren<ParticleSystem> ().Play();
		Destroy (this.gameObject, 2);
	}
}
