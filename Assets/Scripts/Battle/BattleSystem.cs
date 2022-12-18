using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BayatGames.SaveGamePro;

public class BattleSystem : MonoBehaviour {

	public enum Turn{
		Player,
		Enemy
	}

	public enum Status{
		Running,
		WaitingResponse,
		Results
	}

	public enum UIStatus{
		AttackMenu,
		MainMenu,
		EscapeMenu,
		RewardMenu
	}
	public LevelLoader Loader;
	public EnemyGroup EnemyGroupClass; 
	public GameObject[] EnemyGroupObject;
	public PlayerGroup PlayerGroupClass;
	public GameObject[] PlayerGroupObject;
	public Status status;
	public Turn turn;
	public GameObject CurrentCharacter;

	[Header("UI Setting")]
	public Text[] CharacterName;
	public Text[] EnemyName;
	public SimpleHealthBar[] CharacterProgressBar;
	public SimpleHealthBar[] healthBar;
	public SimpleHealthBar[] energyBar;
	public Text FleeAlertScreenText;
	public Text ResultsScreenTitleText;
	public Text[] FleeAlertBtnText;
	public Text[] ResultsScreenCharName;
	public Text[] ResultsScreenCharLv;
	public Text[] ResultsScreenMaterialText;
	public SimpleHealthBar[] ResultsScreenExpBar;
	public GameObject SkillInfoScreen;
	public Text SkillInfoText;
	public Text TurnLeftText;
	public GameObject speedChangeBtn;
	public float battleSpeed = 1.0f;

	[Header("Enemy UI Setting")]
	public Text EnemyStatusName;
	public SimpleHealthBar EnemyHPBar;

	[Header("UI Controll")]
	public GameObject[] PlayerPanel;
	public GameObject skillbtn;
	public GameObject fleebtn;
	public GameObject backbtn;
	public GameObject[] TargetBtn;
	public GameObject EnemyStatus;
	public GameObject LoadingScreen;
	public GameObject FleeAlertScreen;
	public GameObject ResultsScreen;
	public GameObject[] FleeAlertScreenBtn;


	[Header("Position Setting")]
	public GameObject[] PlayerPositionBox;
	[HideInInspector]public Vector3[] PlayerPosition;
	public GameObject[] EnemyPositionBox;
	[HideInInspector]public Vector3[] EnemyPosition;

	public int TurnLeft;


	public int PlayercountNum = 0;
	public int EnemycountNum = 0;

	UIStatus uiStatus = UIStatus.MainMenu;
	Timer timer_Escape, timer_Result;
	bool canEscape = false;


	public void Awake(){
		PlayerPosition = new Vector3[PlayerPositionBox.Length];
		EnemyPosition = new Vector3[EnemyPositionBox.Length];
		for (int i = 0; i < PlayerPositionBox.Length; i++) {
			PlayerPosition [i] = PlayerPositionBox [i].transform.position;
		}

		for (int i = 0; i < EnemyPositionBox.Length; i++) {
			EnemyPosition [i] = EnemyPositionBox [i].transform.position;
		}
		GameManager.GM.gamestatus = GameManager.GameStatus.Battle;
		fleebtn.SetActive (true);
		skillbtn.SetActive (true);
		speedChangeBtn.transform.GetChild(0).GetComponent<Text>().text = "1.0x";
		Time.timeScale = 1.0f;
		battleSpeed = Time.timeScale;
	}

	public void Start(){
		//LoadingCharacter ();
		LoadingScreen.SetActive (false);
		GameElement.GE.BattlePlace = this.gameObject;
		for (int i = 0; i < GameObject.Find ("PlayerCharList").transform.childCount - 1; i++) {
			GameObject.Find ("PlayerCharList").transform.GetChild (i).gameObject.SetActive (false);
		}
		GameManager.GM.gamestatus = GameManager.GameStatus.Battle;
		GameObject temptimer = new GameObject();
		GameObject temptimer1 = new GameObject();
		timer_Escape = temptimer.AddComponent<Timer> ();
		timer_Result = temptimer1.AddComponent<Timer> ();
		GameManager.GM.battleOutcome = GameManager.BattleOutCome.NotReady;
		TurnLeft = GameObject.Find ("EnemyGroupList").transform.Find ("Chapter" + GameManager.GM.Chapter).Find ("Group" + GameManager.GM.Stage).GetComponent<EnemyGroup> ().TurnLimit;
		GameElement.GE.BattlePlace.GetComponent<BattleSystem> ().InitializeLoading ();
	
	}

	public void FixedUpdate(){
		if (status == Status.Results) {
			skillbtn.SetActive (false);
			fleebtn.SetActive (false);
			FleeAlertScreen.SetActive (false);
			SkillInfoScreen.SetActive (false);
			for (int i = 0; i < TargetBtn.Length; i++) {
				TargetBtn [i].SetActive (false);
			}
		}
		LoadingCharacter ();

		ChangeLanguage ();

		PlayercountNum = 0;
		EnemycountNum = 0;


		//Update Results Screen after Finish the battle.
		UpdateTheResults();

		//Check Enemy's and Player's Character status, if all Player Characters are dead, return Lose Results
		//otherwise, return Win Results.
		CheckCharacterStatus();


		// Return To Ship After The Result Screen showing on screen in x sec 
		ResultScreenShowEnd();

		// Return To Preparation Scene After Escape in x sec
		EscapeScreenShowEnd();

		// Update Enemy's Character and Player's Character Status Bar.
		UpdateStatusBar ();
	}

	void Update(){
		GameManager.GM.gamestatus = GameManager.GameStatus.Battle;
	}


