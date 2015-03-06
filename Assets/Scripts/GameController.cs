using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public GUIText GameOverText;
	private string GAME_OVER_TEXT = "You have failed!\nPress Enter to restart!";
	public GUIText RuleText;
	public GUIText StartText;
	public GUIText Points;
	private string POINT_TEXT = "To catch: ";
	private int points;
	public GUIText WinText;
	private string WIN_TEXT = "YOU DID IT!\nPress Enter to restart!";

	// How many cars to catch to win
	private const int WIN_POINT = 2;
	private bool IsGameRunning;
	private LaneController[] LaneControllers;

	// Use this for initialization
	void Start ()
	{
		Init ();
		GetLaneControllers ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (IsGameRunning == false) {
			if (Input.GetKeyDown (KeyCode.Return)) {
				GameStart ();
			}
		}
	}

	private void Init ()
	{
		GameOverText.text = "";
		WinText.text = "";
		RuleText.text = "Click on speeding cars to remove them\nYou will fail if an accident happens.\n" + 
			"Catch " + WIN_POINT + " offenders to win!";
		StartText.text = "Press Enter to start";
		ResetPoint ();
		IsGameRunning = false;
	}

	private void GetLaneControllers ()
	{
		GameObject[] LaneObjects = GameObject.FindGameObjectsWithTag ("Lane");
		LaneControllers = new LaneController [LaneObjects.Length];
		for (int i = 0; i < LaneObjects.Length; i++) {
			LaneControllers [i] = LaneObjects [i].GetComponent <LaneController> ();
			LaneControllers [i].StartSpawnCars ();
		}
	}

	private void GameStart ()
	{
		RuleText.text = "";
		StartText.text = "";
		GameOverText.text = "";
		WinText.text = "";
		IsGameRunning = true;
		Time.timeScale = 1;
		ResetPoint ();
		StartSpawnCarsForAllLanes (IsGameRunning);
	}

	public void GameOver ()
	{
		Time.timeScale = 0;
		GameOverText.text = GAME_OVER_TEXT;
		IsGameRunning = false;
		StartSpawnCarsForAllLanes (IsGameRunning);
	}
	
	void GameWin ()
	{
		Time.timeScale = 0;
		WinText.text = WIN_TEXT;
		IsGameRunning = false;
		StartSpawnCarsForAllLanes (IsGameRunning);
	}

	public void CalculatePoint ()
	{
		points = points + 1;
		UpdatePointText ();
		CheckWinCondition ();
	}

	void UpdatePointText ()
	{
		Points.text = POINT_TEXT + points + " / " + WIN_POINT;
	}

	void CheckWinCondition ()
	{
		if (points == WIN_POINT) {
			GameWin ();
		}
	}

	void ResetPoint ()
	{
		points = 0;
		UpdatePointText ();
	}

	void StartSpawnCarsForAllLanes (bool IsGameRunning)
	{
		foreach (LaneController LaneController in LaneControllers) {
			LaneController.RemoveAllCars ();
			LaneController.SetIsRunning (IsGameRunning);
		}
	}
}
