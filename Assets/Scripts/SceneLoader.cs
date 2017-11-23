using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour {

    public Button[] button;

	// Use this for initialization
	void Start () {
        bool[] passed = Camera.main.GetComponent<CameraScript>().GetPassed();
        for(int i = 0; i < 3; i++) {
            if(passed[i]) {
                var colors = button[i].colors;
                colors.normalColor = Color.green;
                button[i].colors = colors;
            }
        }
	}

    public void ClickG1() {
        Camera.main.GetComponent<CameraScript>().PlayLevel(0);
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        Destroy(transform.gameObject.GetComponentInParent<Canvas>().gameObject);
    }

    public void ClickG2() {
        Camera.main.GetComponent<CameraScript>().PlayLevel(1);
        SceneManager.LoadScene("Level2", LoadSceneMode.Single);
        Destroy(transform.gameObject.GetComponentInParent<Canvas>().gameObject);
    }
    public void ClickG3() {
        Camera.main.GetComponent<CameraScript>().PlayLevel(2);
        SceneManager.LoadScene("Level3", LoadSceneMode.Single);
        Destroy(transform.gameObject.GetComponentInParent<Canvas>().gameObject);
    }
}
