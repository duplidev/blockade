using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour {

    private Animator transition;

    private void Start() {
        transition = GameObject.Find("Transition").GetComponent<Animator>();
    }

    public void TransitionToScene(string sceneName) {
        StartCoroutine(TransitionToScene(sceneName, transition, 0.25f));
    }

    private IEnumerator TransitionToScene(string sceneName, Animator animator, float t) {
        animator.SetTrigger(Animator.StringToHash("Trigger"));

        yield return new WaitForSeconds(t);

        SceneManager.LoadScene(sceneName);
    }
}
