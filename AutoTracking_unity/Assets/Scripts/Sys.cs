using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sys : MonoBehaviour {
	public static Sys GetSys;
	public static float TimedeltaTime = 0.0f;
	public ScanDTM SendToScanDTM;
	public AStar GetAIPath;
	public PlayerController PlayerCon;
	public Transform Player;
	public GameObject MonsterPath;
	public Transform PathList;

	void Init () {
		SendToScanDTM.StartScanDTM ();
		Player.gameObject.SetActive (true);
		MonsterPath.SetActive (true);
	}
	
	void Awake () {
		GetSys = this;
		Init ();
	}

	void Update () {
		TimedeltaTime = Time.deltaTime;
	}

	public static Transform GetPathList () {return Sys.GetSys.PathList;}
	
	public static PlayerController GetPlayerData () {return Sys.GetSys.PlayerCon;}
	
	public static Transform GetPlayer () {return Sys.GetSys.Player;}
}
