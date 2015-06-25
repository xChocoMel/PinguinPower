using UnityEngine;
using System.Collections;

public class DestroyableObject : MonoBehaviour {

	public GameObject model;
	public GameObject[] objectsInside;
	public GameObject barrelTop;

	// Use this for initialization
	void Start () {
		if (this.tag == "Snowman") {
			barrelTop = null;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator Destroy()
	{	
		if (model != null) {
			if (this.tag == "Snowman") {
				this.GetComponentInChildren<ParticleSystem> ().Play ();
			} else if (this.tag == "Barrel") {
				Destroy (this.barrelTop);
				this.GetComponentInChildren<Animation> ().Play();
			}

			GameObject prefab = objectsInside [Random.Range (0, 3)];
			Instantiate (prefab, this.transform.position, this.transform.rotation);

			if (this.tag == "Barrel") {
				yield return new WaitForSeconds (2);
			}

			Destroy (this.model);
			yield return new WaitForSeconds (2);
			Destroy (this.gameObject);
		}
	}
}
