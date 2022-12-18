using System;
using System.Collections.Generic;
using UnityEngine;

public static class DontDestroyOnLoadManager
{
    public static List<GameObject> _ddolObjects = new List<GameObject>();

    public static void DontDestroyOnLoad(this GameObject go) {
       UnityEngine.Object.DontDestroyOnLoad(go);
       _ddolObjects.Add(go);
		Debug.Log (go);
    }

    public static void DestroyAll() {
        foreach(var go in _ddolObjects)
            if(go != null)
                UnityEngine.Object.Destroy(go);

        _ddolObjects.Clear();
    }

	public static void SetNotActive(){
		foreach (var go in _ddolObjects)
			if (go != null) {
				Debug.Log (go);
				go.SetActive (false);
				go.transform.position = new Vector3 (go.transform.position.x, go.transform.position.y -500f, go.transform.position.z);
			}
	}

	public static void SetActive(){
		foreach (var go in _ddolObjects)
			if (go != null) {
				Debug.Log (go);
				go.SetActive (true);
			}
	}

	public static void CheckObject(GameObject check){
		foreach (var go in _ddolObjects)
			if (go != null) {
				if (check != go) {
					UnityEngine.Object.Destroy (check);
				}
			}
	}
}
