using UnityEngine;
using System.Collections;

public class LaneController : MonoBehaviour
{
	public GameObject Vehicle;
	private const int START_WAIT = 2;
	private const int CAR_SPAWN_INTERVAL = 3;
	// To prevent car from spawning at fix interval;
	private const int CAR_SPAWN_INTERVAL_DIFF_MAX = 2;
	private const int CAR_SPAWN_INTERVAL_DIFF_MIN = -2;
	// X DIFF is to make cars in the same lane not exactly behind the previous one
	// But a bit to the right or the left
	// Make things a bit more chaotic
	private const int CAR_SPAWN_X_DIFF_MIN = -3;
	private const int CAR_SPAWN_X_DIFF_MAX = 3;

	// Lane direction. Do not change this
	public const int LANE_DIRECTION_NORTH = 0;
	public const int LANE_DIRECTION_SOUTH = 1;
	
	// Hard code in sence view at the moment.
	//Need to find a way to initialize this and lane properly using code
	public int CurrentLaneDirection;

	private bool IsRunning;

	// Use this for initialization
	void Start ()
	{
		IsRunning = false;
	}
	
	public int GetLaneDirection ()
	{
		return CurrentLaneDirection;
	}

	IEnumerator SpawnCars ()
	{
		yield return new WaitForSeconds (START_WAIT + Random.Range (CAR_SPAWN_INTERVAL_DIFF_MIN, CAR_SPAWN_INTERVAL_DIFF_MAX));
		while (IsRunning) {
			float zStartValue = 0.0f;
			Bounds bounds = GetComponent<Renderer> ().bounds;
			if (CurrentLaneDirection == LANE_DIRECTION_NORTH) {
				zStartValue = bounds.min.z;

			} else if (CurrentLaneDirection == LANE_DIRECTION_SOUTH) {
				zStartValue = bounds.max.z;
			}
			float xStartValue = transform.position.x + Random.Range (CAR_SPAWN_X_DIFF_MIN, CAR_SPAWN_X_DIFF_MAX);
			Vector3 spawnPosition = new Vector3 (xStartValue, 1, zStartValue);
			GameObject aVehicle = (GameObject)Instantiate (Vehicle, spawnPosition, Quaternion.Euler (0, 0, 0));
			aVehicle.transform.parent = gameObject.transform;
			yield return new WaitForSeconds (CAR_SPAWN_INTERVAL + Random.Range (CAR_SPAWN_INTERVAL_DIFF_MIN, CAR_SPAWN_INTERVAL_DIFF_MAX));
		}
	}

	public void StartSpawnCars ()
	{
		StartCoroutine (SpawnCars ());
	}

	public void RemoveAllCars ()
	{
		for (int i =0; i < transform.childCount; i++) {
			Destroy (transform.GetChild (i).gameObject);
		}
	}

	public void SetIsRunning (bool isRunning)
	{
		IsRunning = isRunning;
	}
}