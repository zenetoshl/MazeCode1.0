﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;
using TMPro;

public class BlocoPrint : Bloco
{
    private Var_Vet_Mat var;

    private void Start()
    {
        uiText.text = "---";
        LeanWindow myWindow = this.GetComponent<InstantiateTerminal>().GetMyWindow();
        var = myWindow.transform.Find("Panel/var_vet_mat").GetComponent<Var_Vet_Mat>();
    }
    public override string ToCode()
    {
        return "if(_output.Length >= 1000000) {_loopError = \"Sua saída possui muitos caracteres, por favor, confira os loops da sua solução. \"; return;}_output += " + var.GetText() + "+ \" \";";
    }

    public override void UpdateUI(bool isOk){
        if(isOk){
            Compiler.instance.Uncompile();
            var.SaveConfig();
            Bloco.changed = true;
            ToUI();
        } else {
            var.ResetConfig();
        }
    }

    public override bool Compile(){
        List<string> scope = VariableManager.GetScope(this.GetComponent<RectTransform>());
        bool noError = true;
        if(!(uiText.text != "---"))
        {
            ErrorLogManager.instance.CreateError("Bloco não inicializado corretamente");
            noError = MarkError(false);
        }
        if(!var.Compile (scope)){
            ErrorLogManager.instance.CreateError("Variavel nao existe no escopo deste bloco");
            noError = MarkError(false);
        }
        MarkError(noError);
        return noError;
    }

    public override void ToUI(){
        uiText.text = var.GetText();
    }
}
