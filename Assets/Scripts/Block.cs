using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Block : MonoBehaviour {
    
    [HideInInspector] public Structure.Structure structure;

    private TileManager tileManager;
    private GameObject renderObject;

    private BoxCollider2D boxCollider2D;

    private void Awake() {
        tileManager = FindObjectOfType<TileManager>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        Vector3 finalPos = Vector3Int.RoundToInt(GetMousePostion());
        Vector3 mousePos = GetMousePostion();
        gameObject.transform.position = finalPos;
        renderObject.transform.position = mousePos;
    }

    public void InitBlock() {
        int height = structure.GetGrid().GetLength(0);
        int width = structure.GetGrid().GetLength(1);

        boxCollider2D.size = new Vector2(width, height);
        
        Utils.CreateBlocks(structure, gameObject.transform, -1, 0.2f, true);

        renderObject = new GameObject("Temp Render Object");
        Utils.CreateBlocks(structure, renderObject.transform, 0, 1, true);
    }

    public bool PlaceInGrid() {
        Vector3 roundedPosition = Vector3Int.RoundToInt(GetMousePostion());

        roundedPosition.x -= structure.GetGrid().GetLength(1) / 2f;
        roundedPosition.y += structure.GetGrid().GetLength(0) / 2f;

        bool couldPlaceBlock = tileManager.PlaceBlock(structure, roundedPosition);

        Destroy(gameObject);
        Destroy(renderObject);

        return couldPlaceBlock;
    }

    private Vector3 GetMousePostion() {
        Vector2 mousePos = Input.mousePosition;
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        return mouseWorldPos;
    }
}