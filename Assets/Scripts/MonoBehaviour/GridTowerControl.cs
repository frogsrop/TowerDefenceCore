using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTowerControl : MonoBehaviour
{
    [SerializeField] private int LengthGrid = 2; 
    [SerializeField] private int WidthGrid = 3; 
    [SerializeField] private int SpacingGrid = 2; 
    [SerializeField] private Vector2 PosGrid = new (-1,-3);
    
    public GameObject GreenSquare;
    public GameObject RedSquare;
    
    private Vector2[,] _arrayGridPosElements;
    private bool[,] _arrayGridBool;

    void Start()
    {
        _arrayGridPosElements = new Vector2[LengthGrid,WidthGrid];
        _arrayGridBool = new bool[LengthGrid,WidthGrid];
        for (int i = 0; i < _arrayGridPosElements.GetLength(0); i++)
        {
            for (int j = 0; j < _arrayGridPosElements.GetLength(1); j++)
            {
                _arrayGridPosElements[i, j] = new Vector2( PosGrid.x+i*SpacingGrid, PosGrid.y+j*SpacingGrid);
                _arrayGridBool[i, j] = true;
            }
        }
    }

    public bool[,] GetBoolValueInGrid()
    {
        return _arrayGridBool;
    }
    public Vector2[,] GetPosValueInGrid()
    {
        return _arrayGridPosElements;
    }
    
    public bool[,] SetBoolValueInGrid(int indexBoolI, int indexBoolJ)
    {
        _arrayGridBool[indexBoolI, indexBoolJ] = false;
        return _arrayGridBool;
    }

}
