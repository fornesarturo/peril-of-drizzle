using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void ClickG1() {
        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
        Destroy(transform.gameObject.GetComponentInParent<Canvas>().gameObject);
    }

    public void ClickG2() {
        SceneManager.LoadScene("Level2", LoadSceneMode.Additive);
        Destroy(transform.gameObject.GetComponentInParent<Canvas>().gameObject);
    }
    public void ClickG3() {
        SceneManager.LoadScene("Level3", LoadSceneMode.Additive);
        Destroy(transform.gameObject.GetComponentInParent<Canvas>().gameObject);
    }
}
