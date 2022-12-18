using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGamePro;

public class Character : MonoBehaviour {

	public enum ElementType{
		None,
		Fire,
		Water,
		Wind,
		Earth,
		Light,
		Dark
	}

	public enum Role{
		Attacker,
		Defender,
		Shooter
	}

	public enum Controller{
		Player,
		Computer
	}

	public enum Status{
		Charging,
		Moving,
		Stay
	}

		
	//[HideInInspector]
	public int atk, def, load, leadership;
	//[HideInInspector]
	public float Maxhp, MaxEnergy, CurrentHp, CurrentEnergy, movspeed, cR, cD, hR;
	public int CharacterPower;
	public int MaxCharacterPower;
	[HideInInspector]public Status status = Status.Charging;
	public bool isDead = false;
	public bool isInGroup = false;
	public bool NeedMoveToGroup = false;

	public bool isMainCharacter = false;
	public bool isInitialized = false;

	[Header("Base Character Properties")]
	public string charName;
	public string charName_CHT;
	public Role role;
	public ElementType elementType;
	public Controller controller;
	public int Lv;
	public float Exp;
	public int ExpRequirment;
	public int baseAtk;
	public int baseDef;
	public int baseHp;
	public int baseEnergy;
	public int baseLoad;
	public int baseleadership;
	public float baseMovspeed;
	public float baseCriticalRate;
	public float baseCriticalDamage;
	public float baseHitRate;


	[Header("Grow up: Not Available of MainChar")]
	 public int growHP;
	 public int growATK;
	 public int growDEF;
	 public int growLOAD;

	[HideInInspector] public int totalgrowHP;
	[HideInInspector] public int totalgrowATK;
	[HideInInspector] public int totalgrowDEF;
	[HideInInspector] public int totalgrowLOAD;

	public NormalCharacterSkillList NCSL;

	public bool isAddedExp = false;

	private bool HpCanUpdate = false, isDecreaseHP = false;
	private float HpBeforeChange = 0,HpChangedValue = 0;

	private bool MpCanUpdate = false, isDecreaseMP = false;
	private float MpBeforeChange = 0,MpChangedValue = 0;

	private bool ExpCanUpdate = false;
	private float ExpBeforeChange = 0,ExpChangedValue = 0, AddedExp = 0;

	/*
	name = 角色名稱 / role = 角色定位 / hp = 生命 / atk = 攻擊力
	def = 防禦力 / atkspeed = 攻擊速度 / movspeed = 移動速度 / laad = 負重 / cR = 暴擊率(Critical Rate)
	cD = 暴擊傷害(Critical Damage) / hR = 命中率(Hit Rate) / skillPoint = 角色技能點
	*/

	public void Awake(){
		//DontDestroyOnLoad(this.gameObject);
	}

