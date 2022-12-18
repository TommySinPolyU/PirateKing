using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour{
	public Character MainChar;
	public bool infoIsShow = false;
	private Vector3 PrevirousMousePosition;
	[Header("UI Object")]
	public Text ShopTitle;
	public Text Items1Text;
	public Text Items2Text;
	public GameObject[] ItemsImagePos;
	public GameObject ShopScreen;
	public GameObject InfoScreen;
	public Text InfoText;
	public GameObject ReturnBtn;



	public void LoadShopInfo(){
		ShopScreen.SetActive (true);
		MainChar = GameObject.Find ("PlayerCharList").transform.Find ("MainCharacter").GetComponent<Character> ();
		Debug.Log (MainChar);
	}

	void Update(){
		//Debug.Log ("Now " + Input.mousePosition);
		//Debug.Log ("Previous " + PrevirousMousePosition);
		switch (GameManager.GM.language) {
		case GameManager.Language.English:
			ShopTitle.text = "Shop";
			Items1Text.text = ""+((GameManager.GM.Stage + 1) * 5 + GameManager.GM.itembuytime * 5);
			Items2Text.text = ""+((GameManager.GM.Stage + 1) * 5 + GameManager.GM.itembuytime * 5);
			break;
		case GameManager.Language.TraditionalChinese:
			ShopTitle.text = "商店";
			Items1Text.text = ""+((GameManager.GM.Stage + 1) * 5 + GameManager.GM.itembuytime * 5);
			Items2Text.text = ""+((GameManager.GM.Stage + 1) * 5 + GameManager.GM.itembuytime * 5);
			break;
		}
		if(infoIsShow)
			InfoScreen.SetActive (true);
		else if(!infoIsShow)
			InfoScreen.SetActive (false);
	}

	public void ReturnToShip(){
		ShopScreen.SetActive (false);
	}

	public void ShowItemInfo(int ItemsIndex){
		if (!infoIsShow && Input.mousePosition != PrevirousMousePosition) {
			infoIsShow = true;
			PrevirousMousePosition = Input.mousePosition;
			InfoScreen.transform.position = new Vector3 (PrevirousMousePosition.x + 250f, PrevirousMousePosition.y, PrevirousMousePosition.z);
			if (ItemsIndex == 0) {
				switch (GameManager.GM.language) {
				case GameManager.Language.English:
					InfoText.text = "HP Potion\nRecovery Main Character 10% HP and increase 5 MaxHP";
					break;
				case GameManager.Language.TraditionalChinese:
					InfoText.text = "生命藥水\n恢復主角 10% 生命並增加 5點 最大生命";
					break;
				}
			}
			if (ItemsIndex == 1) {
				switch (GameManager.GM.language) {
				case GameManager.Language.English:
					InfoText.text = "Energy Potion\nRecovery Main Character 10% Energy and increase 5 MaxEnergy";
					break;
				case GameManager.Language.TraditionalChinese:
					InfoText.text = "能量藥水\n恢復主角 10% 能量並增加 5點 最大能量";
					break;
				}
			}
		}
	}

	public void NotShowItemInfo(){
		infoIsShow = false;
	}

	public void BuyItem(int ItemsIndex){
		int NeedMoney = ((GameManager.GM.Stage + 1) * 5 + GameManager.GM.itembuytime * 5);
		if (GameManager.GM.money >= NeedMoney) {
			if (ItemsIndex == 0) {
				GameManager.GM.DeMoney(NeedMoney);
				GameManager.GM.Additembuytime(1);
				MainChar.baseHp += 5;
				MainChar.CurrentHp += 5;
				MainChar.GetHeal ((int)(MainChar.Maxhp * 0.1));
				MainChar.reloadAttributes (true);
			}
			if (ItemsIndex == 1) {
				GameManager.GM.DeMoney(NeedMoney);
				GameManager.GM.Additembuytime(1);
				MainChar.baseEnergy += 5;
				MainChar.CurrentEnergy += 5;
				MainChar.EnergyRecovery ((int)(MainChar.MaxEnergy * 0.1));
				MainChar.reloadAttributes (true);
			}
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				InfoText.text = "Purchase Success";
				break;
			case GameManager.Language.TraditionalChinese:
				InfoText.text = "購買成功";
				break;
			}
		} else {
			switch (GameManager.GM.language) {
			case GameManager.Language.English:
				InfoText.text = "Failed Purchase, You Don't gave enough money";
				break;
			case GameManager.Language.TraditionalChinese:
				InfoText.text = "購買失敗, 你沒有足夠的金錢";
				break;
			}
		}
	}
}
