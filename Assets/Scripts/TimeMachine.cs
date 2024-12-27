using System;
using TMPro;
using UnityEngine;

public class TimeMachine : MonoBehaviour {

    [SerializeField] private GameObject timeMachineText;
    [SerializeField] private Vector3 textOffset;
    [SerializeField] private LayerMask playerLayerMask;

    private TileManager tileManager;

    private SceneTransition transition;

    [SerializeField] private GameObject endObject;
    private TMP_Text statsText;

    private Vector3 wantedTextPos;
    private Vector3 textPos;

    private float timer;

    private bool finished;

    private float timeSum;

    private void Awake() {
        statsText = endObject.GetComponentsInChildren<TMP_Text>()[2];
        endObject.SetActive(false);
        transition = FindObjectOfType<SceneTransition>();
        tileManager = FindObjectOfType<TileManager>();
    }

    private void Start() {
        timer = PlayerPrefs.GetFloat("Total Time");
    }

    private void Update() {
        wantedTextPos = transform.position + textOffset;
        textPos = Vector3.Lerp(textPos, wantedTextPos, Time.deltaTime * 10);

        timeMachineText.transform.position = textPos;

        Collider2D col = Physics2D.OverlapCircle(transform.position, 3, playerLayerMask);
        
        timeMachineText.SetActive(col);

        if (col) {
            if (Input.GetKeyDown(KeyCode.E)) {
                finished = true;
                endObject.SetActive(true);

                if (timer < PlayerPrefs.GetFloat("Best Time")) {
                    PlayerPrefs.SetFloat("Best Time", timer);
                }

                foreach (Section section in tileManager.sections) {
                    timeSum += PlayerPrefs.GetFloat(section.GetName());
                }

                timeSum /= tileManager.sections.Count;

                statsText.SetText(
                    "Best Time: " + PlayerPrefs.GetFloat("Best Time").ToString("0.0") + "\n " + 
                    "Total Time: " + timer.ToString("0.0") + "\n " + 
                    "Average Time per Section: " + timeSum.ToString("0.0")
                );
            }
        }

        if (!finished) {
            timer += Time.deltaTime;
            PlayerPrefs.SetFloat("Total Time", timer);
        }
    }

    public void BackAndReset() {
        PlayerPrefs.SetInt("LastSection", -1);
        PlayerPrefs.SetInt("FinishedTutorial", 0);
        PlayerPrefs.SetFloat("Total Time", 0);
        
        foreach (Section section in tileManager.sections) {
            PlayerPrefs.SetFloat(section.GetName(), 0);
        }
        
        transition.TransitionToScene("TitleScreen");
    }
}
