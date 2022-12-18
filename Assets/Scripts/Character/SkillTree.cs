using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BayatGames.SaveGamePro;

public class SkillTree : MonoBehaviour {

	// 以上為該技能提供之能力加升 //
	public MainCharacter MC;
	public static SkillTree ST;
	public bool isInitialized = false;
	public SaveElements saveElements = new SaveElements();

	public Skill[] CurrentTreeElement;
	public GameObject[] SkillGroupObject;
	public Skill[] ActiveSkill;
	public SkillList[] SkillGroup;
	public bool SkillTreeReset;

	public class SaveElements{
		public int SkillPoint;
		public int totalATK = 0, totalDEF = 0, totalHP = 0, totalEnergy = 0, totalWL = 0, totalDSE = 0, totalLeadership = 0;
		public float totalAS = 0f, totalCR = 0f, totalCD = 0f, totalHR = 0f, totalSD = 0f, totalMS = 0f, totalDSC = 0;
	}

	public void AddTotalATK(int value){
		saveElements.totalATK += value;
		SaveGame.Save("SkillTree/" + name, saveElements);
		MC.reloadAttributes (true);
	}

	public void AddTotalDEF(int value){
		saveElements.totalDEF += value;
		SaveGame.Save("SkillTree/" + name, saveElements);
		MC.reloadAttributes (true);
	}

	public void AddTotalHP(int value){
		saveElements.totalHP += value;
		MC.CurrentHp += value;
		SaveGame.Save("SkillTree/" + name, saveElements);
		MC.reloadAttributes (true);
	}

	public void AddTotalEnergy(int value){
		saveElements.totalEnergy += value;
		MC.CurrentEnergy += value;
		SaveGame.Save("SkillTree/" + name, saveElements);
		MC.reloadAttributes (true);
	}

	public void AddTotalDSE(int value){
		saveElements.totalDSE += value;
		SaveGame.Save("SkillTree/" + name, saveElements);
		MC.reloadAttributes (true);
	}

	public void AddTotalMS(int value){
		saveElements.totalMS += value;
		SaveGame.Save("SkillTree/" + name, saveElements);
		MC.reloadAttributes (true);
	}

	public void AddTotalCR(int value){
		saveElements.totalCR += value;
		SaveGame.Save("SkillTree/" + name, saveElements);
		MC.reloadAttributes (true);
	}

	public void AddTotalCD(int value){
		saveElements.totalCD += value;
		SaveGame.Save("SkillTree/" + name, saveElements);
		MC.reloadAttributes (true);
	}
		

	/*
	 * totalATK = 總攻擊加成 / totalDEF = 總防禦加成 / totalHP = 總最大生命加成 / totalAS = 總攻擊速度加成 / totalMS = 總移動速度加成
	 * totalCR = 總暴擊機率加成 / totalCD = 總暴擊傷害加成 / totalHR = 總命中率加成 / totalWL = 總負重加成
	 * totalSD = 總技能傷害加成 / totalDSC = 總技能冷卻時間減少
	 * totalDSE = 總技能能量消耗減少
	 */

	void Awake(){
		if (ST == null) {
			ST = this;
			DontDestroyOnLoad (gameObject);
		} else if ((ST != this)) {
			Destroy (gameObject);
		}
	}

