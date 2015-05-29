using UnityEngine;
using System.Collections;

public class InputController : MonoBehaviour {

    public CharacterMovement characterMovement;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            //Move forward
            this.characterMovement.ForwardMovementUp();
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            //Stop
            this.characterMovement.ForwardMovementDown();
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            //[Hold]
            //Turn left
            this.characterMovement.Turn(TurnDirection.Left);
        } else if (Input.GetKey(KeyCode.RightArrow)){
            //[Hold]
            //Turn right
            this.characterMovement.Turn(TurnDirection.Right);
        }

        if (Input.GetKey(KeyCode.Z)) {
            //Jump
            this.characterMovement.Jump();
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            //Kick
            this.characterMovement.Kick();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            //Switch Movement mode
            this.characterMovement.SwitchMovementMode(MovementMode.Walk);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            //Switch Movement mode
            this.characterMovement.SwitchMovementMode(MovementMode.Glide);
        }
	}
}
