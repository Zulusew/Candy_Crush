using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject tilePrefab;
    public GameObject[] Dots;
    public GameObject[,] allDots;

    private BackgroundTiles[,] allTile;

    void Start()
    {
        allDots = new GameObject[width, height];
        SetUp();
    }

    void SetUp()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 tempPosition = new Vector2(i,j);
                GameObject _tileprefab = (GameObject)Instantiate(tilePrefab, tempPosition, Quaternion.identity);
                _tileprefab.transform.parent = this.transform;
                _tileprefab.name = "( " + i + "  " + j + " )";
                
                int DotoUse = Random.Range(0, Dots.Length);
                int MaxVariation = 0;
                int _DotoUse = 1000;

                while (MatchesAs(i,j,Dots[DotoUse]) && MaxVariation < 500)
                {
                    _DotoUse = Random.Range(0, Dots.Length);

                    if(_DotoUse == DotoUse)
                    {
                        _DotoUse = Random.Range(0, Dots.Length);
                        DotoUse = _DotoUse;
                    }
                    else if(_DotoUse != DotoUse)
                    {
                        DotoUse = _DotoUse;
                    }
                    MaxVariation++;
                }

                GameObject _Dots = Instantiate(Dots[DotoUse], tempPosition, Quaternion.identity);
                _Dots.transform.parent = this.transform;
                _Dots.name = "Dot - " + i + "  " + j;
                allDots[i, j] = _Dots;
                               
            }
        }
    }

    private bool MatchesAs(int column, int row, GameObject pices)
    {
        if (column > 1 && row > 1)
        {
            if (allDots[column-1, row].tag == pices.tag && allDots[column-2, row].tag == pices.tag )
            {
                return true;
            }

            if (allDots[column, row - 1].tag == pices.tag && allDots[column, row - 2].tag == pices.tag)
            {
                return true;
            }
        } else if( column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if(allDots[column, row - 1].tag == pices.tag && allDots[column, row - 2].tag == pices.tag)
                {
                    return true;
                }
            }

            if (column > 1)
            {
                if (allDots[column - 1, row].tag == pices.tag && allDots[column - 2, row].tag == pices.tag)
                {
                    return true;
                }
            }
        }



        return false;
    }


}
