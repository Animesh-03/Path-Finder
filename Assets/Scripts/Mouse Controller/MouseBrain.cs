using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode {SPAWN,MOVE,EDGE,DELETE};
public class MouseBrain : MonoBehaviour
{
#region InputButtons
    public KeyCode spawnKey;
    public KeyCode moveKey;
    public KeyCode edgeKey;
    public KeyCode deleteKey;
#endregion

#region CameraControls
    public float scrollSensitivity;
    public float spanSensitivity;
    private Vector3 mouseStartPos;
    private bool isSpanning;
#endregion

    public Mode mode = Mode.MOVE;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ModeSelect();
        CameraControl();

        //Handles LMB Click
        if(Input.GetMouseButtonDown(0))
        {
            OnMouseClick();
        }
    }

    //Change the mouse mode based on Keyboard Input
    void ModeSelect()
    {
        if(Input.GetKeyDown(spawnKey))
            mode = Mode.SPAWN;
        else if(Input.GetKeyDown(moveKey))
            mode = Mode.MOVE;
        else if(Input.GetKeyDown(edgeKey))
            mode = Mode.EDGE;
        else if(Input.GetKeyDown(deleteKey))
            mode = Mode.DELETE;
    }
    //Checks if the mouse clicked a gameObject and then sends the raycast info to HandleMouseClick
    void OnMouseClick()
    {
        Vector3 clickPos =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(clickPos);

        if(mode == Mode.SPAWN)
        {
            //Add Node
            Node.SpawnNode(new Vector2(clickPos.x, clickPos.y));
            mode = Mode.MOVE;
        }
        else if(mode == Mode.EDGE)
        {
            //Add Edge
        }
        else
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.transform.position, clickPos - Camera.main.transform.position);
            if(hitInfo)
            {
                HandleMouseClick(hitInfo);
            }
        }
    }

    //If click hit an object then handle it
    void HandleMouseClick(RaycastHit2D hitInfo)
    {
        
        if(mode == Mode.MOVE)
        {
            //Move Object if movable
            var moveScript = hitInfo.collider.gameObject.GetComponent<IMovable>();
            if(moveScript != null)
                moveScript.Pickup();
        }
        else if(mode == Mode.DELETE)
        {
            //Delete Object if deletable
            var deleteScript = hitInfo.collider.gameObject.GetComponent<IDeletable>();
            if(deleteScript != null)
                deleteScript.Delete();
        }
    }

    void CameraControl()
    {
        //Changes Camera viewport size with scroll
        if(Input.mouseScrollDelta.y != 0)
        {
            Camera.main.orthographicSize -= Input.mouseScrollDelta.y * scrollSensitivity;
            Debug.Log("Scrolled " + Camera.main.orthographicSize);
        }

        if(Input.GetMouseButtonDown(1))
        {
            mouseStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isSpanning = true;
        }
        else if(Input.GetMouseButtonUp(1))
        {
            isSpanning = false;
        }

        if(isSpanning)
        {
            Vector3 moveDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - mouseStartPos);
            moveDirection.z = 0f;
            Camera.main.transform.position -= moveDirection*spanSensitivity; 
        }
    }

}
