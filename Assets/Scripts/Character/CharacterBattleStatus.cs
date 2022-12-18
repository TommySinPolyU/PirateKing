using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBattleStatus : MonoBehaviour{

	public int additATK, additDEF;
	public float additSPEED, additCR, additCD;
	public Character character;
	public Status[] statusList;
	private int statusCount = 0;
	void Start(){
		statusCount = 0;
		gameObject.name = "StatusList";
	}

	void Update(){
		if (GameManager.GM.gamestatus != GameManager.GameStatus.Battle) {
			try{
				//Destroy(this);
				//Reset Elements
				additATK = 0; additDEF = 0; additSPEED = 0; additCR = 0; additCD = 0;
				Array.Clear(statusList,0,statusList.Length);
				statusCount = 0;
			}catch(System.Exception e){
				Debug.Log ("Null Object");
			}
		} else if(GameManager.GM.gamestatus == GameManager.GameStatus.Battle) {
			//
		
			character = gameObject.transform.parent.GetComponent<Character>();
		}
	}

	public void createNewStatus(Status.StatusType statusType, Character Target, float statusValue, int period){
		bool Created = false;
		GameObject TempStatus = new GameObject();
		Status statusClass = TempStatus.AddComponent<Status>();
		TempStatus.AddComponent<Image>();
		TempStatus.transform.SetParent (Target.gameObject.transform.Find("StatusList").transform);
		statusClass.statusType = statusType;
		statusClass.image = statusClass.GetComponent<Image>();
		statusClass.image.sprite = Resources.Load<Sprite>(Status.spritePath[(int)statusType]);
		statusClass.value = statusValue;
		statusClass.round = period;
		updateStatusList ();
		for (int i = 0; i < statusList.Length; i++) {
			if (statusList [i].statusType == statusType) {
				Destroy (statusList [i].gameObject);
				statusList [i] = statusClass;
				break;
			} else {
				statusList [i] = statusClass;
			}
		}
		float changedValue;
		if(!Created){
		switch (statusType) {
		case Status.StatusType.ATKDOWN:
			additATK -= (int)statusValue;
			character.reloadAttributes (false);
			if(!character.isMainCharacter){
				character.atk = character.baseAtk + character.totalgrowATK + additATK;
			}
			else{
				character.atk = character.baseAtk + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalATK + additATK;
			}
			break;
		case Status.StatusType.ATKUP:
			additATK += (int)statusValue;
			character.reloadAttributes (false);
			if(!character.isMainCharacter){
				character.atk = character.baseAtk + character.totalgrowATK + additATK;
			}
			else{
				character.atk = character.baseAtk + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalATK + additATK;
			}
			break;
		case Status.StatusType.CRITDAMDOWN:
			additCD -= statusValue;
			character.reloadAttributes (false);
			if(!character.isMainCharacter){
				character.cD = character.baseCriticalDamage + additCD;
			}
			else{
				character.cD = character.baseCriticalDamage + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalCD + additCD;
			}
			break;
		case Status.StatusType.CRITDAMUP:
			additCD += statusValue;
			character.reloadAttributes (false);
			if(!character.isMainCharacter){
				character.cD = character.baseCriticalDamage + additCD;
			}
			else{
				character.cD = character.baseCriticalDamage + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalCD + additCD;
			}
			break;
		case Status.StatusType.CRITRATEDOWN:
			additCR -= statusValue;
			character.reloadAttributes (false);
			if(!character.isMainCharacter){
				character.cR = character.baseCriticalRate + additCR;
			}
			else{
				character.cR = character.baseCriticalRate + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalCR + additCR;
			}
			break;
		case Status.StatusType.CRITRATEUP:
			additCR += statusValue;
			character.reloadAttributes (false);
			if(!character.isMainCharacter){
				character.cR = character.baseCriticalRate + additCR;
			}
			else{
				character.cR = character.baseCriticalRate + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalCR + additCR;
			}
			break;
		case Status.StatusType.DEFDOWN:
			additDEF -= (int)statusValue;
			character.reloadAttributes (false);
			if(!character.isMainCharacter){
				character.def = character.baseDef + additDEF;
			}
			else{
				character.def = character.baseDef + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalDEF + additDEF;
			}
			break;
		case Status.StatusType.DEFUP:
			additDEF += (int)statusValue;
			character.reloadAttributes (false);
			if(!character.isMainCharacter){
				character.def = character.baseDef + additDEF;
			}
			else{
				character.def = character.baseDef + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalDEF + additDEF;
			}
			break;
		case Status.StatusType.SPEEDDOWN:
			changedValue = Target.movspeed * statusValue /100 ;
			additSPEED += changedValue;
			character.reloadAttributes (false);
			if(!character.isMainCharacter){
				character.movspeed = character.baseMovspeed + additSPEED;
			}
			else{
				character.movspeed = character.baseMovspeed + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalMS + additSPEED;
			}
			break;
		case Status.StatusType.SPEEDUP:
			changedValue = Target.movspeed * statusValue /100;
			Debug.Log("ChangeValue" + changedValue);
			Debug.Log("AdditSpeed" + additSPEED);
			additSPEED -= changedValue;
			character.reloadAttributes (false);
			if(!character.isMainCharacter){
				character.movspeed = character.baseMovspeed + additSPEED;
			}
			else{
				character.movspeed = character.baseMovspeed + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalMS + additSPEED;
			}
			Debug.Log("CharSpeed" + character.movspeed);
			break;
		}
		TempStatus.name = statusType.ToString() + "_" + statusValue;
		Created = true;
		}

		updateStatusList ();
	}

	public void updateStatusList(){
		statusList = new Status[this.gameObject.transform.childCount];
		for (int i = 0; i < statusList.Length; i++) {
			try{
				statusList [i] = this.gameObject.transform.GetChild (i).GetComponent<Status> ();
			}
			catch(System.Exception e){
				Debug.Log ("Status Not Found.");
			}
		}
	}

	public void roundCounting(Character Target){
		if (statusList == null)
			return;
		for(int i = 0; i < statusList.Length; i++){
			try{
				statusList[i].round--;
				if(statusList[i].round <= 0){
					float changedValue;
					switch (statusList[i].statusType) {
						case Status.StatusType.ATKDOWN:
							additATK += (int)statusList[i].value;
							character.reloadAttributes (false);
							if(!character.isMainCharacter){
								character.atk = character.baseAtk + character.totalgrowATK + additATK;
							}
							else{
							character.atk = character.baseAtk + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalATK + additATK;
							}
						break;
						case Status.StatusType.ATKUP:
							additATK -= (int)statusList[i].value;
							character.reloadAttributes (false);
							if(!character.isMainCharacter){
								character.atk = character.baseAtk + character.totalgrowATK + additATK;
							}
							else{
								character.atk = character.baseAtk + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalATK + additATK;
							}
						break;
						case Status.StatusType.CRITDAMDOWN:
							additCD += statusList[i].value;
							character.reloadAttributes (false);
							if(!character.isMainCharacter){
								character.cD = character.baseCriticalDamage + additCD;
							}
							else{
								character.cD = character.baseCriticalDamage + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalCD + additCD;
							}
						break;
						case Status.StatusType.CRITDAMUP:
							additCD -= statusList[i].value;
							character.reloadAttributes (false);
							if(!character.isMainCharacter){
								character.cD = character.baseCriticalDamage + additCD;
							}
							else{
								character.cD = character.baseCriticalDamage + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalCD + additCD;
							}
						break;
						case Status.StatusType.CRITRATEDOWN:
							additCR += statusList[i].value;
							character.reloadAttributes (false);
							if(!character.isMainCharacter){
								character.cR = character.baseCriticalRate + additCR;
							}
							else{
								character.cR = character.baseCriticalRate + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalCR + additCR;
							}
						break;
						case Status.StatusType.CRITRATEUP:
							additCR -= statusList[i].value;
							character.reloadAttributes (false);
							if(!character.isMainCharacter){
								character.cR = character.baseCriticalRate + additCR;
							}
							else{
								character.cR = character.baseCriticalRate + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalCR + additCR;
							}
						break;
						case Status.StatusType.DEFDOWN:
							additDEF += (int)statusList[i].value;
							character.reloadAttributes (false);
							if(!character.isMainCharacter){
								character.def = character.baseDef + additDEF;
							}
							else{
								character.def = character.baseDef + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalDEF + additDEF;
							}
						break;
						case Status.StatusType.DEFUP:
							additDEF -= (int)statusList[i].value;
							character.reloadAttributes (false);
							if(!character.isMainCharacter){
								character.def = character.baseDef + additDEF;
							}
							else{
								character.def = character.baseDef + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalDEF + additDEF;
							}
						break;
						case Status.StatusType.SPEEDDOWN:
							changedValue = Target.movspeed * statusList[i].value /100 ;
							additSPEED -= changedValue;
							character.reloadAttributes (false);
							if(!character.isMainCharacter){
								character.movspeed = character.baseMovspeed + additSPEED;
							}
							else{
								character.movspeed = character.baseMovspeed + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalMS + additSPEED;
							}
						break;
						case Status.StatusType.SPEEDUP:
							changedValue = Target.movspeed * statusList[i].value /100;
							Debug.Log("ChangeValue" + changedValue);
							Debug.Log("AdditSpeed" + additSPEED);
							additSPEED += changedValue;
							character.reloadAttributes (false);
							if(!character.isMainCharacter){
								character.movspeed = character.baseMovspeed + additSPEED;
							}
							else{
								character.movspeed = character.baseMovspeed + character.gameObject.GetComponent<MainCharacter>().SkillTree.saveElements.totalMS + additSPEED;
							}
							Debug.Log("CharSpeed" + character.movspeed);
						break;
		}
			Destroy(statusList[i].gameObject);
				}
			}
			catch(System.Exception e){
				Debug.Log ("status not found.");
			}
		}
	}

}
