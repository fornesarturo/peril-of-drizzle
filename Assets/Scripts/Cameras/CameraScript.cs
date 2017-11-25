using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour {

    public Texture defeatImage;
    public Texture victoryImage;
    public Texture clearedImage;
    public Texture gameOverImage;

    public Text score;

    private GameObject[] players;
	private int prevLength;
	private Vector3 offset;
	private Camera cam;
    public int levelCoins;
    private bool showDefeat;
    private bool showVictory;
    private bool showCleared;
    private bool showGameOver;
    private bool fadeDone = false;
    private bool exiting = false;
    private float guiAlpha;

	// Use this for initialization
	void Start () {
        
		players = GameObject.FindGameObjectsWithTag ("PlayerTag");
		prevLength = players.Length;
        offset = new Vector3(0, 0, -10);
		cam = GetComponent<Camera>();
        showDefeat = false;
        showVictory = false;
    }
	
	// Update is called once per frame
	void Update () {

        if(levelCoins == 1) {
            score.text = "";
        }
        else {
            score.text = "Remaining coins: " + levelCoins;
        }
        

        if(levelCoins == 0) {
            StartCoroutine(ReturnTuMenuCleared());
        }
		if (players.Length > 1) {
			FixedCamera ();
		} else if (players.Length == 0 && !exiting) {
            if(SceneManager.GetActiveScene().name != "Menu" && SceneManager.GetActiveScene().name != "CharacterSelect") {
                for (int i = 1; i <= 3; i++) {
                    PlayerPrefs.SetInt("Level" + i, 0);
                }
                exiting = true;
                StartCoroutine(ReturnTuMenu());
            }
		}
        else if (players.Length == 1) {
            transform.position = players[0].transform.position + offset;
			cam.orthographicSize = 7f;
        }
	}

    public void FixedCamera() {
        float zoom = 1.5f;
        float followTime = 0.8f;
        Vector3 middle = new Vector3();
        foreach(GameObject go in players) {
            middle += go.transform.position;
        }
        middle = middle / (float)prevLength;
        float distance = 0;
        for(int i = 0; i < prevLength; i++) {
            for(int j = i+1; j < prevLength; j++) {
                float d = Vector3.Distance(players[i].transform.position, players[j].transform.position);
                if(d > distance) {
                    distance = d;
                }
            }
        }
        if (distance < 10) {
            distance = 10f;
        }
        if (distance > 30) {
            distance = 30f;
        }
        Vector3 cameraDest = middle - cam.transform.forward * distance * zoom;
        if (cam.orthographic) {
            cam.orthographicSize = distance;
        }
        cam.transform.position = Vector3.Slerp(cam.transform.position, cameraDest, followTime);
        if (Vector3.Distance(cameraDest, cam.transform.position) <= 0.05f) {
            cam.transform.position = cameraDest;
        }
    }

    public void TakeCoin() {
        levelCoins--;
    }

    public void SearchPlayers() {
        GameObject[] tempPlayers = GameObject.FindGameObjectsWithTag("PlayerTag");
        players = tempPlayers;
        prevLength = players.Length;
    }

    public IEnumerator ReturnTuMenuCleared() {
        
        PlayerPrefs.SetInt("Level"+PlayerPrefs.GetInt("LevelPlayed"), 1);
        bool allCleared = true;
        for(int i = 0; i < 3; i++) {
            if(PlayerPrefs.GetInt("Level"+(i+1)) == 0) {
                allCleared = false;
                break;
            }
        }
        if(allCleared) {
            killMinions();
            showVictory = true;
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene("Credits", LoadSceneMode.Single);
        }
        else {
            killMinions();
            showCleared = true;
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
        yield break;
    }

    public void killMinions() {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        foreach (GameObject spawner in spawners) {
            Destroy(spawner);
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyTag");
        foreach (GameObject enemy in enemies) {
            Component[] components = enemy.GetComponents<Component>();
            foreach (Component c in components) {
                if (c is Transform || c is SpriteRenderer || c is Animator) {
                    continue;
                }
                Destroy(c);
            }
            foreach (Transform t in enemy.transform) {
                Destroy(t.gameObject);
            }
        }
    }

    public IEnumerator ReturnTuMenu() {
        print("Tries:" + PlayerPrefs.GetInt("Tries"));
        if(PlayerPrefs.GetInt("Tries") > 1) {
            print("Repeat");
            PlayerPrefs.SetInt("Tries", PlayerPrefs.GetInt("Tries") - 1);
            showDefeat = true;
            for (int i = 1; i <= 4; i++) {
                if (PlayerPrefs.GetInt("PlayerSprite" + i) != -1) {
                    PlayerPrefs.SetInt("PlayerLife" + i, 20);
                }
            }
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
        else {
            print("Gameover");
            showGameOver = true;
            yield return new WaitForSeconds(5);
            SceneManager.LoadScene("Splashscreen", LoadSceneMode.Single);
        }
        yield break;
    }

    void OnGUI() {
        Color tempColor = GUI.color;
        tempColor.a = guiAlpha;
        GUI.color = tempColor;

        if(showDefeat) {
            if(!fadeDone) {
                StartCoroutine(GUIFade(0, 1, 2));
            }
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), defeatImage, ScaleMode.StretchToFill);
        }
        else if(showVictory) {
            if (!fadeDone) {
                StartCoroutine(GUIFade(0, 1, 2));
            }
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), victoryImage, ScaleMode.StretchToFill);
        }
        else if(showCleared) {
            if (!fadeDone) {
                StartCoroutine(GUIFade(0, 1, 2));
            }
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), clearedImage, ScaleMode.StretchToFill);
        }
        else if(showGameOver) {
            print("Showing GO");
            if (!fadeDone) {
                StartCoroutine(GUIFade(0, 1, 2));
            }
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), gameOverImage, ScaleMode.StretchToFill);
        }
    }

    IEnumerator GUIFade(float start, float end, float lenght) {
        fadeDone = true;
        for(float i = 0.0f; i < 1.0f; i += Time.deltaTime*(1/lenght)) {
            guiAlpha = Mathf.Lerp(start, end, i);
            yield return null;
        }
        guiAlpha = end;
        yield break;
    }
}
