using System;
using UnityEngine;

public class Pause : MonoBehaviour {

    [SerializeField] private GameObject panel;

    private SceneTransition transition;

    private void Awake() {
        transition = FindObjectOfType<SceneTransition>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Toggle();
        }
    }

    public void Continue() {
        if (panel.activeSelf) {
            Toggle();
        }
    }

    public void Quit() {
        Time.timeScale = 1;
        transition.TransitionToScene("TitleScreen");
    }

    private void Toggle() {
        panel.SetActive(!panel.activeSelf);
        Time.timeScale = panel.activeSelf ? 0 : 1;
    }
}
