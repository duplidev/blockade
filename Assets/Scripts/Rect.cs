using UnityEngine;
using UnityEngine.EventSystems;

public class Rect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    [SerializeField] private GameObject blockPrefab;
    [SerializeField] public float scalingFactor = 40;
    
    [HideInInspector] public Structure.Structure structure;

    private TileManager tileManager;
    private CameraBehaviour cameraBehaviour;
    private Block block;
    private Vector2 offset;
    
    private AudioManager audioManager;

    private RectTransform rectTransform;

    private void Awake() {
        offset = new Vector2(-1.5f * scalingFactor, 1.5f * scalingFactor);
        
        rectTransform = GetComponent<RectTransform>();

        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
        
        tileManager = FindObjectOfType<TileManager>();
        cameraBehaviour = FindObjectOfType<CameraBehaviour>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start() {
        int height = structure.GetGrid().GetLength(0);
        int width = structure.GetGrid().GetLength(1);

        float offsetWidth = (width * scalingFactor) / 2f - scalingFactor / 2f;
        float offsetHeight = (height * scalingFactor) / 2f - scalingFactor / 2f;

        float padding = scalingFactor / 2f;
        
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                GameObject rect = structure.GetRectAtIndex(j, i);
                if (rect != null) {
                    RectTransform tileRectTransform = Instantiate(rect, transform, false).GetComponent<RectTransform>();
                    tileRectTransform.anchoredPosition = new Vector2(-offsetWidth + i * scalingFactor, offsetHeight - j * scalingFactor);
                }
            }
        }

        rectTransform.sizeDelta = new Vector2(width * scalingFactor + padding * 2, height * scalingFactor + padding * 2);
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        block = Instantiate(blockPrefab).GetComponent<Block>();
        block.structure = structure;
        block.InitBlock();
        audioManager.PlaySound("Block Take");
    }

    public void OnPointerUp(PointerEventData eventData) {
        bool couldPlaceBlock = block.PlaceInGrid();
        if (couldPlaceBlock) {
            tileManager.rectsList.Remove(gameObject);
            Destroy(gameObject);
            audioManager.PlaySound("Block Place");
        }
        else
        {
            StartCoroutine(cameraBehaviour.Shake(0.1f, 0.5f));
            audioManager.PlaySound("Block Fail");
        }
    }
}