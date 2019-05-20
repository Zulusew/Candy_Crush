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
        //allTile = new BackgroundTiles[width, height];
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
                GameObject _Dots = Instantiate(Dots[DotoUse], tempPosition, Quaternion.identity);
                _Dots.transform.parent = this.transform;
                _Dots.name = "Dot - " + i + "  " + j;
                allDots[i, j] = _Dots;
                               
            }
        }
    }


}
