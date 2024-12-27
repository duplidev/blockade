using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class CameraBehaviour : MonoBehaviour {

    [SerializeField] public float speed;
    [SerializeField] private float slowDownFactor;
    [SerializeField] private float maxTime;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private float expandedSize;
    [SerializeField] private Animator timerAnimator;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap modifiableTilemap;
    [SerializeField] private Tilemap backgroundTilemap;
    [SerializeField] private float destroyOffset;

    [HideInInspector] public bool slowMotion;
    private float timeScale;
    private float timer;
    private float originalSize;
    private float wantedSize;
    private float finalSize;
    private Camera cam;
    private TileManager tileManager;
    public bool shouldStop;

    private List<int> destroyed = new();

    private void Awake() {
        tileManager = FindObjectOfType<TileManager>();
        cam = GetComponent<Camera>();
    }

    private void Start() {
        originalSize = cam.orthographicSize;
        wantedSize = originalSize;
        finalSize = originalSize;
    }

    public void StartNewSection() {
        timer = maxTime;
    }

    private void Update() {
        int remainingBlocks = tileManager.rectsList.Count;
        
        timer -= Time.deltaTime;

        if (shouldStop) {
            timer = maxTime;
        }
        
        slowMotion = !(remainingBlocks == 0 || timer < 0);

        float currentSpeed = speed;
        
        if (slowMotion) {
            timerText.SetText(timer.ToString("0.0"));

            currentSpeed *= slowDownFactor;

            wantedSize = expandedSize;
        }
        else {
            wantedSize = originalSize;
        }

        if (shouldStop) {
            currentSpeed = 0;
        }

        finalSize = Mathf.Lerp(finalSize, wantedSize, Time.deltaTime * 10);

        cam.orthographicSize = finalSize;

        transform.position += new Vector3(currentSpeed, 0, 0) * Time.deltaTime;
        
        timerAnimator.SetBool(Animator.StringToHash("SlowMotion"), slowMotion);

        if (transform.position.x >= 230.5f) {
            shouldStop = true;
            tileManager.stopTimer = true;
        }

        if (transform.position.x >= 220.5f) {
            return;
        }
        
        for (int i = 0; i < 15; i++) {
            int x = Mathf.RoundToInt(transform.position.x - destroyOffset - i);
            if (!destroyed.Contains(x)) {
                StartCoroutine(DestroyWorld(x));
                destroyed.Add(x);
            }    
        }
    }

    public IEnumerator Shake(float duration, float strength) {
        Vector3 startPos = transform.position;
        float elapsed = 0;

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            transform.position = startPos + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPos;
    }

    private IEnumerator DestroyWorld(int x) {
        for (int i = 0; i < 28; i++) {
            Vector3Int position = new Vector3Int(x, 14 - i, 0);
            if (groundTilemap.GetTile(position) != null) {
                groundTilemap.SetTile(position, null);
                yield return new WaitForSeconds(0.1f);
            }
            if (modifiableTilemap.GetTile(position) != null) {
                modifiableTilemap.SetTile(position, null);
                yield return new WaitForSeconds(0.1f);
            }

            if (backgroundTilemap.GetTile(position) != null) {
                backgroundTilemap.SetTile(position, null);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}