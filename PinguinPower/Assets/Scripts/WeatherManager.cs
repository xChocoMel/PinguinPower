using UnityEngine;
using System.Collections;

public class WeatherManager : MonoBehaviour {

    private GameObject rain;
    private GameObject snow;
    private Quaternion normalRotation;

	// Use this for initialization
	void Start () {

        this.rain = this.transform.GetChild(1).gameObject;
        this.snow = this.transform.GetChild(2).gameObject;
        this.normalRotation = new Quaternion(0, 0, 0, 0);

        this.ClearWeather();
	}
	
	// Update is called once per frame
	void Update () {

        this.rain.transform.rotation = normalRotation;
        this.snow.transform.rotation = normalRotation;

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
	}

    public void ClearWeather()
    {
        this.rain.SetActive(false);
        this.snow.SetActive(false);
    }

    public void Rain()
    {
        this.rain.SetActive(true);
        this.snow.SetActive(false);
    }

    public void Snow()
    {
        this.rain.SetActive(false);
        this.snow.SetActive(true);
    }
}
