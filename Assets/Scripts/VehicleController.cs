using UnityEngine;
using System.Collections;

public class VehicleController : MonoBehaviour
{
	private const float VIEW_PORT_LIMIT = 50; // Hard coded. Don't know how to calculate this yet

	// The chance for a car to violate speeding limit;
	private const int SPEEDING_CHANCE = 50;
	// Maximum legal speed
	private const float DEFAULT_SPEED = 0.5f; // The value that FEELS right
	private float SPEEDING_DIFF_PERCENT_MAX = 1.7f;
	private float SPEEDING_DIFF_PERCENT_MIN = 1.5f;
	private bool isSpeeding;
	private float CurrentSpeed;

	private GameObject Lane;
	private LaneController LaneController;
	private int LaneDirection;

	private GameController GameController;

	// Use this for initialization
	void Start ()
	{
		GameObject GameControllerObject = GameObject.FindWithTag ("GameController");
		if (GameControllerObject != null) {
			GameController = GameControllerObject.GetComponent <GameController> ();
		}
		if (GameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}

		GetLaneDirection ();
		SetSpeed ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		MoveForward ();
		if (CheckOutOfGround ()) {
			RemoveVehicle ();
		}
	}

	void MoveForward ()
	{
		if (LaneDirection == LaneController.LANE_DIRECTION_NORTH) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + CurrentSpeed);
		} else if (LaneDirection == LaneController.LANE_DIRECTION_SOUTH) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - CurrentSpeed);
		}
	}	

	bool CheckOutOfGround ()
	{
		float zPosition = transform.position.z;
		if (zPosition > transform.parent.GetComponent<Renderer> ().bounds.max.z && LaneDirection == LaneController.LANE_DIRECTION_NORTH) {
			return true;
		} else if (zPosition < transform.parent.GetComponent<Renderer> ().bounds.min.z && LaneDirection == LaneController.LANE_DIRECTION_SOUTH) {
			return true;
		}
		return false;
	}

	void RemoveVehicle ()
	{
		Destroy (gameObject);
	}

	void GetLaneDirection ()
	{
		Lane = transform.parent.gameObject;
		LaneController = Lane.GetComponent <LaneController> ();
		LaneDirection = LaneController.GetLaneDirection ();
	}

	void CheckSpeeding ()
	{
		isSpeeding = Random.Range (0, 100) > 50;
	}

	void SetSpeed ()
	{
		CheckSpeeding ();
		if (isSpeeding) {
			CurrentSpeed = DEFAULT_SPEED * Random.Range (SPEEDING_DIFF_PERCENT_MIN, SPEEDING_DIFF_PERCENT_MAX);
		} else {
			CurrentSpeed = DEFAULT_SPEED;
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Vehicle") {
			GameController.GameOver ();
		}
	}

	void OnMouseDown ()
	{
		if (isSpeeding) {
			GameController.CalculatePoint ();
			RemoveVehicle ();
		}
	}   
}
