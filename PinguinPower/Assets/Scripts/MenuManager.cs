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
    private SceneFader sceneFader;

	// Use this for initialization
	void Start () {

        this.sceneFader = this.gameObject.GetComponent<SceneFader>();

        try
        {
            this.MainMenu = GameObject.Find("Canvas").transform.FindChild("MainMenu").gameObject;
            Transform container = this.MainMenu.transform.GetChild(0).GetChild(1).GetChild(0);
            
			if (!this.saveManager.SaveAvailable())
            {
                container.FindChild("BtnContinueGame").GetComponent<Button>().interactable = false;
            }
        }
        catch (Exception ex)
        {
            this.MainMenu = null;
        }

        try
        {
            this.Hud = GameObject.Find("Canvas").transform.FindChild("HUD").gameObject;
            Transform container = this.Hud.transform.GetChild(0).GetChild(0).GetChild(0);
            this.HudValues = new Text[] { container.GetChild(0).GetChild(1).GetComponent<Text>(), container.GetChild(1).GetChild(1).GetComponent<Text>(), container.GetChild(2).GetChild(1).GetComponent<Text>() };
        }
        catch (Exception ex)
        {
            Debug.Log("No HUD :(");
            this.Hud = null;
            this.HudValues = null;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClickNewGame() {

        if (this.saveManager.SaveAvailable())
        {
            this.YesNoMessage.SetActive(true);
        }
        else
        {
            ClickYes();
        }
    }

	public void ClickYes()
    {
        this.saveManager.DeleteSaves();
        this.Pause(false);
        this.StartScene(1);
    }

	public void ClickNo()
    {
        this.YesNoMessage.SetActive(false);
    }

	public void ClickContinueGame()
    {
        this.Pause(false);
        this.StartScene(1);
    }

	public void ClickRetry()
    {
        this.Pause(false);
        this.StartScene(Application.loadedLevel);
    }

    //Could be used for a restart button (removes checkpoint saves)
    //private void ClickRestart()
    //{
    //    this.saveManager.DeleteSaves();
    //    this.StartScene(Application.loadedLevel);
    //}

	public void ClickQuit() {
        Application.Quit();
    }

	public void ClickQuitToMainMenu() {
        //Save characterdata
        int[] values = new int[] { int.Parse(this.HudValues[0].text), int.Parse(this.HudValues[1].text), int.Parse(this.HudValues[2].text) };
        this.saveManager.SaveCharacterdata(Application.loadedLevel, values);
        this.StartScene(0);
    }

	public void ClickResume() {
        
        this.Pause(false);
    }

	public void Pause(bool pause) {

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

	public void StartScene(int index)
    {
        this.sceneFader.EndScene(index);
    }
}
