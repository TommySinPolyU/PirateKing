using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BayatGames.SaveGamePro;

public class NormalCharacterSkillList : MonoBehaviour {

	// 以上為該技能提供之能力加升 //
	public static NormalCharacterSkillList NCSL;
	public bool isInitialized = false;

	public Skill[] CurrentTreeElement;
	public bool SkillTreeReset;
	public Character currentCharacter;


	void Awake(){
		if (NCSL == null) {
			NCSL = this;
			DontDestroyOnLoad (gameObject);
		} else if ((NCSL != this)) {
			Destroy (gameObject);
		}
	}

	void Update(){
		if (currentCharacter != null) {
			CurrentTreeElement[0].UpdateDesc("Heal","治療",
				"Treat a groupmate to restore " + (5 + currentCharacter.Maxhp / 10) + " HP",
				"回復一名隊友 " + (5 + currentCharacter.Maxhp / 10) + " 生命");
		}
	}


	// Use this for initialization
	void Start () {
		InitializeSkillTree ();
	}


	public Skill ReturnSkill(int id){
		return CurrentTreeElement [id];
	}

	public void UseSkill(int id){
		CurrentTreeElement [id].isSkillOn = true;
	}


	public void InitializeSkillTree(){
		int InitializedElement = 0;
		// Skill initialization for Attacker.
			if (!isInitialized) {
				CurrentTreeElement = new Skill[100];
			}
		GameObject[] SkillTempObject = new GameObject[CurrentTreeElement.Length];
			for (int k = 0; k < SkillTempObject.Length; k++) {
				SkillTempObject [k] = new GameObject ();
				CurrentTreeElement [InitializedElement] = SkillTempObject [k].AddComponent<Skill> ();
				DontDestroyOnLoad (SkillTempObject [k]);
			SkillTempObject [k].transform.SetParent (this.gameObject.transform);
				InitializedElement++;
			}


			// Skills Setting
		CurrentTreeElement[0].SetSkill(0,"Heal","治療",
			"Treat a groupmate to restore [5 + 10% of your maximum HP] HP",
			"回復一名隊友 [5 + 你的10%最大生命] 生命",
			"Image/HPUP");

		isInitialized = true;
	}
		

	//Function will calling when player learn skill.


	// Constuctor in this Class



	// Getter Setting for Variables.

	// Getter Setting End //
}
