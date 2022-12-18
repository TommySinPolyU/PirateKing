using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using BayatGames.SaveGamePro;

public class Ships : MonoBehaviour{
	public LevelLoader Loader;
	public static Ships ships;
	public GameObject PlayerCharList;
	public GameObject EnemyGroupList;
	public EnemyGroup enemyGroup;

	[Header("UI Control Element")]
	public GameObject LoadingScreen;
	public GameObject[] UIElement;
	public GameObject InfoScreen;
	public SimpleHealthBar LoadingBar;
	public GameObject AlertScreen;
	public Text AlertScreenText;
	public Text MoneyText;
	public Text FoodText;
	public Text TurnLeftText;
	public GameObject[] TurnImage;

	[Header("Training UI Element")]
	public GameObject TrainingScreen;
	public Text TrainingScreenTitle;
	public Text TrainingScreenText;
	public Text TrainingScreenConfirmBtnText;
	public Text TrainingScreenCancelBtnText;
	[Header("Rest UI Element")]
	public GameObject RestScreen;
	public Text RestScreenTitle;
	public Text RestScreenText;
	public Text RestScreenConfirmBtnText;
	public Text RestScreenCancelBtnText;

	[Header("Successful Screen UI Element")]
	public GameObject SuccessfulScreen;
	public Text SuccessfulScreenText;

	public Character[] PlayerCharacter;
	Timer timer,timer_alert,timer_enemyTurn, timer_enemyToPlayer,timer_MoveTocharData;
	public bool isReturned = false;
	int foodneed;
	bool EnemyisStart = false;

