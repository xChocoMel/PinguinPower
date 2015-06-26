using UnityEngine;
using System.Collections;

public class DestroyableObject : MonoBehaviour {

	private GameObject model;
	public GameObject[] objectsInside;
	private GameObject barrelTop;
	private bool collided;

	// Use this for initialization
	void Start () {
		collided = false;
		model = this.transform.GetChild(0).gameObject;

		if (this.tag == "Snowman") {
			barrelTop = null;
		} else if (this.tag == "Barrel") {
			barrelTop = model.transform.GetChild(0).gameObject;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator Destroy()
	{	
		if (model != null && !collided) {
			collided = true;

			if (this.tag == "Snowman") {
				this.GetComponentInChildren<ParticleSystem> ().Play ();
			} else if (this.tag == "Barrel") {
				Destroy (this.barrelTop);
				this.GetComponentInChildren<Animation> ().Play();
			}

			//First object is standard
			int r = Random.Range(0,5);
			GameObject prefab = objectsInside [0];

			//Get random rare object
			if (r == 0) {
				int r2 = Random.Range(1,objectsInside.Length);
				prefab = objectsInside [r2];
			}

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
