using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementSystem : MonoBehaviour {

	private string achiName, description, reward;
	private int id ,currentPoints = 0, rewardPoints;
	private bool isFinished;
	private AchievementSystem[] aS;

	public AchievementSystem(int id, string name, string description, string reward, int rewardPoints, bool isFinished){
		this.id = id;
		this.name = name;
		this.description = description;
		this.reward = reward;
		this.rewardPoints = rewardPoints;
		this.isFinished = isFinished;
	}

	public int getId(){
		return id;
	}

	public string getName(){
		return achiName;
	}

	public string getDesc(){
		return description;
	}

	public string getReward(){
		return reward;
	}

	public int getRewardPoints(){
		return rewardPoints;
	}

	public bool getisFinished(){
		return isFinished;
	}

	void Start(){
		aS = new AchievementSystem[100]; // Maximum of Achievement.
		//Set information of Each Achievement.
		aS [0] = new AchievementSystem (0, "Test", "Test", "Test", 1, false);
		// Setting End // 
	}

	public void finishAchi(int id){
		aS [id].isFinished = true;
		currentPoints += aS[id].getRewardPoints();
	}

	public void loadingInformation(int id){

	}

}
