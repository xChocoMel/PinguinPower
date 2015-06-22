using UnityEngine;
using System.Collections;

public class Friend : MonoBehaviour {
    public AudioClip[] helpClips;
    public AudioClip[] yayClips;
	public Material[] mats;

    private AudioSource audioSource;
    private float delay = 10f;

	// Use this for initialization
	void Start () {
		this.GetComponentInChildren<Renderer> ().material = mats[Random.Range(0,3)];
        this.audioSource = this.GetComponentInChildren<AudioSource>();
        StartCoroutine(SayHelp());
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    private IEnumerator SayHelp()
    {
        yield return new WaitForSeconds(Random.Range(delay / 2, delay));
        audioSource.PlayOneShot(helpClips[Random.Range(0, helpClips.Length)]);
        StartCoroutine(SayHelp());
    }
}
