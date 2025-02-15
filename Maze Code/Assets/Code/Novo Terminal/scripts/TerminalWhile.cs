﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TerminalWhile : TerminalBlocks {
    public int alternativeScopeId;
    public TerminalBlocks alternativeBlock;
    private bool isScopeCreated = false;
    public string operation;

    private TextMeshProUGUI op;

    private string oldOp;
    
    private void Start() {
        op = window.transform.Find("Panel/Content/Operation").GetComponent<TextMeshProUGUI>();
        oldOp = op.text;
    }

    public override void TurnOn () {
        if (window == null) return;
        ListVars();
        window.TurnOn ();
    }

    public override IEnumerator RunBlock () {
        if(TerminalCancelManager.instance.cancel){
            yield return null;
        }
        alternativeScopeId = SymbolTable.instance.CreateScope (scopeId);
        isScopeCreated = true;
        
        MarkExec();

        if (alternativeBlock != null) {
            alternativeBlock.scopeId = alternativeScopeId;

            while (OperationManager.StartOperation (operation, TerminalEnums.varTypes.Bool, scopeId) == "True" && !TerminalCancelManager.instance.cancel) {
                uiText.text = "True";
                yield return StartCoroutine (alternativeBlock.RunBlock ());
            }
            if(TerminalCancelManager.instance.cancel){
                yield return null;
            }
            uiText.text = "False";
        }

        yield return new WaitForSeconds(ExecTimeManager.instance.execTime);
        //call Next
        if (nextBlock != null  && !TerminalCancelManager.instance.cancel) {
            nextBlock.scopeId = scopeId;
            yield return StartCoroutine (nextBlock.RunBlock ());
        }
        AfterExec();
        yield return null;
    }
    public override void ToUI () {
        operation = op.text;
        uiText.text = operation;

    }
    public override void UpdateUI (bool isOk) {
        if(isOk){
            oldOp = op.text;
            ToUI();
        } else {
            op.text = oldOp;
        }
    }
    public override bool Compile () {
        bool noError = true;
        if(!(uiText.text != "---"))
        {
            ErrorLogManager.instance.CreateError("Bloco não inicializado corretamente");
            noError = MarkError(false);
        }
        if(!(op.text != null && op.text != "")){
            ErrorLogManager.instance.CreateError("Operação invalida");
            noError = MarkError(false);
        }
        MarkError(noError);
        return noError;
    }
    public override void Reset () {
        ToUI();
        return;
    }
    public override void SetNextBlock (TerminalBlocks block, ConnectionPoint.ConnectionDirection cd) {
        if(cd == ConnectionPoint.ConnectionDirection.South){
            alternativeBlock = block;
        } else {
            nextBlock = block;
        }
    }

    public override TerminalBlocks GetNextBlock () {
        return null;
    }
    public override void HidefromCamera () {

    }
}