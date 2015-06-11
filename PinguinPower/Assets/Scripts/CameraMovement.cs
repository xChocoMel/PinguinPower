using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
    public Transform graphics;
    public Transform lookAt;
    public Rigidbody charRigidbody;
    private Camera cam;

    public float minFieldOfView = 60f;
    public float maxFieldOfView = 90f;
    public float maxSpeed = 10f;

    public float aimZ = -2f;
    public float aimY = 4f;

    public float smooth = 0.02f;

	// Use this for initialization
	void Start () {
        this.cam = this.GetComponent<Camera>();
        transform.position = graphics.position + (graphics.forward * aimZ * 3) + (graphics.up * aimY * 3);
        cam.fieldOfView = minFieldOfView;
	}
	
	// Update is called once per frame
	void Update () {
        //Vector3 aimPos = graphics.position + (graphics.forward * aimZ) + (graphics.up * aimY);
        //Vector3 translation = (aimPos - transform.position) * smooth;
        //transform.Translate(translation, Space.World);
        Vector3 aimPos = graphics.position + (graphics.forward * aimZ) + (graphics.up * aimY);
        transform.position = Vector3.Lerp(transform.position, aimPos, smooth);
        transform.LookAt(lookAt);
        float aimFieldOfView = charRigidbody.velocity.magnitude * (maxFieldOfView) / maxSpeed;
        float deltaFieldOfView = aimFieldOfView - cam.fieldOfView;
        cam.fieldOfView = cam.fieldOfView + (deltaFieldOfView * smooth);
        if (cam.fieldOfView > maxFieldOfView) { cam.fieldOfView = maxFieldOfView; }
        else if (cam.fieldOfView < minFieldOfView) { cam.fieldOfView = minFieldOfView; }
	}
}
