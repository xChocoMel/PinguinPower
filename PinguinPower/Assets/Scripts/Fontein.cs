using UnityEngine;
using System.Collections;

public class Fontein : MonoBehaviour {

    public float activetime = 1;
    public float delay = 0.2f;

    private int index;
    private float timer;
    private bool active;

	// Use this for initialization
	void Start () {

        this.active = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (this.transform.childCount == 0)
        {
            return;
        }

        this.timer -= Time.deltaTime;

        if (this.timer < 0)
        {
            if (this.active)
            {
                this.transform.GetChild(this.index).GetComponent<ParticleSystem>().Stop();
                //this.transform.GetChild(this.index).GetComponent<ParticleSystem>().Stop();
                this.active = false;
            }
            else
            {
                if (this.timer < -delay)
                {
                    this.index++;
                    if (this.index > this.transform.childCount)
                    {
                        this.index = 0;
                    }
                    this.transform.GetChild(this.index).GetComponent<ParticleSystem>().Play();
                    //this.transform.GetChild(this.index).GetComponent<ParticleSystem>().Simulate(1);
                    this.active = true;
                    this.timer = activetime;
                }
            }
        }
        
	}
}
