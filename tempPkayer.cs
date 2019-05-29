using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempPkayer : MonoBehaviour
{
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private float swipeAngle;
    private tempProbnic tempProb;
    private Vector2 temp_position;
    private GameObject tempSharik;

    public int row_x;
    public int column_y;
    public int TargetX;
    public int TargetY;

    public int PreviewX;
    public int PreviewY;

    public bool MatchesAs = false;

    void Start()
    {
        tempProb = FindObjectOfType<tempProbnic>();

        row_x = (int)this.transform.position.x;
        column_y = (int)this.transform.position.y;
        TargetY = row_x;
        TargetX = column_y;

        PreviewX = row_x;
        PreviewY = column_y;

}

    void Update()
    {
        FindMatchesAs();
        if (MatchesAs)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(1f, 1f, 1f, .2f);
            //MatchesAs = false;
        }

        TargetY = column_y;
        TargetX = row_x;

        if (Mathf.Abs(row_x - transform.position.x) > .1)
        {
            temp_position = new Vector2(row_x, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, temp_position, .1f);
        }
        else
        {
            temp_position = new Vector2(row_x, transform.position.y);
            transform.position = temp_position;

            tempProb.stena_sharikov[row_x, column_y] = this.gameObject;
        }


        if (Mathf.Abs(column_y - transform.position.y) > .1)
        {
            transform.position = new Vector2(transform.position.x, column_y);
            transform.position = Vector2.Lerp(transform.position, temp_position, .9f);
        }
        else
        {
            temp_position = new Vector2(transform.position.x, column_y);
            transform.position = temp_position;
            tempProb.stena_sharikov[row_x, column_y] = this.gameObject;
        }
    }

    void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(this.name + " - Имя объекта по которуму нажал курсор");
        
    }

    void OnMouseUp(){
        
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
        MovePice();
        
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
        Debug.Log(swipeAngle + " - указывает под каким углом отрезок клика мыши");
    }

    void MovePice()
    {
        Debug.Log("Вызов функции перемещение шарика - " + row_x +" - "+ column_y);

        if (swipeAngle >= -45 && swipeAngle <= 45 && row_x < tempProb.Width-1)
        {
            // Right movement
            tempSharik = tempProb.stena_sharikov[row_x + 1 , column_y];
            tempSharik.GetComponent<tempPkayer>().row_x -= 1;

            row_x += 1;
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135) && row_x > 0)
        {
            //Left movement
            tempSharik = tempProb.stena_sharikov[row_x - 1, column_y];
            tempSharik.GetComponent<tempPkayer>().row_x += 1;

            row_x -= 1;
        }
        else if (swipeAngle <= 135 && swipeAngle > 45 && column_y < tempProb.Height-1 )
        {
            //Up movement
            tempSharik = tempProb.stena_sharikov[row_x, column_y + 1];
            tempSharik.GetComponent<tempPkayer>().column_y -= 1;

            column_y += 1;
        }
        else if (swipeAngle <= -45 && swipeAngle >= -135 && column_y > 0)
        {
            //Down movement
            tempSharik = tempProb.stena_sharikov[row_x, column_y - 1];
            tempSharik.GetComponent<tempPkayer>().column_y += 1;

            column_y -= 1;
            
        }

        StartCoroutine(CheckMoveCo());
    }

    void FindMatchesAs()
    {
        if (row_x > 0 && row_x < tempProb.Width-1)
        {
            
            GameObject LeftSharik_X = tempProb.stena_sharikov[row_x - 1, column_y];
            GameObject RightSharik_X = tempProb.stena_sharikov[row_x + 1, column_y];
            if (LeftSharik_X != null && RightSharik_X != null)
            {
                if (LeftSharik_X.tag == this.gameObject.tag && RightSharik_X.tag == this.gameObject.tag)
                {
                    LeftSharik_X.GetComponent<tempPkayer>().MatchesAs = true;
                    RightSharik_X.GetComponent<tempPkayer>().MatchesAs = true;
                    MatchesAs = true;
                    
                }
            }
        }

        if ( column_y > 0 && column_y < tempProb.Height - 1)
        {

            GameObject TopSharik_Y = tempProb.stena_sharikov[row_x, column_y + 1];
            GameObject DownSharik_Y = tempProb.stena_sharikov[row_x,column_y - 1];
            if (TopSharik_Y != null && DownSharik_Y != null)
            {
                if (TopSharik_Y.tag == this.gameObject.tag && DownSharik_Y.tag == this.gameObject.tag)
                {

                    TopSharik_Y.GetComponent<tempPkayer>().MatchesAs = true;
                    DownSharik_Y.GetComponent<tempPkayer>().MatchesAs = true;
                    MatchesAs = true;

                }
            }
        }


    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);

        if (tempSharik != null)
        {
           if(!MatchesAs && !tempSharik.GetComponent<tempPkayer>().MatchesAs)
            {
                tempSharik.GetComponent<tempPkayer>().column_y = column_y;
                tempSharik.GetComponent<tempPkayer>().row_x = row_x;

                row_x = PreviewX;
                column_y = PreviewY;
            }
            tempSharik = null;
        }


    }
}