	void Start(){
        LoadingScreen.SetActive(false);
        InfoScreen.SetActive (true);
        GameObject tempTimer = new GameObject();
        GameObject tempTimer1 = new GameObject();
        GameObject tempTimer2 = new GameObject();
        GameObject tempTimer3 = new GameObject();
        timer = tempTimer.AddComponent<Timer>();
        timer_alert = tempTimer1.AddComponent<Timer>();
        timer_enemyTurn = tempTimer1.AddComponent<Timer>();
        timer_enemyToPlayer = tempTimer2.AddComponent<Timer>();
        timer_MoveTocharData = tempTimer3.AddComponent<Timer>();
        for (int i = 0; i < UIElement.Length; i++) {
        		UIElement [i].SetActive (true);
        	}

        if (GameManager.GM.isLoadGame) {
			GameManager.GM.MainCharacterName = GameObject.Find("PlayerCharList").transform.Find("MainCharacter").GetComponent<MainCharacter>().charName;
		}
		//if (!GameManager.GM.isLoadGame) { 
			GameManager.GM.gamestatus = GameManager.GameStatus.Ships;
			foodneed = 0;
		//}


			PlayerCharList = GameObject.Find ("PlayerCharList");
			EnemyGroupList = GameObject.Find ("EnemyGroupList");
		PlayerCharList.transform.Find ("NowGroup").SetAsLastSibling ();
			for (int i = 0; i < PlayerCharList.transform.childCount; i++) {
				PlayerCharList.transform.GetChild (i).gameObject.transform.position = GameManager.GM.InitPosition;
			}

			for (int i = 0; i < PlayerCharList.transform.Find ("NowGroup").childCount; i++) {
				PlayerCharList.transform.Find ("NowGroup").GetChild (i).gameObject.transform.position = GameManager.GM.InitPosition;
			}
				
			PlayerCharacter = new Character[GameObject.Find ("PlayerCharList").transform.childCount - 1 + GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").childCount];
			PlayerCharacter [0] = GameObject.Find ("PlayerCharList").transform.Find ("MainCharacter").GetComponent<Character> ();
			int AddCount = 0;
			for (int i = 1; i < (GameObject.Find ("PlayerCharList").transform.childCount - 1 + GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").childCount); i++) {
				if (GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").childCount - 1 >= 1 && AddCount < GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").childCount - 1) {
					PlayerCharacter [i] = GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform.GetChild (i).GetComponent<Character> ();
					PlayerCharacter [i].gameObject.SetActive (true);
					AddCount++;
				} else {
					PlayerCharacter [i] = GameObject.Find ("PlayerCharList").transform.GetChild (i - (GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").childCount)).GetComponent<Character> ();
					PlayerCharacter [i].gameObject.SetActive (true);
				}
			}
		//GameManager.GM.isLoadGame = false;
	}

	void FixedUpdate(){
		GameManager.GM.gamestatus = GameManager.GameStatus.Ships;
		//if (timer.isStarted)
		//	LoadingBar.UpdateBar (timer.timerTime, 0.5f);
		
		//if (timer.CheckTime (0.5f)) {
		//	LoadingScreen.SetActive (false);
		//	InfoScreen.SetActive (true);
		//	for (int i = 0; i < UIElement.Length; i++) {
		//		UIElement [i].SetActive (true);
		//	}
		//	GameManager.GM.isLoadGame = false;
		//	timer.CancleTimer ();
		//}

		if (timer_MoveTocharData.CheckTime (1.5f)) {
			if (GameManager.GM.turn == GameManager.Turn.Player) {
				MoveToCharacterData ();
				timer_MoveTocharData.CancleTimer ();
			}
		}

		if (timer_alert.CheckTime (1f)) {
			AlertScreen.SetActive (false);
			SuccessfulScreen.SetActive (false);
			TrainingScreen.SetActive (false);
			RestScreen.SetActive (false);
			if (!EnemyisStart && GameManager.GM.turn == GameManager.Turn.Enemy) {
				EnemyisStart = true;
				GameManager.GM.Round = 0;
			}
			timer_alert.CancleTimer ();
		}

		if(timer_enemyToPlayer.CheckTime(1.5f)){
			timer_enemyToPlayer.CancleTimer ();
			GameManager.GM.turn = GameManager.Turn.Player;
			GameManager.GM.Round = 0;
			EnemyisStart = false;
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				SuccessfulScreenText.text = "Player Turn";
				break;
			case GameManager.Language.TraditionalChinese:
				SuccessfulScreenText.text = "玩家回合";
				break;
			}
			SuccessfulScreen.SetActive (true);
			if (GameManager.GM.TrainingCoolDown > 0) {
				GameManager.GM.DeTrainingCoolDown(1);
			}
			GameObject[] currentStageEnemy = GameObject.FindGameObjectsWithTag ("C" + GameManager.GM.Chapter + "_S" + GameManager.GM.Stage + "_Enemy");
			for (int i = 0; i < currentStageEnemy.Length; i++) {
				currentStageEnemy [i].transform.SetParent (GameObject.Find ("EnemyGroupList").transform);
			}
			timer_alert.InitializeStart ();
		}

		if (timer_enemyTurn.CheckTime (1.5f)) {
			AlertScreen.SetActive (true);
			GameManager.GM.AddRound(1);
			GameObject[] currentStageEnemy = GameObject.FindGameObjectsWithTag ("C" + GameManager.GM.Chapter + "_S" + GameManager.GM.Stage + "_Enemy");
			for (int i = 0; i < currentStageEnemy.Length; i++) {
				currentStageEnemy [i].transform.SetParent (GameObject.Find ("EnemyGroupList").transform.Find ("Chapter" + GameManager.GM.Chapter).Find ("Group" + GameManager.GM.Stage).transform);
				currentStageEnemy [i].GetComponent<Character> ().reloadPower ();
			}
			enemyGroup = 
				GameObject.Find ("EnemyGroupList").transform.Find ("Chapter" + GameManager.GM.Chapter).Find ("Group" + GameManager.GM.Stage).GetComponent<EnemyGroup>();
			enemyGroup.Reload ();

			if (enemyGroup.GroupMaxPower > enemyGroup.GroupPower) {
				switch (GameManager.GM.language) {
				case GameManager.Language.English:
					AlertScreenText.text = "Turn " + (GameManager.GM.Round) + ": Enemy Rest";
					break;
				case GameManager.Language.TraditionalChinese:
					AlertScreenText.text = "行動回合 " + (GameManager.GM.Round) + ": 敵人休息";
					break;
				}
				for (int i = 0; i < enemyGroup.EnemyCharacter.Length; i++) {
					enemyGroup.EnemyCharacter [i].GetHeal ((int)(enemyGroup.EnemyCharacter [i].Maxhp * 0.2f));
					enemyGroup.EnemyCharacter [i].EnergyRecovery ((int)(enemyGroup.EnemyCharacter [i].MaxEnergy * 0.2f));
					enemyGroup.EnemyCharacter [i].reloadAttributes (true);
					enemyGroup.Reload ();
				}
			}
			else if(enemyGroup.GroupPower >= enemyGroup.GroupMaxPower){
				switch (GameManager.GM.language) {
				case GameManager.Language.English:
					AlertScreenText.text = "Turn " + (GameManager.GM.Round) + ": Enemy Training";
					break;
				case GameManager.Language.TraditionalChinese:
					AlertScreenText.text = "行動回合 " + (GameManager.GM.Round) + ": 敵人訓練";
					break;
				}
				for (int i = 0; i < enemyGroup.EnemyCharacter.Length; i++) {
					enemyGroup.EnemyCharacter [i].AddExp (enemyGroup.ExpReward);
					enemyGroup.EnemyCharacter [i].reloadAttributes (true);
				}
				enemyGroup.Reload ();
			}

			if (GameManager.GM.Round != 3) {
				timer_enemyTurn.CancleTimer ();
				timer_enemyTurn.InitializeStart ();
			} else {
				timer_enemyTurn.CancleTimer ();
				timer_enemyToPlayer.InitializeStart();
			}
		}

		switch (GameManager.GM.language) {
		case GameManager.Language.English:
			UIElement[0].transform.GetChild(0).GetComponent<Text> ().text = "Ship";
			UIElement[1].transform.GetChild(0).GetComponent<Text> ().text = "Battle";
			UIElement[2].transform.GetChild(0).GetComponent<Text> ().text = "Character";
			UIElement[4].transform.GetChild(0).GetComponent<Text> ().text = "Training";
			UIElement[5].transform.GetChild(0).GetComponent<Text> ().text = "Rest";
			UIElement[6].transform.GetChild(0).GetComponent<Text> ().text = "Shop";
			break;
		case GameManager.Language.TraditionalChinese:
			UIElement[0].transform.GetChild(0).GetComponent<Text> ().text = "主艦";
			UIElement[1].transform.GetChild(0).GetComponent<Text> ().text = "戰鬥";
			UIElement[2].transform.GetChild(0).GetComponent<Text> ().text = "角色";
			UIElement[4].transform.GetChild(0).GetComponent<Text> ().text = "訓練";
			UIElement[5].transform.GetChild(0).GetComponent<Text> ().text = "休息";
			UIElement[6].transform.GetChild(0).GetComponent<Text> ().text = "商店";
			break;
		}
	}

	void Update(){
		GameManager.GM.gamestatus = GameManager.GameStatus.Ships;
		if (GameManager.GM.Round == 0) {
			for (int i = 0; i < TurnImage.Length; i++) {
				TurnImage [i].SetActive (true);
			}
		} else if (GameManager.GM.Round == 1) {
			TurnImage [2].SetActive (false);
		} else if (GameManager.GM.Round == 2) {
			TurnImage [2].SetActive (false);
			TurnImage [1].SetActive (false);
		} else if (GameManager.GM.Round == 3) {
			TurnImage [2].SetActive (false);
			TurnImage [1].SetActive (false);
			TurnImage [0].SetActive (false);
		}
		if (GameManager.GM.gamestatus == GameManager.GameStatus.Ships) {
			enemyGroup = 
			GameObject.Find ("EnemyGroupList").transform.Find ("Chapter" + GameManager.GM.Chapter).Find ("Group" + GameManager.GM.Stage).GetComponent<EnemyGroup> ();
		}

		for (int i = 0; i < enemyGroup.EnemyCharacter.Length; i++){
			enemyGroup.EnemyCharacter [i].gameObject.SetActive (true);
		}

		if (GameManager.GM.Round == 3 && GameManager.GM.turn == GameManager.Turn.Player && !EnemyisStart) {
			AlertScreen.SetActive (true);
			GameManager.GM.turn = GameManager.Turn.Enemy;
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				AlertScreenText.text = "Enemy Turn";
				break;

			case GameManager.Language.TraditionalChinese:
				AlertScreenText.text = "敵人回合";
				break;
			}
			timer_alert.InitializeStart ();
			timer_enemyTurn.InitializeStart ();
		}


			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				MoneyText.text = "" + GameManager.GM.money;
				FoodText.text = "" + GameManager.GM.food;
				TurnLeftText.text = "Turn Left: "; 
				//AlertScreenText.text = "You Don't have enough Turn Left!";
				RestScreenConfirmBtnText.text = "Confirm";
				RestScreenCancelBtnText.text = "Cancel";
				TrainingScreenConfirmBtnText.text = "Confirm";
				TrainingScreenCancelBtnText.text = "Cancel";
				RestScreenTitle.text = "Take a Rest";
				RestScreenText.text = "Take a Rest can recover all characters 20% of HP and Energy,\nbut Need one turn for rest and " + foodneed + " food. \n\nAre you ready to rest?";
				TrainingScreenTitle.text = "Training";
				TrainingScreenText.text = "All characters (both within and outside the team) receive an experience value\nbonus equal to the current level exp reward.\n\nBut this action must consume 3 turn.";
				break;
			case GameManager.Language.TraditionalChinese:
				MoneyText.text = "" + GameManager.GM.money;
				FoodText.text = "" + GameManager.GM.food;
				TurnLeftText.text = "剩餘行動力: "; 
				//AlertScreenText.text = "剩餘行動力不足!";
				RestScreenConfirmBtnText.text = "確定";
				RestScreenCancelBtnText.text = "取消";
				TrainingScreenConfirmBtnText.text = "確定";
				TrainingScreenCancelBtnText.text = "取消";
				RestScreenTitle.text = "休息一會";
				RestScreenText.text = "休息將可使所有角色回復 20% 生命及能量,\n但此行動需要 1點 行動點及" + foodneed + " 點食物. \n\n你準備好休息嗎？";
				TrainingScreenTitle.text = "訓練";
				TrainingScreenText.text = "所有角色 (包括隊伍內及隊伍外的角色)\n將可獲得等同於目前關卡經驗的經驗值獎勵.\n\n但此行動需 3點 行動點";
				break;
			}
	}

