using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TargetMouseSelected : MonoBehaviour
{
    [SerializeField] Selectable selectedObject;
    [SerializeField] bool mouseOccupied = false; // True if mouse is occupied by another pocess
    [SerializeField] bool setTargetForAbility = false;
 

    [Tooltip("Mouse Cursor: ")]
    [SerializeField] Texture2D cursorTexture;

    int layerMaskSelectable;
    int layerMaskBuildings;
    int layerMaskUI;
    int layerMask;
    int layerMaskTooltip;

    bool holdingMouse = false;

    private void Start()
    {
        //layerMaskSelectable = 1 << 13;    // Select layerMask 13
        layerMaskBuildings = 1 << 10;    // Select layerMask 13
        layerMaskUI = 1 << 5;   // Select layerMask 5
        layerMask = layerMaskSelectable | layerMaskUI | layerMaskBuildings;  // Select layermask 13 or 5
        layerMaskTooltip = 1 << 5;

        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    private void Update() 
    {
      
        if (Input.GetMouseButton(0) && !mouseOccupied)  // TODO: Select Multiple Targets
        {
            RayCastMouseUI();   // Using layermask for UI
        }
        

        else if(Input.GetMouseButtonUp(0) && !mouseOccupied)    // Target GameObjects
        {
            RayCastMouse();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            UnselectObject();
        }

        //CheckForToolTip();

    }


    private void CheckForToolTip()
    {
        /*
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero, Mathf.Infinity, layerMaskTooltip);

        if (hit)
        {
            Debug.Log("Mouse hit something");
            if (hit.collider.GetComponent<RectTransform>())
            {
                Debug.Log("Mouse colliding with UI element: " + hit.collider.transform.name);
            }
        }
        */

        //Code to be place in a MonoBehaviour with a GraphicRaycaster component
        GraphicRaycaster gr = this.GetComponent<GraphicRaycaster>();
        //Create the PointerEventData with null for the EventSystem
        PointerEventData ped = new PointerEventData(null);
        //Set required parameters, in this case, mouse position
        ped.position = Input.mousePosition;
        //Create list to receive all results
        List<RaycastResult> results = new List<RaycastResult>();
        //Raycast it
        gr.Raycast(ped, results);
        for(int i=0; i<results.Count; i++)
        {
            Debug.Log("Mouse hit: " + results[i].gameObject.name);
        }

        // TODO

        /*
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero, Mathf.Infinity);
        //RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero, Mathf.Infinity);

        if (hit)
        {
            if(hit.collider.tag == "Resource")
            {
                Debug.Log("Mouse collided with resource");
                ResourcePickups resource = hit.collider.GetComponent<ResourcePickups>();
                ToolTip.ShowResourceTooltip_Static(resource.GetResourceType(), screenPos);
            }
        }
        */
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1f);
        Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePos);
        return objectPosition;
    }



    private void RayCastMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero, Mathf.Infinity, layerMask);
        //RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero, Mathf.Infinity);

        if (hit)
        {

            if (hit.collider.tag == "UI")   // Return if UI Canvas is hit
            {
                Debug.Log("Ray hit UI");
                return;
            }

            if (hit.collider.GetComponent<Selectable>())
            {
                Selectable target = hit.collider.GetComponent<Selectable>();
                if (target.GetWaitForClicks() > 0)
                {
                    target.Clicked();
                    return;
                }
                if (!target.IsSelectable())
                {
                    Debug.Log("Target not selectable");
                    return;  // If object is currently not selectable, then return.
                }
                SelectObject(hit.collider.GetComponent<Selectable>());
            }

            else if (selectedObject != null)
            {
                UnselectObject();
            }
        }

        else
        {
            UnselectObject();
        }
    }

    private Collider2D RayCastCollider(string tag)
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero, Mathf.Infinity, layerMask);
        //RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero, Mathf.Infinity);

        if (hit)
        {

            if (hit.collider.tag == "UI")   // Return if UI Canvas is hit
            {
                Debug.Log("Ray hit UI");
                return null;
            }

            if (hit.collider.tag == tag)
            {
           
                return hit.collider;
            }
        }
        return null;
    }

    private void RayCastMouseUI()
    {
        //boxCollider.enabled = true;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;

        Vector3 screenPos = Camera.main.ScreenToWorldPoint(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(screenPos, Vector2.zero, Mathf.Infinity, layerMask);
    }

    public void SelectObject(Selectable selectableObject)
    {
        if(selectedObject != null) UnselectObject();
        selectedObject = selectableObject;
        selectedObject.SelectObject();
    }

    public void UnselectObject()
    {
        if (selectedObject == null) return;
        selectedObject.DeSelectObject();
        selectedObject = null;
    }

    public void SetMouseOccupied(bool value)
    {
        mouseOccupied = value;
    }
}
