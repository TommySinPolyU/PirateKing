using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class TargetSelector : MonoBehaviour
{
	public BattleSystem bS;
	public Character characterClass;
	public Character Attacker;

	void Start(){
		bS = GameObject.Find ("BattlePlace").GetComponent<BattleSystem> ();
		try{
			if (gameObject.name == "Enemy1")
				characterClass = GameObject.Find ("EnemyGroup").gameObject.transform.GetChild(0).GetComponent<Character>();
			else if (gameObject.name == "Enemy2")
				characterClass = GameObject.Find ("EnemyGroup").gameObject.transform.GetChild(1).GetComponent<Character>();
			else if (gameObject.name == "Enemy3")
				characterClass = GameObject.Find ("EnemyGroup").gameObject.transform.GetChild(2).GetComponent<Character>();
			else if (gameObject.name == "Enemy4")
				characterClass = GameObject.Find ("EnemyGroup").gameObject.transform.GetChild(3).GetComponent<Character>();
		}
		catch(System.Exception e){
			Debug.Log ("Can't find enemy from group");
		}
	}

	void FixedUpdate(){

	}

	public void SelectorShow(){
		characterClass.SelectorShow ();
		switch (GameManager.GM.language) {
		case GameManager.Language.English:
			bS.EnemyStatusName.text = characterClass.charName;
			bS.EnemyHPBar.additionalText = "HP: ";
			break;
		case GameManager.Language.TraditionalChinese:
			bS.EnemyStatusName.text = characterClass.charName_CHT;
			bS.EnemyHPBar.additionalText = "生命: ";
			break;
		}
		bS.EnemyHPBar.UpdateBar (characterClass.CurrentHp, characterClass.Maxhp);
		bS.EnemyStatus.SetActive (true);
	}

	public void SelectorNotShow(){
		characterClass.SelectorNotShow ();
		bS.EnemyStatus.SetActive (false);
	}

	public void AttackTarget(){

		characterClass.SelectorShow ();
		for (int i = 0; i < bS.PlayerGroupObject.Length; i++) {
			if (bS.PlayerGroupObject [i].GetComponent<Attack> ().target == characterClass.gameObject) {
				return;
			}
			bS.PlayerGroupObject[i].GetComponent<Attack>().target = characterClass.gameObject;
			bS.PlayerGroupObject[i].GetComponent<Attack>().targetPosition = characterClass.gameObject.transform.position;
		}
		Debug.Log (characterClass.gameObject);
		//bS.attackbtn.SetActive (false);
		//Image tempImage = bS.skillbtn.GetComponent<Image> ();
		//var TempColor = tempImage.color;
		//TempColor.a = 0.5f;
		//tempImage.color = TempColor;
		//bS.skillbtn.SetActive (false);
		//bS.itemsbtn.SetActive (false);
		//bS.backbtn.SetActive (false);
		/*
		for (int i = 0; i < bS.EnemyGroupClass.TotalCharacter; i++) {
			if (bS.EnemyGroupClass.EnemyCharacter [i] != null) {
				bS.TargetBtn [i].SetActive (false);
			}
		}
		*/
	}
}