	void Awake(){
		if (DontDestroyOnLoadManager._ddolObjects.Count == 0) {
			DontDestroyOnLoadManager.DontDestroyOnLoad (PlayerCharList);
			DontDestroyOnLoadManager.DontDestroyOnLoad (EnemyGroupList);
		} else {
			Destroy (PlayerCharList);
			Destroy (EnemyGroupList);
			PlayerCharList = GameObject.Find ("PlayerCharList");
			EnemyGroupList = GameObject.Find ("EnemyGroupList");
		}
	}

	public void MoveToBattle(){
		if (GameManager.GM.Round < 3) {
			DontDestroyOnLoadManager.SetActive ();
			GameManager.GM.gamestatus = GameManager.GameStatus.NonBattle;
			//SceneManager.LoadScene ("BattleSystem");
			//SceneManager.LoadSceneAsync ("BattleSystem");
			Loader.LoadLevel ("BattlePreparation");
		} else {
			AlertScreen.SetActive (true);
			timer_alert.InitializeStart ();
		}
	}

	public void MoveToCharacterData(){
		DontDestroyOnLoadManager.SetActive ();
		GameManager.GM.gamestatus = GameManager.GameStatus.NonBattle;
		//SceneManager.LoadScene ("CharacterDataScreen");
		//SceneManager.LoadSceneAsync ("CharacterDataScreen");
		Loader.LoadLevel ("CharacterDataScreen");
	}

