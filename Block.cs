using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        
    }

    public bool CheckTouchesBlock(Vector3 dir, float length)
    {
        Vector3 below = transform.TransformDirection(dir);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, below, out hit, length) || transform.position.y <= -10.0f)
        {
            if (hit.collider.CompareTag("Ground"))
                return true;
        }

        return false;
    }

    public void MoveBlock(Vector3 dir)
    {
        transform.position += dir;
    }
    
    public void Grounded()
    {
        gameObject.tag = "Ground";
        gameController.MapBlock(transform.localPosition);
    }

    public void DestroyBlock()
    {
        Destroy(gameObject);
    }    

}
