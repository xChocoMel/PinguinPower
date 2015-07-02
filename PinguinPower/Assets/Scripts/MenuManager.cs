using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MenuManager : MonoBehaviour {

    private Transform penguin;
    private GameObject MainMenu;
    private GameObject PauseMenu;
    private GameObject GameOverMenu;
    private GameObject Hud;
    private GameObject YesNoMessage;
	private GameObject CallibratieMenu;
    private Text[] HudValues;

    private SaveManager saveManager = new SaveManager();
    private SceneFader sceneFader;

	private AudioSource audioSource;
	private AudioClip clickClip;

	private int mainMenu = 0;
	private int callibration = 1;
	private int walkScene = 2;
	//private int glide1 = 3;
	//private int glide2 = 4;
	//private int glide3 = 5;
	//private int glideTutorial = 6;

	// Use this for initialization
	void Start () {
		try 
		{
		GameObject p = GameObject.FindGameObjectWithTag("Penguin");
		this.penguin = p.transform;
		}
		catch (Exception)
		{
			this.penguin = null;
		}

        this.sceneFader = this.gameObject.GetComponent<SceneFader>();
		this.audioSource = this.gameObject.GetComponent<AudioSource> ();

        try
        {
            this.MainMenu = GameObject.Find("Canvas").transform.FindChild("MainMenu").gameObject;
            Transform container = this.MainMenu.transform;
			container.FindChild("BtnNewGame").GetComponent<Button>().onClick.AddListener(() => ClickNewGame());
			container.FindChild("BtnContinueGame").GetComponent<Button>().onClick.AddListener(() => ClickContinueGame());
            
			if (!this.saveManager.SaveAvailable())
            {
                container.FindChild("BtnContinueGame").GetComponent<Button>().interactable = false;
            }

			container.FindChild("BtnQuit").GetComponent<Button>().onClick.AddListener(() => ClickQuit());
        }
        catch (Exception)
        {
            this.MainMenu = null;
        }

		try
		{
			this.YesNoMessage = GameObject.Find("Canvas").transform.FindChild("YesNoMessage").gameObject;
			Transform container = this.YesNoMessage.transform;
			container.FindChild("BtnYes").GetComponent<Button>().onClick.AddListener(() => ClickYes());
			container.FindChild("BtnNo").GetComponent<Button>().onClick.AddListener(() => ClickNo());
		}
		catch (Exception)
		{
			this.YesNoMessage = null;
		}

		try
		{
			this.CallibratieMenu = GameObject.Find("Canvas").transform.FindChild("CallibratieMenu").gameObject;
			Transform container = this.CallibratieMenu.transform;
			container.FindChild("BtnYes").GetComponent<Button>().onClick.AddListener(() => ClickYesCallibratie());
			container.FindChild("BtnNo").GetComponent<Button>().onClick.AddListener(() => ClickNoCallibratie());
		}
		catch (Exception)
		{
			this.CallibratieMenu = null;
		}

		try
		{
			this.PauseMenu = GameObject.Find("Canvas").transform.FindChild("PauseMenu").gameObject;
			Transform container = this.PauseMenu.transform;
			container.FindChild("BtnResume").GetComponent<Button>().onClick.AddListener(() => ClickResume());
			container.FindChild("BtnRetry").GetComponent<Button>().onClick.AddListener(() => ClickRetry());
			container.FindChild("BtnQuitToMainMenu").GetComponent<Button>().onClick.AddListener(() => ClickQuitToMainMenu());
		}
		catch (Exception)
		{
			this.PauseMenu = null;
		}
		
		try
		{
			this.GameOverMenu = GameObject.Find("Canvas").transform.FindChild("GameOverMenu").gameObject;
			Transform container = this.GameOverMenu.transform;;
			container.FindChild("BtnRetry").GetComponent<Button>().onClick.AddListener(() => ClickRetry());
			container.FindChild("BtnQuitToMainMenu").GetComponent<Button>().onClick.AddListener(() => ClickQuitToMainMenu());
		}
		catch (Exception)
		{
			this.GameOverMenu = null;
		}

        try
        {
            this.Hud = GameObject.Find("Canvas").transform.FindChild("HUD").gameObject;
			Transform container = this.Hud.transform;
            this.HudValues = new Text[] { container.GetChild(0).GetChild(1).GetComponent<Text>(), container.GetChild(1).GetChild(1).GetComponent<Text>(), container.GetChild(2).GetChild(1).GetComponent<Text>() };
        }
        catch (Exception)
        {
            Debug.Log("No HUD :(");
            this.Hud = null;
            this.HudValues = null;
        }
	}

	private void getAudioClips() {
		AudioClip[] audio = Resources.LoadAll<AudioClip>("Sounds");

		foreach (AudioClip a in audio) {
			if (a.name.Equals("Button_click")) {
				clickClip = a;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClickNewGame() 
	{
		this.audioSource.PlayOneShot (clickClip);

        if (this.saveManager.SaveAvailable())
        {
            this.YesNoMessage.SetActive(true);
        }
        else
        {
            ClickYesCallibratie();
        }
    }

	public void ClickYes()
    {
		this.audioSource.PlayOneShot (clickClip);
        this.saveManager.DeleteSaves();
		this.ClickYesCallibratie ();
    }

	public void ClickNo()
    {
		this.audioSource.PlayOneShot (clickClip);
        this.YesNoMessage.SetActive(false);
    }

	public void ClickYesCallibratie()
	{
		this.audioSource.PlayOneShot (clickClip);
		this.StartScene(this.callibration);
	}
	
	public void ClickNoCallibratie()
	{
		this.audioSource.PlayOneShot (clickClip);
		this.StartScene (this.walkScene);
	}

	public void ClickContinueGame()
    {
		this.audioSource.PlayOneShot (clickClip);

		if (CallibrationData.callibrated) {
			this.CallibratieMenu.SetActive (true);
		} else {
			this.ClickYesCallibratie();
		}
    }

	public void ClickRetry()
    {
		this.audioSource.PlayOneShot (clickClip);
        this.Pause(false);
        this.StartScene(Application.loadedLevel);
    }

    //Could be used for a restart button (removes checkpoint saves)
    //private void ClickRestart()
    //{
    //    this.saveManager.DeleteSaves();
    //    this.StartScene(Application.loadedLevel);
    //}

	public void ClickQuit()
	{
		this.audioSource.PlayOneShot (clickClip);
        Application.Quit();
    }

	public void ClickQuitToMainMenu()
	{
		this.audioSource.PlayOneShot (clickClip);

        //Save characterdata
		string valueLives = this.HudValues[0].text;
		int valueLive = int.Parse(valueLives.Substring (0, valueLives.Length - 1));

		string valueFriends = this.HudValues[1].text;
		int valueFriend = int.Parse(valueFriends.Substring (0, valueFriends.Length - 1));

		string valueFishes = this.HudValues[2].text;
		int valueFish = int.Parse(valueFishes.Substring (0, valueFishes.Length - 1));

		//int[] values = new int[] { valueLive, valueFriend, valueFish};
        //this.saveManager.SaveCharacterdata(Application.loadedLevel, values);
        this.StartScene(0);
    }

	public void ClickResume() 
	{
		this.audioSource.PlayOneShot (clickClip);
        this.Pause(false);
    }

	public void Pause(bool pause) 
	{
        this.PauseMenu.SetActive(pause);

        if (pause) {
            Time.timeScale = 0.0f;
        } else {
            Time.timeScale = 1;
        }
    }

	public void OpenCloseMenu()	{
        if (this.GameOverMenu != null) {
            if (this.GameOverMenu.activeSelf == true) {
                return;
            }
        }

        if (this.PauseMenu != null) {
            this.Pause(!this.PauseMenu.activeSelf);
        }
    }

	public void ShowGameOverMenu() 	{
        if (this.GameOverMenu != null) {
            if (!this.GameOverMenu.activeSelf) {
                if (this.PauseMenu != null) {
                    if (this.PauseMenu.activeSelf) {

                        this.OpenCloseMenu();

						if (this.Hud != null) {
							this.Hud.SetActive(false);
						}
                    }
                }

                this.saveManager.DeleteSaves();
                this.GameOverMenu.SetActive(true);
            }
        }
    }    

    public void UpdateLives(string value)
    {
        if (this.Hud == null) { return; }
		this.HudValues[0].text = value + "x";
    }

    public void UpdateFriends(string value)
    {
        if (this.Hud == null) { return; }
		this.HudValues[1].text = value + "x";
    }

	public void UpdateFish(string value)
	{
		if (this.Hud == null) { return; }
		this.HudValues[2].text = value + "x";
	}

    public SaveManager getSaveManager()
    {
        return this.saveManager;
    }

	public void StartScene(int index)
    {
        this.sceneFader.EndScene(index);
    }
}
