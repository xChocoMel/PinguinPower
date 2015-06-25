using UnityEngine;
using System.Collections;

public class FallingIcicle : MonoBehaviour {

	public GameObject Player;
	public float height;
	public float speed;
	public AudioClip fallingSound;
	public AudioClip shatterSound;
	private bool fall;
	private Vector3 bottom;

	// Use this for initialization
	void Start () {
		fall = false;

		if (height <= 0) {
			height = 10;
		}

		if (speed <= 0) {
			speed = 0.05f;
		}

		this.transform.position = new Vector3(this.transform.parent.position.x + 0.75f, height, this.transform.parent.position.z);
		bottom = new Vector3(this.transform.position.x, 0, this.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (fall) {
			if (this.transform.position.y > 1.5) {
				this.transform.position = Vector3.Lerp(this.transform.position, bottom, speed);
			} else if (this.transform.position.y <= 1.5) {
				this.transform.position = new Vector3(this.transform.position.x, 1.5f, this.transform.position.z);
				StartCoroutine(Shatter());
			}
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (fall) {
			Player.GetComponent<CharacterManager> ().Damage ();
		}
	}

	public void StartFalling() {
		fall = true;
		GetComponent<AudioSource>().PlayOneShot (fallingSound);
	}
	
	private IEnumerator Shatter() {
		fall = false;
		GetComponent<AudioSource>().PlayOneShot(shatterSound);
		GetComponentInParent<icicleScript> ().DestroyThis ();
		yield return new WaitForSeconds(0.1f);
		Destroy (this.gameObject);
	}
}
