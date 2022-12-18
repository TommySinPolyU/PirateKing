using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BayatGames.SaveGamePro;

public class GameManager : MonoBehaviour {

	public enum GameStatus{
		Battle,
		NonBattle,
		Ships,
		MainMenu,
		CharacterData
	}

	public enum BattleOutCome{
		Win,
		Lose,
		NotReady,
		OverLimit
	}

	public enum Language{
		TraditionalChinese,
		English
	}

	public enum Turn
	{
		Enemy,
		Player
	}

	public bool isLoadGame;
    private bool GMloaded;
	public string MainCharacterName;
	public Character.Role MainCharRole;
	public Vector3 InitPosition;

	public static GameManager GM;

	public Language language = Language.English;
	public Turn turn = Turn.Player;
	public int Round;


	public int TotalChapter;
	public int Chapter;
	public int TotalStage;
	public int[] TotalStageEachChapter = {9,9,9};
	public int Stage;
	public int GroupMemberNum;
	public int TryTime;
	public int itembuytime;
	public int TrainingCoolDown;

	public int money;
	public int food;

	public GameStatus gamestatus;
	public BattleOutCome battleOutcome = BattleOutCome.NotReady;

	[HideInInspector]public Vector3 MainCharBasicScale;
	[HideInInspector]public Vector3 SupCharBasicScale;



	public void Initialize(){
		Round = 0;
		Stage = 0;
		Chapter = 0;
		turn = Turn.Player;
		battleOutcome = BattleOutCome.NotReady;
		itembuytime = 0;
		money = 0;
		food = 0;
		TrainingCoolDown = 0;
		TryTime = 5;
		SaveGame.Save ("System/" + name, this);
		//isLoadGame = false;
	}

	public void Deitembuytime(int value){
		itembuytime -= value;
		SaveGame.Save ("System/" + name, this);
	}

	public void Additembuytime(int value){
		itembuytime += value;
		SaveGame.Save ("System/" + name, this);
	}

	public void DeTrainingCoolDown(int value){
		TrainingCoolDown -= value;
		SaveGame.Save ("System/" + name, this);
	}

	public void AddTrainingCoolDown(int value){
		TrainingCoolDown += value;
		SaveGame.Save ("System/" + name, this);
	}

	public void AddTryTime(int value){
		TryTime += value;
		SaveGame.Save ("System/" + name, this);
	}

	public void DeTryTime(int value){
		TryTime -= value;
		SaveGame.Save ("System/" + name, this);
	}

	public void AddStage(int value){
		Stage += value;
		SaveGame.Save ("System/" + name, this);
	}
		
	public void AddRound(int value){
		Round += value;
		SaveGame.Save ("System/" + name, this);
	}

	public void AddFood(int value){
		food += value;
		SaveGame.Save ("System/" + name, this);
	}

	public void AddMoney(int value){
		money += value;
		SaveGame.Save ("System/" + name, this);
	}

	public void DeFood(int value){
		food -= value;
		SaveGame.Save ("System/" + name, this);
	}

	public void DeMoney(int value){
		money -= value;
		SaveGame.Save ("System/" + name, this);
	}

	void Awake(){
		if (GM == null) {
			GM = this;
			DontDestroyOnLoad (gameObject);
		} else if ((GM != this)) {
			Destroy (gameObject);
		}
		TotalStage = 0;
		for (int i = 0; i < TotalStageEachChapter.Length; i++) {
			TotalStage += TotalStageEachChapter [i];
		}
		if (gamestatus == GameStatus.MainMenu)
			return;
		MainCharRole = GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").Find ("MainCharacter").gameObject.GetComponent<MainCharacter> ().role;
	}

	void Reset(){

		for (int i = 0; i < TotalStageEachChapter.Length; i++) {
			TotalStage += TotalStageEachChapter [i];
		}
	}

	void FixedUpdate(){
		if (isLoadGame) {
            if (!GMloaded)
            {
                SaveGame.LoadInto("System/" + name, this);
                GMloaded = true;
            }
			GameManager.GM.isLoadGame = true;
			GameManager.GM.language = GameElement.GE.language;
			return;
		}
		if (gamestatus != GameStatus.Battle) {
			battleOutcome = BattleOutCome.NotReady;
		}
	}

	public void changeScene(string ScenePath){
		SceneManager.LoadScene (ScenePath);
	}

	void Start(){
		Round = 0;
		turn = Turn.Player;
		battleOutcome = BattleOutCome.NotReady;
	}

}
