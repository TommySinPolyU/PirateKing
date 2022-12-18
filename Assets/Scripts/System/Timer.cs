using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour{
	public float timerTime = 0;
	public bool isStarted = false;

	void Start(){
		Debug.Log ("TimerCreated");
	}

	public void InitializeStart(){
		if (isStarted) {
			return;
		}
		InvokeRepeating ("timerStart", 0.1f, 0.1f);
		isStarted = true;
	}

	void timerStart(){
		timerTime += 0.1f;

	}

	public void CancleTimer(){
		CancelInvoke ();
		timerTime = 0f;
		isStarted = false;
	}

	public bool CheckTime(float TimerLimit){
		if (timerTime >= TimerLimit) {
			//Debug.Log ("TimeLeft: " + timerTime);
			return true;
		}
		else
			return false;
	}

	public float ReturnTime(){
		return timerTime;	
	}
}