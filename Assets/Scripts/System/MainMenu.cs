using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BayatGames.SaveGamePro;
using System.IO;

public class MainMenu : MonoBehaviour{
	public LevelLoader Loader;
	public Text GameTitle;
	public Text StartBtnText;
	public Text SettingBtnText;
	public Text ExitBtnText;
	public Button LoadBtn;
	public Text LoadBtnText;
	public GameObject MainUI;

	[Header("Character UI Setting")]
	public Text CharacterUISettingTitle;
	public Text EnterToWorldText;
	public Text AlertText;
	public Text NameSetTitle;
	public InputField NameInput;
	public GameObject PlayBtn;
	public GameObject ReturnBtn;
	public GameObject CharacterUI;
	[Header("Setting UI Setting")]
	public Text SettingTitle;
	public Text LanguageSetTitle;
	public GameObject SettingUI;
	public Dropdown LanguageSelection;

	private bool waitForRespond = false;
	private int ClickedTime = 0;

	GameObject tempTimerGameObject;
	Timer tempTimer;

	void Start(){
		GameManager.GM.isLoadGame = false;
	}

	public void LoadGame(){
		GameManager.GM.isLoadGame = true;
		StartGame ();
	}

	void Update(){
		GameManager.GM.gamestatus = GameManager.GameStatus.MainMenu;
		if (SaveGame.Exists ("Character") || SaveGame.Exists ("SkillTree")) {
			LoadBtn.gameObject.SetActive (true);
		} else {
			LoadBtn.gameObject.SetActive (false);
		}
		if (ClickedTime >= 3 && tempTimer.CheckTime (1.5f)) {
			AlertText.gameObject.SetActive (false);
			CharacterUI.gameObject.SetActive (false);
			MainUI.gameObject.SetActive (true);
			ClickedTime = 0;
			tempTimer.CancleTimer ();
			Destroy(tempTimerGameObject);
		}
		switch (GameManager.GM.language) {
		case GameManager.Language.English:
			GameTitle.text = "Pirate King";
			StartBtnText.text = "Start New Game";
			SettingBtnText.text = "Setting";
			ExitBtnText.text = "Exit";
			SettingTitle.text = "Setting";
			LoadBtnText.text = "Load Game";
			LanguageSetTitle.text = "Language";
			CharacterUISettingTitle.text = "Main Character Setting";
			NameSetTitle.text = "Name";
			NameInput.placeholder.GetComponent<Text> ().text = "Enter Your Name...";
			LanguageSelection.value = 0;
			//EnterToWorldText.text = "Enter To World";
			break;
		case GameManager.Language.TraditionalChinese:
			GameTitle.text = "海盜霸主";
			StartBtnText.text = "開新遊戲";
			LoadBtnText.text = "讀取遊戲";
			SettingBtnText.text = "設定";
			ExitBtnText.text = "退出遊戲";
			SettingTitle.text = "設定";
			LanguageSetTitle.text = "語言";
			NameSetTitle.text = "名字";
			CharacterUISettingTitle.text = "主角設定";
			NameInput.placeholder.GetComponent<Text>().text = "請輸入你的名字...";
			LanguageSelection.value = 1;
			//EnterToWorldText.text = "進入世界";
			break;
		}
	}

	public void HandleLanguageSelection(){
		if (LanguageSelection.value == 0) {
			GameManager.GM.language = GameManager.Language.English;
			GameElement.GE.language = GameManager.Language.English;
		}
		if (LanguageSelection.value == 1) {
			GameManager.GM.language = GameManager.Language.TraditionalChinese;
			GameElement.GE.language = GameManager.Language.TraditionalChinese;
		}
	}

