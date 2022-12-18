using System;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour{

	public enum StatusType{
		ATKUP = 0,
		ATKDOWN = 1,
		DEFUP = 2,
		DEFDOWN = 3,
		SPEEDUP = 4,
		SPEEDDOWN = 5,
		CRITRATEUP = 6,
		CRITRATEDOWN = 7,
		CRITDAMUP = 8,
		CRITDAMDOWN = 9,
		HPPOSION = 10,
		MPPOSION = 11,
		HPRECOVERY = 12,
		MPRECOVERY = 13
	}


	public static string[] spritePath = { 
		"Image/AtkUp", // ATKUP
		"Image/ATKDOWN", //ATKDOWN
		"Image/DEFUP", //DEFUP
		"Image/DEFDOWN", //DEFDOWN
		"Image/Speed", //SPEEDUP
		"Image/SPEEDDOWN", //SPEEDDOWN
		"Image/CRITRATEUP", //CRITRATEUP
		"Image/CRITRATEDOWN", //CRITRATEDOWN
		"Image/CRITDAMUP", //CRITDAMUP
		"Image/CRITDAMDOWN", //CRITDAMDOWN
		"Image/HPPOSION", //HPPOSION
		"Image/MPPOSION", //MPPOSION
		"Image/HPRECOVERY", //HPRECOVERY
		"Image/MPRECOVERY"}; //MPRECOVERY


    public Image image;
	public float value;
	public int round;
	public StatusType statusType;


}
