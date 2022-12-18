using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillHandler : MonoBehaviour
{
	public Attack attackClass;
	public SkillActiveType ActiveSkill;
	public Character characterClass;

	public enum SkillActiveType{
		None,
		// Attacker Skill
		FatalBlow,
		FatalBlowWithFatalBlowPlus,
		FatalBlowWithBlastBlow,
		FatalBlowWithFatalBlowPlusandSunder,
		FatalBlowWithFatalBlowPlusandBlastBlowPlus,
		FatalBlowWithBlastBlowandSunder,
		FatalBlowWithBlastBlowandBlastBlowPlus
	}

	void Start(){
		attackClass = gameObject.transform.parent.GetComponent<Attack> ();
		characterClass = gameObject.transform.parent.GetComponent<Character> ();
		gameObject.name = "SkillHandler";
	}

	void Update(){
		if (GameManager.GM.gamestatus != GameManager.GameStatus.Battle)
			return;
		
	}


}
