using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BayatGames.SaveGamePro;

public class PlayerGroup : MonoBehaviour {
	[Header("Group Properties")]
	public Character[] SupCharacter;
	[SerializeField] public MainCharacter MainCharacter;
	public float GroupPower;
	public int TotalCharacter;

	void Reset(){
		if(this.gameObject.transform.GetChild(0) != null)
			MainCharacter = this.gameObject.transform.GetChild (0).GetComponent<MainCharacter>();
	}

	void Start(){
		Reload ();
		TotalCharacter = SupCharacter.Length + 1;
		Debug.Log ("Total Character: " + TotalCharacter);
	}

	public void Awake(){
		//DontDestroyOnLoad(this.gameObject);
	}

	void Update(){
		if (GameManager.GM.gamestatus == GameManager.GameStatus.Battle) {
			Reload ();
		}
	}

	public void Reload(){
		GroupPower = 0;
		this.transform.SetAsLastSibling ();
		if (this.transform.childCount == 0) {
			MainCharacter = null;
		}
		if (MainCharacter != null) {
			SupCharacter = new Character[gameObject.transform.childCount - 1];
		}else SupCharacter = new Character[gameObject.transform.childCount];
		TotalCharacter = SupCharacter.Length + 1;
		try{
		if (this.gameObject.transform.GetChild (0) != null) {
			MainCharacter = this.gameObject.transform.GetChild (0).GetComponent<MainCharacter> ();
			if (GameManager.GM.gamestatus != GameManager.GameStatus.Battle) {
				MainCharacter.reloadAttributes (true);
			} else if (GameManager.GM.gamestatus == GameManager.GameStatus.Battle) {
				MainCharacter.reloadPower ();
			}
		}
		
		for (int i = 1; i < gameObject.transform.childCount; i++) {
			if (gameObject.transform.childCount > 1) {
				SupCharacter [i - 1] = gameObject.transform.GetChild (i).GetComponent<Character> ();
					if (GameManager.GM.gamestatus != GameManager.GameStatus.Battle) {
						SupCharacter [i - 1].reloadAttributes (true);
					} else if (GameManager.GM.gamestatus == GameManager.GameStatus.Battle) {
						SupCharacter [i - 1].reloadPower ();
					}
			}
		}

		for (int i = 0; i < SupCharacter.Length; i++) {
			if(SupCharacter[i] != null)
				GroupPower += SupCharacter [i].CharacterPower;
		}
		GroupPower += MainCharacter.CharacterPower;
		Mathf.RoundToInt (GroupPower);
		}
		catch (System.Exception e){

		}
	}
}
