﻿using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

abstract public class TerminalBlocks : MonoBehaviour {
    public Image ShadowError;
    public Image ShadowExec;
    public int angle = 0;
    private bool initialized = false;
    public int scopeId;
    public TerminalBlocks nextBlock = null;
    public TextMeshProUGUI uiText;
    public GameObject windowGO;
    public LeanWindow window = null;

    public abstract IEnumerator RunBlock ();
    public abstract void ToUI ();
    public abstract void SetNextBlock (TerminalBlocks block, ConnectionPoint.ConnectionDirection cd);
    public abstract TerminalBlocks GetNextBlock ();
    public abstract void UpdateUI (bool isOk);
    public abstract bool Compile ();
    public abstract void Reset ();
    public abstract void HidefromCamera ();

    public bool MarkError (bool b) {
        ShadowError.enabled = !b;
        return b;
    }

    public bool MarkExec () {
        ShadowExec.enabled = true;
        return true;
    }

    public void AfterExec(){
        ShadowExec.enabled = false;
    }

    private void Awake () {
        uiText.text = "---";
        TerminalEventManager.instance.resetEvent.AddListener(Reset);
        if (windowGO == null) return;
        
        GameObject terminal = GameObject.Find ("/Terminal");
        GameObject go = Instantiate (windowGO, terminal.transform);
        window = go.transform.GetComponent<LeanWindow> ();
        window.GetComponent<BlockWindow>().myBlock = this.GetComponent<TerminalBlocks>();
    }

    public virtual void TurnOn () {
        if (window == null) return;

        window.TurnOn ();
    }

    public void ListVars(){
        VarLister vl = window.transform.GetComponent<VarLister>();
        if(vl == null) return;
        Debug.Log(VariableManager.ListNames()[0]);
        vl.ListVars(VariableManager.ListNames());
    }
}