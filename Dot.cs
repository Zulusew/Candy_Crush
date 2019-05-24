using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public int column;
    public int row;
    public int TargetX;
    public int TargetY;
    public float swipeAngle =0;
    public bool isMatched = false;
    public int previusRow;
    public int previusCollumn;
    public float swipeRisest = 1;

    private GameObject otherDot;
    private Board BOARD;

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    public Vector2 temposition;
    

    void Start()
    {
        BOARD = FindObjectOfType<Board>();

        TargetY = (int)transform.position.y;
        TargetX = (int)transform.position.x;

        row = TargetY;
        column = TargetX;

        previusRow = row;
        previusCollumn = column;
    }

    void Update()
    {
        FindMatches();
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color( 1f, 1f, 1f, .2f);
        }
        // MovemenuDot();
        TargetX = column;
        TargetY = row;

        if (Mathf.Abs(TargetX - transform.position.x) > .1)
        {
            temposition = new Vector2(TargetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, temposition, .1f);
        }
        else
        {
            temposition = new Vector2(TargetX, transform.position.y);
            transform.position = temposition;
            BOARD.allDots[column, row] = this.gameObject;
        }

        if (Mathf.Abs(TargetY - transform.position.y) > .1)
        {
            temposition = new Vector2(transform.position.x, TargetY);
            transform.position = Vector2.Lerp(transform.position, temposition, .1f);
        }
        else
        {
            temposition = new Vector2(transform.position.x, TargetY);
            transform.position = temposition;
            BOARD.allDots[column, row] = this.gameObject;
        }


    }

    void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
       
    }

    void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeRisest || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeRisest)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
        }
    }

    void MovemenuDot()
    {
        TargetX = column;
        TargetY = row;

        if (Mathf.Abs(TargetX - transform.position.x) > .3)
        {
            temposition = new Vector2(TargetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, temposition, 0.1f);
        }
        else
        {
            temposition = new Vector2(TargetX, transform.position.y);
            transform.position = temposition;
            BOARD.allDots[column, row] = this.gameObject;
        }

        if (Mathf.Abs(TargetY - transform.position.y) > .3)
        {
            temposition = new Vector2(transform.position.x, TargetY);
            transform.position = Vector2.Lerp(transform.position, temposition, .4f);
        }
        else
        {
            temposition = new Vector2(transform.position.x, TargetY);
            transform.position = temposition;
            BOARD.allDots[column, row] = this.gameObject;
        }
    }

    void MovePieces()
    {
        if(swipeAngle >= -45 && swipeAngle <= 45 && column < BOARD.width-1)
        {
            // Right movement
            otherDot = BOARD.allDots[column + 1, row];
            otherDot.GetComponent<Dot>().column -= 1;

            column += 1;
        } else if ( (swipeAngle > 135 || swipeAngle <= -135) && column > 0)
        {
            //Left movement
            otherDot = BOARD.allDots[column-1, row ];
            otherDot.GetComponent<Dot>().column += 1;

            column -= 1;
        } else if (swipeAngle <= 135 && swipeAngle > 45 && row < BOARD.height-1)
        {
            //Up movement
            otherDot = BOARD.allDots[column, row + 1];
            otherDot.GetComponent<Dot>().row -= 1;

            row += 1;
        } else if (swipeAngle <= -45 && swipeAngle >= -135 && row > 0)
        {
            //Down movement
            otherDot = BOARD.allDots[column, row - 1];
            otherDot.GetComponent<Dot>().row += 1;

            row -= 1;
        }

        StartCoroutine(CheckMoveCo());
        
    }

    void FindMatches()
    {
        if (column > 0 && column < BOARD.width - 1)
        {
            GameObject LeftDot1 = BOARD.allDots[column + 1, row];
            GameObject RightDot1 = BOARD.allDots[column - 1, row];

            if (LeftDot1 != null && RightDot1 != null)
            {
                if (LeftDot1.tag == this.gameObject.tag && RightDot1.tag == this.gameObject.tag)
                {
                    LeftDot1.GetComponent<Dot>().isMatched = true;
                    RightDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }

        if (row > 0 && row < BOARD.height - 1)
        {
                GameObject UpDot1 = BOARD.allDots[column, row + 1];
                GameObject DownDot1 = BOARD.allDots[column, row - 1];
            if (UpDot1 != null && DownDot1 != null)
            {

                if (UpDot1.tag == this.gameObject.tag && DownDot1.tag == this.gameObject.tag)
                {
                    UpDot1.GetComponent<Dot>().isMatched = true;
                    DownDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);

        if (otherDot != null)
        {
            
            if (!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = previusRow;
                column = previusCollumn;
              
            }
            otherDot = null;
        }
    }
}
