using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour, IMovable, IDeletable
{
#region IMovableVars
    private bool isPickedUp = false;
    public float pickupScale;
#endregion

#region Components
    private SpriteRenderer sr;
#endregion

#region StatusColors
    public Color activeColor;
    public Color baseColor;
#endregion

#region NodeVars
    private string nodeName;
#endregion

    private static GameObject nodePrefab;


    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        nodePrefab = Resources.Load("Node") as GameObject;
    }

    void Update()
    {
        if(isPickedUp)
        {

            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePos.x, mousePos.y, transform.position.z);

            if(Input.GetMouseButtonUp(0))
                Drop();
        }
    }

    public void Pickup()
    {
        isPickedUp = true;
        transform.localScale *= pickupScale;
        SetActive();
    }

    public void Drop()
    {
        isPickedUp = false;
        transform.localScale /= pickupScale;
        SetInactive();
    }

    public void Delete()
    {
        Destroy(gameObject);
    }

    public static void SpawnNode(Vector3 pos)
    {
        GameObject.Instantiate(nodePrefab, pos, Quaternion.identity);
    }

    public void SetActive()
    {
        sr.color = activeColor;
    }

    public void SetInactive()
    {
        sr.color = baseColor;
    }

    public void SetName(string newName)
    {
        if(newName == "")
            nodeName = "Node";
        else
            nodeName = newName;
    }

}
