using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MenuManager : MonoBehaviour {

    public Transform penguin;

    private GameObject MainMenu;
    private GameObject PauseMenu;
    private GameObject GameOverMenu;
    private GameObject Hud;
    private GameObject YesNoMessage;
    private Text[] HudValues;

    private SaveManager saveManager = new SaveManager();

	// Use this for initialization
	void Start () {

        //this.saveManager = new SaveManager();

        try
        {
            this.MainMenu = GameObject.Find("Canvas").transform.FindChild("MainMenu").gameObject;
            Transform container = this.MainMenu.transform.GetChild(0).GetChild(1).GetChild(0);
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
            Transform container = this.YesNoMessage.transform.GetChild(0).GetChild(1).GetChild(0);
            container.FindChild("BtnYes").GetComponent<Button>().onClick.AddListener(() => ClickYes());
            container.FindChild("BtnNo").GetComponent<Button>().onClick.AddListener(() => ClickNo());
        }
        catch (Exception)
        {
            this.MainMenu = null;
        }

        try
        {
            this.PauseMenu = GameObject.Find("Canvas").transform.FindChild("PauseMenu").gameObject;
            Transform container = this.PauseMenu.transform.GetChild(0).GetChild(1).GetChild(0);
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
            Transform container = this.GameOverMenu.transform.GetChild(0).GetChild(1).GetChild(0);
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
            Transform container = this.Hud.transform.GetChild(0).GetChild(0).GetChild(0);
            this.HudValues = new Text[] { container.GetChild(0).GetChild(1).GetComponent<Text>(), container.GetChild(1).GetChild(1).GetComponent<Text>(), container.GetChild(2).GetChild(1).GetComponent<Text>() };
        }
        catch (Exception)
        {
            Debug.Log("No HUD :(");
            this.Hud = null;
            this.HudValues = null;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void ClickNewGame() {

        if (this.saveManager.SaveAvailable())
        {
            this.YesNoMessage.SetActive(true);
        }
        else
        {
            ClickYes();
        }
    }

    private void ClickYes()
    {
        this.saveManager.DeleteSaves();
        this.Pause(false);
        Application.LoadLevel(1);
    }

    private void ClickNo()
    {
        this.YesNoMessage.SetActive(false);
    }

    private void ClickContinueGame()
    {
        this.Pause(false);
        Application.LoadLevel(1);
    }

    private void ClickRetry()
    {
        this.Pause(false);
        Vector3 checkpoint = this.saveManager.LoadCheckpoint(Application.loadedLevel);
        if (checkpoint == Vector3.zero)
        {
            Application.LoadLevel(Application.loadedLevel);
        }
        else
        {
            this.penguin.GetComponent<CharacterMovement>().ForwardMovementDown();
            this.penguin.position = checkpoint;
        }
    }

    //Could be used for a restart button (removes checkpoint saves)
    //private void ClickRestart()
    //{
    //    this.saveManager.DeleteSaves();
    //    Application.LoadLevel(Application.loadedLevel);
    //}

    private void ClickQuit() {
        Application.Quit();
    }

    private void ClickQuitToMainMenu() {
        //Save characterdata
        int[] values = new int[] { int.Parse(this.HudValues[0].text), int.Parse(this.HudValues[1].text), int.Parse(this.HudValues[2].text) };
        this.saveManager.SaveCharacterdata(Application.loadedLevel, values);
        Application.LoadLevel(0);
    }

    private void ClickResume() {
        
        this.Pause(false);
    }

    private void Pause(bool pause) {

        this.PauseMenu.SetActive(pause);

        if (pause) {
            Time.timeScale = 0.0f;
        } else {
            Time.timeScale = 1;
        }
    }

    public void OpenCloseMenu() {

        if (this.GameOverMenu != null) {
            if (this.GameOverMenu.activeSelf == true) {
                return;
            }
        }

        if (this.PauseMenu != null) {
            this.Pause(!this.PauseMenu.activeSelf);
        }
    }

    public void ShowGameOverMenu() {
        if (this.GameOverMenu != null) {
            if (!this.GameOverMenu.activeSelf) {
                if (this.PauseMenu != null) {
                    if (this.PauseMenu.activeSelf) {
                        this.OpenCloseMenu();
                    }
                }

                this.saveManager.DeleteSaves();
                this.GameOverMenu.SetActive(true);
            }
        }
    }

    public void UpdateFish(string value)
    {
        if (this.Hud == null) { return; }
        this.HudValues[0].text = value;
    }

    public void UpdateLives(string value)
    {
        if (this.Hud == null) { return; }
        this.HudValues[1].text = value;
    }

    public void UpdateFriends(string value)
    {
        if (this.Hud == null) { return; }
        this.HudValues[2].text = value;
    }

    public SaveManager getSaveManager()
    {
        return this.saveManager;
    }
}
