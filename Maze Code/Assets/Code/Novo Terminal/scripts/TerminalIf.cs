﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalIf : TerminalBlocks
{
    public int trueAlternativeScopeId;
    public int falseAlternativeScopeId;
    //MathOperation condition;
    public override IEnumerator RunBlock(){
        nextBlock.scopeId = scopeId;
        yield return null;
    }
    public override void ToUI (){

    }
    public override void UpdateUI (bool isOk){

    }
    public override bool Compile (){
        return true;
    }
    public override bool Reset (){
        return true;
    }
    public override void SetNextBlock (TerminalBlocks block){
        
    }
    public override TerminalBlocks GetNextBlock (){
        return null;
    }
    public override void HidefromCamera (){

    }
}
