using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour {
    
    [SerializeField] private Tilemap modifiableTilemap;
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] public List<Section> sections = new();
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Sprite fillSprite;

    [SerializeField] private GameObject rectPrefab;

    [HideInInspector] public List<GameObject> rectsList = new();
    private List<GameObject> previewBlocksList = new();
    private List<GameObject> darkObjectsList = new();

    private GameObject cam;
    private CameraBehaviour camBehaviour;

    [HideInInspector] public int sectionIndex = -1;

    private AudioManager audioManager;

    [HideInInspector] public bool stopTimer;

    private float timer;

    private void Awake() {
        cam = GameObject.Find("Main Camera");
        camBehaviour = cam.GetComponent<CameraBehaviour>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start() {
        CheckSection();
        
        if (PlayerPrefs.GetInt("FinishedTutorial") == 1) {
            Section lastSection = sections[PlayerPrefs.GetInt("LastSection")];
            RespawnPlayer(lastSection);
        }

        if (sectionIndex >= 0 && !stopTimer) {
            timer = PlayerPrefs.GetFloat(sections[sectionIndex].GetName());
        }
    }

    private void Update() {
        CheckSection();
        DrawSections();

        timer += Time.deltaTime;
        if (sectionIndex >= 0) {
            PlayerPrefs.SetFloat(sections[sectionIndex].GetName(), timer);
        }
    }

    private void UpdateRects() {
        camBehaviour.StartNewSection();
        
        foreach (GameObject rect in rectsList) { Destroy(rect); }
        foreach(GameObject previewBlock in previewBlocksList) { Destroy(previewBlock); }
        foreach(GameObject darkObject in darkObjectsList) { Destroy(darkObject); }

        rectsList.Clear();
        previewBlocksList.Clear();
        darkObjectsList.Clear();

        GameObject previewBlocks = new GameObject("Preview Blocks");
        GameObject darkObjects = new GameObject("Dark Objects");

        float distance = 1;
        int index = 0;
        foreach (Section section in sections) {
            if (index != sectionIndex) {
                GameObject darkObject = new GameObject("Dark Object");
                darkObject.transform.SetParent(darkObjects.transform);
                
                SpriteRenderer darkSpriteRenderer = darkObject.AddComponent<SpriteRenderer>();
                darkSpriteRenderer.color = Color.black;
                darkSpriteRenderer.sprite = fillSprite;
                
                Color tempColor = darkSpriteRenderer.material.color; 
                tempColor.a = 0.2f;
                
                darkSpriteRenderer.material.color = tempColor;

                int yExpand = 12;
                
                darkObject.transform.localScale = new Vector2(section.dimension.x, section.dimension.y + yExpand);
                darkObject.transform.position = new Vector2(section.position.x + section.dimension.x / 2f,
                    section.position.y + section.dimension.y / 2f);
                
                darkObjectsList.Add(darkObject);
            }
            
            float xOffset = 0;
            float previewXOffset = 0;
            foreach (Structure.Structure structure in section.structures) {
                if (index == sectionIndex) {
                    // create blocks for ui
                    GameObject rectObject = Instantiate(rectPrefab, canvas.transform);
                    
                    // move object up so scene transition covers block
                    rectObject.transform.SetAsFirstSibling();

                    Rect rect = rectObject.GetComponent<Rect>();
                    RectTransform rectTransform = rectObject.GetComponent<RectTransform>();

                    float scale = rect.scalingFactor;
        
                    rect.structure = structure;
                    rectTransform.anchoredPosition = new Vector2(xOffset + distance * scale, -(distance * scale));

                    rectsList.Add(rectObject);

                    xOffset += (structure.GetGrid().GetLength(1) + distance) * scale;
                }
                else {
                    // preview blocks above section
                    if (index > sectionIndex) {
                        GameObject previewBlock = new GameObject { name = "Preview Block" };
                        previewBlock.transform.SetParent(previewBlocks.transform);
                        
                        Utils.CreateBlocks(structure, previewBlock.transform, 1, 0.2f, false);

                        previewBlock.transform.position = new Vector2(section.position.x + previewXOffset, section.position.y + section.dimension.y + structure.GetGrid().GetLength(0));

                        previewBlocksList.Add(previewBlock);
                    
                        previewXOffset += structure.GetGrid().GetLength(1) + distance;
                    }
                }
            }
            index++;
        }
    }

    private void CheckSection() {
        int index = 0;
        foreach (Section section in sections) {
            if (section.Inside(player.transform.position)) {
                if (index > sectionIndex) {
                    sectionIndex = index;
                    UpdateRects();
                    audioManager.PlaySound("Section Complete");
                    PlayerPrefs.SetInt("LastSection", index);
                    
                }
            }

            index++;
        }
    }
    
    private void DrawSections() {
        foreach (Section section in sections) {
            Debug.DrawLine(new Vector2(section.position.x, section.position.y),
                new Vector2(section.position.x + section.dimension.x, section.position.y), Color.red);
            Debug.DrawLine(new Vector2(section.position.x + section.dimension.x, section.position.y),
                new Vector2(section.position.x + section.dimension.x, section.position.y + section.dimension.y),
                Color.red);
            Debug.DrawLine(
                new Vector2(section.position.x + section.dimension.x, section.position.y + section.dimension.y),
                new Vector2(section.position.x, section.position.y + section.dimension.y), Color.red);
            Debug.DrawLine(new Vector2(section.position.x, section.position.y + section.dimension.y),
                new Vector2(section.position.x, section.position.y), Color.red);
        }
    }

    public bool PlaceBlock(Structure.Structure structure, Vector3 position) {
        if (IsInBlock(structure, position, modifiableTilemap) || 
            IsInBlock(structure, position, groundTilemap)) {
            return false;
        }

        if (!IsSurroundedByBlock(structure, position, groundTilemap) && !IsSurroundedByBlock(structure, position, modifiableTilemap)) {
            return false;
        }

        Vector3Int positionInt = modifiableTilemap.WorldToCell(position);

        for (int i = 0; i < structure.GetGrid().GetLength(1); i++) {
            for (int j = 0; j < structure.GetGrid().GetLength(0); j++) {
                Vector3Int newPosition = new Vector3Int(positionInt.x + i, positionInt.y - j, positionInt.z);
                Tile newTile = structure.GetTileAtIndex(j, i);
                if (newTile != null) {
                    modifiableTilemap.SetTile(newPosition, newTile);
                }
            }
        }

        return true;
    }

    private bool IsInBlock(Structure.Structure structure, Vector3 position, Tilemap tilemap) {
        bool inBlock = false;
        Vector3Int positionInt = tilemap.WorldToCell(position);

        for (int i = 0; i < structure.GetGrid().GetLength(1); i++) {
            for (int j = 0; j < structure.GetGrid().GetLength(0); j++) {
                Vector3Int newPosition = new Vector3Int(positionInt.x + i, positionInt.y - j, positionInt.z);

                Tile newTile = structure.GetTileAtIndex(j, i);
                if (newTile != null) {
                    if (Utils.IsCellFull(newPosition, tilemap)) {
                        inBlock = true;
                    }
                }
            }
        }

        return inBlock;
    }

    private Vector3Int[] vectorsToCheck = {
        new(0, 1),
        new(0, -1),
        new(-1, 0),
        new(1, 0)
    };

    private bool IsSurroundedByBlock(Structure.Structure structure, Vector3 position, Tilemap tilemap) {
        bool surroundedByBlock = false;

        Vector3Int positionInt = tilemap.WorldToCell(position);

        for (int i = 0; i < structure.GetGrid().GetLength(1); i++) {
            for (int j = 0; j < structure.GetGrid().GetLength(0); j++) {
                Vector3Int newPosition = new Vector3Int(positionInt.x + i, positionInt.y - j, positionInt.z);

                Tile newTile = structure.GetTileAtIndex(j, i);
                if (newTile != null) {
                    for (int k = 0; k < vectorsToCheck.Length; k++) {
                        Vector3Int checkPosition = newPosition + vectorsToCheck[k];
                        
                        if (Utils.IsCellFull(checkPosition, tilemap)) {
                            surroundedByBlock = true;
                        }
                    }
                }
            }
        }

        return surroundedByBlock;
    }

    public void RespawnPlayer(Section section) {
        player.transform.position = section.playerSpawnPos;
        cam.transform.position = new Vector3(section.position.x + section.dimension.x / 2f - section.dimension.x / 4f, 0, -10);
    }
    
    private void OnApplicationQuit() {
        PlayerPrefs.SetInt("LastSection", -1);
        PlayerPrefs.SetInt("FinishedTutorial", 0);
    }
}