using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour {

    private SceneTransition sceneTransition;

    private void Awake() {
        sceneTransition = FindObjectOfType<SceneTransition>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            sceneTransition.TransitionToScene(SceneManager.GetActiveScene().name);
        }
    }
}