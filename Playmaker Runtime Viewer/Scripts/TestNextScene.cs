using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TestNextScene : MonoBehaviour {

	public KeyCode nextSceneKey = KeyCode.PageDown;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (nextSceneKey)) {
			int nowScene = SceneManager.GetActiveScene ().buildIndex;

			if (SceneManager.GetSceneAt (nowScene + 1) != null)
				SceneManager.LoadScene (nowScene + 1);
		}
	}
}
