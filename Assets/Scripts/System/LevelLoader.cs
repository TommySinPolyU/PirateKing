using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
	public GameObject loadingScreen;
	public Image ProcessBar;
	public Text ProcessBarText;


	public void LoadLevel(string sceneName){
		StartCoroutine(LoadAsynchronously(sceneName));
	}

	IEnumerator LoadAsynchronously(string sceneName){
		AsyncOperation operation = SceneManager.LoadSceneAsync (sceneName);
		loadingScreen.SetActive (true);
        switch (GameManager.GM.language)
        {
            case GameManager.Language.English:
                loadingScreen.transform.Find("Text").GetComponent<Text>().text = "Loading and Saving Game Data and Elements\n*The game process will save automatically.";
                break;
            case GameManager.Language.TraditionalChinese:
                loadingScreen.transform.Find("Text").GetComponent<Text>().text = "載入及保存遊戲數據中\n遊戲將會自動存檔";
                break;
        }
        while (!operation.isDone) {
			float progress = Mathf.Clamp01 (operation.progress / .9f);
			Debug.Log ("Loading Progress: " + ProcessBar.fillAmount);
			ProcessBar.fillAmount = progress;
			ProcessBarText.text = Mathf.RoundToInt(progress * 100f) + "%";
			yield return null;
		}
	}
}