	void Update(){
		if (MC == null)
			return;
		switch (GameManager.GM.MainCharRole) {
		case MainCharacter.Role.Attacker:
			CurrentTreeElement [0].UpdateDesc
			("Source of Power I", "力量之源", "Increase extra 1 ATK and extra 3 HP / LV\nTotal Extra ATK Increase: " + (MC.Lv - 1) + "\nTotal Extra HP Increase: " + ((MC.Lv - 1) * 3), "每次升級，將額外提高 1 點攻擊及 3 點生命。\n總額外攻擊加成： " + (MC.Lv - 1) + "\n總額外生命加成： " + ((MC.Lv - 1) * 3));

			if (CurrentTreeElement [11].isLearned) {
				CurrentTreeElement [1].UpdateDesc
				("Fatal Blow", "致命打擊",
					"This turn of attack must CRIT, and Attach additional [" + (MC.Lv * 3) + "] points of physical damage;" + "" +
					"\n 'BlastBlow' : After using 'Fatal Blow', Increase 50% Speed in lasts 4 turns.", 
					"本次攻擊必然暴擊，且額外附加[" + (MC.Lv * 3) + "]點物理傷害" + "" +
					"\n '疾風打擊' : 使用「致命打擊」後，於接下來 4 回合內增加 50% 速度");
				
				if (CurrentTreeElement [25].isLearned) {
					CurrentTreeElement [1].UpdateDesc
					("Fatal Blow", "致命打擊",
						"This turn of attack must CRIT, and Attach additional [" + (MC.Lv * 3) + "] points of physical damage;" + "" +
						"\n 'BlastBlow' : After using 'Fatal Blow', Increase 50% Speed in lasts 4 turns." + 
						"\n 'Blast Blow+' ： Attach [" + Mathf.RoundToInt((2 * Mathf.Abs((MC.movspeed - 50) * 0.25f))) + "] Additional True Damage (Ignore target ALL DEF)",
						"本次攻擊必然暴擊，且額外附加[" + (MC.Lv * 3) + "]點物理傷害" + 
						"\n '疾風打擊' : 使用「致命打擊」後，於接下來 4 回合內增加 50% 速度" + 
						"\n '疾風打擊+' ： 附加額外 [" + Mathf.RoundToInt((2 * Mathf.Abs((MC.movspeed - 50) * 0.25f))) + "] 點真實傷害 (此傷害無視敵人防禦)");
				}

				if (CurrentTreeElement [24].isLearned) {
					CurrentTreeElement [1].UpdateDesc
					("Fatal Blow", "致命打擊",
						"This turn of attack must CRIT, and Attach additional [" + (MC.Lv * 3) + "] points of physical damage;" + "" +
						"\n 'BlastBlow' : After using 'Fatal Blow', Increase 50% Speed in lasts 4 turns." +
						"\n 'Sunder' ： Ignore target 25% DEF",
						"本次攻擊必然暴擊，且額外附加[" + (MC.Lv * 3) + "]點物理傷害" +
						"\n '疾風打擊' : 使用「致命打擊」後，於接下來 4 回合內增加 50% 速度" +
						"\n '破甲' ： 無視 25% 目標的防禦");
				}
			}

			if (CurrentTreeElement [10].isLearned) {
				CurrentTreeElement [1].UpdateDesc
				("Fatal Blow", "致命打擊",
					"This turn of attack must CRIT, and Attach additional [" + (MC.Lv * 3) + "] points of physical damage;" + 
					"\n 'FatalBlow+ :' Absorb target 5% of Current Hp.",
					"本次攻擊必然暴擊，且額外附加[" + (MC.Lv * 3) + "]點物理傷害" + "" +
					"\n '致命打擊+ :' 額外吸取敵人 5% 生命");
				if (CurrentTreeElement [24].isLearned) {
					CurrentTreeElement [1].UpdateDesc
					("Fatal Blow", "致命打擊",
						"This turn of attack must CRIT, and Attach additional [" + (MC.Lv * 3) + "] points of physical damage;" + "" +
						"\n 'BlastBlow' : After using 'Fatal Blow', Increase 50% Speed in lasts 4 turns." + 
						"\n 'Sunder' ： Ignore target 25% DEF",
						"本次攻擊必然暴擊，且額外附加[" + (MC.Lv * 3) + "]點物理傷害" + 
						"\n '致命打擊+' : 額外吸取敵人 5% 生命" + 
						"\n '破甲' ： 無視 25% 目標的防禦");
				}

				if (CurrentTreeElement [25].isLearned) {
					CurrentTreeElement [1].UpdateDesc
					("Fatal Blow", "致命打擊",
						"This turn of attack must CRIT, and Attach additional [" + (MC.Lv * 3) + "] points of physical damage;" + "" +
						"\n 'FatalBlow+ :' Absorb target 5% of Current Hp." + 
						"\n 'Blast Blow+' ： Attach [" + Mathf.RoundToInt((2 * Mathf.Abs((MC.movspeed - 50) * 0.25f))) + "] Additional True Damage (Ignore target ALL DEF)",
						"本次攻擊必然暴擊，且額外附加[" + (MC.Lv * 3) + "]點物理傷害" + 
						"\n '致命打擊+' : 額外吸取敵人 5% 生命" +
						"\n '疾風打擊+' ： 附加額外 [" + Mathf.RoundToInt((2 * Mathf.Abs((MC.movspeed - 50) * 0.25f))) + "] 點真實傷害 (此傷害無視敵人防禦)");
				}
			}

			CurrentTreeElement [25].UpdateValue (Mathf.RoundToInt ((2 * Mathf.Abs ((MC.movspeed - 50) * 0.25f))));
			break;
		}
	}
	// Setting Number of skill for each floor / LV;


