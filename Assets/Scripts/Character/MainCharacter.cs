using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGamePro;

public class MainCharacter : Character {

	//以下為基礎能力變數
	[Header("Main Character Properties")]
	[SerializeField]public SkillTree SkillTree;

	/*
	name = 角色名稱 / role = 角色定位 / hp = 生命 / atk = 攻擊力 / leadership = 領導力
	def = 防禦力 / atkspeed = 攻擊速度 / movspeed = 移動速度 / laad = 負重 / cR = 暴擊率(Critical Rate)
	cD = 暴擊傷害(Critical Damage) / hR = 命中率(Hit Rate) / skillPoint = 角色技能點
	*/
	// Use this for initialization
	void Start () {

		if (GameManager.GM.isLoadGame) {
			SaveGame.LoadInto ("Character/" + name, this);
			SkillTree = GameObject.Find ("SkillTree").GetComponent<SkillTree> ();
			SkillTree.MC = this;
			reloadAttributes (true);
			Debug.Log ("Character/" + name + "Touch Load");
		} else {
			isInitialized = false;
			SkillTree = GameObject.Find ("SkillTree").GetComponent<SkillTree> ();
			SkillTree.MC = this;
			isMainCharacter = true;
			Maxhp = baseHp + SkillTree.saveElements.totalHP;
			MaxEnergy = baseEnergy + SkillTree.saveElements.totalEnergy;
			CurrentEnergy = MaxEnergy;
			CurrentHp = Maxhp;
			charName = GameManager.GM.MainCharacterName;
			charName_CHT = GameManager.GM.MainCharacterName;
			reloadAttributes (true);
		}
	}

	// Update is called once per frame
	void Update () {
		if (this.gameObject.transform.GetSiblingIndex () != 0) {
			this.gameObject.transform.SetAsFirstSibling ();
		}
		if(GameManager.GM.gamestatus == GameManager.GameStatus.Battle)
			SkillTree = GameObject.Find ("SkillTree").GetComponent<SkillTree> ();
	}


}
