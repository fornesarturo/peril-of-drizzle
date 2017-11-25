using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour {

    public Button[] buttons;

	// Use this for initialization
	void Start () {
        for(int i = 1; i <= 3; i++) {
            if(PlayerPrefs.GetInt("Level"+i) == 1) {
                int j = i - 1;
                print(j);
                var colors = buttons[j].colors;
                colors.normalColor = Color.green;
                buttons[j].colors = colors;
            }
        }
	}

	void Update() {
		if (Input.GetKeyDown ("joystick " + 1 + " button 1") || Input.GetKeyDown ("joystick " + 2 + " button 1") || Input.GetKeyDown ("joystick " + 3 + " button 1") || Input.GetKeyDown ("joystick " + 4 + " button 1")) {
			SceneManager.LoadScene ("CharacterSelect", LoadSceneMode.Single);
		}
	}

    public void ClickG1() {
        PlayerPrefs.SetInt("LevelPlayed", 1);
		GameObject soundMenu = GameObject.FindGameObjectWithTag ("MenuSound");
		Destroy (soundMenu);
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    public void ClickG2() {
        PlayerPrefs.SetInt("LevelPlayed", 2);
		GameObject soundMenu = GameObject.FindGameObjectWithTag ("MenuSound");
		Destroy (soundMenu);
        SceneManager.LoadScene("Level2", LoadSceneMode.Single);
    }
    public void ClickG3() {
        PlayerPrefs.SetInt("LevelPlayed", 3);
		GameObject soundMenu = GameObject.FindGameObjectWithTag ("MenuSound");
		Destroy (soundMenu);
        SceneManager.LoadScene("Level3", LoadSceneMode.Single);
    }
}
