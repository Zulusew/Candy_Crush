using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempProbnic : MonoBehaviour
{
    public int Width;
    public int Height;
    public GameObject[] shariki;
    public GameObject[,] stena_sharikov;

    void Start()
    {
        stena_sharikov = new GameObject[Width, Height];

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                int random_sharik = Random.Range(0, shariki.Length);
                int MaxVariation = 0;

                while (Perebor(i, j, shariki[random_sharik]) && MaxVariation < 100)
                {
                    int temp_random_sharik = Random.Range(0, shariki.Length);
                    if (random_sharik == temp_random_sharik)
                    {
                        temp_random_sharik = Random.Range(0, shariki.Length);
                        random_sharik = temp_random_sharik;
                    }
                    random_sharik = temp_random_sharik;
                    MaxVariation++;
                }

                Vector3 temp_position = new Vector2(i, j);
                stena_sharikov[i,j] = (GameObject)Instantiate(shariki[random_sharik], temp_position, Quaternion.identity);
                stena_sharikov[i, j].transform.parent = this.transform;
                stena_sharikov[i,j].name = "Sharik: " + i + " - " + j;

            }
        }
    }

    void Update()
    {
        
    }

    private bool Perebor(int i, int j, GameObject sharik)
    {
        if( i > 1 && j > 1)
        {
            if (stena_sharikov[i - 1,j].tag == sharik.tag && stena_sharikov[i - 2,j].tag == sharik.tag)
            {
                return true;
            }
            if (stena_sharikov[i, j - 1].tag == sharik.tag && stena_sharikov[i, j - 2].tag == sharik.tag)
            {
                return true;
            }
        } else if(i <= 1 || j <= 1)
        {
            if (i > 1)
            {
                if (stena_sharikov[i -1, j].tag == sharik.tag && stena_sharikov[i -2, j].tag == sharik.tag)
                {
                    return true;
                }
            }
            if (j > 1)
            {
                if (stena_sharikov[i, j - 1].tag == sharik.tag && stena_sharikov[i, j - 2].tag == sharik.tag)
                {
                    return true;
                }
            }
        }


        return false;
    }

    private void DestroyMatchesAt(int row_x, int column_y)
    {
        if (stena_sharikov[row_x, column_y].GetComponent<tempPkayer>().MatchesAs)
        {
            Destroy(stena_sharikov[row_x, column_y]);
            stena_sharikov[row_x, column_y] = null;
        }
    }

    private void DestroyShariki()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if (stena_sharikov[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
    }

    public IEnumerator DescrewRowCo()
    {
        int NullCount = 0;

        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                if(stena_sharikov[i,j] == null)
                {
                    NullCount++;
                    
                }
                if(NullCount > 0)
                {
                    stena_sharikov[i, j].GetComponent<tempPkayer>().row_x -= NullCount;
                }
            }

            NullCount = 0;
        }

        yield return new WaitForSeconds(0.5f);

    }
}
