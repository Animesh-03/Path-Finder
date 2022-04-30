using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour, IDeletable
{
    
#region Components
    private LineRenderer lr;
    private CapsuleCollider2D cc;
#endregion

    public bool edgeSet = false;

#region EdgeVars
    private string weight;
    [SerializeField]
    private Node startNode;
    [SerializeField]
    private Node endNode;
#endregion


    void Start()
    {
        
    }

    void Update()
    {   
        //While edge is not confirmed make the end point of line renderer follow the mouse
        if(!edgeSet)
        {
            lr.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else
        {
            lr.SetPosition(0, startNode.transform.position);
            lr.SetPosition(1, endNode.transform.position);
        }
        //If edge was not confirmed and mouse button was released then call SetNode
        if(!edgeSet && Input.GetMouseButtonUp(0))
        {
            SetNode();
        }
    }
    //Called by MouseBrain after Instantiating. Sets the number of points in Line renderer to 2 and sets the staring point to the node selected
    public void SpawnEdge(Node par)
    {
        lr = GetComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.SetPosition(0, par.transform.position);
        startNode = par;
        startNode.SetActive();
        endNode = null;
    }

    private void SetNode()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position, Mathf.Infinity, LayerMask.GetMask("Node"));
        
        if(hitInfo.collider != null)
            endNode = hitInfo.collider.gameObject.GetComponent<Node>();
        //If the point where mouse was released did not contain a node then delete the edge
        if(endNode != null)
        {
            //Confirms the edge
            edgeSet = true;                
            lr.SetPosition(1, endNode.transform.position);
            transform.position = (startNode.transform.position + endNode.transform.position)/2f;

            //Adds a capsule collider and adjusts its properties
            cc = gameObject.AddComponent<CapsuleCollider2D>();
            cc.direction = CapsuleDirection2D.Horizontal;
            float angle = Mathf.Atan2((lr.GetPosition(0) - lr.GetPosition(1)).y, (lr.GetPosition(0) - lr.GetPosition(1)).x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
            cc.offset = new Vector2();
            cc.size = new Vector2(GetEdgeLength() * 0.9f, 0.5f);

            startNode.SetInactive();
        }
        else
        {
            Delete();
        }
    }

    //Get the distance between two nodes using line renderer
    private float GetEdgeLength()
    {
        float len = (lr.GetPosition(0) - lr.GetPosition(1)).magnitude;
        // Debug.Log("l1: " + lr.GetPosition(0) + " l2: " + lr.GetPosition(1) + " len:" + (lr.GetPosition(0) - lr.GetPosition(1)).magnitude);
        return len;
    }

    public void Delete()
    {
        Destroy(gameObject);
    }
}
