using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGamePro;

public class Skill : MonoBehaviour {


	public enum PassiveSkillType{
		None,
		Attack,
		Defend,
		Hp,
		Energy,
		Load,
		Speed,
		SkillDamage,
		SkillEnergy,
		SkillCooldown,
		CriticalDamage,
		CriticalRate
	}

	public int neededSP;
	public float addvalue, decreaseValue;
	public string skillName, skillDesc, skillName_CHT, skillDesc_CHT;
	public bool isLearned = false;
	public bool isSkillOn = false;
	public Skill PreviousSkill;
	public SkillList PreviousGroup;
	public PassiveSkillType passiveSkillType, decreaseSkillType;
	public string skillImagePath;
	public Character equippedCharacter;
	public int coolDown, currentCD = 0;

	public SkillList skillAreaGroup;

	public float energyNeed;


	/*
	 * totalATK = 總攻擊加成 / totalDEF = 總防禦加成 / totalHP = 總最大生命加成 / totalAS = 總攻擊速度加成 / totalMS = 總移動速度加成
	 * totalCR = 總暴擊機率加成 / totalCD = 總暴擊傷害加成 / totalHR = 總命中率加成 / totalWL = 總負重加成
	 * totalSD = 總技能傷害加成 / totalDSC = 總技能冷卻時間減少
	 * totalDSE = 總技能能量消耗減少
	 */



	//Function will calling when player learn skill.


	// Constuctor in this Class

	public Skill(int neededSP, string skillName, string skillName_CHT, string skillDesc, string skillDesc_CHT, PassiveSkillType Type = PassiveSkillType.None, float value = 0, PassiveSkillType DecreaseType = PassiveSkillType.None, float decreaseValue = 0,  Skill PreviousSkill = null){
		skillAreaGroup = this.transform.parent.GetComponent<SkillList>();
		this.neededSP = neededSP;
		this.skillName = skillName;
		this.skillDesc = skillDesc;
		this.skillDesc_CHT = skillDesc_CHT;
		this.skillName_CHT = skillName_CHT;
		this.passiveSkillType = Type;
		this.addvalue = value;
		this.PreviousSkill = PreviousSkill;
		this.decreaseSkillType = DecreaseType;
		this.decreaseValue = decreaseValue;
	}

	public void SetSkill(int neededSP, string skillName, string skillName_CHT, string skillDesc, string skillDesc_CHT, string SkillImagePath, PassiveSkillType Type = PassiveSkillType.None, float value = 0, SkillList PreviousGroup = null, PassiveSkillType DecreaseType = PassiveSkillType.None, float decreaseValue = 0,  Skill PreviousSkill = null, Character EquippedCaracter = null){
		skillAreaGroup = this.transform.parent.GetComponent<SkillList>();
		name = skillName;
		this.PreviousSkill = PreviousSkill;
		this.neededSP = neededSP;
		this.skillName = skillName;
		this.skillDesc = skillDesc;
		this.skillDesc_CHT = skillDesc_CHT;
		this.skillName_CHT = skillName_CHT;
		this.passiveSkillType = Type;
		this.addvalue = value;
		this.skillImagePath = SkillImagePath;
		this.PreviousGroup = PreviousGroup;
		this.decreaseSkillType = DecreaseType;
		this.decreaseValue = decreaseValue;
		this.equippedCharacter = EquippedCaracter;
		if (GameManager.GM.isLoadGame) {
			isLearned = SaveGame.Load<bool> ("SkillTree/" + name, isLearned);
			//return;
		}
		if(!SaveGame.Exists("SkillTree/" + name))
			SaveGame.Save<bool> ("SkillTree/" + name, isLearned);
		Debug.Log (SkillImagePath);
	}

	public void SetActiveSkill(int neededSP, string skillName, string skillName_CHT, string skillDesc, string skillDesc_CHT, float EnergyNeed, int CoolDown, string SkillImagePath, PassiveSkillType Type = PassiveSkillType.None, float value = 0, SkillList PreviousGroup = null, PassiveSkillType DecreaseType = PassiveSkillType.None, float decreaseValue = 0,  Skill PreviousSkill = null, Character EquippedCaracter = null){
		skillAreaGroup = this.transform.parent.GetComponent<SkillList>();
		this.neededSP = neededSP;
		this.skillName = skillName;
		this.PreviousGroup = PreviousGroup;
		this.skillDesc = skillDesc;
		this.skillDesc_CHT = skillDesc_CHT;
		this.skillName_CHT = skillName_CHT;
		this.passiveSkillType = Type;
		this.addvalue = value;
		this.energyNeed = EnergyNeed;
		this.skillImagePath = SkillImagePath;
		this.PreviousSkill = PreviousSkill;
		this.decreaseSkillType = DecreaseType;
		this.decreaseValue = decreaseValue;
		this.equippedCharacter = EquippedCaracter;
		this.coolDown = CoolDown;
		name = skillName;
		if (GameManager.GM.isLoadGame) {
			isLearned = SaveGame.Load<bool> ("SkillTree/" + name, isLearned);
			//return;
		}
		if(!SaveGame.Exists("SkillTree/" + name))
			SaveGame.Save<bool> ("SkillTree/" + name, isLearned);
		Debug.Log (SkillImagePath);
	}

	public void UpdateDesc(string skillName, string skillName_CHT, string skillDesc, string skillDesc_CHT){
		this.skillName = skillName;
		this.skillDesc = skillDesc;
		this.skillDesc_CHT = skillDesc_CHT;
		this.skillName_CHT = skillName_CHT;
	}

	public void UpdateValue(float value){
		this.addvalue = value;
	}

	public void UpdateEquippedChar(Character character){
		this.equippedCharacter = character;
	}

	public bool isHasPreSkill(){
		if (PreviousSkill != null)
			return true;
		else
			return false;
	}

	public bool isHasPreGroup(){
		if (PreviousGroup != null)
			return true;
		else
			return false;
	}

	public void startCD(){
		currentCD = coolDown;
		isSkillOn = false;
	}

	public void coolDownCourt(){
		if (currentCD != 0) {
			currentCD--;
		} else {
			currentCD = coolDown;
		}
	}

	public void ChangeLearned(bool value){
		isLearned = value;
		SaveGame.Save<bool> ("SkillTree/" + name, isLearned);
	}

	// Getter Setting for Variables.

	// Getter Setting End //
}
