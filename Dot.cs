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
    }

    void Update()
    {
        TargetX = column;
        TargetY = row;

        if (Mathf.Abs(TargetX - transform.position.x) > .1)
        {
            Debug.Log("Target X  -" + TargetX);
            Debug.Log("transform.position.x   -" + transform.position.x);
            temposition = new Vector2(TargetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, temposition, .4f);
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
            transform.position = Vector2.Lerp(transform.position, temposition, .4f);
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
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) *180/ Mathf.PI;
        MovePieces();
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
    }
}
