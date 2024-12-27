using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    private GameObject cam;
    private SceneTransition transition;

    private Vector3 mainMenuPos = new(0.5f, 0.5f, -10);
    private Vector3 creditsMenuPos = new(18.5f, 10.5f, -10);
    private Vector3 pos;
    private Vector3 wantedPos;

    private bool inMainMenu = true;

    private void Awake() {
        cam = GameObject.Find("Main Camera");
        transition = FindObjectOfType<SceneTransition>();

        wantedPos = mainMenuPos;
        pos = wantedPos;
    }

    public void LoadGame() {
        transition.TransitionToScene("Main");
    }

    public void ShowCredits() {
        inMainMenu = false;
    }

    public void ShowMainMenu() {
        inMainMenu = true;
    }

    public void QuitGame() {
        Application.Quit();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            inMainMenu = true;
            
        }
        wantedPos = inMainMenu ? mainMenuPos : creditsMenuPos;
        pos = Vector3.Lerp(pos, wantedPos, Time.deltaTime * 5);
        cam.transform.position = pos;
    }
}
