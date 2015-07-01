using UnityEngine;
using System.Collections;

public class WeatherManager : MonoBehaviour {

    public bool AutoChange;
    public float MinimumDelay = 10;
    public float MaximumDelay = 60;

    private float autoTimer;
    private int currentWeather;

    private Transform weatherTransform;
    private ParticleSystem rain;
    private ParticleSystem snow;
    private Transform camTransform;

	// Use this for initialization
	void Start () {

        this.weatherTransform = this.transform.GetChild(2);
        this.rain = this.weatherTransform.GetChild(0).GetComponent<ParticleSystem>();
        this.snow = this.weatherTransform.GetChild(1).GetComponent<ParticleSystem>();
        this.camTransform = this.transform.GetChild(1);

        this.autoTimer = 0;
        
        this.ClearWeather();
	}
	
	// Update is called once per frame
	void Update () {

        Debug.Log("Rotating to: " + this.camTransform.rotation.y);
        this.weatherTransform.rotation = new Quaternion(0, this.camTransform.rotation.y, 0, this.weatherTransform.rotation.w);

        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            this.Rain();
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            this.Snow();
        }
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            this.ClearWeather();
        }

        this.autoTimer -= Time.deltaTime;

        if (this.AutoChange && this.autoTimer < 0)
        {
            int r = Random.Range(1, 2);
            this.currentWeather += r;

            if (this.currentWeather == 3)
            {
                this.currentWeather = 0;
            }
            else if (this.currentWeather == 4)
            {
                this.currentWeather = 1;
            }

            this.autoTimer = MinimumDelay;

            switch (this.currentWeather)
            {
                case 0: this.ClearWeather(); break;
                case 1: this.Rain(); break;
                case 2: this.Snow(); break;
            }

            if (this.MinimumDelay < 0)
            {
                this.MinimumDelay = 0;
            }
            
            if (this.MaximumDelay < this.MinimumDelay)
            {
                this.MaximumDelay = this.MinimumDelay;
                this.autoTimer = this.MinimumDelay;
            }
            else
            {
                this.autoTimer = Random.Range(this.MinimumDelay, this.MaximumDelay);
            }
        }
	}

    public void ClearWeather()
    {
        this.currentWeather = 0;
        this.rain.Stop();
        this.snow.Stop();
    }

    public void Rain()
    {
        this.currentWeather = 1;
        this.rain.Play();
        this.snow.Stop();
    }

    public void Snow()
    {
        this.currentWeather = 2;
        this.rain.Stop();
        this.snow.Play();
    }
}