	public void HandleCharacterNameInput(){
		if (NameInput.text.Length < 6){
			AlertText.gameObject.SetActive (true);
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				AlertText.text = "Please Input at least 6 Character.";
				break;
			case GameManager.Language.TraditionalChinese:
				AlertText.text = "請輸入最少 6 個字符。";
				break;
			}
		}else 
			AlertText.gameObject.SetActive (false);	
	}

	public void ShowSetting(){
		MainUI.SetActive (false);
		SettingUI.SetActive (true);
	}

	public void NotShowSetting(){
		MainUI.SetActive (true);
		SettingUI.SetActive (false);
	}

	public void ShowCharacterSetting(){
		if (GameManager.GM.MainCharacterName != "" || GameManager.GM.MainCharacterName.Length >= 6) {
			NameInput.text = GameManager.GM.MainCharacterName;
			//StartGame();
		}
		MainUI.SetActive (false);
		CharacterUI.SetActive (true);
		PlayBtn.SetActive (true);
		ReturnBtn.SetActive (true);
	}

	public void NotShowCharacterSetting(){
		MainUI.SetActive (true);
		CharacterUI.SetActive (false);
	}

	public void StartGame(){
		if (waitForRespond) {
			AlertText.gameObject.SetActive (true);
			ClickedTime++;
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				AlertText.text = "Warning: \nThe System detected a game save data on your hard drive\n If you want to clear the data, please click the 'play' button 3 times.\nClickedTime: " + ClickedTime;
				break;
			case GameManager.Language.TraditionalChinese:
				AlertText.text = "警告：\n系統於你的硬碟內偵測到已有存檔。\n如果你想清除存檔，請按三次 '開始' 按鈕。\n已按下次數：" + ClickedTime;
				break;
			}
			if (ClickedTime >= 3) {
				tempTimerGameObject = new GameObject ();
				tempTimer = tempTimerGameObject.AddComponent<Timer>();
				tempTimer.InitializeStart ();
				switch (GameManager.GM.language) {
				case GameManager.Language.English:
					AlertText.text = "Game Save Data has been deleted.";
					break;
				case GameManager.Language.TraditionalChinese:
					AlertText.text = "存檔已被清除。";
					break;
				}
				waitForRespond = false;
				SaveGame.Delete ( "Character" );
				SaveGame.Delete ( "System" );
				SaveGame.Delete ( "SkillTree" );
				try{
					Destroy(GameObject.Find("PlayerCharList"));
					Destroy(GameObject.Find("EnemyGroupList"));
					Destroy(GameObject.Find("SkillTree"));
					DontDestroyOnLoadManager._ddolObjects.Clear();
				}catch (System.Exception e){
					Debug.Log ("Can't Find Object.");
				}
				PlayBtn.SetActive (false);
				ReturnBtn.SetActive (false);
			}
			return;
		}

		if (GameManager.GM.isLoadGame) {
			GameManager.GM.gamestatus = GameManager.GameStatus.Ships;
			//SceneManager.LoadScene ("Ships");
			Loader.LoadLevel ("Ships");
			GameManager.GM.isLoadGame = true;
		}
		if (NameInput.text.Length < 6) {
			AlertText.gameObject.SetActive (true);
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				AlertText.text = "Please Input at least 6 Character.";
				break;
			case GameManager.Language.TraditionalChinese:
				AlertText.text = "請輸入最少 6 個字符。";
				break;
			}
			return;
		} else {
			if (SaveGame.Exists("Character") || SaveGame.Exists("SkillTree")) {
				AlertText.gameObject.SetActive (true);
				switch (GameManager.GM.language) {
				case GameManager.Language.English:
					AlertText.text = "Warning:\n The System detected a game save data on your hard drive\n If you want to clean the data, please click the 'play' button 3 times.";
					break;
				case GameManager.Language.TraditionalChinese:
					AlertText.text = "警告：\n系統於你的硬碟內偵測到已有存檔。\n如果你想清除存檔，請按三次 '開始' 按鈕。";
					break;
				}
				waitForRespond = true;
				return;
			} else {
				GameManager.GM.MainCharacterName = NameInput.text;
				AlertText.gameObject.SetActive (false);	
				GameManager.GM.gamestatus = GameManager.GameStatus.Ships;
				//SceneManager.LoadScene ("Ships");
				//SceneManager.LoadSceneAsync("Ships");
				Loader.LoadLevel("Ships");
				GameManager.GM.Initialize ();
			
			}
		}
	}

	public void ExitGame(){
		Application.Quit ();
		//UnityEditor.EditorApplication.isPlaying = false;
	}
		
}