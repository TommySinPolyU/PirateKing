using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterDataScreen : MonoBehaviour
{

	public enum Status
	{
		DataView,
		SkillTree
	}

	public LevelLoader Loader;
	public int selection = 0;
	public Character[] PlayerCharacter;

	[Header("UI Setting")]
	public GameObject TitleObject;
	public GameObject DataObject;
	public GameObject AlertObject;
	public Text Title;
	public Text LV;
	public Text ATK;
	public Text DEF;
	public Text SPEED;
	public Text LOAD;
	public Text CRITRATE;
	public Text CRITDAM;
	public Text HP;
	public Text Energy;
	public Text NAME;
	public Text CharacterPower;
	public Text SkillTreeBtnText;
	public Text SkillPointText;
	public Text SkillLearnText;
	public Text SkillResetText;
	public Text AlertText;


	public SimpleHealthBar ExpBar;
	public SimpleHealthBar LoadingBar;

	[Header("SkillTree UI Setting")]
	public GameObject SkillTreeScreen;
	public GameObject InfoScreen;
	public Text InfoText;
	public Text SkillTitleText;

	Status Screenstatus = Status.DataView;
	public GameObject LoadingScreen;
	public GameObject[] Btn;
	public GameObject CharacterPosition;
	public GameObject SkillTreeBtn;

	private Vector3 PrevirousMousePosition;

	private bool isInitialized = false;

	public GameObject[] SkillArea;
	public Image[] SkillAreaBGImage;
	public Image[] SkillImage;

	Timer timer;
	Timer timer_return;
	Timer InfoTimer;
	Timer ResetTimer;

	public int SelectedSkill;
	public GameObject Trigger;
	public GameObject PrevirousTrigger;
	public bool InfoisShow = false;

	void Awake(){
		GameManager.GM.gamestatus = GameManager.GameStatus.NonBattle;
	}

	void Start(){
		LoadingScreen.SetActive(false);

		GameObject tempTimer = new GameObject ();
		GameObject tempTimer1 = new GameObject ();
	
		timer = tempTimer.AddComponent<Timer>();
		timer_return = tempTimer1.AddComponent<Timer>();
		timer.InitializeStart ();

		for (int i = 0; i < Btn.Length; i++) {
			Btn [i].SetActive(false);
		}
		Btn [0].SetActive(true);
		//Btn [3].SetActive(true);

		PlayerCharacter = new Character[GameObject.Find ("PlayerCharList").transform.childCount - 1 + GameObject.Find ("PlayerCharList").transform.Find("NowGroup").childCount];
		PlayerCharacter [0] = GameObject.Find ("PlayerCharList").transform.Find("MainCharacter").GetComponent<Character>();
		int AddCount = 0;
		for (int i = 0; i < (GameObject.Find ("PlayerCharList").transform.childCount - 1 + GameObject.Find ("PlayerCharList").transform.Find("NowGroup").childCount); i++) {
			if (GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").childCount - 1 >= 1 && AddCount < GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").childCount - 1) {
				PlayerCharacter [i] = GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform.GetChild (i).GetComponent<Character> ();
				PlayerCharacter [i].gameObject.transform.position = CharacterPosition.transform.position;
				PlayerCharacter [i].gameObject.SetActive (false);
				PlayerCharacter [i].SelectorNotShow ();
				AddCount++;

			} else {
				PlayerCharacter [i] = GameObject.Find ("PlayerCharList").transform.GetChild (i - (GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").childCount)).GetComponent<Character> ();
				PlayerCharacter [i].gameObject.transform.position = CharacterPosition.transform.position;
				PlayerCharacter [i].gameObject.SetActive (false);
				PlayerCharacter [i].SelectorNotShow ();
			}
		}

		if (selection == 0) {
			PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().reloadAttributes (false);
			reloadTextMain ();
			PlayerCharacter [selection].gameObject.transform.position = CharacterPosition.transform.position;
			PlayerCharacter [selection].gameObject.SetActive (true);
			PlayerCharacter [selection].SelectorNotShow ();
		}
		else {
			PlayerCharacter [selection].reloadAttributes (false);
			reloadTextSub ();
		}
		isInitialized = true;
	}

	void Update(){
		GameManager.GM.gamestatus = GameManager.GameStatus.CharacterData;
		if (Screenstatus == Status.SkillTree) {
			MainCharacter MC = GameObject.Find ("PlayerCharList").transform.Find ("MainCharacter").gameObject.GetComponent<MainCharacter> ();
			MC.SkillTree = GameObject.Find ("SkillTree").GetComponent<SkillTree> ();
			MC.SkillTree.MC = MC;
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				SkillPointText.text = "SP: " + MC.SkillTree.saveElements.SkillPoint;
				break;
			case GameManager.Language.TraditionalChinese:
				SkillPointText.text = "SP: " + MC.SkillTree.saveElements.SkillPoint;
				break;
			}
			if(InfoTimer != null && InfoTimer.CheckTime(1.0f)){
				//ShowSkillInfo (SelectedSkill);
				NotShowSkillInfo();
				InfoTimer.CancleTimer();
			}
				
		}
		if(ResetTimer != null && ResetTimer.CheckTime(1.25f)){
			AlertObject.SetActive (false);
			OpenSkillTree();
			ResetTimer.CancleTimer ();
			Destroy (ResetTimer.gameObject);
		}
	}

	void FixedUpdate(){
		//if (timer.isStarted)
		//	LoadingBar.UpdateBar (timer.timerTime, 0.5f);
		//if (timer_return.isStarted)
		//	LoadingBar.UpdateBar (timer_return.timerTime, 0.5f);
		ExpBar.UpdateBar (PlayerCharacter [selection].Exp, PlayerCharacter [selection].ExpRequirment);
		//if (timer.CheckTime (0.5f)) {
		//	timer.CancleTimer ();
		//	LoadingScreen.SetActive (false);
		//}
		if (timer_return.CheckTime (0.5f)) {
			timer_return.CancleTimer ();
			//DontDestroyOnLoadManager.SetNotActive ();
			//SceneManager.LoadScene ("Ships");
			//SceneManager.LoadSceneAsync ("Ships");
			Loader.LoadLevel ("Ships");
		}
		if (selection == 0) {
			PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().reloadAttributes (false);
			reloadTextMain ();
		}
		else {
			PlayerCharacter [selection].reloadAttributes (false);
			reloadTextSub ();
		}
	}

	public void selectionNext(){
		if (selection < PlayerCharacter.Length - 1) {

			if (PlayerCharacter [selection] == PlayerCharacter [0]) {
				PlayerCharacter [selection].gameObject.SetActive (false);
				PlayerCharacter [selection + 1].gameObject.SetActive (true);
			} else {
				PlayerCharacter [selection].gameObject.SetActive (false);
				PlayerCharacter [selection + 1].gameObject.SetActive (true);
			}

		

			selection++;

			NAME.text = PlayerCharacter [selection].charName;
			if (selection == 0) {
				PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().reloadAttributes (false);
				reloadTextMain ();
			}
			else {
				PlayerCharacter [selection].reloadAttributes (false);
				reloadTextSub ();
			}			
		}
	}

	public void selectionPrevious(){
		if (selection > 0) {
			PlayerCharacter [selection].gameObject.SetActive (false);

			selection--;

			PlayerCharacter [selection].gameObject.SetActive (true);
			NAME.text = PlayerCharacter [selection].charName;

			if (selection == 0) {
				//Btn [0].SetActive (false);
				//Btn [1].SetActive (false);
				PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().reloadAttributes (false);
				reloadTextMain ();
			}
			else {
				PlayerCharacter [selection].reloadAttributes (false);
				reloadTextSub ();
			}
		}
	}

	public void reloadTextMain(){
		SkillTreeBtn.SetActive (true);
		PlayerCharacter [selection].GetComponent<MainCharacter> ().SkillTree = GameObject.Find ("SkillTree").GetComponent<SkillTree>();
		NormalCharacterSkillList.NCSL.currentCharacter = PlayerCharacter [selection];
		switch (GameManager.GM.language) {
		case GameManager.Language.English:
			Title.text = "Character";
			CharacterPower.text = "Power: " + PlayerCharacter [selection].CharacterPower;
			//Btn [0].transform.GetChild(0).GetComponent<Text> ().text = "Return to Map";
			//Btn [1].transform.GetChild(0).GetComponent<Text> ().text = "Remove From Group";
			//Btn [2].transform.GetChild(0).GetComponent<Text> ().text = "Start Battle";
			//Btn [3].transform.GetChild(0).GetComponent<Text> ().text = "Return to Map";
			SkillTitleText.text = "Skill Tree";
			SkillTreeBtnText.text = "Skill Tree";
			try{
			NAME.text = PlayerCharacter [selection].charName;
			LV.text = "LV: " + PlayerCharacter [selection].Lv;
			ATK.text = "ATK: " + "<color=black>" + PlayerCharacter [selection].baseAtk + "</color> + " + "<color=green>" + (PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().SkillTree.saveElements.totalATK) + "</color>";
			DEF.text = "DEF: " + "<color=black>" + PlayerCharacter [selection].baseDef + "</color> + " + "<color=green>" + (PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().SkillTree.saveElements.totalDEF) + "</color>";
			//SPEED.text = "Speed: " + "<color=black>" + PlayerCharacter [selection].baseMovspeed + "</color> - " + "<color=green>" + (PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().SkillTree.totalMS) + "</color> s / Movement";
			SPEED.text = "Speed: " + "<color=black>" + PlayerCharacter [selection].movspeed + "</color> s / Movement";
			LOAD.text = "Load: " + "<color=black>" + PlayerCharacter [selection].baseLoad + "</color> + " + "<color=green>" + (PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().SkillTree.saveElements.totalWL) + "</color>";
			CRITRATE.text = "CriticalRate: " + "<color=black>" + PlayerCharacter [selection].baseCriticalRate + "</color> + " + "<color=green>" + (PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().SkillTree.saveElements.totalCR) + "</color>%";
			CRITDAM.text = "CriticalDamage: " + "<color=black>" + PlayerCharacter [selection].baseCriticalDamage + "</color> + " + "<color=green>" + (PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().SkillTree.saveElements.totalCD) + "</color>%";
			HP.text = "HP: " + "<color=black>" + PlayerCharacter [selection].CurrentHp + "</color> / " + "<color=red>" + (PlayerCharacter [selection].Maxhp) + "</color>";
			Energy.text = "Energy: " + "<color=black>" + PlayerCharacter [selection].CurrentEnergy + "</color> / " + "<color=red>" + (PlayerCharacter [selection].MaxEnergy) + "</color>";
				SkillLearnText.text = "Learn";
				SkillResetText.text = "Reset";
			}catch(System.Exception e){

			}
			break;
		case GameManager.Language.TraditionalChinese:
			Title.text = "角色";
			CharacterPower.text = "戰力: " + PlayerCharacter [selection].CharacterPower;
			//Btn [0].transform.GetChild(0).GetComponent<Text> ().text = "返回地圖";
			//Btn [1].transform.GetChild(0).GetComponent<Text> ().text = "移出隊伍";
			//Btn [2].transform.GetChild(0).GetComponent<Text> ().text = "開始戰鬥";
			//Btn [3].transform.GetChild(0).GetComponent<Text> ().text = "返回地圖";
			SkillTitleText.text = "技能樹";
			SkillTreeBtnText.text = "技能樹";
			try{
			NAME.text = PlayerCharacter [selection].charName_CHT;
			LV.text = "LV: " + PlayerCharacter [selection].Lv;
			ATK.text = "攻擊: " + "<color=black>" + PlayerCharacter [selection].baseAtk + "</color> + " + "<color=green>" + (PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().SkillTree.saveElements.totalATK) + "</color>";
			DEF.text = "防禦: " + "<color=black>" + PlayerCharacter [selection].baseDef + "</color> + " + "<color=green>" + (PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().SkillTree.saveElements.totalDEF) + "</color>";
			//SPEED.text = "速度: " + "<color=black>" + PlayerCharacter [selection].baseMovspeed + "</color> - " + "<color=green>" + (PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().SkillTree.totalMS) + "</color> 秒 / 行動";
			SPEED.text = "速度: " + "<color=black>" + PlayerCharacter [selection].movspeed + "</color> 秒 / 行動";
			LOAD.text = "負重: " + "<color=black>" + PlayerCharacter [selection].baseLoad + "</color> + " + "<color=green>" + (PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().SkillTree.saveElements.totalWL) + "</color>";
			CRITRATE.text = "暴擊機率: " + "<color=black>" + PlayerCharacter [selection].baseCriticalRate + "</color> + " + "<color=green>" + (PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().SkillTree.saveElements.totalCR) + "</color>%";
			CRITDAM.text = "暴擊傷害: " + "<color=black>" + PlayerCharacter [selection].baseCriticalDamage + "</color> + " + "<color=green>" + (PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().SkillTree.saveElements.totalCD) + "</color>%";
			HP.text = "生命: " + "<color=black>" + PlayerCharacter [selection].CurrentHp + "</color> / " + "<color=red>" + (PlayerCharacter [selection].Maxhp) + "</color>";
			Energy.text = "能量: " + "<color=black>" + PlayerCharacter [selection].CurrentEnergy + "</color> / " + "<color=red>" + (PlayerCharacter [selection].MaxEnergy) + "</color>";
				SkillLearnText.text = "學習";
				SkillResetText.text = "重置";
			}catch(System.Exception e){

			}
			break;
		}
	}

	public void reloadTextSub(){
		SkillTreeBtn.SetActive (false);
		NormalCharacterSkillList.NCSL.currentCharacter = PlayerCharacter [selection];
		switch (GameManager.GM.language) {
		case GameManager.Language.English:
			Title.text = "Character";
			CharacterPower.text = "Power: " + PlayerCharacter [selection].CharacterPower;
			//Btn [0].transform.GetChild(0).GetComponent<Text> ().text = "Return to Map";
			//Btn [1].transform.GetChild(0).GetComponent<Text> ().text = "Remove From Group";
			//Btn [2].transform.GetChild(0).GetComponent<Text> ().text = "Start Battle";
			//Btn [3].transform.GetChild(0).GetComponent<Text> ().text = "Return to Map";
			NAME.text = PlayerCharacter [selection].charName;
			LV.text = "LV: " + PlayerCharacter [selection].Lv;
			ATK.text = "ATK: " + "<color=black>" +  PlayerCharacter [selection].baseAtk +  "</color> + " + "<color=green>"  + (PlayerCharacter [selection].totalgrowATK) + "</color>" ;
			DEF.text = "DEF: " + "<color=black>" +  PlayerCharacter [selection].baseDef +  "</color> + " + "<color=green>"  + (PlayerCharacter [selection].totalgrowDEF) + "</color>" ;
			SPEED.text = "Speed: " + "<color=black>" +  PlayerCharacter [selection].movspeed + "</color> s / Movement";
			LOAD.text = "Load: " + "<color=black>" +  PlayerCharacter [selection].baseLoad +  "</color> + " + "<color=green>"  + (PlayerCharacter [selection].totalgrowLOAD) + "</color>" ;
			CRITRATE.text = "CriticalRate: " + "<color=black>" + PlayerCharacter [selection].cR + "</color>%";
			CRITDAM.text = "CriticalDamage: " + "<color=black>" + PlayerCharacter [selection].cD + "</color>%";
			HP.text = "HP: " + "<color=black>" + PlayerCharacter [selection].CurrentHp + "</color> / " + "<color=red>"  + (PlayerCharacter [selection].Maxhp) + "</color>" ;
			Energy.text = "Energy: " + "<color=black>" + PlayerCharacter [selection].CurrentEnergy + "</color> / " + "<color=red>"  + (PlayerCharacter [selection].MaxEnergy) + "</color>" ;	
			SkillLearnText.text = "Learn";
			SkillResetText.text = "Reset";
			break;
		case GameManager.Language.TraditionalChinese:
			Title.text = "角色";
			CharacterPower.text = "戰力: " + PlayerCharacter [selection].CharacterPower;
			//Btn [0].transform.GetChild(0).GetComponent<Text> ().text = "返回地圖";
			//Btn [1].transform.GetChild(0).GetComponent<Text> ().text = "移出隊伍";
			//Btn [2].transform.GetChild(0).GetComponent<Text> ().text = "開始戰鬥";
			//Btn [3].transform.GetChild(0).GetComponent<Text> ().text = "返回地圖";
			NAME.text = PlayerCharacter [selection].charName_CHT;
			LV.text = "LV: " + PlayerCharacter [selection].Lv;
			ATK.text = "攻擊: " + "<color=black>" +  PlayerCharacter [selection].baseAtk +  "</color> + " + "<color=green>"  + (PlayerCharacter [selection].totalgrowATK) + "</color>" ;
			DEF.text = "防禦: " + "<color=black>" +  PlayerCharacter [selection].baseDef +  "</color> + " + "<color=green>"  + (PlayerCharacter [selection].totalgrowDEF) + "</color>" ;
			SPEED.text = "速度: " + "<color=black>" +  PlayerCharacter [selection].movspeed + "</color> 秒 / 行動";
			LOAD.text = "負重: " + "<color=black>" +  PlayerCharacter [selection].baseLoad +  "</color> + " + "<color=green>"  + (PlayerCharacter [selection].totalgrowLOAD) + "</color>" ;
			CRITRATE.text = "暴擊機率: " + "<color=black>" + PlayerCharacter [selection].cR + "</color>%";
			CRITDAM.text = "暴擊傷害: " + "<color=black>" + PlayerCharacter [selection].cD + "</color>%";
			HP.text = "生命: " + "<color=black>" + PlayerCharacter [selection].CurrentHp + "</color> / " + "<color=red>"  + (PlayerCharacter [selection].Maxhp) + "</color>" ;
			Energy.text = "能量: " + "<color=black>" + PlayerCharacter [selection].CurrentEnergy + "</color> / " + "<color=red>"  + (PlayerCharacter [selection].MaxEnergy) + "</color>" ;	
			SkillLearnText.text = "學習";
			SkillResetText.text = "重置";
			break;
		}
	}
		
	public void ReturnToShip(){
		//LoadingScreen.SetActive (true);
		timer_return.InitializeStart ();
	}

	public void OpenSkillTree(){
		InfoScreen.SetActive (false);
		Screenstatus = Status.SkillTree;
		SkillTreeScreen.SetActive (true);
		SkillArea = new GameObject[GameObject.Find ("SkillTree").transform.childCount];
		int SkillCount = 0;
		SelectedSkill = 0;

		SkillAreaBGImage = new Image[SkillArea.Length];
		int countLoadedImage = 0;
		MainCharacter MC = GameObject.Find ("PlayerCharList").transform.Find ("MainCharacter").gameObject.GetComponent<MainCharacter> ();
		SkillTree CurrentSkillTree = MC.SkillTree;

		for (int x = 0; x < SkillArea.Length; x++) {
			SkillArea [x] = GameObject.Find ("SkillTree").transform.Find ("SkillGroup" + x).gameObject;
			SkillAreaBGImage [x] = this.gameObject.transform.Find ("SkillTree").Find ("SkillTreeAttacker").Find ("SkillArea" + x).Find("SkillComponents").gameObject.GetComponent<Image>();
			SkillCount += SkillArea [x].GetComponent<SkillList>().skillitem.Length;
		}
		SkillImage = new Image[SkillCount];

		for (int x = 0; x < SkillArea.Length; x++) {
			try{
				var tempImage = SkillAreaBGImage[x].GetComponent<Image>();
				var tempColor = tempImage.color;
				if(SkillArea[x].GetComponent<SkillList>().isUnlocked && !SkillArea[x].GetComponent<SkillList>().areaLearned){
					tempColor.a = 0.5f;
					tempColor = Color.green;
				}
				else if(SkillArea[x].GetComponent<SkillList>().areaLearned && SkillArea[x].GetComponent<SkillList>().isUnlocked){
					tempColor.a = 0.5f;
					tempColor = Color.blue;
				}
				else{
					tempColor.a = 0.5f;
					tempColor = Color.red;
				}
				SkillAreaBGImage[x].GetComponent<Image>().color = tempColor;
				Debug.Log ("" + x);
			}catch(System.Exception e){
				Debug.Log ("Get Object Fail.");
			}
			try{
				for (int i = 0; i < SkillArea[x].transform.GetComponent<SkillList>().skillitem.Length; i++) {
					Skill LearnSkill = CurrentSkillTree.ReturnSkill (countLoadedImage);
					Debug.Log(""+LearnSkill);
					SkillImage [countLoadedImage] = SkillAreaBGImage[x].transform.Find ("Skill" + i).GetComponent<Image>();
					SkillImage [countLoadedImage].sprite = Resources.Load<Sprite>(LearnSkill.skillImagePath);
					Debug.Log("" + SkillImage [countLoadedImage].sprite);
					if (LearnSkill.isLearned) {
						SkillImage [countLoadedImage].color = Color.white;
					}
					countLoadedImage++;
					Debug.Log ("" + i + "Count: " + countLoadedImage);
				}
			}catch(System.Exception e){
				Debug.Log ("Get Object Fail.");
			}

		}
	}

	public void CloseSkillTree(){
		Screenstatus = Status.DataView;
		if(InfoisShow)
			NotShowSkillInfo ();
		PrevirousTrigger = null;
		Trigger = null;
		SkillTreeScreen.SetActive (false);
	}

	public void ShowSkillInfo(int SkillIndex){

		if (PrevirousTrigger == null) {
			PrevirousTrigger = Trigger;
		}

		if (InfoisShow && PrevirousTrigger != Trigger) {
			
			//NotShowSkillInfo ();
			Color tempcolor = PrevirousTrigger.transform.GetChild (0).GetComponent<Image> ().color;
			tempcolor.a = 0f;
			PrevirousTrigger.transform.GetChild (0).GetComponent<Image> ().color = tempcolor;
			InfoisShow = false;
			PrevirousTrigger = Trigger;
		}

		if (!InfoisShow) {
			Color tempcolor = Trigger.transform.GetChild (0).GetComponent<Image> ().color;
			tempcolor.a = 0.65f;
			Trigger.transform.GetChild (0).GetComponent<Image> ().color = tempcolor;
			InfoisShow = true;
			PrevirousMousePosition = Input.mousePosition;
		
			InfoScreen.SetActive (true);
			MainCharacter MC = GameObject.Find ("PlayerCharList").transform.Find ("MainCharacter").gameObject.GetComponent<MainCharacter> ();
			Skill CurrentSkill = MC.SkillTree.ReturnSkill (SkillIndex);
			SelectedSkill = SkillIndex;
			Debug.Log(CurrentSkill.PreviousSkill);
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				if (CurrentSkill.PreviousSkill != null)
					InfoText.text = "\n<color=red>" + CurrentSkill.skillName + "</color>\t\tSP Need:" + CurrentSkill.neededSP + "\n" + CurrentSkill.skillDesc + "\n<color=blue>Pre-Skill: " + CurrentSkill.PreviousSkill.skillName + "</color>\n";
				else if (CurrentSkill.PreviousSkill == null)
					InfoText.text = "\n<color=red>" + CurrentSkill.skillName + "</color>\t\tSP Need:" + CurrentSkill.neededSP + "\n" + CurrentSkill.skillDesc + "\n";
				break;
			case GameManager.Language.TraditionalChinese:
				if (CurrentSkill.isHasPreSkill ())
					InfoText.text = "\n<color=red>" + CurrentSkill.skillName_CHT + "</color>\t\tt所需技能點:" + CurrentSkill.neededSP + "\n" + CurrentSkill.skillDesc_CHT + "\n<color=blue>前置技能: " + CurrentSkill.PreviousSkill.skillName_CHT + "</color>\n";
				else if (!CurrentSkill.isHasPreSkill ())
					InfoText.text = "\n<color=red>" + CurrentSkill.skillName_CHT + "</color>\t\tt所需技能點:" + CurrentSkill.neededSP + "\n" + CurrentSkill.skillDesc_CHT + "\n";
				break;
			}
		}
	}

	public void NotShowSkillInfo(){
		Color tempcolor = Trigger.transform.GetChild (0).GetComponent<Image> ().color;
		tempcolor.a = 0f;
		Trigger.transform.GetChild (0).GetComponent<Image> ().color = tempcolor;
		InfoScreen.SetActive (false);
		InfoisShow = false;
		PrevirousTrigger = null;
	}

	public void SetToTrigger(GameObject trigger){
		Trigger = trigger;
	}

	public void CallLearnSkill(){
		MainCharacter MC = GameObject.Find ("PlayerCharList").transform.Find ("MainCharacter").gameObject.GetComponent<MainCharacter> ();
		MC.AddExp (500);
		SkillTree CurrentSkillTree = MC.SkillTree;
		Skill LearnSkill = CurrentSkillTree.ReturnSkill (SelectedSkill);
		GameObject TempTimerInfo = new GameObject ();
		InfoTimer = TempTimerInfo.AddComponent<Timer> ();
		InfoTimer.InitializeStart ();
		Destroy (TempTimerInfo, 1.15f);
		if (!LearnSkill.skillAreaGroup.isUnlocked) {
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				InfoText.text = "The Skill of this Area are not unlocked";
				break;
			case GameManager.Language.TraditionalChinese:
				InfoText.text = "此區域的技能尚未解鎖.";
				break;
			}
			return;
		} else if (LearnSkill.isLearned) {
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				InfoText.text = "You are learned this Skill";
				break;
			case GameManager.Language.TraditionalChinese:
				InfoText.text = "你已經學習本技能.";
				break;
			}
			return;
		} else if (LearnSkill.skillAreaGroup.areaLearned && !LearnSkill.isLearned) {
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				InfoText.text = "You are learned another skill in this area.";
				break;
			case GameManager.Language.TraditionalChinese:
				InfoText.text = "你已於此層級中學習了其他技能";
				break;
			}
			return;
		} else if (CurrentSkillTree.saveElements.SkillPoint < LearnSkill.neededSP) {
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				InfoText.text = "You are not enough Skill Point";
				break;
			case GameManager.Language.TraditionalChinese:
				InfoText.text = "你沒有足夠的技能點";
				break;
			}
			Debug.Log ("You are not enough Skill Point.");
			return;
		} else if (LearnSkill.PreviousSkill != null && !LearnSkill.PreviousSkill.isLearned) {
				switch (GameManager.GM.language) {
				case GameManager.Language.English:
					InfoText.text = "Pre-skill have not been learned";
					break;
				case GameManager.Language.TraditionalChinese:
					InfoText.text = "前置技能尚未學習";
					break;
				}
				Debug.Log ("Pre-skill have not been learned.");
				return;
			}
			else {
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				InfoText.text = "Learn successfully";
				break;
			case GameManager.Language.TraditionalChinese:
				InfoText.text = "學習成功";
				break;
			}
			CurrentSkillTree.LearnSkill (SelectedSkill);
			var tempImage = Trigger.transform.parent.GetComponent<Image> ();
			Debug.Log (tempImage);
			var tempColor = tempImage.color;
			if(CurrentSkillTree.CurrentTreeElement[SelectedSkill].skillAreaGroup.isUnlocked && !CurrentSkillTree.CurrentTreeElement[SelectedSkill].skillAreaGroup.areaLearned){
				tempColor.a = 0.5f;
				tempColor = Color.green;
				Debug.Log ("Color Change To Green");
			}
			else if(CurrentSkillTree.CurrentTreeElement[SelectedSkill].skillAreaGroup.areaLearned && CurrentSkillTree.CurrentTreeElement[SelectedSkill].skillAreaGroup.isUnlocked){
				tempColor.a = 0.5f;
				tempColor = Color.blue;
				Debug.Log ("Color Change To Blue");
			}
			else{
				tempColor.a = 0.5f;
				tempColor = Color.red;
				Debug.Log ("Color Change To Red");
			}
			tempImage.color = tempColor;
			Trigger.GetComponent<Image> ().color = Color.white;
			}
		}

	public void ResetSkillTree(){
		GameObject TempTimer = new GameObject();
		ResetTimer = TempTimer.AddComponent<Timer> ();
		MainCharacter MC = GameObject.Find ("PlayerCharList").transform.Find ("MainCharacter").gameObject.GetComponent<MainCharacter> ();
		MC.SkillTree.SkillTreeReset = true;
		MC.SkillTree.isInitialized = true;
		MC.SkillTree.InitializeSkillTree ();
		MC.SkillTree = GameObject.Find ("SkillTree").GetComponent<SkillTree> ();
		for (int i = 0; i < SkillImage.Length; i++) {
			SkillImage [i].color = new Color (77f / 255f, 77f / 255f, 77f / 255f);
		}
		MC.SkillTree.GetSkillPoint (MC.Lv - 1);
		CloseSkillTree ();
		AlertObject.SetActive (true);
		switch (GameManager.GM.language) {
		case GameManager.Language.English:
			AlertText.text = "Reset Skill Successful";
			break;
		case GameManager.Language.TraditionalChinese:
			AlertText.text = "重置技能成功";
			break;
		}
		ResetTimer.InitializeStart ();
		//OpenSkillTree ();

	}

	}

