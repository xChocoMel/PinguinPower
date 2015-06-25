using UnityEngine;
using System.Collections;
using Leap;

public class LeapInputController : MonoBehaviour
{
    public bool useLeap = false;

    public float maxRollLeft = 1.2f;
    public float maxRollRight = -2.2f;

    public float maxGrabPlayer = 1f;

    public float maxPitchPlayer = 1f;
    private bool canJump = true;

    public float maxPalmZ = -70f;
    private bool canKick = true;

    private float minPercentage = 0.25f;

    public CharacterMovement player;

    private Controller leapController;

    void Start()
    {
        this.leapController = new Controller();
    }

    void Update()
    {
        if (useLeap)
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
            //print(roll);
            //print(rollPercentage);
            if (rollPercentage < minPercentage)
            {
                player.Turn(TurnDirection.Stop, 0);
            }

            // Moving forward
            float grab = hand.GrabStrength;
            float grabPercentage = 1 * grab / maxGrabPlayer;
            //print(grabPercentage);

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
            //print(pitch);
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
            // TODO kijken naar speed waarin dit gebeurt!
            float palmZ = hand.PalmPosition.z;
            print(palmZ);
            if (canKick && palmZ < (maxPalmZ - (maxPalmZ * minPercentage)))
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


    //private Controller leapController;

    //int motionRange;

    //void Start()
    //{

    //    Debug.Log("Starting Leap Controller");

    //    this.motionRange = 30;

    //    leapController = new Controller();
    //    //leapController.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
    //    //leapController.EnableGesture(Gesture.GestureType.TYPECIRCLE);
    //    //leapController.EnableGesture(Gesture.GestureType.TYPESWIPE);

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //    if (!leapController.IsConnected)
    //    {
    //        return;
    //    }

    //    Frame frame = leapController.Frame();

    //    Hand hand = frame.Hands.Rightmost;

    //    if (hand.PalmPosition.z < -motionRange)
    //    {
    //        //Penguin.Move(MoveDirection.Forward);
    //    }
    //    else if (hand.PalmPosition.z > motionRange)
    //    {
    //        //Penguin.Move(MoveDirection.Backward);
    //    }
    //    else
    //    {
    //        //Penguin.Move(MoveDirection.Base);
    //    }

    //    if (hand.PalmPosition.x > motionRange)
    //    {
    //        //Penguin.MoveSideways(MoveDirection.Right);
    //        Penguin.Turn(TurnDirection.Right);
    //    }
    //    else if (hand.PalmPosition.x < -motionRange)
    //    {
    //        //Penguin.MoveSideways(MoveDirection.Left);
    //        Penguin.Turn(TurnDirection.Left);
    //    }
    //    else
    //    {
    //        Penguin.Turn(TurnDirection.Base);
    //    }

    //    if (hand.PalmPosition.y > motionRange * 5)
    //    {
    //        Penguin.Jump();
    //    }

    //    //Debug.Log("Palm roll: " + hand.PalmNormal.Roll);

    //    if (hand.PalmNormal.Roll < -0.5f)
    //    {
    //        //Penguin.Turn(TurnDirection.Right);
    //    }
    //    else if (hand.PalmNormal.Roll > 0.5f)
    //    {
    //        //Penguin.Turn(TurnDirection.Left);
    //    }
    //    else
    //    {
    //        //Penguin.Turn(TurnDirection.Base);
    //    }

    //    if (hand.GrabStrength > 0.7)
    //    {
    //        Penguin.Move(MoveDirection.Base);

    //        if (hand.PalmPosition.z < -motionRange)
    //        {
    //            Penguin.Kick();
    //        }
    //    }
    //    else
    //    {
    //        if (hand.PalmPosition.z < -motionRange)
    //        {
    //            Penguin.Move(MoveDirection.Forward2);
    //        }
    //        else if (hand.PalmPosition.z < -motionRange / 2)
    //        {
    //            Penguin.Move(MoveDirection.Forward);
    //        }
    //    }

    //    //Hand hand2 = frame.Hands.Leftmost;

    //    //if (hand2.GrabStrength > 0.7)
    //    //{
    //    //    if (hand2.PalmPosition.z < -motionRange)
    //    //    {
    //    //        Penguin.Kick();
    //    //    }
    //    //}

    //    //GestureList gestures = frame.Gestures();

    //    //foreach (Gesture g in gestures)
    //    //{
    //    //    Debug.Log("Gesture: " + g.Type);

    //    //    if (g.Type == Gesture.GestureType.TYPESCREENTAP)
    //    //    {
    //    //        Penguin.Move(MoveDirection.Forward);
    //    //    }
    //    //}

    //}

}