	// Use this for initialization
	void Start () {
		if (GameManager.GM.isLoadGame) {
			if (!isMainCharacter) {
				SaveGame.LoadInto ("Character/" + charName, this);
				reloadAttributes (true);
			}
		} else {
			Maxhp = baseHp + growHP * (Lv - 1);
			MaxEnergy = baseEnergy;
			CurrentEnergy = MaxEnergy;
			CurrentHp = Maxhp;
			reloadAttributes (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.GM.gamestatus == GameManager.GameStatus.Battle || GameManager.GM.gamestatus == GameManager.GameStatus.CharacterData) {
			if (NCSL == null) {
				NCSL = GameObject.Find ("NormalSkillList").GetComponent<NormalCharacterSkillList> ();
				Debug.Log (GameManager.GM.gamestatus);
				Debug.Log ("Update NCSL");
			}
		} else if(GameManager.GM.gamestatus == GameManager.GameStatus.Ships) {
			if (NCSL != null) {
				NCSL = null;
				reloadAttributes (false);
				Debug.Log (GameManager.GM.gamestatus);
				Debug.Log ("Set NCSL TO NULL");
			}
		}
		if (GameManager.GM.gamestatus == GameManager.GameStatus.Ships) {
			if (!isDead)
				this.gameObject.SetActive (true);
			else if (isDead) {
				this.gameObject.SetActive (false);
			}
		}
	}

	void FixedUpdate(){
		if(Lv > 0 && Lv <= 5)
			ExpRequirment = 0 + Lv * 15;
		else if(Lv > 5 && Lv <= 10)
			ExpRequirment = 15 + Lv * 15;
		else if(Lv > 10 && Lv <= 15)
			ExpRequirment = 30 + Lv * 15;
		else if(Lv > 15 && Lv <= 20)
			ExpRequirment = 45 + Lv * 15;
		else if(Lv > 20)
			ExpRequirment = 60 + Lv * 15;
		
		if (CurrentHp > Maxhp) {
			CurrentHp = Maxhp;
			HpCanUpdate = false;
		}
		if (CurrentEnergy > MaxEnergy) {
			CurrentEnergy = MaxEnergy;
			MpCanUpdate = false;
		}
		if (CurrentHp <= 0)
			isDead = true;
		else if (CurrentHp > 0)
			isDead = false;
		if (HpCanUpdate) {
			//Debug.Log (HpChangedValue)
			
			if (CurrentHp > (HpBeforeChange - HpChangedValue) && isDecreaseHP) {
				CurrentHp -= HpChangedValue / 20;
				CurrentHp = Mathf.Round (CurrentHp * 100f) / 100f;
				if (CurrentHp > (HpBeforeChange - HpChangedValue) + 0.5f && (CurrentHp < (HpBeforeChange - HpChangedValue) + 1.0f)) {
					CurrentHp = Mathf.CeilToInt (CurrentHp);
					CurrentHp = HpBeforeChange - HpChangedValue;
					Debug.Log ("Round");
				}
			} else if ((HpBeforeChange + HpChangedValue) > CurrentHp && !isDecreaseHP) {
				CurrentHp += HpChangedValue / 20;
				CurrentHp = Mathf.Round (CurrentHp * 100f) / 100f;
				if (CurrentHp > (HpBeforeChange - HpChangedValue) + 0.5f && (CurrentHp < (HpBeforeChange - HpChangedValue) + 1.0f)) {
					CurrentHp = Mathf.CeilToInt (CurrentHp);
					Debug.Log ("Round");
				}
			} else {
				HpCanUpdate = false;
				reloadPower ();
			}
		}

		if (MpCanUpdate) {
			//Debug.Log (HpChangedValue)

			if (CurrentEnergy > (MpBeforeChange - MpChangedValue) && isDecreaseMP) {
				CurrentEnergy -= MpChangedValue / 20;
				CurrentEnergy = Mathf.Round (CurrentEnergy * 100f) / 100f;
				if (CurrentEnergy > (MpBeforeChange - MpChangedValue) + 0.5f && (CurrentEnergy < (MpBeforeChange - MpChangedValue) + 1.0f)) {
					CurrentEnergy = Mathf.CeilToInt (CurrentEnergy);
					CurrentEnergy = MpBeforeChange - MpChangedValue;
					Debug.Log ("Round");
				}
			} else if ((MpBeforeChange + MpChangedValue) > CurrentEnergy && !isDecreaseMP) {
				CurrentEnergy += MpChangedValue / 20;
				CurrentEnergy = Mathf.Round (CurrentEnergy * 100f) / 100f;
				if (CurrentEnergy > (MpBeforeChange - MpChangedValue) + 0.5f && (CurrentEnergy < (MpBeforeChange - MpChangedValue) + 1.0f)) {
					CurrentEnergy = Mathf.CeilToInt (CurrentEnergy);
					Debug.Log ("Round");
				}
			} else {
				MpCanUpdate = false;
				reloadPower ();
			}
		}

		if (ExpCanUpdate) {
			if(Lv > 0 && Lv <= 5)
				ExpRequirment = 0 + Lv * 15;
			else if(Lv > 5 && Lv <= 10)
				ExpRequirment = 15 + Lv * 15;
			else if(Lv > 10 && Lv <= 15)
				ExpRequirment = 30 + Lv * 15;
			else if(Lv > 15 && Lv <= 20)
				ExpRequirment = 45 + Lv * 15;
			else if(Lv > 20)
				ExpRequirment = 60 + Lv * 15;
			
			if (Exp < (ExpBeforeChange + ExpChangedValue)) {
				Exp += ExpChangedValue / 40;
				Exp = Mathf.CeilToInt (Exp * 100f) / 100f;
				AddedExp += ExpChangedValue / 40;
				AddedExp = Mathf.CeilToInt (AddedExp * 100f) / 100f;
				Debug.Log ("Exp Before: " + ExpBeforeChange);
				Debug.Log ("Exp ChangeValue: " + ExpChangedValue);
				Debug.Log ("AddedExp: " + AddedExp);
				Debug.Log (Exp);
			} else if (Exp >= (ExpBeforeChange + ExpChangedValue)) {
				ExpCanUpdate = false;
				AddedExp = 0;
			}

			if (Exp >= ExpRequirment) {
				//Debug.Log (gameObject + " ,Exp = " + Exp + " ,Exp Requirement = " + ExpRequirment);
				Lv += 1;
				if (isMainCharacter) {
					this.GetComponent<MainCharacter> ().SkillTree = GameObject.Find ("SkillTree").GetComponent<SkillTree> ();
					GameObject.Find ("SkillTree").GetComponent<SkillTree> ().GetSkillPoint(1);
					for (int i = 0; i < Lv; i++) {
						if (i < this.GetComponent<MainCharacter> ().SkillTree.SkillGroup.Length) {
							this.GetComponent<MainCharacter> ().SkillTree.SkillGroup [i].ChangeisUnlocked (true);
						} else
							break;
					}

					if (GameManager.GM.MainCharRole == Role.Attacker) {
						GameObject.Find ("SkillTree").GetComponent<SkillTree> ().AddTotalATK(1);
						GameObject.Find ("SkillTree").GetComponent<SkillTree> ().AddTotalHP (3);
						CurrentHp += 3;
					}
				}
				Exp -= ExpRequirment;
				ExpChangedValue -= AddedExp;
				AddedExp = 0;
				ExpBeforeChange = 0;
				Debug.Log ("Exp Before: " + ExpBeforeChange);
				Debug.Log ("Exp ChangeValue: " + ExpChangedValue);
				Debug.Log ("AddedExp: " + AddedExp);
				Debug.Log (Exp);

				if(Lv > 0 && Lv <= 5)
					ExpRequirment = 0 + Lv * 15;
				else if(Lv > 5 && Lv <= 10)
					ExpRequirment = 15 + Lv * 15;
				else if(Lv > 10 && Lv <= 15)
					ExpRequirment = 30 + Lv * 15;
				else if(Lv > 15 && Lv <= 20)
					ExpRequirment = 45 + Lv * 15;
				else if(Lv > 20)
					ExpRequirment = 60 + Lv * 15;
				CurrentHp += growHP;
				reloadAttributes (false);
				//Debug.Log (gameObject + " ,Exp = " + Exp + " ,Exp Requirement = " + ExpRequirment);
			}
		}
	}

	public void reloadPower(){
		CharacterPower = ((atk + (int)CurrentHp + (int)CurrentEnergy + def) * 2) + (Lv * 5) + (int)(cR * 1.5 + (cD-100)) + (int)(Mathf.Abs(movspeed - 100)*2);
		MaxCharacterPower = ((atk + (int)Maxhp + (int)MaxEnergy + def) * 2) + (Lv * 5) + (int)(cR * 1.5 + (cD-100)) + (int)(Mathf.Abs(movspeed - 100)*2);
	}

	public void reloadAttributes(bool SaveOrNot){
		if (!isMainCharacter) {
			totalgrowHP = growHP * (Lv - 1);
			totalgrowATK = growATK * (Lv - 1);
			totalgrowDEF = growDEF * (Lv - 1);
			totalgrowLOAD = growLOAD * (Lv - 1);
			atk = baseAtk + totalgrowATK;
			Maxhp = baseHp + totalgrowHP;
			MaxEnergy = baseEnergy;
			def = baseDef + totalgrowDEF;
			load = baseLoad + totalgrowLOAD;
			movspeed = baseMovspeed;
			cR = baseCriticalRate;
			cD = baseCriticalDamage;
			hR = baseHitRate;


			if (!isInitialized) {
				CurrentEnergy = MaxEnergy;
				CurrentHp = Maxhp;
				isInitialized = true;
			}

			leadership = baseleadership;
			CharacterPower = ((atk + (int)CurrentHp + (int)CurrentEnergy + def) * 2) + (Lv * 5) + (int)(cR * 1.5 + (cD-100)) + (int)(Mathf.Abs(movspeed - 100)*2);
			MaxCharacterPower = ((atk + (int)Maxhp + (int)MaxEnergy + def) * 2) + (Lv * 5) + (int)(cR * 1.5 + (cD-100)) + (int)(Mathf.Abs(movspeed - 100)*2);
			this.GetComponent<Attack> ().canMove = false;
			this.GetComponent<Attack> ().canAttack = false;
			this.GetComponent<Attack> ().canRecharge = true;
			this.GetComponent<Attack> ().MovementSpeed = 0f;
            if (SaveOrNot)
            {
                if (SaveGame.Exists("Character/" + charName))
                {
                    SaveGame.Delete("Character/" + charName);
                }
                SaveGame.Save("Character/" + charName, this);
            }
        } else if (isMainCharacter) {
			this.GetComponent<MainCharacter> ().SkillTree = GameObject.Find("SkillTree").GetComponent<SkillTree>();
			atk = baseAtk + gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalATK;
			Maxhp = baseHp + gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalHP;
			MaxEnergy = baseEnergy + gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalEnergy;
			if (!isInitialized) {
				CurrentEnergy = MaxEnergy;
				CurrentHp = Maxhp;
				isInitialized = true;
			} else {
				CurrentHp = CurrentHp;
				CurrentEnergy = CurrentEnergy;
			}
			def = baseDef + gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalDEF;
			load = baseLoad + gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalWL;
			movspeed = baseMovspeed + gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalMS;
			cR = baseCriticalRate + gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalCR;
			cD = baseCriticalDamage + gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalCD;
			hR = baseHitRate + gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalHR;
			leadership = baseleadership + gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalLeadership;
			CharacterPower = ((atk + (int)CurrentHp + (int)CurrentEnergy + def) * 2) + (Lv * 5) + (int)(cR * 1.5 + (cD-100)) + (int)(Mathf.Abs(movspeed - 100)*2);
			MaxCharacterPower = ((atk + (int)Maxhp + (int)MaxEnergy + def) * 2) + (Lv * 5) + (int)(cR * 1.5 + (cD-100)) + (int)(Mathf.Abs(movspeed - 100)*2);
			this.GetComponent<Attack> ().canMove = false;
			this.GetComponent<Attack> ().canAttack = false;
			this.GetComponent<Attack> ().canRecharge = true;
			this.GetComponent<Attack> ().MovementSpeed = 0f;
			this.GetComponent<MainCharacter> ().SkillTree = null;
            if (SaveOrNot)
            {
                if (SaveGame.Exists ("Character/" + name)) {
				    SaveGame.Delete ("Character/" + name);
			}
			    SaveGame.Save ("Character/" + name, this);
			}
		}
	}

	public void SelectorShow(){
		gameObject.transform.Find ("Selector").gameObject.SetActive (true);
	}

	public void SelectorNotShow(){
		gameObject.transform.Find ("Selector").gameObject.SetActive (false);
	}

	public void AddExp(float Value){
		ExpChangedValue = Value;
		ExpBeforeChange = Exp;
		ExpCanUpdate = true;
		//Debug.Log (gameObject + " ,Exp = " + Exp + " ,Exp Requirement = " + ExpRequirment);
	
	}

	public void GetHurt(int Value){
		HpChangedValue = Value;
		HpBeforeChange = CurrentHp;
		HpCanUpdate = true;
		isDecreaseHP = true;
	}

	public void EnergyLost(int Value){
		MpChangedValue = Value;
		MpBeforeChange = CurrentEnergy;
		MpCanUpdate = true;
		isDecreaseMP = true;
	}

	public void EnergyRecovery(int Value){
		MpChangedValue = Value;
		MpBeforeChange = CurrentEnergy;
		MpCanUpdate = true;
		isDecreaseMP = false;
	}


	public void GetHeal(int Value){
		HpChangedValue = Value;
		HpBeforeChange = CurrentHp;
		HpCanUpdate = true;
		isDecreaseHP = false;
	}
}
