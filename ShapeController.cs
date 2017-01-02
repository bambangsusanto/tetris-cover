using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeController : MonoBehaviour
{
    public Block[] blocks;
    
    GameController gameController;
    
    Vector3 spawnPoint = new Vector3(-0.5f, 10.5f, 0f);

    bool touchesBlock;
    bool touchesBlockHorizontal;

    float moveRate = 0.1f;
    float nextMove = 0.0f;

    float rotateRate = 0.05f;
    float nextRotate = 0.0f;

    string shape;
    int rotateCount;

    float downSpeed;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        
        shape = gameObject.name;
        rotateCount = 0;

        transform.position = spawnPoint;
        touchesBlock = false;

        downSpeed = 2f;
        StartCoroutine(MoveDownShapeOverTime(downSpeed));
    }

    private void Update()
    {
        if (!gameObject.CompareTag("Ground"))
        {
            /* Move shape horizontally */
            /*===============================================================================*/
            if (Input.GetAxisRaw("Horizontal") > 0 && Time.time > nextMove)
            {
                nextMove = Time.time + moveRate;
                touchesBlockHorizontal = CheckTouches(Vector3.right, 1f);
                if (!touchesBlockHorizontal)
                    MoveShape(Vector3.right);
            }
            else if (Input.GetAxisRaw("Horizontal") < 0 && Time.time > nextMove)
            {
                nextMove = Time.time + moveRate;
                touchesBlockHorizontal = CheckTouches(Vector3.left, 1f);
                if (!touchesBlockHorizontal)
                    MoveShape(Vector3.left);
            }
            /*===============================================================================*/
            
            /* Rotate shape */
            /*===============================================================================*/
            if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextRotate)
            {
                nextRotate = Time.time + rotateRate;
                AudioController.instance.PlaySFX("Rotate");
                switch (shape)
                {
                    case "Rod(Clone)":
                        RotateRod();
                        break;
                    case "Tee(Clone)":
                        RotateTee();
                        break;
                    case "Seven(Clone)":
                        RotateSeven();
                        break;
                    case "Bolt(Clone)":
                        RotateBolt();
                        break;
                    default:
                        break;
                }
            }
            /*===============================================================================*/

            /* Accelerate Down */
            /*===============================================================================*/
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                StopAllCoroutines();
                downSpeed = 10;
                StartCoroutine(MoveDownShapeOverTime(downSpeed));
            }
            else if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                StopAllCoroutines();
                downSpeed = 2;
                StartCoroutine(MoveDownShapeOverTime(downSpeed));
            }
            /*===============================================================================*/
        }
    }

    IEnumerator MoveDownShapeOverTime(float downSpeed)
    {
        while (!gameObject.CompareTag("Ground"))
        {
            float downTime = 1 / downSpeed;
            yield return new WaitForSeconds(downTime);

            touchesBlock = CheckTouches(Vector3.down, 1f);

            if (!touchesBlock)
                MoveShape(Vector3.down);
            else
            {
                gameObject.tag = "Ground";
                for (int i = 0; i < blocks.Length; i++)
                {
                    blocks[i].Grounded();
                }
                gameController.CheckLines();
                gameController.SpawnNextBlock();
                break;
            }

        }
    }

    public void MoveShape(Vector3 dir)
    {
        if (dir == Vector3.down)
            AudioController.instance.PlaySFX("Down");

        // Move blocks
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].MoveBlock(dir);
        }
    }

    private bool CheckTouches(Vector3 dir, float length)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i].CheckTouchesBlock(dir, length))
                return true;
        }

        return false;
    }

    void RotateRod()
    {
        bool touchesUp = CheckTouches(Vector3.up, 1f);
        bool touchesDown = CheckTouches(Vector3.down, 2f);
        bool touchesLeft = CheckTouches(Vector3.left, 1f);
        bool touchesRight = CheckTouches(Vector3.right, 2f);
        
        switch (rotateCount)
        {
            case 0:
                if (!touchesLeft && !touchesRight)
                {
                    blocks[0].transform.position += new Vector3(-1f, -1f, 0f);
                    blocks[2].transform.position += new Vector3(1f, 1f, 0f);
                    blocks[3].transform.position += new Vector3(2f, 2f, 0f);
                    rotateCount++;
                }
                break;
            case 1:
                if (!touchesUp && !touchesDown)
                {
                    blocks[0].transform.position += new Vector3(1f, 1f, 0f);
                    blocks[2].transform.position += new Vector3(-1f, -1f, 0f);
                    blocks[3].transform.position += new Vector3(-2f, -2f, 0f);
                    rotateCount--;
                }
                break;
            default:
                break;
        }


    }

    void RotateTee()
    {
        bool touchesUp = CheckTouches(Vector3.up, 1f);
        bool touchesDown = CheckTouches(Vector3.down, 1f);
        bool touchesLeft = CheckTouches(Vector3.left, 1f);
        bool touchesRight = CheckTouches(Vector3.right, 1f);

        switch (rotateCount)
        {
            case 0:
                if (!touchesUp && !touchesDown)
                {
                    blocks[0].transform.position += new Vector3(1f, -1f, 0f);
                    blocks[2].transform.position += new Vector3(-1f, 1f, 0f);
                    blocks[3].transform.position += new Vector3(1f, 1f, 0f);
                    rotateCount++;
                }
                break;
            case 1:
                if (!touchesLeft)
                {
                    blocks[0].transform.position += new Vector3(1f, 1f, 0f);
                    blocks[2].transform.position += new Vector3(-1f, -1f, 0f);
                    blocks[3].transform.position += new Vector3(-1f, 1f, 0f);
                    rotateCount++;
                }
                break;
            case 2:
                if (!touchesUp && !touchesDown)
                {
                    blocks[0].transform.position += new Vector3(-1f, 1f, 0f);
                    blocks[2].transform.position += new Vector3(1f, -1f, 0f);
                    blocks[3].transform.position += new Vector3(-1f, -1f, 0f);
                    rotateCount++;
                }
                break;
            case 3:
                if (!touchesRight)
                {
                    blocks[0].transform.position += new Vector3(-1f, -1f, 0f);
                    blocks[2].transform.position += new Vector3(1f, 1f, 0f);
                    blocks[3].transform.position += new Vector3(1f, -1f, 0f);
                    rotateCount = 0;
                }
                break;
            default:
                break;
        }

    }

    void RotateSeven()
    {
        bool touchesUp = CheckTouches(Vector3.up, 2f);
        bool touchesDown = CheckTouches(Vector3.down, 2f);
        bool touchesLeft = CheckTouches(Vector3.left, 2f);
        bool touchesRight = CheckTouches(Vector3.right, 2f);

        switch (rotateCount)
        {
            case 0:
                if (!touchesRight)
                {
                    blocks[0].transform.position += new Vector3(1f, -1f, 0f);
                    blocks[2].transform.position += new Vector3(1f, 1f, 0f);
                    blocks[3].transform.position += new Vector3(2f, 2f, 0f);
                    rotateCount++;
                }
                break;
            case 1:
                if (!touchesUp)
                {
                    blocks[0].transform.position += new Vector3(1f, 1f, 0f);
                    blocks[2].transform.position += new Vector3(-1f, 1f, 0f);
                    blocks[3].transform.position += new Vector3(-2f, 2f, 0f);
                    rotateCount++;
                }
                break;
            case 2:
                if (!touchesLeft)
                {
                    blocks[0].transform.position += new Vector3(-1f, 1f, 0f);
                    blocks[2].transform.position += new Vector3(-1f, -1f, 0f);
                    blocks[3].transform.position += new Vector3(-2f, -2f, 0f);
                    rotateCount++;
                }
                break;
            case 3:
                if (!touchesDown)
                {
                    blocks[0].transform.position += new Vector3(-1f, -1f, 0f);
                    blocks[2].transform.position += new Vector3(1f, -1f, 0f);
                    blocks[3].transform.position += new Vector3(2f, -2f, 0f);
                    rotateCount = 0;
                }
                break;
            default:
                break;
        }

    }

    void RotateBolt()
    {
        bool touchesUp = CheckTouches(Vector3.up, 1f);
        bool touchesDown = CheckTouches(Vector3.down, 1f);
        bool touchesLeft = CheckTouches(Vector3.left, 1f);
        bool touchesRight = CheckTouches(Vector3.right, 1f);

        switch (rotateCount)
        {
            case 0:
                if (!touchesDown && !touchesUp)
                {
                    blocks[0].transform.position += new Vector3(1f, -1f, 0f);
                    blocks[2].transform.position += new Vector3(1f, 1f, 0f);
                    blocks[3].transform.position += new Vector3(0f, 2f, 0f);
                    rotateCount++;
                }
                break;
            case 1:
                if (!touchesLeft && !touchesRight)
                {
                    blocks[0].transform.position += new Vector3(-1f, 1f, 0f);
                    blocks[2].transform.position += new Vector3(-1f, -1f, 0f);
                    blocks[3].transform.position += new Vector3(0f, -2f, 0f);
                    rotateCount--;
                }
                break;
            default:
                break;
        }
    }
}
