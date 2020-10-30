﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NewConnection : MonoBehaviour
{
    public Transform prefab;
    private Transform spawn;
    public GameObject connectionMaker;

    private bool wasPressed = false;
    public ConnectionPoint.ConnectionDirection connectionDir;
    public bool isEmpty = true;
    public bool changed = false;
    public Color[] sprites = new Color[2];
    private Image image;
    public Image auxImg = null;
    private CircleCollider2D collider;

    private int plusOrMinus()
    {
        return isEmpty ? 0 : 1;
    }

    private void CancelConnection()
    {
        Debug.Log("cancel");
        GameObject gObj = ConnectionManager.GetOtherSide(this.transform.parent.GetComponent<RectTransform>(), connectionDir);
        if (gObj != null)
        {
            Debug.Log(gObj);
            EntryPoint ep = gObj.GetComponent<EntryPoint>();
            if (ep != null) if( ConnectionManager.DeleteThisConnection(this.transform.parent.GetComponent<RectTransform>(), connectionDir))
            {
                isEmpty = true;
                changed = true;
                ep.isEmpty = true;
                ep.changed = true;
                Debug.Log("sucesso");
            }
            else
            {
                Debug.Log("sem sucesso");
            }
        }
    }


    public void OnMouseUpAsButton()
    {
        if (!ConnectionManager.isConnectionMode && !isEmpty && !EventSystem.current.IsPointerOverGameObject())
        {
            CancelConnection();
        }
    }
    // Start is called before the first frame update
    private void Start()
    {
        image = GetComponent<Image>();
        collider = GetComponent<CircleCollider2D>();
        updateSprite();
    }

    private void OnDestroy()
    {
        if (!isEmpty)
        {
            CancelConnection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!ConnectionManager.isConnectionMode)
        {
            wasPressed = false;
        }
        if (ConnectionManager.isConnectionMode && !changed)
        {
            if (wasPressed)
            {
                changed = true;
                collider.enabled = true;
                image.enabled = true;
                if(auxImg != null){
                    auxImg.enabled = true;
                }
            }
            else
            {
                changed = true;
                collider.enabled = false;
                image.enabled = false;
                if(auxImg != null){
                    auxImg.enabled = false;
                }
            }
            updateSprite();
        }
        else if (!ConnectionManager.isConnectionMode && changed)
        {
            if (connectionDir == ConnectionPoint.ConnectionDirection.South || connectionDir == ConnectionPoint.ConnectionDirection.East)
            {
                collider.enabled = true;
                changed = false;
                image.enabled = true;
                if(auxImg != null){
                    auxImg.enabled = true;
                }
            }
            else
            {
                changed = false;
                collider.enabled = true;
                image.enabled = true;
                if(auxImg != null){
                    auxImg.enabled = true;
                }
            }

            updateSprite();
        }
    }
    private void updateSprite()
    {
        image.color = sprites[plusOrMinus()];
    }

    private bool FindUpperConnection()
    {
        //return true if exists an free upper connection, false if don't
        NewConnection[] conns = this.transform.parent.GetComponentsInChildren<NewConnection>();
        foreach (NewConnection c in conns)
        {
            if (c.connectionDir == ConnectionPoint.ConnectionDirection.North && c.isEmpty)
            {
                return true;
            }
        }
        return false;
    }


    //parte do drag and drop

    private void LateUpdate() {
        if (Input.GetMouseButton(0) && spawn != null)
        {
            var pos = Input.mousePosition;
            pos.z = -Camera.main.transform.position.z;
            spawn.transform.position = Camera.main.ScreenToWorldPoint(pos);
        }

        if (Input.GetMouseButtonUp(0) && spawn != null)
        {
            spawn.transform.GetComponent<linker>().isDeselected = true;
            ClickController.isClickingOnObject = false;
            spawn = null;
        }
    }

    private void OnMouseDown() {
        if(isEmpty){
            isEmpty = false;
            wasPressed = true;
            var pos = Input.mousePosition;
            pos.z = -Camera.main.transform.position.z;
            pos = Camera.main.ScreenToWorldPoint(pos);
            spawn = Instantiate(prefab, pos, Quaternion.identity) as Transform;
            Instantiate(connectionMaker);
            ConnectionMaker connectionM = GameObject.Find("_ConnectionMaker(Clone)").GetComponent<ConnectionMaker>();
            connectionM.AddConnection(this.transform.parent.GetComponent<RectTransform>(), connectionDir);
            connectionM.AddConnection(spawn.GetComponent<RectTransform>(), ConnectionPoint.ConnectionDirection.West);
        }
        
    
    }
}