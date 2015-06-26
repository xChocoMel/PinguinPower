using UnityEngine;
using System.Collections;
using Leap;

public class LeapCallibration : MonoBehaviour {
    private float secondsToWait = 5f;

    private Controller leapController;
    private Hand hand;

    private int kickCallibratedCount = 0;
    private bool callibrateKick = false;
    private bool canKick = true;

	// Use this for initialization
	void Start () {
        this.leapController = new Controller();
        StartCoroutine(CallibrateMaxRollLeft());
	}
	
	// Update is called once per frame
	void Update () {
        if (!leapController.IsConnected)
        {
            return;
        }

        Frame frame = leapController.Frame();

        // Load Hand
        if (frame.Hands.Count != 1)
        {
            hand = null;
            return;
        }
        hand = frame.Hands[0];

        // Kick callibration
        if (callibrateKick)
        {
            if (hand != null)
            {
                // Kicking
                float palmZ = hand.PalmPosition.z;
                Vector speed = hand.PalmVelocity;

                if (canKick && palmZ < (CallibrationData.GetMaxPalmZ() - (CallibrationData.GetMaxPalmZ() * CallibrationData.GetMinPercentage())))
                {
                    canKick = false;
                    if (speed.z < CallibrationData.maxSpeedZ)
                    {
                        CallibrationData.maxSpeedZ = speed.z;
                    }
                    kickCallibratedCount++;
                }
                if (!canKick && palmZ > (CallibrationData.GetMaxPalmZ() * CallibrationData.GetMinPercentage()))
                {
                    canKick = true;
                }

                if (kickCallibratedCount >= 3)
                {
                    print("Callibratie speed: " + CallibrationData.maxSpeedZ);
                    CallibrationData.callibrated = true;
                    callibrateKick = false;

                    //TODO naar juiste scene
                    Application.LoadLevel(2);
                }
            }
            else
            {
                print("Hand is uit beeld");
            }
        }
	}

    /// <summary>
    /// Callibrate turn left
    /// </summary>
    /// <returns></returns>
    private IEnumerator CallibrateMaxRollLeft()
    {
        if (hand != null)
        {
            print("Draai je pols zover mogelijk naar links");
            yield return new WaitForSeconds(secondsToWait);
            if (hand != null)
            {
                CallibrationData.maxRollLeft = hand.PalmNormal.Roll;
                print("Callibratie roll left: " + CallibrationData.maxRollLeft);
                this.StopCoroutine(CallibrateMaxRollLeft());
                StartCoroutine(CallibrateMaxRollRight());
            }
            else
            {
                print("Hand is uit beeld");
                yield return new WaitForFixedUpdate();
                StartCoroutine(CallibrateMaxRollLeft());
            }
        }
        else
        {
            print("Hand is uit beeld");
            yield return new WaitForFixedUpdate();
            StartCoroutine(CallibrateMaxRollLeft());
        }
    }

    /// <summary>
    /// Callibrate turn right
    /// </summary>
    /// <returns></returns>
    private IEnumerator CallibrateMaxRollRight()
    {
        if (hand != null)
        {
            print("Draai je pols zover mogelijk naar rechts");
            yield return new WaitForSeconds(secondsToWait);
            if (hand != null)
            {
                CallibrationData.maxRollRight = hand.PalmNormal.Roll;
                print("Callibratie roll right: " + CallibrationData.maxRollRight);
                this.StopCoroutine(CallibrateMaxRollRight());
                StartCoroutine(this.CallibrateMaxGrabPlayer());
            }
            else
            {
                print("Hand is uit beeld");
                yield return new WaitForFixedUpdate();
                StartCoroutine(CallibrateMaxRollRight());
            }
        }
        else
        {
            print("Hand is uit beeld");
            yield return new WaitForFixedUpdate();
            StartCoroutine(CallibrateMaxRollRight());
        }
    }

    /// <summary>
    /// Callibrate move forward
    /// </summary>
    /// <returns></returns>
    private IEnumerator CallibrateMaxGrabPlayer()
    {
        if (hand != null)
        {
            print("Maak een vuist");
            yield return new WaitForSeconds(secondsToWait);
            if (hand != null)
            {
                CallibrationData.maxGrabPlayer = hand.GrabStrength;
                print("Callibratie grab: " + CallibrationData.maxGrabPlayer);
                this.StopCoroutine(CallibrateMaxGrabPlayer());
                StartCoroutine(CallibrateMaxPitchPlayer());
            }
            else
            {
                print("Hand is uit beeld");
                yield return new WaitForFixedUpdate();
                StartCoroutine(CallibrateMaxGrabPlayer());
            }
        }
        else
        {
            print("Hand is uit beeld");
            yield return new WaitForFixedUpdate();
            StartCoroutine(CallibrateMaxGrabPlayer());
        }
    }

    /// <summary>
    /// Callibrate jump
    /// </summary>
    /// <returns></returns>
    private IEnumerator CallibrateMaxPitchPlayer()
    {
        if (hand != null)
        {
            print("Draai je hand omhoog");
            yield return new WaitForSeconds(secondsToWait);
            if (hand != null)
            {
                CallibrationData.maxPitchPlayer = hand.Direction.Pitch;
                print("Callibratie pitch: " + CallibrationData.maxPitchPlayer);
                this.StopCoroutine(CallibrateMaxPitchPlayer());
                StartCoroutine(CallibrateMaxSpeedZ());
            }
            else
            {
                print("Hand is uit beeld");
                yield return new WaitForFixedUpdate();
                StartCoroutine(CallibrateMaxPitchPlayer());
            }
        }
        else
        {
            print("Hand is uit beeld");
            yield return new WaitForFixedUpdate();
            StartCoroutine(CallibrateMaxPitchPlayer());
        }
    }

    /// <summary>
    /// Enable kick callibration
    /// </summary>
    /// <returns></returns>
    private IEnumerator CallibrateMaxSpeedZ()
    {
        yield return new WaitForFixedUpdate();
        if (hand != null)
        {
            print("Beweeg je hand 3x zo snel mogelijk naar voren");
            callibrateKick = true;
            this.StopCoroutine(CallibrateMaxSpeedZ());
        }
        else
        {
            print("Hand is uit beeld");
            yield return new WaitForFixedUpdate();
            StartCoroutine(CallibrateMaxSpeedZ());
        }
    }
}
