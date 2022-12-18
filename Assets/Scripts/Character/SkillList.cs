using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BayatGames.SaveGamePro;

public class SkillList : MonoBehaviour
{
    public Skill[] skillitem;
    public SkillTree ST;
    public bool areaLearned = false;
	public bool isUnlocked = false;
	public SaveElement SE = new SaveElement ();

	public class SaveElement{
		public bool areaLearned = false;
		public bool isUnlocked = false;
	}

    // Start is called before the first frame update
    void Start()
    {

        ST = GameObject.Find("SkillTree").GetComponent<SkillTree>();
		if (GameManager.GM.isLoadGame) {
			SE = SaveGame.Load<SaveElement> ("SkillTree/SkillList/" + name, SE);
			Debug.Log (gameObject.name + " AreaLearned " + SE.areaLearned);
			Debug.Log (gameObject.name + " Unlocked " + SE.isUnlocked);
			areaLearned = SE.areaLearned;
			isUnlocked = SE.isUnlocked;
			Debug.Log ("Load");
			//return;
		} else {
			if (!SaveGame.Exists ("SkillTree/SkillList/" + name)) {
				ChangeAreaLearned (false);
				ChangeisUnlocked (false);
			}
		}
    }

	public void ChangeAreaLearned (bool value){
		areaLearned = value;
		SE.areaLearned = areaLearned;
		SaveGame.Save<SaveElement> ("SkillTree/SkillList/" + name, SE);
	}

	public void ChangeisUnlocked (bool value){
		isUnlocked = value;
		SE.isUnlocked = isUnlocked;
		SaveGame.Save<SaveElement> ("SkillTree/SkillList/" + name, SE);
	}

    // Update is called once per frame
    void Update()
    {
        try{
            if(ST == null){
                ST = GameObject.Find("SkillTree").GetComponent<SkillTree>();
			}
		}catch(System.Exception e){

		}
		skillitem = new Skill[this.transform.childCount];

		for(int i = 0; i < skillitem.Length; i++){
			skillitem[i] = this.transform.GetChild(i).GetComponent<Skill>();
		}          
    }
}
