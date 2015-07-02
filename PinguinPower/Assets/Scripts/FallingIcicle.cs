using UnityEngine;
using System.Collections;

public class FallingIcicle : MonoBehaviour {

	private GameObject Player;
	public float height = 15;
	public float speed = 0.04f;
	private AudioClip fallingClip;
	private AudioClip shatterClip;
	private bool fall;
	private Vector3 bottom;

	// Use this for initialization
	void Start () {
		getAudioClips ();
		this.Player = GameObject.FindGameObjectWithTag ("Penguin");
		fall = false;
		this.transform.position = new Vector3(this.transform.parent.position.x + 0.75f, this.transform.parent.position.y + height, this.transform.parent.position.z);
		bottom = new Vector3(this.transform.position.x, this.transform.parent.position.y, this.transform.position.z);
	}

	private void getAudioClips() {
		AudioClip[] audio = Resources.LoadAll<AudioClip>("Sounds");
		
		foreach (AudioClip a in audio) {
			if (a.name.Equals("Swoosh")) {
				fallingClip = a;
			} else if (a.name.Equals("Collision")) {
				shatterClip = a;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (fall) {
			if (this.transform.position.y > (this.transform.parent.position.y + 1.5)) {
				this.transform.position = Vector3.Lerp(this.transform.position, bottom, speed);
			} else if (this.transform.position.y <= (this.transform.parent.position.y + 1.5)) {
				this.transform.position = new Vector3(this.transform.position.x, this.transform.parent.position.y + 1.5f, this.transform.position.z);
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
		GetComponent<AudioSource>().PlayOneShot (fallingClip);
	}
	
	private IEnumerator Shatter() {
		fall = false;
		GetComponent<AudioSource>().PlayOneShot(shatterClip);
		GetComponentInParent<icicleScript> ().DestroyThis ();
		yield return new WaitForSeconds(0.1f);
		Destroy (this.gameObject);
	}
}
