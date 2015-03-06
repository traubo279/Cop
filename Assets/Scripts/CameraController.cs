using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public GameObject Ground;

	// Camera zoom parameters
	// Play around to make things feel right
	private const float NORMAL_ZOOM = 50;
	private const float ZOOM_INCREMENT = 20;
	private const float MAX_ZOOM_IN_LEVEL = 1;
	private const float MAX_ZOOM_OUT_LEVEL = 5;
	private float MaxZoomIn;
	private float MaxZoomOut;
	
	// Use this for initialization
	void Start ()
	{
		transform.position = new Vector3 (0, NORMAL_ZOOM, 0);
		
		MaxZoomIn = NORMAL_ZOOM - (MAX_ZOOM_IN_LEVEL * ZOOM_INCREMENT);
		MaxZoomOut = NORMAL_ZOOM + (MAX_ZOOM_OUT_LEVEL * ZOOM_INCREMENT);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void FixedUpdate ()
	{
		float moveVertical = Input.GetAxis ("Vertical");
		if (moveVertical != 0) {

			GroundController GroundController = Ground.GetComponent <GroundController> ();
			float newZposition = Mathf.Clamp (transform.position.z + moveVertical,
			                                  GroundController.GetMaxSouthValue (), 
			                                  GroundController.GetMaxNorthValue ());

			transform.position = new Vector3 (transform.position.x, transform.position.y, newZposition);
		}
		
		float moveZoom = Input.GetAxisRaw ("Mouse ScrollWheel") * 10; // Multiply by 10 to round up scroll value from 0.1 or -0.1 to 1 or -1;
		if (moveZoom != 0) {
			float currentZoom = transform.position.y;
			currentZoom = Mathf.Clamp (currentZoom - (moveZoom * ZOOM_INCREMENT), MaxZoomIn, MaxZoomOut); 
			transform.position = new Vector3 (transform.position.x, currentZoom, transform.position.z);
		}
	}
}