	public void LoadingCharacter(){
		//GameElement.GE.BattlePlace.transform.GetChild(0).gameObject.SetActive (true);
		PlayerGroupClass = GameObject.Find ("PlayerGroup").GetComponent<PlayerGroup> ();
		EnemyGroupClass = GameObject.Find ("EnemyGroup").GetComponent<EnemyGroup> ();

		EnemyGroupObject = new GameObject[EnemyGroupClass.TotalCharacter];

		for (int i = 0; i < EnemyGroupClass.TotalCharacter; i++) {
			if (EnemyGroupClass.EnemyCharacter [i] != null) {
				//Debug.Log ("Character Loading :" + i + " " + EnemyGroupClass.EnemyCharacter [i]);
				EnemyGroupObject [i] = EnemyGroupClass.EnemyCharacter [i].gameObject;
				EnemyName [i].text = EnemyGroupClass.EnemyCharacter [i].charName;
				EnemyGroupObject [i].GetComponent<Attack> ().bS = this;
			}
		}

		//Debug.Log (PlayerGroupClass.TotalCharacter);
		PlayerGroupObject = new GameObject[PlayerGroupClass.TotalCharacter];

		for (int i = 1; i < PlayerGroupClass.TotalCharacter; i++) {
			if (PlayerGroupClass.SupCharacter [i - 1] != null) {
				PlayerGroupObject [i] = PlayerGroupClass.SupCharacter [i - 1].gameObject;
				CharacterName [i].text = PlayerGroupClass.SupCharacter [i - 1].charName;
				PlayerPanel [i].SetActive (true); 
				PlayerGroupObject [i].GetComponent<Attack> ().bS = this;
			}else
				PlayerPanel [i].SetActive (false); 
		}

		if (PlayerGroupClass.MainCharacter != null) {
			PlayerGroupObject [0] = PlayerGroupClass.MainCharacter.gameObject;
			CharacterName [0].text = PlayerGroupClass.MainCharacter.charName;
			PlayerPanel [0].SetActive (true); 
			PlayerGroupObject [0].GetComponent<Attack> ().bS = this;
		}else
			PlayerPanel [0].SetActive (false); 


	}

	private void ChangeLanguage(){
		try{
		switch (GameManager.GM.language) {
		case GameManager.Language.English:
			//skillbtn.transform.GetChild (0).GetComponent<Text> ().text = "Skill";
			fleebtn.transform.GetChild (0).GetComponent<Text> ().text = "Escape";
			backbtn.transform.GetChild (0).GetComponent<Text> ().text = "Back";
			TurnLeftText.text = "TurnLeft: " + TurnLeft;
			if (uiStatus != UIStatus.EscapeMenu) {
				FleeAlertScreenText.text = 
					"Do you really want to escape?\nEscape may fail, Failure will lose the current round of action opportunities;\nSuccessful escape will be judged as a battle failure and cannot get any EXP, \nand<color=red> deduct all characters in a group of 20% of their current Hp </color>.";
			}
			FleeAlertBtnText[0].text = "Confirm";
			FleeAlertBtnText[1].text = "No";

			for (int i = 0; i < EnemyGroupObject.Length; i++) {
				EnemyName[i].text = EnemyGroupObject[i].GetComponent<Character>().charName;
			}
			for (int i = 0; i < PlayerGroupObject.Length; i++) {
				CharacterName [i].text = PlayerGroupObject[i].GetComponent<Character>().charName;
				ResultsScreenCharName [i].text = PlayerGroupObject [i].GetComponent<Character> ().charName;
			}

			break;
		case GameManager.Language.TraditionalChinese:
			//skillbtn.transform.GetChild(0).GetComponent<Text> ().text = "技能";
			fleebtn.transform.GetChild(0).GetComponent<Text> ().text = "逃跑";
			backbtn.transform.GetChild(0).GetComponent<Text> ().text = "返回";
			TurnLeftText.text = "剩餘回合: " + TurnLeft;
			if (uiStatus != UIStatus.EscapeMenu) {
				FleeAlertScreenText.text = 
					"你確定要逃跑嗎？\n逃跑有可能失敗，失敗將失去當前回合行動機會；\n成功逃跑戰鬥將判定為失敗，並無法取得任何經驗值，\n<color=red>同時所有上陣角色將會失去 20% 當前生命</color>";
			}
			FleeAlertBtnText[0].text = "確定";
			FleeAlertBtnText[1].text = "不逃跑";
			for (int i = 0; i < EnemyGroupObject.Length; i++) {
				EnemyName[i].text = EnemyGroupObject[i].GetComponent<Character>().charName_CHT;
			}
			for (int i = 0; i < PlayerGroupObject.Length; i++) {
				CharacterName [i].text = PlayerGroupObject[i].GetComponent<Character>().charName_CHT;
				ResultsScreenCharName [i].text = PlayerGroupObject [i].GetComponent<Character> ().charName_CHT;
			}
			break;

		}
		}
		catch(System.Exception e){

		}
	}

	public void FleeAlert(){
		FleeAlertScreen.SetActive (true);
		skillbtn.SetActive (false);
		fleebtn.SetActive (false);
		FleeAlertScreenBtn [0].SetActive (true);
		FleeAlertScreenBtn [1].SetActive (true);
		uiStatus = UIStatus.EscapeMenu;
	}

	public void NotFlee(){
		FleeAlertScreen.SetActive (false);
		skillbtn.SetActive (true);
		fleebtn.SetActive (true);
		uiStatus = UIStatus.MainMenu;
	}

	public void flee(){
		Character Attacker = new Character();
		float TempRandomNum = Random.Range (0, 99);
		for (int i = 0; i < GameObject.Find ("PlayerGroup").gameObject.transform.childCount; i++) {
			if (GameObject.Find ("PlayerGroup").gameObject.transform.GetChild (i).GetComponent<Attack> ().canAttack) {
				Attacker = GameObject.Find ("PlayerGroup").gameObject.transform.GetChild (i).GetComponent<Character> ();
				break;
			}
		}
		if (Attacker.movspeed /100 < TempRandomNum / 100) {
			Debug.Log ("RandomNum: " + TempRandomNum / 100 + ", You Can Run away!");
			canEscape = true;
			FleeAlertScreenBtn [0].SetActive (false);
			FleeAlertScreenBtn [1].SetActive (false);
			timer_Escape.InitializeStart ();
			switch (GameManager.GM.language) {
			case GameManager.Language.English:				
				FleeAlertScreenText.text = "Escaped Successfully!";
				break;
			case GameManager.Language.TraditionalChinese:
				FleeAlertScreenText.text = "逃跑成功！";
				break;
			}
		} else {
			Debug.Log ("RandomNum: " + TempRandomNum / 100 + "You Cannot Run away!");
			canEscape = false;
			Attacker.GetComponent<Attack> ().canMove = false;
			Attacker.GetComponent<Attack> ().canAttack = false;
			Attacker.GetComponent<Attack> ().MovementSpeed = 0f;
			FleeAlertScreenBtn [0].SetActive (false);
			FleeAlertScreenBtn [1].SetActive (false);
			timer_Escape.InitializeStart ();
			switch (GameManager.GM.language) {
			case GameManager.Language.English:				
				FleeAlertScreenText.text = "Escape Failure!";
				break;
			case GameManager.Language.TraditionalChinese:
				FleeAlertScreenText.text = "逃跑失敗！";
				break;
			}
		}
	}

	public void AttackTargetMenu(){
		skillbtn.SetActive (false);
		//fleebtn.SetActive (false);
		for (int i = 0; i < EnemyGroupClass.TotalCharacter; i++) {
			if (EnemyGroupClass.EnemyCharacter [i].isDead != true) {
				TargetBtn [i].SetActive (true);
			}
		}
		backbtn.SetActive (true);
		uiStatus = UIStatus.AttackMenu;
	}

