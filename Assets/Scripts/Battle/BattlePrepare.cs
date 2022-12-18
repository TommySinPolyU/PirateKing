using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattlePrepare : MonoBehaviour
{
	public enum StatusOfPositionBtn{
		Add,
		Remove
	}
	public LevelLoader Loader;
	public static BattlePrepare BP;

	public EnemyGroup EnemyGroup;
	public PlayerGroup PlayerGroup;

	public int selection = 0;
	public Character[] PlayerCharacter;

	[Header("UI Setting")]
	public GameObject TitleObject;
	public GameObject DataObject;
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
	public Text Position1Name;
	public Text Position2Name;
	public Text Position3Name;
	public Text Position4Name;
	public Text PlayerGroupPower;
	public Text EnemyGroupPower;
	public Text VS;
	public Text CharacterPower;
	public SimpleHealthBar ExpBar;
	public SimpleHealthBar LoadingBar;



	StatusOfPositionBtn statusOfPositionBtn;
	public Character[] CharacterNowGroupInPosition;
	public GameObject battlePlace;
	public GameObject LoadingScreen;
	public GameObject[] Btn;
	public GameObject[] CharacterPositionBtn;
	public GameObject[] CharacterPosition;
	public GameObject[] CharacterRawImage;



	private bool waitForResponse = false;
	private bool isInitialized = false;

	Timer timer;
	Timer timer_battle;
	Timer timer_return;

	void Awake(){
		GameManager.GM.gamestatus = GameManager.GameStatus.NonBattle;
	}

	void Start(){
		GameManager.GM.gamestatus = GameManager.GameStatus.NonBattle;
		GameObject tempTimer = new GameObject ();
		GameObject tempTimer1 = new GameObject ();
		GameObject tempTimer2 = new GameObject ();
		timer = tempTimer.AddComponent<Timer>();
		timer_battle = tempTimer1.AddComponent<Timer>();
		timer_return = tempTimer2.AddComponent<Timer>();
		timer.CancleTimer ();
		LoadingScreen.SetActive (false);
		Btn [3].SetActive (true);
		for (int i = 0; i < CharacterNowGroupInPosition.Length; i++) {
			if (CharacterNowGroupInPosition [i] != null) {
				CharacterNowGroupInPosition [i].gameObject.SetActive (true);
				switch (i) {
				case 1:
					CharacterNowGroupInPosition [i].gameObject.transform.position = CharacterPosition [i].transform.position;
					switch (GameManager.GM.language) {
					case GameManager.Language.English:
						Position2Name.text = CharacterNowGroupInPosition [i].charName;
						break;

					case GameManager.Language.TraditionalChinese:
						Position2Name.text = CharacterNowGroupInPosition [i].charName_CHT;
						break;
					}
					Position2Name.gameObject.SetActive (true);
					break;
				case 2:
					CharacterNowGroupInPosition [i].gameObject.transform.position = CharacterPosition [i].transform.position;
					switch (GameManager.GM.language) {
					case GameManager.Language.English:
						Position3Name.text = CharacterNowGroupInPosition [i].charName;
						break;

					case GameManager.Language.TraditionalChinese:
						Position3Name.text = CharacterNowGroupInPosition [i].charName_CHT;
						break;
					}
					Position3Name.gameObject.SetActive (true);
					break;
				case 3:
					CharacterNowGroupInPosition [i].gameObject.transform.position = CharacterPosition [i].transform.position;
					switch (GameManager.GM.language) {
					case GameManager.Language.English:
						Position4Name.text = CharacterNowGroupInPosition [i].charName;
						break;

					case GameManager.Language.TraditionalChinese:
						Position4Name.text = CharacterNowGroupInPosition [i].charName_CHT;
						break;
					}
					Position4Name.gameObject.SetActive (true);
					break;
				}
			}
		}
		//timer.InitializeStart ();
		BP = this;
		CharacterNowGroupInPosition = new Character[4];
		PlayerGroup = GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").GetComponent<PlayerGroup>();
		EnemyGroup = GameObject.Find ("EnemyGroupList").transform.Find("Chapter" + GameManager.GM.Chapter).transform.Find ("Group" + GameManager.GM.Stage).GetComponent<EnemyGroup>();

		PlayerGroup.Reload ();
		EnemyGroup.Reload ();
		EnemyGroupPower.text = EnemyGroup.GroupPower.ToString();
		PlayerGroupPower.text = PlayerGroup.GroupPower.ToString();
		for (int i = 0; i < Btn.Length; i++) {
			Btn [i].SetActive(false);
		}
		Btn [2].SetActive(true);
		Btn [3].SetActive(true);
		GameObject.Find ("PlayerCharList").transform.Find ("MainCharacter").SetParent (GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform);
		CharacterNowGroupInPosition [0] = GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform.Find ("MainCharacter").GetComponent<Character> ();	
		GameManager.GM.MainCharBasicScale = CharacterNowGroupInPosition [0].gameObject.transform.localScale;
		for (int i = 1; i < CharacterNowGroupInPosition.Length; i++) {
			if (GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform.childCount < (i + 1))
				break;
			CharacterNowGroupInPosition [i] = GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform.GetChild (i).GetComponent<Character> ();
			CharacterNowGroupInPosition [i].gameObject.SetActive (true);
		}
		GameManager.GM.GroupMemberNum = PlayerGroup.transform.childCount;

		PlayerCharacter = new Character[GameObject.Find ("PlayerCharList").transform.childCount - 1 + GameObject.Find ("PlayerCharList").transform.Find("NowGroup").childCount];
		PlayerCharacter [0] = GameObject.Find ("PlayerCharList").transform.Find("NowGroup").transform.Find("MainCharacter").GetComponent<Character>();
		CharacterRawImage [0].SetActive (true);
		CharacterRawImage [4].SetActive (false);
		int AddCount = 0;
		for (int i = 1; i < (GameObject.Find ("PlayerCharList").transform.childCount - 1 + GameObject.Find ("PlayerCharList").transform.Find("NowGroup").childCount); i++) {
			if (GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").childCount - 1 >= 1 && AddCount < GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").childCount - 1) {
				PlayerCharacter [i] = GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").transform.GetChild (i).GetComponent<Character> ();
				PlayerCharacter [i].gameObject.SetActive (false);
				AddCount++;

			} else {
				PlayerCharacter [i] = GameObject.Find ("PlayerCharList").transform.GetChild (i - (GameObject.Find ("PlayerCharList").transform.Find ("NowGroup").childCount)).GetComponent<Character> ();
				PlayerCharacter [i].gameObject.transform.position = CharacterPosition [4].transform.position;
				PlayerCharacter [i].gameObject.SetActive (false);
				PlayerCharacter [i].isInGroup = false;
				CharacterRawImage [i].SetActive (false);
			}
		}


		if (PlayerCharacter [selection].isInGroup)
			PlayerCharacter [selection].SelectorShow ();


		if (selection == 0) {
			PlayerCharacter [selection].gameObject.SetActive (true);
			PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().reloadAttributes (false);
			reloadTextMain ();
			PlayerCharacter [selection].gameObject.transform.position = CharacterPosition [0].transform.position;
		}
		else {
			PlayerCharacter [selection].reloadAttributes (false);
			reloadTextSub ();
		}
		isInitialized = true;
	
	}

	void FixedUpdate(){
		GameManager.GM.gamestatus = GameManager.GameStatus.NonBattle;

		//if (timer.isStarted)
		//	LoadingBar.UpdateBar (timer.timerTime, 1.5f);
		//if (timer_battle.isStarted)
		//	LoadingBar.UpdateBar (timer_battle.timerTime, 3.0f);
		//if (timer_return.isStarted)
		//	LoadingBar.UpdateBar (timer_return.timerTime, 2.0f);

		//if (timer.CheckTime (1.5f)) {

		//}

		//if(timer_battle.CheckTime(3.0f)){
		//	timer_battle.CancleTimer ();
		//	Loader.LoadLevel ("BattlePlace");
		//}

		//if (timer_return.CheckTime (2.0f)) {
		//	timer_return.CancleTimer ();
			//DontDestroyOnLoadManager.SetNotActive ();
			//SceneManager.LoadScene ("Ships");
			//SceneManager.LoadSceneAsync ("Ships");
		//	Loader.LoadLevel ("Ships");
		//}

	}

	public void selectionNext(){
		if (selection < PlayerCharacter.Length - 1) {
			ExpBar.UpdateBar (PlayerCharacter [selection].Exp, PlayerCharacter [selection].ExpRequirment);
			if (PlayerCharacter [selection] == PlayerCharacter [0]) {
				Btn [0].SetActive (false);
				Btn [1].SetActive (false);
				PlayerCharacter [selection].gameObject.SetActive (true);
				PlayerCharacter [selection + 1].gameObject.SetActive (true);
				CharacterRawImage [4].SetActive (false);
			} else {
				PlayerCharacter [selection].gameObject.SetActive (false);
				PlayerCharacter [selection + 1].gameObject.SetActive (true);
				CharacterRawImage [4].SetActive (true);
			}

			if (PlayerCharacter [selection].isInGroup) {
				Btn [1].SetActive (true);
				PlayerCharacter [selection].SelectorNotShow ();
				PlayerCharacter [selection].gameObject.SetActive (true);
			}
			else Btn [1].SetActive (false);

			if (!PlayerCharacter [selection].isInGroup) {
				Btn [0].SetActive (true);
			}
			else Btn [0].SetActive (false);

			selection++;

			if (PlayerCharacter [selection].isInGroup) {
				Btn [1].SetActive (true);
				PlayerCharacter [selection].SelectorShow ();
				CharacterRawImage [4].SetActive (false);
			}
			else Btn [1].SetActive (false);

			if (!PlayerCharacter [selection].isInGroup) {
				Btn [0].SetActive (true);
				PlayerCharacter [selection].SelectorNotShow ();
				CharacterRawImage [4].SetActive (true);
			}
			else Btn [0].SetActive (false);


			
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
		NotShowButton ();
	}

	public void selectionPrevious(){
		if (selection > 0) {
			PlayerCharacter [selection].gameObject.SetActive (false);
			ExpBar.UpdateBar (PlayerCharacter [selection].Exp, PlayerCharacter [selection].ExpRequirment);

			if (PlayerCharacter [selection].isInGroup) {
				Btn [1].SetActive (true);
				PlayerCharacter [selection].SelectorNotShow ();
				PlayerCharacter [selection].gameObject.SetActive (true);
				CharacterRawImage [4].SetActive (false);
			}
			else Btn [1].SetActive (false);

			if (!PlayerCharacter [selection].isInGroup) {
				Btn [0].SetActive (true);
				CharacterRawImage [4].SetActive (true);
			}
			else Btn [0].SetActive (false);

			selection--;

			PlayerCharacter [selection].gameObject.SetActive (true);
			NAME.text = PlayerCharacter [selection].charName;


			if (PlayerCharacter [selection].isInGroup) {
				Btn [1].SetActive (true);
				PlayerCharacter [selection].SelectorShow ();
				CharacterRawImage [4].SetActive (false);
			}
			else Btn [1].SetActive (false);

			if (!PlayerCharacter [selection].isInGroup) {
				Btn [0].SetActive (true);
				PlayerCharacter [selection].SelectorNotShow ();
				CharacterRawImage [4].SetActive (true);
			}
			else Btn [0].SetActive (false);



			if (selection == 0) {
				Btn [0].SetActive (false);
				Btn [1].SetActive (false);
				CharacterRawImage [4].SetActive (false);
				PlayerCharacter [selection].gameObject.GetComponent<MainCharacter> ().reloadAttributes (false);
				reloadTextMain ();
			}
			else {
				PlayerCharacter [selection].reloadAttributes (false);
				reloadTextSub ();
			}
		}
		NotShowButton ();
	}

	public void ShowAddButton(){
		statusOfPositionBtn = StatusOfPositionBtn.Add;
		for (int i = 0; i < CharacterPositionBtn.Length; i++) {
			CharacterPositionBtn [i].SetActive (true);
		}
	}

	public void NotShowButton(){
		for (int i = 0; i < CharacterPositionBtn.Length; i++) {
			CharacterPositionBtn [i].SetActive (false);
		}
	}

	public void ShowRemoveButton(){
		statusOfPositionBtn = StatusOfPositionBtn.Remove;
		for (int i = 0; i < CharacterPositionBtn.Length; i++) {
			CharacterPositionBtn [i].SetActive (true);
		}
	}

	public void HandlePositionBtn(int position){
		switch (statusOfPositionBtn) {
		case StatusOfPositionBtn.Add:
			AddToGroup (position);
			break;
		}
	}

	public void AddToGroup(int position){
		Character character = PlayerCharacter [selection];
		character.gameObject.transform.SetParent (PlayerGroup.gameObject.transform);

		if (CharacterNowGroupInPosition[position] != null)
			RemoveFromGroup (position);
		
		CharacterNowGroupInPosition [position] = character;
		PlayerGroup.Reload ();
		GameManager.GM.GroupMemberNum++;
		Debug.Log ("GroupMember" + GameManager.GM.GroupMemberNum);
		character.isInGroup = true;
		character.gameObject.transform.position = CharacterPosition [position].transform.position;
		CharacterRawImage [4].SetActive (false);
		switch (position) {
		case 1:
			GameManager.GM.SupCharBasicScale = CharacterNowGroupInPosition [1].gameObject.transform.localScale;
			reloadTextSub ();
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				Position2Name.text = character.charName;
				break;

			case GameManager.Language.TraditionalChinese:
				Position2Name.text = character.charName_CHT;
				break;
			}
			Position2Name.gameObject.SetActive (true);
			CharacterRawImage [1].SetActive (true);
			break;
		case 2:
			GameManager.GM.SupCharBasicScale = CharacterNowGroupInPosition [2].gameObject.transform.localScale;
			reloadTextSub ();
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				Position3Name.text = character.charName;
				break;

			case GameManager.Language.TraditionalChinese:
				Position3Name.text = character.charName_CHT;
				break;
			}
			Position3Name.gameObject.SetActive (true);
			CharacterRawImage [2].SetActive (true);
			break;
		case 3:
			GameManager.GM.SupCharBasicScale = CharacterNowGroupInPosition [3].gameObject.transform.localScale;
			reloadTextSub ();
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				Position4Name.text = character.charName;
				break;

			case GameManager.Language.TraditionalChinese:
				Position4Name.text = character.charName_CHT;
				break;
			}
			Position4Name.gameObject.SetActive (true);
			CharacterRawImage [3].SetActive (true);
			break;
		}
		NotShowButton ();
		Btn [0].SetActive (false);
		Btn [1].SetActive (true);
		PlayerGroup.transform.SetAsLastSibling();
		PlayerGroup.Reload ();
		EnemyGroup.Reload ();
		EnemyGroupPower.text = EnemyGroup.GroupPower.ToString ();
		PlayerGroupPower.text = PlayerGroup.GroupPower.ToString ();
		ExpBar.UpdateBar (PlayerCharacter [selection].Exp, PlayerCharacter [selection].ExpRequirment);

	}

	public void RemoveFromGroup(int position){
		CharacterNowGroupInPosition [position].isInGroup = false;
		GameManager.GM.GroupMemberNum--;
		Debug.Log ("GroupMember " + GameManager.GM.GroupMemberNum);
		CharacterNowGroupInPosition [position].gameObject.transform.position = CharacterPosition [4].transform.position;
		//character.gameObject.transform.position = CharacterPosition [4].transform.position;
		CharacterNowGroupInPosition [position].gameObject.transform.SetParent (GameObject.Find ("PlayerCharList").transform);
		CharacterNowGroupInPosition [position].gameObject.SetActive (false);
		CharacterNowGroupInPosition [position] = null;
		PlayerGroup.Reload ();
		CharacterRawImage [4].SetActive (true);

		switch (position) {
		case 1:
			reloadTextSub ();
			Position2Name.text = "";
			Position2Name.gameObject.SetActive (false);
			CharacterRawImage [1].SetActive (false);
			break;
		case 2:
			reloadTextSub ();
			Position3Name.text = "";
			Position3Name.gameObject.SetActive (false);
			CharacterRawImage [2].SetActive (false);
			break;
		case 3:
			reloadTextSub ();
			Position4Name.text = "";
			Position4Name.gameObject.SetActive (false);
			CharacterRawImage [3].SetActive (false);
			break;
		}
		Btn [0].SetActive (true);
		Btn [1].SetActive (false);
		PlayerGroup.transform.SetAsLastSibling();
		PlayerGroup.Reload ();
		EnemyGroup.Reload ();
		EnemyGroupPower.text = EnemyGroup.GroupPower.ToString ();
		PlayerGroupPower.text = PlayerGroup.GroupPower.ToString ();
		ExpBar.UpdateBar (PlayerCharacter [selection].Exp, PlayerCharacter [selection].ExpRequirment);
	}
		
	public void RemoveFromGroupWithoutPosition(){
		Character character = PlayerCharacter [selection];
		int position = ReturnPosition ();

		character.isInGroup = false;
		GameManager.GM.GroupMemberNum--;
		character.gameObject.transform.position = CharacterPosition [4].transform.position;
		character.gameObject.transform.SetParent (GameObject.Find ("PlayerCharList").transform);
		CharacterNowGroupInPosition [position] = null;
		PlayerGroup.Reload ();
		CharacterRawImage [4].SetActive (true);
		switch (position) {
		case 1:
			reloadTextSub ();
			Position2Name.text = "";
			Position2Name.gameObject.SetActive (false);
			CharacterRawImage [1].SetActive (false);
			break;
		case 2:
			reloadTextSub ();
			Position3Name.text = "";
			Position3Name.gameObject.SetActive (false);
			CharacterRawImage [2].SetActive (false);
			break;
		case 3:
			reloadTextSub ();
			Position4Name.text = "";
			Position4Name.gameObject.SetActive (false);
			CharacterRawImage [3].SetActive (false);
			break;
		}
		Btn [0].SetActive (true);
		Btn [1].SetActive (false);
		PlayerGroup.transform.SetAsLastSibling();
		PlayerGroup.Reload ();
		EnemyGroup.Reload ();
		EnemyGroupPower.text = EnemyGroup.GroupPower.ToString ();
		PlayerGroupPower.text = PlayerGroup.GroupPower.ToString ();
		ExpBar.UpdateBar (PlayerCharacter [selection].Exp, PlayerCharacter [selection].ExpRequirment);
	}

	public int ReturnPosition(){
		int pointer = 0;
		for (int i = 0; i < CharacterNowGroupInPosition.Length; i++) {
			if (CharacterNowGroupInPosition [i] != PlayerCharacter [selection]) {
				pointer++;
				Debug.Log (pointer);
			} else {
				Debug.Log ("Return: " + pointer);
				return pointer;
			}
		}
		return 0;
	}

	public void reloadTextMain(){
		PlayerCharacter [selection].GetComponent<MainCharacter> ().SkillTree = GameObject.Find ("SkillTree").GetComponent<SkillTree>();
		switch (GameManager.GM.language) {
		case GameManager.Language.English:
			Title.text = "Battle Preparation";
			VS.text = "VS";
			CharacterPower.text = "Power: " + PlayerCharacter [selection].CharacterPower;
			Btn [0].transform.GetChild(0).GetComponent<Text> ().text = "Add To Group";
			Btn [1].transform.GetChild(0).GetComponent<Text> ().text = "Remove From Group";
			Btn [2].transform.GetChild(0).GetComponent<Text> ().text = "Start Battle" + "\n-1 Turn";
			//Btn [3].transform.GetChild(0).GetComponent<Text> ().text = "Return to Map";
			CharacterPositionBtn [0].transform.GetChild(0).GetComponent<Text> ().text = "Add";
			CharacterPositionBtn [1].transform.GetChild(0).GetComponent<Text> ().text = "Add";
			CharacterPositionBtn [2].transform.GetChild(0).GetComponent<Text> ().text = "Add";
			NAME.text = PlayerCharacter [selection].charName;
			Position1Name.text = PlayerCharacter [selection].charName;
			if(CharacterNowGroupInPosition [1] != null)
				Position2Name.text = CharacterNowGroupInPosition[1].charName;
			if(CharacterNowGroupInPosition [2] != null)
				Position3Name.text = CharacterNowGroupInPosition[2].charName;
			if(CharacterNowGroupInPosition [3] != null)
				Position4Name.text = CharacterNowGroupInPosition[3].charName;
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
			break;
		case GameManager.Language.TraditionalChinese:
			Title.text = "戰鬥準備";
			VS.text = "對戰";
			CharacterPower.text = "戰力: " + PlayerCharacter [selection].CharacterPower;
			Btn [0].transform.GetChild(0).GetComponent<Text> ().text = "加入隊伍";
			Btn [1].transform.GetChild(0).GetComponent<Text> ().text = "移出隊伍";
			Btn [2].transform.GetChild(0).GetComponent<Text> ().text = "開始戰鬥" + "\n-1 行動點";
			//Btn [3].transform.GetChild(0).GetComponent<Text> ().text = "返回地圖";
			CharacterPositionBtn [0].transform.GetChild(0).GetComponent<Text> ().text = "加入";
			CharacterPositionBtn [1].transform.GetChild(0).GetComponent<Text> ().text = "加入";
			CharacterPositionBtn [2].transform.GetChild(0).GetComponent<Text> ().text = "加入";
			NAME.text = PlayerCharacter [selection].charName_CHT;
			Position1Name.text = PlayerCharacter [selection].charName_CHT;
			if(CharacterNowGroupInPosition [1] != null)
				Position2Name.text = CharacterNowGroupInPosition[1].charName_CHT;
			if(CharacterNowGroupInPosition [2] != null)
				Position3Name.text = CharacterNowGroupInPosition[2].charName_CHT;
			if(CharacterNowGroupInPosition [3] != null)
				Position4Name.text = CharacterNowGroupInPosition[3].charName_CHT;
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
			break;
		}
		PlayerGroup.Reload ();
		EnemyGroup.Reload ();
		EnemyGroupPower.text = EnemyGroup.GroupPower.ToString ();
		PlayerGroupPower.text = PlayerGroup.GroupPower.ToString ();
		ExpBar.UpdateBar (PlayerCharacter [selection].Exp, PlayerCharacter [selection].ExpRequirment);
	}

	public void reloadTextSub(){
		switch (GameManager.GM.language) {
		case GameManager.Language.English:
			Title.text = "Battle Preparation";
			VS.text = "VS";
			CharacterPower.text = "Power: " + PlayerCharacter [selection].CharacterPower;
			Btn [0].transform.GetChild(0).GetComponent<Text> ().text = "Add To Group";
			Btn [1].transform.GetChild(0).GetComponent<Text> ().text = "Remove From Group";
			Btn [2].transform.GetChild(0).GetComponent<Text> ().text = "Start Battle" + "\n-1 Turn";;
			//Btn [3].transform.GetChild(0).GetComponent<Text> ().text = "Return to Map";
			CharacterPositionBtn [0].transform.GetChild(0).GetComponent<Text> ().text = "Add";
			CharacterPositionBtn [1].transform.GetChild(0).GetComponent<Text> ().text = "Add";
			CharacterPositionBtn [2].transform.GetChild(0).GetComponent<Text> ().text = "Add";
			NAME.text = PlayerCharacter [selection].charName;
			LV.text = "LV: " + PlayerCharacter [selection].Lv;
			if(CharacterNowGroupInPosition [1] != null)
				Position1Name.text = CharacterNowGroupInPosition[0].charName;
			if(CharacterNowGroupInPosition [1] != null)
				Position2Name.text = CharacterNowGroupInPosition[1].charName;
			if(CharacterNowGroupInPosition [2] != null)
				Position3Name.text = CharacterNowGroupInPosition[2].charName;
			if(CharacterNowGroupInPosition [3] != null)
				Position4Name.text = CharacterNowGroupInPosition[3].charName;
			ATK.text = "ATK: " + "<color=black>" +  PlayerCharacter [selection].baseAtk +  "</color> + " + "<color=green>"  + (PlayerCharacter [selection].totalgrowATK) + "</color>" ;
			DEF.text = "DEF: " + "<color=black>" +  PlayerCharacter [selection].baseDef +  "</color> + " + "<color=green>"  + (PlayerCharacter [selection].totalgrowDEF) + "</color>" ;
			SPEED.text = "Speed: " + "<color=black>" +  PlayerCharacter [selection].movspeed + "</color> s / Movement";
			LOAD.text = "Load: " + "<color=black>" +  PlayerCharacter [selection].baseLoad +  "</color> + " + "<color=green>"  + (PlayerCharacter [selection].totalgrowLOAD) + "</color>" ;
			CRITRATE.text = "CriticalRate: " + "<color=black>" + PlayerCharacter [selection].cR + "</color>%";
			CRITDAM.text = "CriticalDamage: " + "<color=black>" + PlayerCharacter [selection].cD + "</color>%";
			HP.text = "HP: " + "<color=black>" + PlayerCharacter [selection].CurrentHp + "</color> / " + "<color=red>"  + (PlayerCharacter [selection].Maxhp) + "</color>" ;
			Energy.text = "Energy: " + "<color=black>" + PlayerCharacter [selection].CurrentEnergy + "</color> / " + "<color=red>"  + (PlayerCharacter [selection].MaxEnergy) + "</color>" ;			
			break;
		case GameManager.Language.TraditionalChinese:
			Title.text = "戰鬥準備";
			VS.text = "對戰";
			CharacterPower.text = "戰力: " + PlayerCharacter [selection].CharacterPower;
			Btn [0].transform.GetChild(0).GetComponent<Text> ().text = "加入隊伍";
			Btn [1].transform.GetChild(0).GetComponent<Text> ().text = "移出隊伍";
			Btn [2].transform.GetChild(0).GetComponent<Text> ().text = "開始戰鬥" + "\n-1 行動點";
			//Btn [3].transform.GetChild(0).GetComponent<Text> ().text = "返回地圖";
			CharacterPositionBtn [0].transform.GetChild(0).GetComponent<Text> ().text = "加入";
			CharacterPositionBtn [1].transform.GetChild(0).GetComponent<Text> ().text= "加入";
			CharacterPositionBtn [2].transform.GetChild(0).GetComponent<Text> ().text = "加入";
			if(CharacterNowGroupInPosition [1] != null)
				Position1Name.text = CharacterNowGroupInPosition[0].charName_CHT;
			if(CharacterNowGroupInPosition [1] != null)
				Position2Name.text = CharacterNowGroupInPosition[1].charName_CHT;
			if(CharacterNowGroupInPosition [2] != null)
				Position3Name.text = CharacterNowGroupInPosition[2].charName_CHT;
			if(CharacterNowGroupInPosition [3] != null)
				Position4Name.text = CharacterNowGroupInPosition[3].charName_CHT;
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
			break;
		}
		PlayerGroup.Reload ();
		EnemyGroup.Reload ();
		EnemyGroupPower.text = EnemyGroup.GroupPower.ToString ();
		PlayerGroupPower.text = PlayerGroup.GroupPower.ToString ();
		ExpBar.UpdateBar (PlayerCharacter [selection].Exp, PlayerCharacter [selection].ExpRequirment);
	}


	public void StartBattle(){
		//LoadingScreen.SetActive (true);
		GameManager.GM.AddRound(1);
		Loader.LoadLevel ("BattlePlace");
	}

	public void ReturnToShip(){
		LoadingScreen.SetActive (true);
		for (int i = 0; i < CharacterNowGroupInPosition.Length; i++) {
			try{
			CharacterNowGroupInPosition[i].isInGroup = false;
			CharacterNowGroupInPosition[i].gameObject.transform.SetParent(GameObject.Find("PlayerCharList").transform);
			}
			catch(System.Exception e){
				Debug.Log("Character Not Found in " + i + " Position.");
			}
		}
		Loader.LoadLevel ("Ships");
		//timer_return.InitializeStart ();
	}

}

