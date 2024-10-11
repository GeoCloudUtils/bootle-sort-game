using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

// ReSharper disable All

public class GridManager : MonoBehaviour
{
    #region Singleton

    private static GridManager _instance;
    public static GridManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    [SerializeField] private Bottle bottlePrefab;

    [SerializeField] private RectTransform originalGrid;

    [SerializeField] private RectTransform mixedGrid;

    private Color[] colors = new Color[16]
    {
        Color.red, // 0 - Red
        Color.green, // 1 - Green
        Color.blue, // 2 - Blue
        Color.yellow, // 3 - Yellow
        Color.cyan, // 4 - Cyan
        Color.magenta, // 5 - Magenta
        Color.white, // 6 - White
        Color.black, // 7 - Black
        new Color(1f, 0.5f, 0f), // 8 - Orange
        new Color(0.5f, 0f, 0.5f), // 9 - Purple
        new Color(0.5f, 0.25f, 0f), // 10 - Brown
        new Color(0.75f, 0.75f, 0.75f), // 11 - Light Gray
        new Color(0.5f, 1f, 0.5f), // 12 - Light Green
        new Color(1f, 0.75f, 0.8f), // 13 - Light Pink
        new Color(1f, 1f, 0f), // 14 - Light Yellow
        new Color(0.5f, 0.5f, 0.0f) // 15 - Olive
    };

    private List<Bottle> originalBottles = new List<Bottle>(); // List to store original bottles

    [SerializeField] private Bottle _selectedBottle_1 = null;
    [SerializeField] private Bottle _selectedBottle_2 = null;

    public void Initialize(Vector2Int gridSize)
    {
        Debug.Log("GridManager Initialized");

        ShuffleArray(colors);

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                if (x * gridSize.y + y >= colors.Length)
                {
                    Debug.LogWarning("Not enough unique colors for the grid size!");
                    return;
                }

                Bottle bottle = Instantiate(bottlePrefab, originalGrid);
                bottle.Initialize(colors[x * gridSize.y + y]);
                originalBottles.Add(bottle);
            }
        }

        CreateMixedGrid();
    }

    private void CreateMixedGrid()
    {
        ShuffleList(originalBottles);

        foreach (Bottle bottle in originalBottles)
        {
            Bottle mixedBottle = Instantiate(bottlePrefab, mixedGrid);
            mixedBottle.Initialize(bottle.GetColor());
            mixedBottle.BottleClicked += OnBottleClicked;
        }
    }

    private void OnBottleClicked(Bottle target)
    {
        if (_selectedBottle_1 == null)
        {
            _selectedBottle_1 = target;
        }
        else if (_selectedBottle_1 == target)
        {
            _selectedBottle_1 = null;
            target.OnBottleDeslect();
            return;
        }
        else if (_selectedBottle_2 == null)
        {
            _selectedBottle_2 = target;
        }
        else if (_selectedBottle_2 == target)
        {
            _selectedBottle_2 = null;
            target.OnBottleDeslect();
            return;
        }

        if (_selectedBottle_1 && _selectedBottle_2)
        {
            Vector3 pos1 = _selectedBottle_2.transform.localPosition;
            Vector3 pos2 = _selectedBottle_1.transform.localPosition;
            _selectedBottle_1.GetComponent<RectTransform>().DOLocalMove(pos1, 0.5f);
            _selectedBottle_2.GetComponent<RectTransform>().DOLocalMove(pos2, 0.5f).OnComplete(() =>
            {
                _selectedBottle_1.OnBottleDeslect();
                _selectedBottle_2.OnBottleDeslect();
                _selectedBottle_1 = null;
                _selectedBottle_2 = null;
            });
        }
    }

    private void ShuffleArray(Color[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Color temp = array[i];
            int randomIndex = UnityEngine.Random.Range(i, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}