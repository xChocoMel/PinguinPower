using UnityEngine;
using System.Collections;
using System;

public class InputController : MonoBehaviour {

    private MenuManager menuManager;
    public CharacterMovement characterMovement;

	// Use this for initialization
	void Start () {
        this.menuManager = this.GetComponent<MenuManager>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            //Move forward
            this.characterMovement.MoveForward(1);
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            //Stop
            this.characterMovement.MoveForward(0);
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            //[Hold]
            //Turn left
            this.characterMovement.Turn(TurnDirection.Left, 1);
        } else if (Input.GetKey(KeyCode.RightArrow)){
            //[Hold]
            //Turn right
            this.characterMovement.Turn(TurnDirection.Right, 1);
        }

        if (Input.GetKey(KeyCode.Z)) {
            //Jump
            this.characterMovement.Jump();
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            //Kick
            this.characterMovement.Kick();
		}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuManager.OpenCloseMenu();
        }
	}
}
