using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageInfo : MonoBehaviour{
	public EnemyGroup enemyGroup;
	public bool infoIsShow = false;
	private Vector3 PrevirousMousePosition;
	[Header("UI Object")]
	public Text StageTitle;
	public Text StagePower;
	public Text StageExpReward;
	public Text StageFoodReward;
	public Text StageMoneyReward;
	public Text StageInfoText;
	public GameObject[] EnemyImagePos;
	public GameObject StageScreen;
	public GameObject InfoScreen;
	public GameObject ShipInfoScreen;
	public Text InfoText;
	public GameObject StartBtn;
	public GameObject ReturnBtn;

	public void LoadStageInfo(){
		StageScreen.SetActive (true);
		ShipInfoScreen.SetActive (false);
		GameObject[] currentStageEnemy = GameObject.FindGameObjectsWithTag ("C" + GameManager.GM.Chapter + "_S" + GameManager.GM.Stage + "_Enemy");
		for (int i = 0; i < currentStageEnemy.Length; i++) {
			currentStageEnemy [i].transform.SetParent (GameObject.Find ("EnemyGroupList").transform.Find ("Chapter" + GameManager.GM.Chapter).Find ("Group" + GameManager.GM.Stage).transform);
		}
		enemyGroup = 
			GameObject.Find ("EnemyGroupList").transform.Find ("Chapter" + GameManager.GM.Chapter).Find ("Group" + GameManager.GM.Stage).GetComponent<EnemyGroup>();
		for (int i = 0; i < EnemyImagePos.Length; i++) {
			EnemyImagePos [i].SetActive (false);
		}
		enemyGroup.Reload ();
		switch (GameManager.GM.language) {
		case GameManager.Language.English:
			StageTitle.text = "Stage " + GameManager.GM.Stage;
			StagePower.text = "Enemy Group Power: " + enemyGroup.GroupPower;
			StageExpReward.text = "Exp Reward: " + enemyGroup.ExpReward;
			StageFoodReward.text = "Food Reward: " + enemyGroup.FoodReward;
			StageMoneyReward.text = "Money Reward: " + enemyGroup.MoneyReward;
			StageInfoText.text = "Stage Info";
			StartBtn.transform.Find("Text").GetComponent<Text>().text = "Battle Preparation";
			break;
		case GameManager.Language.TraditionalChinese:
			StageTitle.text = "關卡 " + GameManager.GM.Stage;
			StagePower.text = "敵人戰力： " + enemyGroup.GroupPower;
			StageExpReward.text = "經驗獎勵： " + enemyGroup.ExpReward;
			StageFoodReward.text = "食物獎勵： " + enemyGroup.FoodReward;
			StageMoneyReward.text = "金錢獎勵： " + enemyGroup.MoneyReward;
			StageInfoText.text = "關卡資訊";
			StartBtn.transform.Find("Text").GetComponent<Text>().text = "戰鬥準備";
			break;
		}
		for (int i = 0; i < enemyGroup.EnemyCharacter.Length; i++) {
			if (enemyGroup.EnemyCharacter [i] != null) {
				EnemyImagePos [i].SetActive (true);
			} else
				EnemyImagePos [i].SetActive (false);
		}
	}

	void Update(){
		LoadStageInfo ();
		if(infoIsShow)
			InfoScreen.SetActive (true);
		else if(!infoIsShow)
			InfoScreen.SetActive (false);
	}

	public void ReturnToShip(){
		GameObject[] currentStageEnemy = GameObject.FindGameObjectsWithTag ("C" + GameManager.GM.Chapter + "_S" + GameManager.GM.Stage + "_Enemy");
		for (int i = 0; i < currentStageEnemy.Length; i++) {
			currentStageEnemy [i].transform.SetParent (GameObject.Find ("EnemyGroupList").transform);
		}
		StageScreen.SetActive (false);
		ShipInfoScreen.SetActive (true);
	}

	public void ShowEnemyInfo(int EnemyIndex){
		if (!infoIsShow) {
			infoIsShow = true;
			PrevirousMousePosition = Input.mousePosition;
			InfoScreen.transform.position = new Vector3 (PrevirousMousePosition.x + 250f, PrevirousMousePosition.y, PrevirousMousePosition.z);
			if (enemyGroup.EnemyCharacter [EnemyIndex] != null) {
				Character enemyChar = enemyGroup.EnemyCharacter [EnemyIndex];
				switch (GameManager.GM.language) {
				case GameManager.Language.English:
					InfoText.text = enemyChar.charName + "\nLv: " + enemyChar.Lv + "\n" + "HP: " + enemyChar.CurrentHp + " / " + enemyChar.Maxhp + "\nPower: " + enemyChar.CharacterPower;
					break;
				case GameManager.Language.TraditionalChinese:
					InfoText.text = enemyChar.charName_CHT + "\n等級: " + enemyChar.Lv + "\n" + "生命: " + enemyChar.CurrentHp + " / " + enemyChar.Maxhp + "\n戰力: " + enemyChar.CharacterPower;
					break;
				}
			}
		} else {
			NotShowEnemyInfo ();
		}
	}

	public void NotShowEnemyInfo(){
		infoIsShow = false;
	}
}
