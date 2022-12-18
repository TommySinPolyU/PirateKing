using System;
using UnityEngine;
	
public class DontDestory : MonoBehaviour
{
	public static DontDestory ThisObject;

	void Awake(){
		if (ThisObject == null) {
			ThisObject = this;
			DontDestroyOnLoadManager.DontDestroyOnLoad (this.gameObject);
		} else if (ThisObject != this) {
			Destroy (gameObject);
		}
	}
}


