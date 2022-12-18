using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGamePro;

public class EnemyGroup : MonoBehaviour {
	[Header("Group Properties")]
	public float GroupPower;
	public float GroupMaxPower;
	public float GroupCharacterHP;
	public float GroupCharacterMaxHP;
	public float GroupCharacterHPBeforeStart;
	private bool GroupHPSaved;
	public string groupId;
	[HideInInspector] public int TotalCharacter;
	public float ExpReward, MoneyReward, FoodReward;
	public int TurnLimit;

	[Header("Click Reset After Import the gameobject of character")]
	[SerializeField]public Character[] EnemyCharacter;

	void Reset(){
		groupId = gameObject.name;
		for (int i = 0; i < EnemyCharacter.Length; i++) {
			if(this.gameObject.transform.GetChild (i) != null)
				EnemyCharacter [i] = this.gameObject.transform.GetChild (i).GetComponent<Character>();
		}
	}

	void Start(){
		groupId = gameObject.name;
		TotalCharacter = gameObject.transform.childCount;
		EnemyCharacter = new Character[TotalCharacter];
		Reload ();
	}

	public void Awake(){
		GroupHPSaved = false;
		DontDestroyOnLoad(this.gameObject);
	}

	void Update(){
		if (GameManager.GM.gamestatus == GameManager.GameStatus.Battle) {
			Reload ();
		}
	}

	public void Reload(){
		GroupPower = 0;
		GroupMaxPower = 0;
		GroupCharacterHP = 0;
		GroupCharacterMaxHP = 0;
		TotalCharacter = gameObject.transform.childCount;
		EnemyCharacter = new Character[TotalCharacter];
		for (int i = 0; i < EnemyCharacter.Length; i++) {
			if(this.gameObject.transform.GetChild (i) != null)
				EnemyCharacter [i] = this.gameObject.transform.GetChild (i).GetComponent<Character>();
			if (GameManager.GM.gamestatus != GameManager.GameStatus.Battle) {
				EnemyCharacter [i].reloadAttributes (true);
			} else if (GameManager.GM.gamestatus == GameManager.GameStatus.Battle) {
				EnemyCharacter [i].reloadPower ();
			}
		}
		for (int i = 0; i < EnemyCharacter.Length; i++) {
			if (EnemyCharacter [i] != null) {
				GroupPower += EnemyCharacter [i].CharacterPower;
				GroupMaxPower += EnemyCharacter [i].MaxCharacterPower;
				GroupCharacterHP += EnemyCharacter [i].CurrentHp;
				GroupCharacterMaxHP += EnemyCharacter [i].Maxhp;
			}
		}

		if (!GroupHPSaved) {
			GroupCharacterHPBeforeStart = GroupCharacterHP;
			GroupHPSaved = true;
		}

		Mathf.RoundToInt (GroupPower);
		Mathf.RoundToInt (GroupMaxPower);
		ExpReward = GroupMaxPower / 10;
		ExpReward = Mathf.RoundToInt (ExpReward);
		MoneyReward = GroupMaxPower / 6;
		MoneyReward = Mathf.RoundToInt (MoneyReward);
		FoodReward = GroupMaxPower / 8;
		FoodReward = Mathf.RoundToInt (FoodReward);

	}
}