	public void BackToMain(){
		skillbtn.SetActive (true);
		//if(PlayerGroupObject [0].GetComponent<Attack> ().canAttack == true)
			//fleebtn.SetActive (true);
		backbtn.SetActive (false);
		for (int i = 0; i < EnemyGroupClass.TotalCharacter; i++) {
			if (EnemyGroupClass.EnemyCharacter [i] != null) {
				TargetBtn [i].SetActive (false);
			}
		}
		uiStatus = UIStatus.MainMenu;
	}

	public void InitializeLoading(){
        GameManager.GM.battleOutcome = GameManager.BattleOutCome.NotReady;
		PlayerGroupClass = GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("PlayerGroup").GetComponent<PlayerGroup> ();

		EnemyGroupClass = GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("EnemyGroup").GetComponent<EnemyGroup> ();
		Debug.Log ("Now Stage: " + GameManager.GM.Stage);
		Debug.Log (GameObject.Find ("EnemyGroupList"));
		Debug.Log (GameObject.Find ("EnemyGroupList").transform.Find("Chapter" + GameManager.GM.Chapter).transform.Find ("Group" + GameManager.GM.Stage));
		Debug.Log (GameObject.Find ("EnemyGroupList").transform.Find("Chapter" + GameManager.GM.Chapter).transform.Find ("Group" + GameManager.GM.Stage).GetComponent<EnemyGroup> ().TotalCharacter);
		Debug.Log (GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("PlayerGroup"));

		do{
			GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").GetChild (0).gameObject.SetActive(true);
			GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").GetChild (0).SetParent (PlayerGroupClass.gameObject.transform);
			Debug.Log (" Move Successfully!");
		}
		while
			(GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform.childCount >= 1);

		if (GameObject.Find ("PlayerGroup").transform.childCount >= 1) {

			GameObject.Find ("PlayerGroup").transform.GetChild (0).position = PlayerPosition[0];
			GameObject.Find ("PlayerGroup").transform.GetChild (0).eulerAngles = 
				new Vector3 (GameObject.Find ("PlayerGroup").transform.GetChild (0).eulerAngles.x,
					GameObject.Find ("PlayerGroup").transform.GetChild (0).eulerAngles.y + 140,
					GameObject.Find ("PlayerGroup").transform.GetChild (0).eulerAngles.z);
			//GameObject.Find ("PlayerGroup").transform.GetChild (0).transform.localScale = BattlePrepare.BP.MainCharBasicScale;
			GameObject.Find ("PlayerGroup").transform.GetChild (0).GetComponent<Attack>().enabled = true;
			Debug.Log ("Point Set Finish.");
		}
		if (GameObject.Find ("PlayerGroup").transform.childCount >= 2) {
			GameObject.Find ("PlayerGroup").transform.GetChild (1).position = PlayerPosition [1];
			GameObject.Find ("PlayerGroup").transform.GetChild (1).eulerAngles = 
				new Vector3 (GameObject.Find ("PlayerGroup").transform.GetChild (1).eulerAngles.x,
					GameObject.Find ("PlayerGroup").transform.GetChild (1).eulerAngles.y + 140,
					GameObject.Find ("PlayerGroup").transform.GetChild (1).eulerAngles.z);
			GameObject.Find ("PlayerGroup").transform.GetChild (1).GetComponent<Attack>().enabled = true;
		}
		if (GameObject.Find ("PlayerGroup").transform.childCount >= 3) {
			GameObject.Find ("PlayerGroup").transform.GetChild (2).position = PlayerPosition [2];
			GameObject.Find ("PlayerGroup").transform.GetChild (2).eulerAngles = 
				new Vector3 (GameObject.Find ("PlayerGroup").transform.GetChild (2).eulerAngles.x,
					GameObject.Find ("PlayerGroup").transform.GetChild (2).eulerAngles.y + 140,
					GameObject.Find ("PlayerGroup").transform.GetChild (2).eulerAngles.z);
			GameObject.Find ("PlayerGroup").transform.GetChild (2).GetComponent<Attack>().enabled = true;
		}
		if (GameObject.Find ("PlayerGroup").transform.childCount >= 4) {
			GameObject.Find ("PlayerGroup").transform.GetChild (3).position = PlayerPosition [3];
			GameObject.Find ("PlayerGroup").transform.GetChild (3).eulerAngles = 
				new Vector3 (GameObject.Find ("PlayerGroup").transform.GetChild (0).eulerAngles.x,
					GameObject.Find ("PlayerGroup").transform.GetChild (3).eulerAngles.y + 140,
					GameObject.Find ("PlayerGroup").transform.GetChild (3).eulerAngles.z);
			GameObject.Find ("PlayerGroup").transform.GetChild (3).GetComponent<Attack>().enabled = true;
		}

		do{
			GameObject.Find ("EnemyGroupList").transform.Find("Chapter" + GameManager.GM.Chapter).transform.Find ("Group" + GameManager.GM.Stage).GetChild (0).gameObject.SetActive(true);
			GameObject.Find ("EnemyGroupList").transform.Find("Chapter" + GameManager.GM.Chapter).transform.Find ("Group" + GameManager.GM.Stage).GetChild (0).SetParent (EnemyGroupClass.gameObject.transform);
			Debug.Log (" Move Successfully!");
		}
		while
			(GameObject.Find ("EnemyGroupList").transform.Find("Chapter" + GameManager.GM.Chapter).transform.Find ("Group" + GameManager.GM.Stage).transform.childCount >= 1);


		if (GameObject.Find ("EnemyGroup").transform.childCount >= 1) {
			GameObject.Find ("EnemyGroup").transform.GetChild (0).position = EnemyPosition [0];
			GameObject.Find ("EnemyGroup").transform.GetChild (0).GetComponent<Attack>().enabled = true;
		}
		if (GameObject.Find ("EnemyGroup").transform.childCount >= 2) {
			GameObject.Find ("EnemyGroup").transform.GetChild (1).position = EnemyPosition [1];
			GameObject.Find ("EnemyGroup").transform.GetChild (1).GetComponent<Attack>().enabled = true;
		}
		if (GameObject.Find ("EnemyGroup").transform.childCount >= 3) {
			GameObject.Find ("EnemyGroup").transform.GetChild (2).position = EnemyPosition [2];
			GameObject.Find ("EnemyGroup").transform.GetChild (2).GetComponent<Attack>().enabled = true;
		}
		if (GameObject.Find ("EnemyGroup").transform.childCount >= 4) {
			GameObject.Find ("EnemyGroup").transform.GetChild (3).position = EnemyPosition [3];
			GameObject.Find ("EnemyGroup").transform.GetChild (3).GetComponent<Attack>().enabled = true;

		}

		Debug.Log("Add To BattlePlace Successfully!");
		PlayerGroupClass.Reload ();
		EnemyGroupClass.Reload ();
	}

