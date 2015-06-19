using UnityEngine;
using System.Collections;

public class Snowman : MonoBehaviour {

	public GameObject model;
	public GameObject[] objectsInside;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator Destroy()
	{	
		this.GetComponentInChildren<ParticleSystem>().Play();
		Destroy(model);
		GameObject prefab = objectsInside [Random.Range(0,3)];
		Instantiate(prefab, this.transform.position, this.transform.rotation);
		yield return new WaitForSeconds(2);
		Destroy(this);
	}
}
