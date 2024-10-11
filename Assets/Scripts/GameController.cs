using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ReSharper disable All

public class GameController : MonoBehaviour
{
    private void Start()
    {
        Initialize(new Vector2Int(2,2));
    }

    public void Initialize(Vector2Int gridSize)
    {
        Debug.Log("GameController Initialized");
        GridManager.Instance.Initialize(gridSize);
    }
}