	// Use this for initialization
	void Start () {
		InitializeSkillTree ();
	}

	public void GetSkillPoint(int value){
		saveElements.SkillPoint += value;
		SaveGame.Save("SkillTree/" + name, saveElements);
	}

	public Skill ReturnSkill(int id){
		return CurrentTreeElement [id];
	}

	public void UseSkill(int id){
		CurrentTreeElement [id].isSkillOn = true;
	}
	private int[] AttackChoicesAtArea = {2,2,3,3,2,3,3,3,3,2}; // Selections of Attacker

	public void InitializeSkillTree(){
		saveElements = new SaveElements ();
		saveElements.SkillPoint = 0;
		int InitializedElement = 0;
		// Skill initialization for Attacker.
		switch (GameManager.GM.MainCharRole) {
		case MainCharacter.Role.Attacker:
			if (!isInitialized) {
				CurrentTreeElement = new Skill[100];
				SkillGroup = new SkillList[AttackChoicesAtArea.Length];
				SkillGroupObject = new GameObject[AttackChoicesAtArea.Length];
			}

			for (int i = 0; i < AttackChoicesAtArea.Length; i++) {
				if (SkillGroupObject [i] != null) {
					Destroy (SkillGroupObject [i]);
					Debug.Log ("Delete");
				}
				SkillGroupObject [i] = new GameObject ();
				SkillGroupObject [i].transform.SetParent (gameObject.transform);
				SkillGroupObject [i].name = "SkillGroup" + i;
				SkillGroup [i] = SkillGroupObject [i].AddComponent<SkillList> ();

				GameObject[] SkillTempObject = new GameObject[AttackChoicesAtArea [i]];
				for (int k = 0; k < SkillTempObject.Length; k++) {
					SkillTempObject [k] = new GameObject ();
					CurrentTreeElement [InitializedElement] = SkillTempObject [k].AddComponent<Skill> ();
					CurrentTreeElement [InitializedElement].UpdateEquippedChar (MC);
					DontDestroyOnLoad (SkillTempObject [k]);
					SkillTempObject [k].transform.SetParent (SkillGroupObject [i].gameObject.transform);
					InitializedElement++;
				}
			}


			// Group 0
			CurrentTreeElement [0].SetSkill 
			(0, "Source of Power I", "力量之源", "Increase extra 1 ATK and extra 3 HP / LV\nTotal Extra ATK Increase: " + (MC.Lv - 1) + "\nTotal Extra HP Increase: " + ((MC.Lv - 1) * 3), "每次升級，將額外提高 1 點攻擊及 3 點生命。\n總額外攻擊加成： " + (MC.Lv - 1) + "\n總額外生命加成： " + ((MC.Lv - 1) * 3), "Image/PassiveSkill_Attacker");
			CurrentTreeElement [1].SetActiveSkill
			(0, "Fatal Blow", "致命打擊", "This turn of attack must CRIT, and Attach additional [" + (MC.Lv * 3) + "] points of physical damage.", "本次攻擊必然暴擊，且額外附加[" + (MC.Lv * 3) + "]點物理傷害", 10, 3, "Image/ActiveSkill_Attacker");
			// Group 1
			CurrentTreeElement [2].SetSkill 
			(1, "Attack Up I", "攻擊強化 I", "Increase 2 ATK", "提高 2 點攻擊", "Image/ATKUP", Skill.PassiveSkillType.Attack, (int)2);
			CurrentTreeElement [3].SetSkill 
			(1, "Critical Rate Up I", "暴擊率強化 I", "Increase 1 % Critical Rate", "提高 1 %暴擊機率", "Image/CRITRATEUP", Skill.PassiveSkillType.CriticalRate, (int)1);
			// Group 2
			CurrentTreeElement [4].SetSkill
			(1, "Hp Up I", "生命強化 I", "Increase 15 MaxHp", "提高 15 點最大生命", "Image/HPUP", Skill.PassiveSkillType.Hp, (int)15);
			CurrentTreeElement [5].SetSkill
			(1, "Energy Up I", "能量強化 I", "Increase 10 MaxEnergy", "提高 10 點最大能量", "Image/MPUP", Skill.PassiveSkillType.Energy, (int)10);
			CurrentTreeElement [6].SetSkill
			(1, "Attack Up II", "攻擊強化 II", "Increase 2 ATK", "提高 2 點攻擊", "Image/ATKUP", Skill.PassiveSkillType.Attack, (int)2);
			// Group 3
			CurrentTreeElement [7].SetSkill
			(1, "Critical Rate Up II", "暴擊率強化 II", "Increase 2 % Critical Rate", "提高 2 %暴擊機率", "Image/CRITRATEUP", Skill.PassiveSkillType.CriticalRate, (int)2);
			CurrentTreeElement [8].SetSkill
			(1, "Attack Up III", "攻擊強化 III", "Increase 3 ATK", "提高 3 點攻擊", "Image/ATKUP", Skill.PassiveSkillType.Attack, (int)3);
			CurrentTreeElement [9].SetSkill
			(1, "Defend Up I", "防禦強化 I", "Increase 2 DEF", "提高 2 點防禦", "Image/Defend", Skill.PassiveSkillType.Defend, (int)2);
			// Group 4
			CurrentTreeElement [10].SetSkill
			(1, "Fatal Blow+", "致命打擊+", "'Fatal Blow' will Absorb target 5% of Current Hp", "「致命打擊」將會額外吸取敵人 5% 生命", "Image/ActiveSkill_Attacker");
			CurrentTreeElement [11].SetSkill
			(1, "Blast Blow", "疾風打擊", "After using 'Fatal Blow', Increase 50% Speed in lasts 4 turns.", "使用「致命打擊」後，於接下來 4 回合內增加 50% 速度", "Image/ActiveSkill_Attacker");
			// Group 5
			CurrentTreeElement [12].SetSkill
			(1, "Fatal Feast", "致命饗宴", "Increase 25% Critical Damage", "提高 25% 暴擊時造成的傷害", "Image/CRITDAMUP", Skill.PassiveSkillType.CriticalDamage, (int)25);
			CurrentTreeElement [13].SetSkill
			(1, "Attack Up IV", "攻擊強化 IV", "Increase 4 ATK", "提高 4 點攻擊", "Image/ATKUP", Skill.PassiveSkillType.Attack, (int)4);
			CurrentTreeElement [14].SetSkill
			(1, "Critical Rate Up III", "暴擊率強化 III", "Increase 3 % Critical Rate", "提高 3 %暴擊機率", "Image/CRITRATEUP", Skill.PassiveSkillType.CriticalRate, (int)3);
			//Group 6
			CurrentTreeElement [15].SetSkill
			(2, "Flash", "快如閃電", "Reduce 1 s turn charge time", "減少 1 秒 回合充能時間", "Image/Speed", Skill.PassiveSkillType.Speed, (int)1);
			CurrentTreeElement [16].SetSkill
			(2, "Attack Up V", "攻擊強化 V", "Increase 5 ATK", "提高 5 點攻擊", "Image/ATKUP", Skill.PassiveSkillType.Attack, (int)5);
			CurrentTreeElement [17].SetSkill
			(1, "Defend Up II", "防禦強化 II", "Increase 3 DEF", "提高 3 點防禦", "Image/Defend", Skill.PassiveSkillType.Defend, (int)3);
			// Group 7
			CurrentTreeElement [18].SetSkill
			(2, "Fatal Feast+", "致命饗宴+", "Increase 15% Critical Damage and 2% Critical Rate", "提高 15% 暴擊時造成的傷害及 2% 暴擊機率", "Image/CRITDAMUP", Skill.PassiveSkillType.CriticalDamage, (int)15, null, Skill.PassiveSkillType.CriticalRate, -2, CurrentTreeElement [12]);
			CurrentTreeElement [19].SetSkill
			(1, "Hp Up II", "生命強化 II", "Increase 30 MaxHp", "提高 30 點最大生命", "Image/HPUP", Skill.PassiveSkillType.Hp, (int)30);
			CurrentTreeElement [20].SetSkill
			(1, "Energy Up II", "能量強化 II", "Increase 20 MaxEnergy", "提高 20 點最大能量", "Image/MPUP", Skill.PassiveSkillType.Energy, (int)20);
			// Group 8
			CurrentTreeElement [21].SetSkill
			(2, "Attack Up+ I", "攻擊強化+ I", "Increase 6 ATK, but Decrease 1 DEF", "提高 6 點攻擊，但降低 1 點防禦", "Image/ATKUP", Skill.PassiveSkillType.Attack, (int)6, null, Skill.PassiveSkillType.Defend, (int)1);
			CurrentTreeElement [22].SetSkill
			(1, "Hp Up III", "生命強化 III", "Increase 30 MaxHp", "提高 30 點最大生命", "Image/HPUP", Skill.PassiveSkillType.Hp, (int)30);
			CurrentTreeElement [23].SetSkill
			(1, "Energy Up III", "能量強化 III", "Increase 20 MaxEnergy", "提高 20 點最大能量", "Image/MPUP", Skill.PassiveSkillType.Energy, (int)20);
			// Group 9
			CurrentTreeElement [24].SetSkill
			(2, "Sunder", "破甲",
				"'Fatal Blow' will Ignore target 25% DEF",
				"「致命打擊」將會無視 25% 目標的防禦",
				"Image/ActiveSkill_Attacker", Skill.PassiveSkillType.None, 0, CurrentTreeElement [10].skillAreaGroup);
			CurrentTreeElement [25].SetSkill
			(2, "Blast Blow+", "疾風打擊+",
				"'Fatal Blow' wlll attach [" + Mathf.RoundToInt ((2 * Mathf.Abs ((MC.movspeed - 50) * 0.25f))) + "] Additional True Damage (Ignore target ALL DEF)",
				"「致命打擊」將會附加額外 [" + Mathf.RoundToInt ((2 * Mathf.Abs ((MC.movspeed - 50) * 0.25f))) + "] 點真實傷害 (此傷害無視敵人防禦)",
				"Image/ActiveSkill_Attacker", Skill.PassiveSkillType.None, Mathf.RoundToInt ((2 * Mathf.Abs ((MC.movspeed - 50) * 0.25f))), CurrentTreeElement [10].skillAreaGroup);


			if (SkillTreeReset) {
				for (int i = 0; i < CurrentTreeElement.Length; i++) {
					try {
						CurrentTreeElement [i].ChangeLearned(false);
					} catch (System.Exception e) {
						//
					}
				}
				for (int i = 0; i < CurrentTreeElement.Length; i++) {
					try {
						CurrentTreeElement [i].skillAreaGroup.ChangeisUnlocked (false);
						CurrentTreeElement [i].skillAreaGroup.ChangeAreaLearned (false);
					} catch (System.Exception e) {
						//
					}
				}
					
				for (int i = 0; i < (MC.Lv - 1); i++) {
					if (GameManager.GM.MainCharRole == Character.Role.Attacker) {
						GameObject.Find ("SkillTree").GetComponent<SkillTree> ().AddTotalATK (1);
						GameObject.Find ("SkillTree").GetComponent<SkillTree> ().AddTotalHP (3);
					}
				}

				MC.SkillTree = GameObject.Find ("SkillTree").GetComponent<SkillTree> ();
				Debug.Log (MC);
				Debug.Log (MC.SkillTree);
				if (MC.SkillTree != null) {
					for (int i = 0; i < MC.Lv; i++) {
						if (i < MC.SkillTree.SkillGroup.Length) {
							MC.SkillTree.SkillGroup [i].ChangeisUnlocked (true);
						} else
							break;
					}
				}
			}
			CurrentTreeElement [1].ChangeLearned(true);
			CurrentTreeElement [0].ChangeLearned(true);
			CurrentTreeElement [1].skillAreaGroup.ChangeisUnlocked (true);
			CurrentTreeElement [1].skillAreaGroup.ChangeAreaLearned (true);
			break;

		}
		isInitialized = true;
		if (GameManager.GM.isLoadGame) {
			saveElements = SaveGame.Load<SaveElements> ("SkillTree/" + name, saveElements);
			Debug.Log ("Load");
			//return;
		}
		// Skill initailization End. //
		if(!SaveGame.Exists("SkillTree/" + name))
			SaveGame.Save("SkillTree/" + name, saveElements);
	}
		