	public void ReturnCharToList(){
		PlayerGroupClass = GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").GetComponent<PlayerGroup> ();

		EnemyGroupClass = GameObject.Find("EnemyGroupList").transform.Find("Chapter" + GameManager.GM.Chapter).transform.Find ("Group" + GameManager.GM.Stage).GetComponent<EnemyGroup> ();
		do{
			if(GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("PlayerGroup").GetChild(0).GetComponent<Character>().isDead){

				GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("PlayerGroup").GetChild(0).GetComponent<Character>().isDead = false;
				GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("PlayerGroup").GetChild(0).GetComponent<Character>().CurrentHp = 1;
			}
			GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("PlayerGroup").GetChild(0).GetComponent<Character>().isInGroup = false;
			GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find ("PlayerGroup").GetChild (0).GetComponent<Attack>().enabled = false;
			GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find ("PlayerGroup").GetChild (0).eulerAngles = 
				new Vector3 (GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find ("PlayerGroup").GetChild (0).eulerAngles.x,
					GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find ("PlayerGroup").GetChild (0).eulerAngles.y - 140,
					GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find ("PlayerGroup").GetChild (0).eulerAngles.z);
			GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("PlayerGroup").GetChild(0).GetComponent<Character>().NCSL = null;
			if(GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("PlayerGroup").GetChild (0).name == "MainCharacter"){
				GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("PlayerGroup").GetChild (0).transform.localScale = GameManager.GM.MainCharBasicScale;
			}else{
				GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("PlayerGroup").GetChild (0).transform.localScale = GameManager.GM.SupCharBasicScale;
			}
            GameObject.Find("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("PlayerGroup").GetChild(0).GetComponent<Character>().reloadAttributes(true);

            GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find ("PlayerGroup").GetChild (0).SetParent (GameObject.Find("PlayerCharList").transform);

			Debug.Log (" Move Successfully!");
		}
		while
			(GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find ("PlayerGroup").transform.childCount >= 1);

		//BattlePrepare.BP.selection = 0;
		//BattlePrepare.BP.reloadTextMain();

		if (GameObject.Find ("PlayerCharList").transform.Find("NowGroup").childCount >= 1) {
			GameObject.Find ("PlayerCharList").transform.Find("NowGroup").transform.GetChild (0).transform.position = BattlePrepare.BP.CharacterPosition[0].transform.position;
			//GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform.GetChild (0).gameObject.SetActive (true);
			//GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform.GetChild(0).GetComponent<MainCharacter> ().reloadAttributes (true);
			Debug.Log ("Point Set Finish.");
		}
		if (GameObject.Find ("PlayerCharList").transform.Find("NowGroup").childCount >= 2) {
			GameObject.Find ("PlayerCharList").transform.Find("NowGroup").GetChild (1).transform.position =  BattlePrepare.BP.CharacterPosition [1].transform.position;
			//GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform.GetChild (1).gameObject.SetActive (true);
			//GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform.GetChild(1).GetComponent<Character> ().reloadAttributes (true);
		}
		if (GameObject.Find ("PlayerCharList").transform.Find("NowGroup").childCount >= 3) {
			GameObject.Find ("PlayerCharList").transform.Find("NowGroup").GetChild (2).transform.position =  BattlePrepare.BP.CharacterPosition [2].transform.position;
			//GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform.GetChild (2).gameObject.SetActive (true);
			//GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform.GetChild(2).GetComponent<Character> ().reloadAttributes (true);
		}
		if (GameObject.Find("PlayerCharList").transform.Find("NowGroup").childCount >= 4) {
			GameObject.Find ("PlayerCharList").transform.Find("NowGroup").GetChild (3).transform.position =  BattlePrepare.BP.CharacterPosition [3].transform.position;
			//GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform.GetChild (3).gameObject.SetActive (true);
			//GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform.GetChild(3).GetComponent<Character> ().reloadAttributes (true);
		}



		do{
			GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("EnemyGroup").GetChild(0).gameObject.SetActive(true);
			GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("EnemyGroup").GetChild(0).gameObject.transform.position = GameManager.GM.InitPosition;
			GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("EnemyGroup").GetChild(0).GetComponent<Character>().NCSL = null;
			GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find("EnemyGroup").GetChild(0).GetComponent<Character>().reloadAttributes(true);
			GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find ("EnemyGroup").GetChild (0).GetComponent<Attack>().enabled = false;
			GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find ("EnemyGroup").GetChild (0).SetParent (GameObject.Find("EnemyGroupList").transform);

			Debug.Log (" Move Successfully!");
		}
		while
			(GameObject.Find ("BattlePlaceScreen").transform.Find("BattlePlace").transform.Find ("EnemyGroup").transform.childCount >= 1);

		GameManager.GM.gamestatus = GameManager.GameStatus.NonBattle;
        GameManager.GM.battleOutcome = GameManager.BattleOutCome.NotReady;
        Debug.Log("Return Successfully!");
		PlayerGroupClass.Reload ();
		EnemyGroupClass.Reload ();
	}

	void CheckCharacterStatus(){
		for (int i = 0; i < EnemyGroupObject.Length; i++) {
			if (EnemyGroupObject [i] != null) {
				//Debug.Log (EnemyGroupObject [i] + " " + EnemyGroupObject [i].GetComponent<Character> ().isDead);
				if (EnemyGroupObject [i].GetComponent<Character> ().isDead) {
					//EnemyGroupObject [i].SetActive (false);
					TargetBtn [i].SetActive (false);
					EnemyStatus.SetActive (false);
					EnemyGroupObject [i].GetComponent<Attack>().Anim.SetTrigger("Die");
					//EnemyGroupObject [i] = null;
					EnemycountNum++;
					if (EnemycountNum == EnemyGroupObject.Length) {
						//PlayerGroupObject = null;
						// GameOver Statement
						Time.timeScale = 1.0f;
						battleSpeed = Time.timeScale;
						speedChangeBtn.SetActive (false);
						GameManager.GM.battleOutcome = GameManager.BattleOutCome.Win;
						timer_Result.InitializeStart ();
						ResultsScreen.SetActive (true);
						//Application.Quit ();
						//UnityEditor.EditorApplication.isPlaying = false;
					}	
				}
			}
		}

		for (int i = 0; i < PlayerGroupObject.Length; i++) {
			//Debug.Log (PlayerGroupObject [i]);
			if (PlayerGroupObject [i] != null) {
				if (PlayerGroupObject [i].GetComponent<Character> ().isDead) {
					//Destroy (PlayerGroupObject [i].GetComponent<Attack>);
					//PlayerGroupObject [i].SetActive (false);
					PlayerGroupObject [i].GetComponent<Attack>().Anim.SetTrigger("Die");
					PlayercountNum++;
					//Debug.Log (countNum);	
				}
				if (PlayercountNum == PlayerGroupObject.Length) {
					//PlayerGroupObject = null;
					// GameOver Statement
					Time.timeScale = 1.0f;
					battleSpeed = Time.timeScale;
					speedChangeBtn.SetActive (false);
					GameManager.GM.battleOutcome = GameManager.BattleOutCome.Lose;
					ResultsScreen.SetActive (true);
					timer_Result.InitializeStart ();
				}
			}
		}

		if (TurnLeft <= 0 && (GameManager.GM.battleOutcome != GameManager.BattleOutCome.Lose && GameManager.GM.battleOutcome != GameManager.BattleOutCome.Win)) {
			Time.timeScale = 1.0f;
			battleSpeed = Time.timeScale;
			speedChangeBtn.SetActive (false);
			GameManager.GM.battleOutcome = GameManager.BattleOutCome.OverLimit;
			for (int i = 0; i < EnemyGroupObject.Length; i++) {
				if(EnemyGroupObject [i] != null && !EnemyGroupObject [i].GetComponent<Character> ().isDead)
					EnemyGroupObject [i].GetComponent<Attack> ().canRecharge = false;
			}
			for (int i = 0; i < PlayerGroupObject.Length; i++) {
				if (PlayerGroupObject [i] != null && !PlayerGroupObject [i].GetComponent<Character> ().isDead) {
					PlayerGroupObject [i].GetComponent<Attack> ().canRecharge = false;
					PlayerGroupObject [i].GetComponent<Attack> ().target = null;
				}
			}
			timer_Result.InitializeStart ();
			skillbtn.SetActive (false);
			fleebtn.SetActive (false);
			FleeAlertScreen.SetActive (false);
			SkillInfoScreen.SetActive (false);
			for (int i = 0; i < TargetBtn.Length; i++) {
				TargetBtn [i].SetActive (false);
			}
			ResultsScreen.SetActive (true);
		}
	}

	void UpdateTheResults(){
		if (GameManager.GM.battleOutcome != GameManager.BattleOutCome.NotReady) {
			switch (GameManager.GM.battleOutcome) {
			case GameManager.BattleOutCome.Win:

				for (int i = 0; i < PlayerGroupObject.Length; i++) {
					if (PlayerGroupObject [i] != null) {
						//Debug.Log (PlayerGroupObject [i]);
						if (PlayerGroupObject [i].GetComponent<Character> ().isAddedExp != true) {
							PlayerGroupObject [i].GetComponent<Character> ().isAddedExp = true;
							if (!PlayerGroupObject [i].GetComponent<Character> ().isDead) {
								PlayerGroupObject [i].GetComponent<Character> ().AddExp ((int)(EnemyGroupClass.ExpReward * (float)((EnemyGroupClass.GroupCharacterHPBeforeStart - EnemyGroupClass.GroupCharacterHP) / EnemyGroupClass.GroupCharacterMaxHP)));
								//Debug.Log("Gain Exp: " + (int)(EnemyGroupClass.ExpReward * (float)((EnemyGroupClass.GroupCharacterHPBeforeStart - EnemyGroupClass.GroupCharacterHP) / EnemyGroupClass.GroupCharacterMaxHP)));
							} else
								PlayerGroupObject [i].GetComponent<Character> ().AddExp ((int)(EnemyGroupClass.ExpReward * (float)((EnemyGroupClass.GroupCharacterHPBeforeStart - EnemyGroupClass.GroupCharacterHP) / EnemyGroupClass.GroupCharacterMaxHP)) / 2);
							ResultsScreenCharName [i].gameObject.SetActive (true);
						}
						ResultsScreenExpBar [i].UpdateBar (PlayerGroupObject [i].GetComponent<Character> ().Exp, PlayerGroupObject [i].GetComponent<Character> ().ExpRequirment);
						ResultsScreenCharLv [i].text = "Lv: " + PlayerGroupObject [i].GetComponent<Character> ().Lv;
					} else
						ResultsScreenCharName [i].gameObject.SetActive (false);
				}
				switch (GameManager.GM.language) {
				case GameManager.Language.English:
					ResultsScreenTitleText.text = "Win";
					break;
				case GameManager.Language.TraditionalChinese:
					ResultsScreenTitleText.text = "勝利";
					break;
				}


				ResultsScreenMaterialText [0].text = "+ " + Mathf.CeilToInt (EnemyGroupClass.MoneyReward * (float)((EnemyGroupClass.GroupCharacterHPBeforeStart) / EnemyGroupClass.GroupCharacterMaxHP));
				ResultsScreenMaterialText [1].text = "+ " + Mathf.CeilToInt (EnemyGroupClass.FoodReward * (float)((EnemyGroupClass.GroupCharacterHPBeforeStart) / EnemyGroupClass.GroupCharacterMaxHP));
				break;
			case GameManager.BattleOutCome.Lose:
				for (int i = 0; i < PlayerGroupObject.Length; i++) {
					if (PlayerGroupObject [i] != null) {
						ResultsScreenCharName [i].gameObject.SetActive (true);
						ResultsScreenExpBar [i].UpdateBar (PlayerGroupObject [i].GetComponent<Character> ().Exp, PlayerGroupObject [i].GetComponent<Character> ().ExpRequirment);
						ResultsScreenCharLv [i].text = "Lv: " + PlayerGroupObject [i].GetComponent<Character> ().Lv;
					} else
						ResultsScreenCharName [i].gameObject.SetActive (false);
				}
				switch (GameManager.GM.language) {
				case GameManager.Language.English:
					ResultsScreenTitleText.text = "Lose";
					break;
				case GameManager.Language.TraditionalChinese:
					ResultsScreenTitleText.text = "戰敗";
					break;
				}
				ResultsScreenMaterialText [0].text = "+ " + 0;
				ResultsScreenMaterialText [1].text = "+ " + 0;
				break;

			case GameManager.BattleOutCome.OverLimit:
				for (int i = 0; i < PlayerGroupObject.Length; i++) {
					if (PlayerGroupObject [i] != null) {
						//Debug.Log (PlayerGroupObject [i]);
						if (PlayerGroupObject [i].GetComponent<Character> ().isAddedExp != true) {
							PlayerGroupObject [i].GetComponent<Character> ().isAddedExp = true;
							if (!PlayerGroupObject [i].GetComponent<Character> ().isDead) {
								PlayerGroupObject [i].GetComponent<Character> ().AddExp ((int)(EnemyGroupClass.ExpReward * (float)((EnemyGroupClass.GroupCharacterHPBeforeStart - EnemyGroupClass.GroupCharacterHP) / EnemyGroupClass.GroupCharacterMaxHP)));
								//Debug.Log("Gain Exp: " + (int)(EnemyGroupClass.ExpReward * (float)((EnemyGroupClass.GroupCharacterHPBeforeStart - EnemyGroupClass.GroupCharacterHP) / EnemyGroupClass.GroupCharacterMaxHP)));
							} else
								PlayerGroupObject [i].GetComponent<Character> ().AddExp ((EnemyGroupClass.ExpReward * (float)((EnemyGroupClass.GroupCharacterHPBeforeStart - EnemyGroupClass.GroupCharacterHP) / EnemyGroupClass.GroupCharacterMaxHP)) / 2);
							ResultsScreenCharName [i].gameObject.SetActive (true);
						}
						ResultsScreenExpBar [i].UpdateBar (PlayerGroupObject [i].GetComponent<Character> ().Exp, PlayerGroupObject [i].GetComponent<Character> ().ExpRequirment);
						ResultsScreenCharLv [i].text = "Lv: " + PlayerGroupObject [i].GetComponent<Character> ().Lv;
					} else
						ResultsScreenCharName [i].gameObject.SetActive (false);
				}
				switch (GameManager.GM.language) {
				case GameManager.Language.English:
					ResultsScreenTitleText.text = "Over Turn Limit.";
					break;
				case GameManager.Language.TraditionalChinese:
					ResultsScreenTitleText.text = "超出回合上限";
					break;
				}


				ResultsScreenMaterialText [0].text = "+ " + Mathf.CeilToInt (EnemyGroupClass.MoneyReward * (float)((EnemyGroupClass.GroupCharacterHPBeforeStart - EnemyGroupClass.GroupCharacterHP) / EnemyGroupClass.GroupCharacterMaxHP));
				ResultsScreenMaterialText [1].text = "+ " + Mathf.CeilToInt (EnemyGroupClass.FoodReward * (float)((EnemyGroupClass.GroupCharacterHPBeforeStart - EnemyGroupClass.GroupCharacterHP) / EnemyGroupClass.GroupCharacterMaxHP));
				break;
			}
		} else {
			fleebtn.SetActive (true);
		}
	}

	void ResultScreenShowEnd(){
		//PlayerGroupClass.Reload ();
		//EnemyGroupClass.Reload ();
		if (timer_Result.CheckTime (5.0f)) {
			for (int i = 0; i < PlayerGroupObject.Length; i++) {
				if (PlayerGroupObject [i] != null) {
					PlayerGroupObject[i].GetComponent<Attack>().Anim.ResetTrigger ("Walk");
					PlayerGroupObject[i].GetComponent<Attack>().Anim.ResetTrigger ("Hit");
				}
			}
			for (int i = 0; i < EnemyGroupObject.Length; i++) {
				if (EnemyGroupObject [i] != null) {
					EnemyGroupObject [i].GetComponent<Attack> ().Anim.ResetTrigger ("Walk");
					EnemyGroupObject [i].GetComponent<Attack> ().Anim.ResetTrigger ("Hit");
				}
			}
			switch (GameManager.GM.battleOutcome) {
			case GameManager.BattleOutCome.Win:
				Debug.Log ("EnemyCount: " + EnemycountNum);
				GameManager.GM.AddMoney (Mathf.CeilToInt (EnemyGroupClass.MoneyReward * (float)((EnemyGroupClass.GroupCharacterHPBeforeStart) / EnemyGroupClass.GroupCharacterMaxHP)));
				GameManager.GM.AddFood (Mathf.CeilToInt (EnemyGroupClass.FoodReward * (float)((EnemyGroupClass.GroupCharacterHPBeforeStart) / EnemyGroupClass.GroupCharacterMaxHP)));
				Debug.Log (Mathf.CeilToInt (EnemyGroupClass.FoodReward * (float)((EnemyGroupClass.GroupCharacterHPBeforeStart) / EnemyGroupClass.GroupCharacterMaxHP)));
				for (int i = 0; i < PlayerGroupObject.Length; i++) {
					if (PlayerGroupObject [i] != null) {

						if (PlayerGroupObject [i].GetComponent<Character> ().isAddedExp != false) {
							PlayerGroupObject [i].GetComponent<Character> ().isAddedExp = false;
						}
					}
					ResultsScreenCharName [i].gameObject.SetActive (true);
				}
				timer_Result.CancleTimer ();
				ReturnCharToList ();
				//GameElement.GE.BattlePlace.transform.GetChild (0).gameObject.SetActive (false);
				//GameElement.GE.BattlePreparation.SetActive (true);
				GameManager.GM.AddStage (1);
				//SceneManager.LoadScene ("Ships");
				//SceneManager.LoadSceneAsync ("Ships");
				Loader.LoadLevel ("Ships");
				break;

			case GameManager.BattleOutCome.Lose:
				for (int i = 0; i < PlayerGroupObject.Length; i++) {
					if (PlayerGroupObject [i] != null) {
						if (PlayerGroupObject [i].GetComponent<Character> ().isAddedExp != false) {
							PlayerGroupObject [i].GetComponent<Character> ().isAddedExp = false;
						}
					}
					ResultsScreenCharName [i].gameObject.SetActive (true);
				}
				GameManager.GM.DeTryTime(1);
				timer_Result.CancleTimer ();
				ReturnCharToList ();
				if (GameManager.GM.TryTime <= 0) {
					Application.Quit ();
					//UnityEditor.EditorApplication.isPlaying = false;
				}
				//GameElement.GE.BattlePlace.transform.GetChild (0).gameObject.SetActive (false);
//				GameElement.GE.BattlePreparation.SetActive (true);
				//SceneManager.LoadScene ("Ships");
				//SceneManager.LoadSceneAsync ("Ships");
				Loader.LoadLevel ("Ships");
				break;

			case GameManager.BattleOutCome.OverLimit:
				Debug.Log ("EnemyCount: " + EnemycountNum);
				GameManager.GM.AddMoney((int)Mathf.CeilToInt(EnemyGroupClass.MoneyReward * (float)((EnemyGroupClass.GroupCharacterHPBeforeStart - EnemyGroupClass.GroupCharacterHP) / EnemyGroupClass.GroupCharacterMaxHP)));
				GameManager.GM.AddFood((int)Mathf.CeilToInt(EnemyGroupClass.FoodReward * (float)((EnemyGroupClass.GroupCharacterHPBeforeStart - EnemyGroupClass.GroupCharacterHP) / EnemyGroupClass.GroupCharacterMaxHP)));
				for (int i = 0; i < PlayerGroupObject.Length; i++) {
					if (PlayerGroupObject [i] != null) {
						if (PlayerGroupObject [i].GetComponent<Character> ().isAddedExp != false) {
							PlayerGroupObject [i].GetComponent<Character> ().isAddedExp = false;
						}
					}
					ResultsScreenCharName [i].gameObject.SetActive (true);
				}
				timer_Result.CancleTimer ();
				ReturnCharToList ();
				//GameElement.GE.BattlePlace.transform.GetChild (0).gameObject.SetActive (false);
				//GameElement.GE.BattlePreparation.SetActive (true);
				//SceneManager.LoadScene ("Ships");
				//SceneManager.LoadSceneAsync ("Ships");
				Loader.LoadLevel ("Ships");
				break;
			}
			GameManager.GM.battleOutcome = GameManager.BattleOutCome.NotReady;
		}
	}

	void EscapeScreenShowEnd(){
		if (timer_Escape.CheckTime (1f)) {
			if (canEscape) {
				PlayerGroupClass.Reload ();
				EnemyGroupClass.Reload ();
				ReturnCharToList ();
				for (int i = 0; i < EnemyGroupObject.Length; i++) {
					if (EnemyGroupObject [i] != null) {
						EnemyGroupObject [i].SetActive (false);
					}
				}
				skillbtn.SetActive (false);
				fleebtn.SetActive (false);
				FleeAlertScreen.SetActive (false);
				for (int i = 0; i < PlayerGroupClass.TotalCharacter; i++) {
					PlayerGroupObject [i].transform.Find ("Selector").gameObject.SetActive (false);
					PlayerGroupObject [i].GetComponent<Character> ().CurrentHp = PlayerGroupObject [i].GetComponent<Character> ().CurrentHp - PlayerGroupObject [i].GetComponent<Character> ().CurrentHp / 5;
					PlayerPanel [i].SetActive (false); 
				}
				status = Status.Running;
				uiStatus = UIStatus.MainMenu;
				timer_Escape.CancleTimer ();
				//GameElement.GE.BattlePlace.transform.GetChild (0).gameObject.SetActive (false);
				//GameElement.GE.BattlePreparation.SetActive (true);
				//SceneManager.LoadScene ("Ships");
				//SceneManager.LoadSceneAsync ("Ships");
				Loader.LoadLevel ("Ships");
			} else if (!canEscape) {
				skillbtn.SetActive (true);
				fleebtn.SetActive (true);
				FleeAlertScreen.SetActive (false);
				for (int i = 0; i < EnemyGroupObject.Length; i++) {
					if(EnemyGroupObject [i] != null && !EnemyGroupObject [i].GetComponent<Character> ().isDead)
						EnemyGroupObject [i].GetComponent<Attack> ().canRecharge = true;
				}
				for (int i = 0; i < PlayerGroupObject.Length; i++) {
					if(PlayerGroupObject [i] != null && !PlayerGroupObject [i].GetComponent<Character> ().isDead)
						PlayerGroupObject [i].GetComponent<Attack> ().canRecharge = true;
				}
				timer_Escape.CancleTimer ();
				uiStatus = UIStatus.MainMenu;
				status = Status.Running;
			}
		}
	}

	void UpdateStatusBar(){
		if (GameManager.GM.battleOutcome == GameManager.BattleOutCome.NotReady) {
			speedChangeBtn.SetActive (true);
			for (int i = 0; i < EnemyGroupClass.TotalCharacter; i++) {
				if (EnemyGroupClass.EnemyCharacter [i].isDead != true) {
					TargetBtn [i].SetActive (true);
				}
			}
		}
		if (turn == Turn.Player && GameManager.GM.gamestatus == GameManager.GameStatus.Battle) {
			Character Attacker = new Character();
			for (int i = 0; i < GameObject.Find ("PlayerGroup").gameObject.transform.childCount; i++) {
				if (GameObject.Find ("PlayerGroup").gameObject.transform.GetChild (i).GetComponent<Attack> ().isAttacked) {
					Attacker = GameObject.Find ("PlayerGroup").gameObject.transform.GetChild (i).GetComponent<Character> ();
					break;
				}
			}
			if (Attacker != null) {
				EnemyHPBar.UpdateBar (Attacker.GetComponent<Attack> ().target.GetComponent<Character> ().CurrentHp, Attacker.GetComponent<Attack> ().target.GetComponent<Character> ().Maxhp);
			}
		}


		for (int i = 1; i < PlayerGroupClass.TotalCharacter; i++) {
			if (PlayerGroupClass.SupCharacter [i - 1] != null) {
				PlayerGroupObject [i] = PlayerGroupClass.SupCharacter [i - 1].gameObject;
				healthBar [i].UpdateBar (PlayerGroupObject [i].GetComponent<Character> ().CurrentHp, PlayerGroupObject [i].GetComponent<Character> ().Maxhp);
				energyBar [i].UpdateBar (PlayerGroupObject [i].GetComponent<Character> ().CurrentEnergy, PlayerGroupObject [i].GetComponent<Character> ().MaxEnergy);
				CharacterProgressBar [i].UpdateBar (PlayerGroupObject [i].GetComponent<Attack> ().MovementSpeed, PlayerGroupObject [i].GetComponent<Character> ().movspeed);
				if (PlayerGroupObject [i].GetComponent<Attack> ().canAttack == true) {
					Image image = PlayerPanel [i].GetComponent<Image> ();
					var tempColor = image.color;
					tempColor.a = 0.5f;
					image.color = tempColor;
					if (turn == Turn.Player && status == Status.Running) {
						//skillbtn.SetActive (true);
						status = Status.WaitingResponse;
						//PlayerGroupObject [i].transform.Find ("Selector").gameObject.SetActive (true);
					}
				} else {
					Image image = PlayerPanel [i].GetComponent<Image> ();
					var tempColor = image.color;
					tempColor.a = 0f;
					image.color = tempColor;
					//PlayerGroupObject [i].transform.Find ("Selector").gameObject.SetActive (false);
				}
			}
		}
		if (PlayerGroupObject [0] != null) {
			healthBar [0].UpdateBar (PlayerGroupObject [0].GetComponent<Character> ().CurrentHp, PlayerGroupObject [0].GetComponent<Character> ().Maxhp);
			energyBar [0].UpdateBar (PlayerGroupObject [0].GetComponent<Character> ().CurrentEnergy, PlayerGroupObject [0].GetComponent<Character> ().MaxEnergy);
			CharacterProgressBar [0].UpdateBar (PlayerGroupObject [0].GetComponent<Attack> ().MovementSpeed, PlayerGroupObject [0].GetComponent<Character> ().movspeed);
			//Debug.Log (PlayerGroupObject [0].GetComponent<Attack> ().MovementSpeed);
			//Debug.Log (PlayerGroupObject [0].GetComponent<Character> ().movspeed);
			//if (PlayerGroupObject [0].GetComponent<Attack> ().canAttack == true) {
				Image image = PlayerPanel [0].GetComponent<Image> ();
				var tempColor = image.color;
				tempColor.a = 0.5f;
				image.color = tempColor;
				if (turn == Turn.Player && status == Status.Running) {
					Debug.Log ("Active Successfully!");
					skillbtn.SetActive (true);
					skillbtn.GetComponent<Image> ().sprite = Resources.Load<Sprite> ("Image/ActiveSkill_Attacker");
					//fleebtn.SetActive (true);
					status = Status.WaitingResponse;
					//PlayerGroupObject [0].transform.Find ("Selector").gameObject.SetActive (true);
				}
			/*} else {
				Image image = PlayerPanel [0].GetComponent<Image> ();
				var tempColor = image.color;
				tempColor.a = 0f;
				image.color = tempColor;
				PlayerGroupObject [0].transform.Find ("Selector").gameObject.SetActive (false);
			}
			*/
		}
	}

	public void skillOn(int skillIndex){
		SkillTree ST;
		Character skillCharacter;
		NormalCharacterSkillList NCSL;
		ST = GameObject.Find ("SkillTree").GetComponent<SkillTree> ();
		NCSL = GameObject.Find ("NormalSkillList").GetComponent<NormalCharacterSkillList> ();
		try{
			skillCharacter = ST.MC;
		}catch(System.Exception e){
			Debug.Log ("Can't find skill from Main Character Skill Tree...");
			skillCharacter = NCSL.currentCharacter;
		}
		if (skillCharacter.GetComponent<Character> ().isMainCharacter) {
			if (!ST.CurrentTreeElement [skillIndex].isSkillOn) {
				if (ST.CurrentTreeElement [skillIndex].currentCD > 0) {
					switch (GameManager.GM.language) {
					case GameManager.Language.English:
						SkillInfoText.text = "Skill is in CoolDown.";
						break;
					case GameManager.Language.TraditionalChinese:
						SkillInfoText.text = "技能正在冷卻中!";
						break;
					}
					ST.CurrentTreeElement [skillIndex].isSkillOn = false;
					Image tempImage = skillbtn.GetComponent<Image> ();
					var TempColor = tempImage.color;
					TempColor.a = 0.5f;
					tempImage.color = TempColor;
					return;
				}
				if (skillCharacter.GetComponent<Character> ().CurrentEnergy >= skillCharacter.GetComponent<MainCharacter> ().SkillTree.CurrentTreeElement [1].energyNeed) {
					ST.CurrentTreeElement [skillIndex].isSkillOn = true;
					Image tempImage = skillbtn.GetComponent<Image> ();
					var TempColor = tempImage.color;
					TempColor.a = 1f;
					tempImage.color = TempColor;
				} else {
					switch (GameManager.GM.language) {
					case GameManager.Language.English:
						SkillInfoText.text = "You Don't have enough energy!";
						break;
					case GameManager.Language.TraditionalChinese:
						SkillInfoText.text = "你沒有足夠能量!";
						break;
					}
					ST.CurrentTreeElement [skillIndex].isSkillOn = false;
					Image tempImage = skillbtn.GetComponent<Image> ();
					var TempColor = tempImage.color;
					TempColor.a = 0.5f;
					tempImage.color = TempColor;
				}
			} else {
				ST.CurrentTreeElement [skillIndex].isSkillOn = false;
				Image tempImage = skillbtn.GetComponent<Image> ();
				var TempColor = tempImage.color;
				TempColor.a = 0.5f;
				tempImage.color = TempColor;
			}
		}
	}

	public void SkillInfoShow(int SkillIndex){
		SkillTree ST;
		Character skillCharacter;
		NormalCharacterSkillList NCSL;
		ST = GameObject.Find ("SkillTree").GetComponent<SkillTree> ();
		NCSL = GameObject.Find ("NormalSkillList").GetComponent<NormalCharacterSkillList> ();
		try{
			skillCharacter = ST.MC;
		}catch(System.Exception e){
			Debug.Log ("Can't find skill from Main Character Skill Tree...");
			skillCharacter = NCSL.currentCharacter;
		}
		SkillInfoScreen.SetActive (true);
		switch (GameManager.GM.language) {
		case GameManager.Language.English:
			SkillInfoText.text = ST.CurrentTreeElement[SkillIndex].skillName + "\t\tEnergy Needed: " + ST.CurrentTreeElement[SkillIndex].energyNeed + "\n" + ST.CurrentTreeElement[SkillIndex].skillDesc;
			break;
		case GameManager.Language.TraditionalChinese:
			SkillInfoText.text = ST.CurrentTreeElement[SkillIndex].skillName_CHT + "\t\t能量需求: " + ST.CurrentTreeElement[SkillIndex].energyNeed + "\n" + ST.CurrentTreeElement[SkillIndex].skillDesc_CHT;
			break;
		}
	}

	public void SkillNotShow(){
		SkillInfoScreen.SetActive (false);
	}

	public void changeSpeed(){
		if (battleSpeed == 1.0f) {
			Time.timeScale = 1.5f;
			battleSpeed = Time.timeScale;
			speedChangeBtn.transform.GetChild (0).GetComponent<Text> ().text = "1.5x";
		} else if (battleSpeed == 1.5f) {
			Time.timeScale = 2.0f;
			battleSpeed = Time.timeScale;
			speedChangeBtn.transform.GetChild (0).GetComponent<Text> ().text = "2.0x";
		} else if (battleSpeed == 2.0f) {
			Time.timeScale = 4.0f;
			battleSpeed = Time.timeScale;
			speedChangeBtn.transform.GetChild (0).GetComponent<Text> ().text = "4.0x";
		} else if (battleSpeed == 4.0f) {
			Time.timeScale = 1.0f;
			battleSpeed = Time.timeScale;
			speedChangeBtn.transform.GetChild (0).GetComponent<Text> ().text = "1.0x";
		}
	}
}