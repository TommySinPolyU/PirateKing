using System;
using UnityEngine;
using UnityEngine.UI;

public class GameElement : MonoBehaviour{



	public static GameElement GE;
	public GameObject BattlePlace = null;
	public GameObject BattlePreparation = null;
	public GameManager.Language language = GameManager.Language.English;


	void Start(){
		GE = this;
	}

	void Update(){
		if (GameManager.GM.gamestatus == GameManager.GameStatus.NonBattle) {
			try{
				BattlePreparation = GameObject.Find ("BattlePreparation");
			}
			catch(System.Exception e){
			}
		}
		if(GameManager.GM.gamestatus == GameManager.GameStatus.Battle){
			try{
				BattlePlace = GameObject.Find ("BattlePlaceScreen");
			}
			catch(System.Exception e){
			}
		}
	}

}