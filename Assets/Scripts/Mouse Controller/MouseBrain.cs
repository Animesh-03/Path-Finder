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
        Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        clickPos.z = 0;

        if(mode == Mode.SPAWN)
        {
            Node.SpawnNode(clickPos);
            mode = Mode.MOVE;
        }
        else if(mode == Mode.MOVE)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.transform.position, clickPos - Camera.main.transform.position, Mathf.Infinity, LayerMask.GetMask("Node"));
            if(hitInfo.collider != null)
            {
                IMovable moveScript = hitInfo.collider.gameObject.GetComponent<IMovable>();
                moveScript.Pickup();
            }
        }
        else if(mode == Mode.DELETE)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.transform.position, clickPos - Camera.main.transform.position, Mathf.Infinity, LayerMask.GetMask("Node", "Edge"));
            if(hitInfo.collider != null)
            {
                IDeletable deleteScript = hitInfo.collider.gameObject.GetComponent<IDeletable>();
                deleteScript.Delete();
            }
        }
        else if(mode == Mode.EDGE)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.transform.position, clickPos - Camera.main.transform.position, Mathf.Infinity, LayerMask.GetMask("Node"));
            if(hitInfo.collider != null)
            {
                GameObject edge = Instantiate<GameObject>(Resources.Load("Edge") as GameObject);
                edge.GetComponent<Edge>().SpawnEdge(hitInfo.collider.gameObject.GetComponent<Node>());
            }
        }
    }

    void CameraControl()
    {
        // Changes Camera viewport size with scroll
        if(Input.mouseScrollDelta.y != 0)
        {
            if(Input.mouseScrollDelta.y > 0 && Camera.main.orthographicSize > 3f || Input.mouseScrollDelta.y < 0 && Camera.main.orthographicSize < 10f )
                Camera.main.orthographicSize -= Input.mouseScrollDelta.y * scrollSensitivity;
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
