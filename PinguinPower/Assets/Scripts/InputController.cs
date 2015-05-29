using UnityEngine;
using System.Collections;
using System;

public class InputController : MonoBehaviour {

    MenuManager menuManager;
    CharacterManager characterManager;

    int fish;
    int lives;
    int friends;

	// Use this for initialization
	void Start () {

        try {
            this.characterManager = GameObject.Find("Penguin").GetComponent<CharacterManager>();
        } catch (Exception ex) {
            this.characterManager = null;
        }
        
        this.menuManager = this.GetComponent<MenuManager>();

        this.fish = 0;
        this.lives = 1;
        this.friends = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (this.characterManager != null) {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //Move forward
                this.characterManager.ForwardMovementUp();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                //Stop
                this.characterManager.ForwardMovementDown();
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                //[Hold]
                //Turn left
                this.characterManager.Turn(TurnDirection.Left);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                //[Hold]
                //Turn right
                this.characterManager.Turn(TurnDirection.Right);
            }

            if (Input.GetKey(KeyCode.Z))
            {
                //Jump
                this.characterManager.Jump();
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                //Kick
                this.characterManager.Kick();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //TODO remove:
                //Switch Movement mode for debugging
                this.characterManager.SwitchMovementMode(MovementMode.Walk);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                //Todo remove:
                //Switch Movement mode for debuggin
                this.characterManager.SwitchMovementMode(MovementMode.Glide);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Open/close the pause menu
            this.menuManager.OpenCloseMenu();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //TODO remove:
            //Debug for game overmenu
            this.menuManager.ShowGameOverMenu();
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            //TODO remove:
            fish++;
            this.menuManager.UpdateFish(fish.ToString());
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            //Todo remove:
            lives++;
            this.menuManager.UpdateLives(lives.ToString());
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Debug.Log("Pressing 0");
            //Todo remove:
            friends++;
            this.menuManager.UpdateFriends(friends.ToString());
        }
	}
}