	public void ReturnToMainMenu(){
		//DontDestroyOnLoadManager.SetNotActive ();
		GameManager.GM.gamestatus = GameManager.GameStatus.MainMenu;
		//SceneManager.LoadSceneAsync ("MainMenu");
		Loader.LoadLevel ("MainMenu");
	}

	public void RestScreenShow(){
		RestScreen.SetActive (true);
		foodneed = 0;
		for (int i = 0; i < PlayerCharacter.Length; i++) {
			foodneed += ((PlayerCharacter [i].MaxCharacterPower - PlayerCharacter [i].CharacterPower)/2);
		}
	}

	public void RestScreenNotShow(){
		RestScreen.SetActive (false);
	}

	public void Rest(){
		if (foodneed == 0) {
			SuccessfulScreen.SetActive (true);
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				SuccessfulScreenText.text = "Your Character doest'n need to rest  ";
				break;
			case GameManager.Language.TraditionalChinese:
				SuccessfulScreenText.text = "你的所有角色暫時不用休息";
				break;
			}
			timer_alert.InitializeStart ();
			return;
		}

		if (GameManager.GM.Round < 3 && GameManager.GM.food >= foodneed) {
			for (int i = 0; i < PlayerCharacter.Length; i++) {
				PlayerCharacter [i].GetHeal ((int)(PlayerCharacter [i].Maxhp * 0.2f));
				PlayerCharacter [i].EnergyRecovery ((int)(PlayerCharacter [i].MaxEnergy * 0.2f));
			}
			GameManager.GM.food -= foodneed;
			SuccessfulScreen.SetActive (true);
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				SuccessfulScreenText.text = "Rest Successfully";
				break;
			case GameManager.Language.TraditionalChinese:
				SuccessfulScreenText.text = "休息成功";
				break;
			}
			GameManager.GM.AddRound(1);
			foodneed = 0;
			for (int i = 0; i < PlayerCharacter.Length; i++) {
				foodneed += ((PlayerCharacter [i].MaxCharacterPower - PlayerCharacter [i].CharacterPower)/2);
			}
			timer_alert.InitializeStart ();
			timer_MoveTocharData.InitializeStart ();
		} else if (GameManager.GM.Round < 3 && GameManager.GM.food < foodneed ) {
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				AlertScreenText.text = "You Don't have enough Food.";
				break;
			case GameManager.Language.TraditionalChinese:
				AlertScreenText.text = "你沒有足夠的食物.";
				break;
			}
			AlertScreen.SetActive (true);
			timer_alert.InitializeStart ();
		} else {
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				AlertScreenText.text = "You Don't have enough Turn Left!";
				break;
			case GameManager.Language.TraditionalChinese:
				AlertScreenText.text = "剩餘行動力不足!";
				break;
			}
			AlertScreen.SetActive (true);
			timer_alert.InitializeStart ();
		}
	}

	public void Training(){
		if (GameManager.GM.Round == 0 && GameManager.GM.TrainingCoolDown <= 0) {
			GameObject[] currentStageEnemy = GameObject.FindGameObjectsWithTag ("C" + GameManager.GM.Chapter + "_S" + GameManager.GM.Stage + "_Enemy");
			for (int i = 0; i < currentStageEnemy.Length; i++) {
				currentStageEnemy [i].transform.SetParent (GameObject.Find ("EnemyGroupList").transform.Find ("Chapter" + GameManager.GM.Chapter).Find ("Group" + GameManager.GM.Stage).transform);
			}
			enemyGroup.Reload ();
			for (int i = 0; i < PlayerCharacter.Length; i++) {
				PlayerCharacter [i].AddExp (enemyGroup.ExpReward);
				}
			SuccessfulScreen.SetActive (true);
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				SuccessfulScreenText.text = "Training Successfully, Gained Exp = " + enemyGroup.ExpReward;
				break;
			case GameManager.Language.TraditionalChinese:
				SuccessfulScreenText.text = "訓練成功， 已獲得經驗 = " + enemyGroup.ExpReward;
				break;
			}
			GameManager.GM.AddRound(3);
			GameManager.GM.AddTrainingCoolDown(2);
			timer_alert.InitializeStart ();
			timer_MoveTocharData.InitializeStart ();
		} else if(GameManager.GM.Round > 0 && GameManager.GM.TrainingCoolDown <= 2) {
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				AlertScreenText.text = "You Don't have enough Turn Left!";
				break;
			case GameManager.Language.TraditionalChinese:
				AlertScreenText.text = "剩餘行動力不足!";
				break;
			}
			AlertScreen.SetActive (true);
			timer_alert.InitializeStart ();
		} else if(GameManager.GM.Round == 0 && GameManager.GM.TrainingCoolDown > 0) {
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				AlertScreenText.text = "Training Cooldown has " + GameManager.GM.TrainingCoolDown + " round(s)";
				break;
			case GameManager.Language.TraditionalChinese:
				AlertScreenText.text = "訓練冷卻時間尚有 " + GameManager.GM.TrainingCoolDown + " 回合";
				break;
			}
			AlertScreen.SetActive (true);
			timer_alert.InitializeStart ();
		}
	}

	public void TrainingScreenShow(){
		TrainingScreen.SetActive (true);
	}

	public void TrainingScreenNotShow(){
		TrainingScreen.SetActive (false);
	}
}

