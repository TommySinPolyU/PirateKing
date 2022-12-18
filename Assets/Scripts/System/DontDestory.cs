using System;
using UnityEngine;
	
public class DontDestoryOnLoadDefault : MonoBehaviour
{
	public static DontDestoryOnLoadDefault ThisObject;

	void Awake(){
		if (ThisObject == null) {
			ThisObject = this;
			DontDestroyOnLoad (this.gameObject);
		} else if (ThisObject != this) {
			Destroy (gameObject);
		}
	}
}


