using UnityEngine;
using System.Collections;
using Leap;

public class LeapInputController : MonoBehaviour
{
    private float maxRollLeft = 1.2f;
    private float maxRollRight = -2.2f;

    private float maxGrabPlayer = 1f;

    private float maxPitchPlayer = 1f;
    private bool canJump = true;

    private float maxPalmZ = -70f;
    private float maxSpeedZ = -220f;
    private bool canKick = true;

    private float minPercentage = 0.25f;

    public CharacterMovement player;

    private Controller leapController;

    void Start()
    {
        this.leapController = new Controller();

        print(CallibrationData.toString());

        if (CallibrationData.callibrated)
        {
            this.maxRollLeft = CallibrationData.maxRollLeft;
            this.maxRollRight = CallibrationData.maxRollRight;
            this.maxGrabPlayer = CallibrationData.maxGrabPlayer;
            this.maxPitchPlayer = CallibrationData.maxPitchPlayer;
            this.maxPalmZ = CallibrationData.GetMaxPalmZ();
            this.maxSpeedZ = CallibrationData.maxSpeedZ;
            this.minPercentage = CallibrationData.GetMinPercentage();
        }
    }

    void Update()
    {
        if (!leapController.IsConnected)
        {
            return;
        }

        Frame frame = leapController.Frame();

        // Load Hand
        if (frame.Hands.Count != 1)
        {
            player.MoveForward(0f);
            player.Turn(TurnDirection.Stop, 0);
            return;
        }
        Hand hand = frame.Hands[0];

        // Turning
        float roll = hand.PalmNormal.Roll;
        float rollPercentage = 0;

        if (roll > 0)
        {
            // Left
            rollPercentage = Mathf.Abs(1 * roll / maxRollLeft);
            if (rollPercentage >= minPercentage)
            {
                player.Turn(TurnDirection.Left, rollPercentage);
            }
        }
        else if (roll < 0)
        {
            // Right
            rollPercentage = Mathf.Abs(1 * roll / maxRollRight);
            if (rollPercentage >= minPercentage)
            {
                player.Turn(TurnDirection.Right, rollPercentage);
            }
        }
        if (rollPercentage < minPercentage)
        {
            player.Turn(TurnDirection.Stop, 0);
        }

        // Moving forward
        float grab = hand.GrabStrength;
        float grabPercentage = 1 * grab / maxGrabPlayer;

        if (grabPercentage >= minPercentage)
        {
            player.MoveForward(grabPercentage);
        }
        else
        {
            player.MoveForward(0f);
        }

        // Jumping
        float pitch = hand.Direction.Pitch;
        if (canJump && pitch > (maxPitchPlayer - (maxPitchPlayer * minPercentage)))
        {
            canJump = false;
            player.Jump();
        }
        if (!canJump && pitch < (maxPitchPlayer * minPercentage))
        {
            canJump = true;
        }

        // Kicking
        float palmZ = hand.PalmPosition.z;
        Vector speed = hand.PalmVelocity;

        if (canKick && palmZ < (maxPalmZ - (maxPalmZ * minPercentage)) && speed.z < (maxSpeedZ - (maxSpeedZ * minPercentage)))
        {
            canKick = false;
            player.Kick();
        }
        if (!canKick && palmZ > (maxPalmZ * minPercentage))
        {
            canKick = true;
        }
    }
}

