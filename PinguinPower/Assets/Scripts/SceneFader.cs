using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour {

    private Image Fader;
    private GameObject cameraParticles;

    public float fadeSpeed = 1.5f;
    private int fading;
    private int newIndex;

    // Use this for initialization
    void Start()
    {
        this.Fader = GameObject.Find("Canvas").transform.FindChild("Fader").gameObject.GetComponent<Image>();
        this.cameraParticles = GameObject.Find("Camera").transform.GetChild(0).gameObject;
        this.fading = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (fading == 1)
        {
            FadeToClear();
            if (this.Fader.color.a <= 0.05f)
            {
                this.Fader.color = Color.clear;
                this.Fader.enabled = false;
                fading = 0;
                this.cameraParticles.SetActive(false);
            }
        }
        else if (fading == -1)
        {
            //FadeToBlack();
            FadeToWhite();
            if (this.Fader.color.a >= 0.95f)
            {
                Debug.Log("Starting new level");
                Application.LoadLevel(this.newIndex);
            }
        }
    }

    private void FadeToClear()
    {
        this.Fader.color = Color.Lerp(this.Fader.color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    private void FadeToBlack()
    {
        this.Fader.color = Color.Lerp(this.Fader.color, Color.black, fadeSpeed * Time.deltaTime);
    }

    private void FadeToWhite()
    {
        this.Fader.color = Color.Lerp(this.Fader.color, Color.white, fadeSpeed * Time.deltaTime);
    }

    public void EndScene(int index)
    {
        Debug.Log("Fading scene...");
        Time.timeScale = 1;
        this.newIndex = index;
        this.Fader.enabled = true;
        this.fading = -1;
        this.cameraParticles.SetActive(true);
    }
}
