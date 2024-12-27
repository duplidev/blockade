using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour  {

    private GameObject player;
    private PlayerMovement playerMovement;
    private GameObject cam;
    private CameraBehaviour camBehaviour;

    private TileManager tileManager;

    private Animator dialogAnimator;
    private TMP_Text text;
    private Slider slider;

    private Image skipImage;
    private GameObject skipObject;
    private RectTransform skipRect;
    private TMP_Text skipText;

    private Coroutine tutorial;

    private float skipTimer;

    [SerializeField] private float skipNeededTime;

    private float wantedSkipTextPosition;
    private float finalSkipTextPosition;

    private void Awake() {
        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();

        cam = GameObject.Find("Main Camera");
        camBehaviour = cam.GetComponent<CameraBehaviour>();

        tileManager = FindObjectOfType<TileManager>();

        skipImage = GameObject.Find("Skip").GetComponent<Image>();
        skipObject = GameObject.Find("SkipText");
        skipRect = skipObject.GetComponent<RectTransform>();
        skipText = skipObject.GetComponent<TMP_Text>();

        GameObject dialogBox = GameObject.Find("DialogBox");
        dialogAnimator = dialogBox.GetComponent<Animator>();
        slider = dialogBox.GetComponent<Slider>();
        text = dialogBox.GetComponentInChildren<TMP_Text>();
    }

    private void Start() {
        if (PlayerPrefs.GetInt("FinishedTutorial") == 0) {
            tutorial = StartCoroutine(TutorialSequence());
        }
    }

    private void Update() {
        if (PlayerPrefs.GetInt("FinishedTutorial") == 0) {
            if (Input.GetKey(KeyCode.Return)) {
                skipTimer += Time.deltaTime;

                float t = skipTimer / skipNeededTime;
                skipImage.fillAmount = Utils.EaseInCubic(t);
                wantedSkipTextPosition = -350;

                if (skipTimer > skipNeededTime) {
                    StopCoroutine(tutorial);
                    StopTutorial();
                }
            }
            else {
                skipTimer = 0;
                skipImage.fillAmount = 0;
                wantedSkipTextPosition = -227.5f;
            }

            finalSkipTextPosition = Mathf.Lerp(finalSkipTextPosition, wantedSkipTextPosition, Time.deltaTime * 10);
        
            skipRect.anchoredPosition = new Vector2(finalSkipTextPosition, skipRect.anchoredPosition.y);    
        }
        else {
            skipTimer = 0;
            skipImage.fillAmount = 0;
            skipText.SetText("");
        }
    }

    private void StopTutorial() {
        // reset values changed by tutorial
        camBehaviour.shouldStop = false;
        camBehaviour.speed = 5f;
        slider.value = 0;
        dialogAnimator.SetBool(Animator.StringToHash("Visible"), false);
        playerMovement.lockWalking = false;
        playerMovement.lockJumping = false;
        
        tileManager.RespawnPlayer(tileManager.sections[0]);

        PlayerPrefs.SetInt("FinishedTutorial", 1);
    }

    private IEnumerator TutorialSequence() {
        playerMovement.lockWalking = true;
        playerMovement.lockJumping = true;
        camBehaviour.shouldStop = true;
        slider.value = 0;
        dialogAnimator.SetBool(Animator.StringToHash("Visible"), true);
        
        text.SetText("The world has been alive for so long that it's starting to collapse.");

        yield return new WaitForSeconds(3);
        
        text.SetText("You were so lucky to not get crushed yet. You have to find a time machine to revert all the damage.");

        yield return new WaitForSeconds(4);
        
        text.SetText("Use [W] and [D] to move left and right.");

        playerMovement.lockWalking = false;

        Vector3 prevPos = player.transform.position;
        float distanceTraveled = 0;
        float neededTravelDistance = 10;
        
        while (true) {
            Vector3 currentPos = player.transform.position;

            distanceTraveled += Mathf.Abs(currentPos.x - prevPos.x);
            
            prevPos = player.transform.position;
            
            slider.value = distanceTraveled / neededTravelDistance;
            
            if (distanceTraveled > neededTravelDistance) {
                break;
            }
            
            yield return null;
        }

        slider.value = 0;
        
        text.SetText("Walking up to a 1 block height increase will cause you to jump automatically.");

        while (true) {
            if (playerMovement.hasUsedAutoJump) {
                break;
            }
            yield return null;
        }

        camBehaviour.shouldStop = false;
        
        text.SetText("Sometimes you need to jump manually if you want to get over a hole.");

        yield return new WaitForSeconds(4);

        camBehaviour.shouldStop = true;

        playerMovement.lockJumping = false;
        
        text.SetText("Use [SPACE] to jump.");

        prevPos = player.transform.position;
        
        while (true) {
            Vector3 currentPos = player.transform.position;

            float jumpedDistance = currentPos.y - prevPos.y;
            
            if (jumpedDistance > 1f) {
                break;
            }

            slider.value = Mathf.Min(jumpedDistance / 1f, 1);

            yield return null;
        }

        slider.value = 0;
        
        text.SetText("You just saw the camera move to the right. The world is collapsing behind you, so you have keep to be moving.");

        yield return new WaitForSeconds(4);
        
        text.SetText("If you are not fast enough, you will die because of the collapsing world.");

        yield return new WaitForSeconds(4);
        
        dialogAnimator.SetBool(Animator.StringToHash("Visible"), false);
        
        camBehaviour.shouldStop = false;
        camBehaviour.speed = 2f;

        while (true) {
            if (tileManager.sectionIndex >= 0) {
                break;
            }

            yield return null;
        }

        camBehaviour.shouldStop = true;
        
        dialogAnimator.SetBool(Animator.StringToHash("Visible"), true);
        
        text.SetText("You have been given the power to transform the world by a godly entity.");

        yield return new WaitForSeconds(4);
        
        text.SetText("In the top left-hand corner you will see blocks which you can place into the world.");

        yield return new WaitForSeconds(4);
        
        text.SetText("You simply take and drag them with your mouse. They will get placed on release.");

        yield return new WaitForSeconds(4);
        
        text.SetText("They can only be placed if connected to other blocks. Placing in the air or inside other blocks, won't work.");

        yield return new WaitForSeconds(4);

        camBehaviour.shouldStop = false;
        camBehaviour.speed = 5f;
        
        text.SetText("Time is slowed down for a limited time. Once if runs out or all blocks are placed it goes back to normal.");

        while (true) {
            if (!camBehaviour.slowMotion) {
                break;
            }

            yield return null;
        }
        
        dialogAnimator.SetBool(Animator.StringToHash("Visible"), false);
        
        while (true) {
            if (tileManager.sectionIndex >= 1) {
                break;
            }

            yield return null;
        }
        
        dialogAnimator.SetBool(Animator.StringToHash("Visible"), true);
        
        text.SetText("While time is slowed down, you can see future situations and their blocks. Think ahead!");

        yield return new WaitForSeconds(4);
        
        dialogAnimator.SetBool(Animator.StringToHash("Visible"), false);

        PlayerPrefs.SetInt("FinishedTutorial", 1);
    }
}
