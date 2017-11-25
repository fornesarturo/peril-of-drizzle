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

    public void ClickG1() {
        PlayerPrefs.SetInt("LevelPlayed", 1);
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    public void ClickG2() {
        PlayerPrefs.SetInt("LevelPlayed", 2);
        SceneManager.LoadScene("Level2", LoadSceneMode.Single);
    }
    public void ClickG3() {
        PlayerPrefs.SetInt("LevelPlayed", 3);
        SceneManager.LoadScene("Level3", LoadSceneMode.Single);
    }
}
