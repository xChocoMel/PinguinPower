using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

    CharacterManager characterManager;

	// Use this for initialization
	void Start () {
        this.characterManager = GameObject.Find("Penguin").GetComponent<CharacterManager>();
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            //Move forward
            this.characterManager.ForwardMovementUp();
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            //Stop
            this.characterManager.ForwardMovementDown();
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            //[Hold]
            //Turn left
            this.characterManager.Turn(TurnDirection.Left);
        } else if (Input.GetKey(KeyCode.RightArrow)){
            //[Hold]
            //Turn right
            this.characterManager.Turn(TurnDirection.Right);
        }

        if (Input.GetKey(KeyCode.Z)) {
            //Jump
            this.characterManager.Jump();
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            //Kick
            this.characterManager.Kick();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Switch Movement mode
            this.characterManager.SwitchMovementMode(MovementMode.Walk);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Switch Movement mode
            this.characterManager.SwitchMovementMode(MovementMode.Glide);
        }
	}
}
