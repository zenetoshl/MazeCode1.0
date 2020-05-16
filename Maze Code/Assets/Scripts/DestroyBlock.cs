﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBlock : MonoBehaviour {

    public InventoryItem blockType;

    private void Start () {
        TerminalInventoryManager.UpdateItemInventory (blockType, -1);
    }

    private void OnDestroy () {
        TerminalInventoryManager.UpdateItemInventory (blockType, 1);
    }
    
}