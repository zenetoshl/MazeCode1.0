﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linker : MonoBehaviour {
    // Start is called before the first frame update
    public bool isDeselected = false;

    // Update is called once per frame
    private void Update () {
        if (isDeselected) {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction);
            if (hit.collider != null) {
                TerminalEntryConnection rt = hit.transform.GetComponent<TerminalEntryConnection> ();
                if (rt != null) {
                    if (rt.transform.parent != null) {
                        rt.isEmpty = false;
                        rt.changed = true;
                        TerminalBlocks tb2 = rt.transform.parent.GetComponent<TerminalBlocks> ();
                        TerminalBlocks tb = ConnectionManager.GetOtherSide(this.GetComponent<RectTransform> (), ConnectionPoint.ConnectionDirection.West).transform.parent.GetComponent<TerminalBlocks> ();
                        Debug.Log(tb);
                        if (tb2 != null && tb != null) {
                            tb.SetNextBlock (tb2, TerminalConnectionManager.connectionDirections[0]);
                        }
                        ConnectionManager.ChangeTarget (this.GetComponent<RectTransform> (), rt.transform.parent.GetComponent<RectTransform> ());
                        Destroy (this.gameObject);
                        return;
                    }
                }
            }
            CancelConnection ();
            Destroy (this.gameObject);
        }
    }

    private void OnDestroy () {
        ClickController.isClickingOnObject = false;
        ConnectionManager.isConnectionMode = false;
    }

    private void Awake () {
        ConnectionManager.isConnectionMode = true;
        ClickController.isClickingOnObject = true;
    }

    private void CancelConnection () {
        RectTransform transform = this.GetComponent<RectTransform> ();
        Debug.Log (transform == null);
        GameObject gObj = ConnectionManager.GetOtherSide (transform, ConnectionPoint.ConnectionDirection.West);
        if (gObj != null) {
            TerminalBlockConnection nc = gObj.GetComponent<TerminalBlockConnection> ();
            if (nc != null)
                if (ConnectionManager.DeleteThisConnection (this.GetComponent<RectTransform> (), ConnectionPoint.ConnectionDirection.West)) {
                    nc.isEmpty = true;
                    nc.changed = true;
                    //Debug.Log("sucesso");F
                }
            else {
                //Debug.Log("sem sucesso");
            }
        }
    }

}