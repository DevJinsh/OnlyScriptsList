using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Preview : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _previewGroup;
    [SerializeField] private GameObject _previewPrefabs;
    private int _cellCount;
    public Sprite correctImage;
    public Sprite defaultImage;

    public Image[,] Tiles { get; private set; }
    public void Init()
    {
        _cellCount = GameManager.Instance.tileManager.cellCount;
        _previewGroup.constraintCount = _cellCount;
        _previewGroup.cellSize = Vector2.one * _previewGroup.GetComponent<RectTransform>().sizeDelta.x / _cellCount;
        CreateTile();
    }

    public void CreateTile()
    {
        Tiles = new Image[_cellCount, _cellCount];
        for (int y = 0; y < _cellCount; y++)
        {
            for (int x = 0; x < _cellCount; x++)
            {
                var obj = Instantiate(_previewPrefabs, _previewGroup.transform);
                obj.name = "Cell" + y + "," + x;
                Tiles[y, x] = obj.GetComponent<Image>();
            }
        }
    }
}
