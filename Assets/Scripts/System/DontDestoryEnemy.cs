using System;
using UnityEngine;
	
public class DontDestoryEnemy : MonoBehaviour
{
	public static DontDestoryEnemy ThisObject;

	void Awake(){
		if (ThisObject == null) {
			ThisObject = this;
			DontDestroyOnLoadManager.DontDestroyOnLoad (this.gameObject);
		} else if (ThisObject != this) {
			Destroy (gameObject);
		}
	}
}