	//Function will calling when player learn skill.
	public void LearnSkill(int id){
		saveElements.SkillPoint -= CurrentTreeElement [id].neededSP;
		CurrentTreeElement [id].ChangeLearned(true);
		CurrentTreeElement[id].skillAreaGroup.ChangeAreaLearned(true);
			switch (CurrentTreeElement [id].passiveSkillType) {
			case Skill.PassiveSkillType.Attack:
			saveElements.totalATK += (int)CurrentTreeElement [id].addvalue;
				break;
			case Skill.PassiveSkillType.Defend:
			saveElements.totalDEF += (int)CurrentTreeElement [id].addvalue;
				break;
			case Skill.PassiveSkillType.CriticalDamage:
			saveElements.totalCD += CurrentTreeElement [id].addvalue;
				break;
			case Skill.PassiveSkillType.CriticalRate:
			saveElements.totalCR += CurrentTreeElement [id].addvalue;
				break;
			case Skill.PassiveSkillType.Energy:
			saveElements.totalEnergy += (int)CurrentTreeElement [id].addvalue;
				break;
			case Skill.PassiveSkillType.Hp:
			saveElements.totalHP += (int)CurrentTreeElement [id].addvalue;
				break;
			case Skill.PassiveSkillType.Load:
			saveElements.totalWL += (int)CurrentTreeElement [id].addvalue;
				break;
			case Skill.PassiveSkillType.SkillCooldown:
			saveElements.totalDSC += CurrentTreeElement [id].addvalue;
				break;
			case Skill.PassiveSkillType.SkillDamage:
			saveElements.totalSD += CurrentTreeElement [id].addvalue;
				break;
			case Skill.PassiveSkillType.SkillEnergy:
			saveElements.totalDSE += (int)CurrentTreeElement [id].addvalue;
				break;
			case Skill.PassiveSkillType.Speed:
			saveElements.totalMS -= CurrentTreeElement [id].addvalue;
				break;
			}

			switch (CurrentTreeElement [id].decreaseSkillType) {
			case Skill.PassiveSkillType.Attack:
			saveElements.totalATK -= (int)CurrentTreeElement [id].decreaseValue;
				break;
			case Skill.PassiveSkillType.Defend:
			saveElements.totalDEF -= (int)CurrentTreeElement [id].decreaseValue;
				break;
			case Skill.PassiveSkillType.CriticalDamage:
			saveElements.totalCD -= CurrentTreeElement [id].decreaseValue;
				break;
			case Skill.PassiveSkillType.CriticalRate:
			saveElements.totalCR -= CurrentTreeElement [id].decreaseValue;
				break;
			case Skill.PassiveSkillType.Energy:
			saveElements.totalEnergy -= (int)CurrentTreeElement [id].decreaseValue;
				break;
			case Skill.PassiveSkillType.Hp:
			saveElements.totalHP -= (int)CurrentTreeElement [id].decreaseValue;
				break;
			case Skill.PassiveSkillType.Load:
			saveElements.totalWL -= (int)CurrentTreeElement [id].decreaseValue;
				break;
			case Skill.PassiveSkillType.SkillCooldown:
			saveElements.totalDSC -= CurrentTreeElement [id].decreaseValue;
				break;
			case Skill.PassiveSkillType.SkillDamage:
			saveElements.totalSD -= CurrentTreeElement [id].decreaseValue;
				break;
			case Skill.PassiveSkillType.SkillEnergy:
			saveElements.totalDSE -= (int)CurrentTreeElement [id].decreaseValue;
				break;
			case Skill.PassiveSkillType.Speed:
			saveElements.totalMS += CurrentTreeElement [id].decreaseValue;
				break;
			}
		SaveGame.Save ("SkillTree/" + CurrentTreeElement [id].name, CurrentTreeElement [id].isLearned);
		SaveGame.Save("SkillTree/" + name, saveElements);
		MC.reloadAttributes (true);

	}

	// Constuctor in this Class



	// Getter Setting for Variables.

	// Getter Setting End //
}
