﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolTable : MonoBehaviour {
    public class Symbol {
        public string varValue;
        public TerminalEnums.varTypes varType;
        public TerminalEnums.varStructure varStructure;
        public int sizex;
        public int sizey;

        public Symbol (string value, TerminalEnums.varTypes type, TerminalEnums.varStructure structure) {
            varStructure = structure;
            varType = type;
            varValue = value;
            sizex = -1;
            sizey = -1;
        }
        public Symbol (string value, TerminalEnums.varTypes type, TerminalEnums.varStructure structure, int size) {
            varStructure = structure;
            varType = type;
            varValue = value;
            sizex = size;
            sizey = -1;
        }
        public Symbol (string value, TerminalEnums.varTypes type, TerminalEnums.varStructure structure, int size1, int size2) {
            varStructure = structure;
            varType = type;
            varValue = value;
            sizex = size1;
            sizey = size2;
        }
    }

    public class Table {
        public Dictionary<string, Symbol> scope;
        public int parent;

        public Table () {
            parent = -1;
            scope = new Dictionary<string, Symbol> ();
        }

        public Table (int fatherScope) {
            if (fatherScope < 0) {
                parent = -1;
            } else {
                parent = fatherScope;
            }
            scope = new Dictionary<string, Symbol> ();
        }

        public bool CreateVar (string name, string value, TerminalEnums.varTypes type) {
            try {
                scope.Add (name, new Symbol (value, type, TerminalEnums.varStructure.Variable));
                return true;
            } catch (Exception) {
                scope[name] = new Symbol (value, type, TerminalEnums.varStructure.Variable);
                return true;
            }

            return false;
        }
        public bool CreateVar (string name, string value, TerminalEnums.varTypes type, int i) {
            try {
                scope.Add (name, new Symbol (value, type, TerminalEnums.varStructure.Array, i));
                return true;
            } catch (Exception) {
                scope[name] = new Symbol (value, type, TerminalEnums.varStructure.Array, i);
                return true;
            }

            return false;
        }
        public bool CreateVar (string name, string value, TerminalEnums.varTypes type, int i, int j) {
            try {
                scope.Add (name, new Symbol (value, type, TerminalEnums.varStructure.Matrix, i, j));
                return true;
            } catch (Exception) {
                scope[name] = new Symbol (value, type, TerminalEnums.varStructure.Matrix, i, j);
                return true;
            }

            return false;
        }

        public bool ModifyVarValue (string name, string value) {
            try {
                scope[name].varValue = value;
                return true;
            } catch (Exception) {
                Debug.Log ("var not found");
                return false;
            }
            return false;
        }

        public TerminalEnums.varTypes GetVarType (string name) {
            try {
                Symbol s = scope[name];
                return s.varType;
            } catch (Exception) {
                Debug.Log ("var not found");
                return TerminalEnums.varTypes.Null;
            }
            return TerminalEnums.varTypes.Null;
        }

        public string GetVarValue (string name) {
            try {
                Symbol s = scope[name];
                if(s != null)
                if (s.varStructure == TerminalEnums.varStructure.Variable)
                    return s.varValue;
            } catch (Exception) {
                Debug.Log ("var not found");
                return null;
            }
            return null;
        }

        public string GetVarValue (string name, int i) {
            try {
                Symbol s = scope[name];
                if (s.varStructure == TerminalEnums.varStructure.Array && s.sizex >= 0) {
                    string[] splited = s.varValue.Split (',');
                    return splited[i];
                }
            } catch (Exception) {
                Debug.Log ("var not found");
                return null;
            }
            return null;
        }

        public string GetVarValue (string name, int i, int j) {
            try {
                Symbol s = scope[name];
                if (s.varStructure == TerminalEnums.varStructure.Matrix && s.sizex >= 0 && s.sizey >= 0) {
                    string[] splited = s.varValue.Split (',');
                    return splited[(i * s.sizex) + j];
                }
            } catch (Exception) {
                Debug.Log ("var not found");
                return null;
            }
            return null;
        }

        public bool SetVarValue (string name, string value) {
            try {
                Symbol s = scope[name];
                if (s.varStructure == TerminalEnums.varStructure.Variable)
                    ModifyVarValue(name, value);
                    return true;
            } catch (Exception) {
                Debug.Log ("var not found");
                return false;
            }
            return false;
        }

        public bool SetVarValue (string name, string value, int i) {
            try {
                Symbol s = scope[name];
                if (s.varStructure == TerminalEnums.varStructure.Array && s.sizex >= 0) {
                    string[] splited = s.varValue.Split (',');
                    splited[i] = value;
                    string newArr = string.Join(",", splited);
                    ModifyVarValue(name, newArr);
                    return true;
                }
            } catch (Exception) {
                Debug.Log ("var not found");
                return false;
            }
            return false;
        }

        public bool SetVarValue (string name, string value, int i, int j) {
            try {
                Symbol s = scope[name];
                if (s.varStructure == TerminalEnums.varStructure.Matrix && s.sizex >= 0 && s.sizey >= 0) {
                    string[] splited = s.varValue.Split (',');
                    splited[(i * s.sizex) + j] = value;
                    string newArr = string.Join(",", splited);
                    ModifyVarValue(name, newArr);
                    return true;
                }
            } catch (Exception) {
                Debug.Log ("var not found");
                return false;
            }
            return false;
        }

        public void DeleteVar (string name) {
            scope.Remove (name);
        }

        public void PrintTable () {
            Debug.Log ("Father = " + parent);
            foreach (KeyValuePair<string, Symbol> kvp in scope) {
                Debug.Log ("Key = " + kvp.Key + " Value = " + kvp.Value.varValue + " Type = " + kvp.Value.varType);
            }
        }
    }

    public static SymbolTable instance;
    public List<Table> symbolTable;

    private void Awake () {
        instance = this;
        Reset();
    }

    private void Start() {
        TerminalEventManager.instance.resetEvent.AddListener(Reset);
    }

    public void Reset(){
        symbolTable = new List<Table> ();
    }

    public int CreateScope () {
        symbolTable.Add (new Table ());
        return symbolTable.Count - 1;
    }

    public int CreateScope (int outerScope) {
        symbolTable.Add (new Table (outerScope));
        return symbolTable.Count - 1;
    }

    public void PrintSymbolTable () {
        int i = 0;
        foreach (Table t in symbolTable) {
            Debug.Log (i + " - ");
            t.PrintTable ();
            i++;
        }
    }

    public bool CreateVar (int scope, string name, string value, TerminalEnums.varTypes type, TerminalEnums.varStructure structure) {
        if (scope < symbolTable.Count) {
            return symbolTable[scope].CreateVar (name, value, type);
        } else return false;
    }

    public int FindVarScope (string name, int startScope) {
        int searchScope = startScope;
        while (searchScope >= 0) {
            string s = symbolTable[searchScope].GetVarValue (name);
            if (s != null) {
                return searchScope;
            } else searchScope = symbolTable[searchScope].parent;
        }
        return -1;
    }

    public TerminalEnums.varTypes GetVarType (string name, int startScope) {
        int searchScope = startScope;
        while (searchScope >= 0) {
            TerminalEnums.varTypes s = symbolTable[searchScope].GetVarType (name);
            if (s != TerminalEnums.varTypes.Null) {
                return s;
            } else searchScope = symbolTable[searchScope].parent;
        }
        return TerminalEnums.varTypes.Null;
    }

    public string GetVarValue (string name, int startScope) {
        int searchScope = startScope;
        while (searchScope >= 0 && symbolTable.Count > searchScope) {
            string s = symbolTable[searchScope].GetVarValue (name);
            if (s != null) {
                return s;
            } else searchScope = symbolTable[searchScope].parent;
        }
        return null;
    }

    public string GetVarValue (string name, int startScope, int i) {
        int searchScope = startScope;
        while (searchScope >= 0 && symbolTable.Count > searchScope) {
            string s = symbolTable[searchScope].GetVarValue (name, i);
            if (s != null) {
                return s;
            } else searchScope = symbolTable[searchScope].parent;
        }
        return null;
    }

    public string GetVarValue (string name, int startScope, int i, int j) {
        int searchScope = startScope;
        while (searchScope >= 0) {
            string s = symbolTable[searchScope].GetVarValue (name, i, j);
            if (s != null) {
                return s;
            } else searchScope = symbolTable[searchScope].parent;
        }
        return null;
    }

    public bool SetVarValue (string name, string value, int startScope) {
        int searchScope = startScope;
        Debug.Log("-----------------");
        PrintSymbolTable();
        Debug.Log(name);
        Debug.Log(startScope);
        while (searchScope >= 0 && symbolTable.Count > searchScope) {
            if (symbolTable[searchScope].SetVarValue (name, value)) {
                return true;
            } else searchScope = symbolTable[searchScope].parent;
        }
        return false;
    }

    public bool SetVarValue (string name, string value, int startScope, int i) {
        int searchScope = startScope;
        while (searchScope >= 0) {
            if (symbolTable[searchScope].SetVarValue (name, value, i)) {
                return true;
            } else searchScope = symbolTable[searchScope].parent;
        }
        return false;
    }

    public bool SetVarValue (string name, string value, int startScope, int i, int j) {
        int searchScope = startScope;
        while (searchScope >= 0) {
            if (symbolTable[searchScope].SetVarValue (name, value, i, j)) {
                return true;
            } else searchScope = symbolTable[searchScope].parent;
        }
        return false;
    }

    public string GetValueFromString (string input, int scope) { //var, var[i], var[i][j]
        Dictionary<char, char> bracketPairs = new Dictionary<char, char> () { { '[', ']' }
        };
        Stack<char> brackets = new Stack<char> ();
        input = RemoveSpaces (input);
        int init = -1;
        int[] indexes = {-1, -1 };
        int j = 0;
        // Iterate through each character in the input string
        for (int i = 0; i < input.Length; i++) {
            // check if the character is one of the 'opening' brackets
            if (input[i] == '[') {
                if (brackets.Count == 0)
                    init = i + 1;
                // if yes, push to stack
                brackets.Push (input[i]);
            } else
                // check if the character is one of the 'closing' brackets
                if (brackets.Count > 0) {
                    // check if the closing bracket matches the 'latest' 'opening' bracket
                    if (input[i] == ']') {
                        brackets.Pop ();
                        if (brackets.Count == 0) {
                            if (input[init] >= '0' && input[init] <= '9') {
                                indexes[j] = Convert.ToInt32 (input.Substring (init, i - init));
                            } else {
                                indexes[j] = Convert.ToInt32 (GetValueFromString (input.Substring (init, i - init), scope));
                            }
                            j++;
                            continue;
                        }
                    }
                } else
                    continue;
        }
        input = input.Split('[')[0];
        // Ensure all brackets are closed
        if( indexes[1] > -1){
            return GetVarValue (input, scope, indexes[0], indexes[1]);
        } else if( indexes[0] > -1){
            return GetVarValue (input, scope, indexes[0]);
        }
        return GetVarValue (input, scope);
    }

    static string RemoveSpaces (string s) {
        if (s[0] == ' ') {
            s = s.Substring (1);
        }
        if (s[s.Length - 1] == ' ') {
            s = s.Substring (0, s.Length - 1);
        }
        return s;
    }

    public bool SetValueFromString (string input, int scope, string value) { //var, var[i], var[i][j]
        Dictionary<char, char> bracketPairs = new Dictionary<char, char> () { { '[', ']' }
        };
        Stack<char> brackets = new Stack<char> ();
        input = RemoveSpaces (input);
        int init = -1;
        int[] indexes = {-1, -1 };
        int j = 0;
        // Iterate through each character in the input string
        for (int i = 0; i < input.Length; i++) {
            // check if the character is one of the 'opening' brackets
            if (input[i] == '[') {
                if (brackets.Count == 0)
                    init = i + 1;
                // if yes, push to stack
                brackets.Push (input[i]);
            } else
                // check if the character is one of the 'closing' brackets
                if (brackets.Count > 0) {
                    // check if the closing bracket matches the 'latest' 'opening' bracket
                    if (input[i] == ']') {
                        brackets.Pop ();
                        if (brackets.Count == 0) {
                            if (input[init] >= '0' && input[init] <= '9') {
                                indexes[j] = Convert.ToInt32 (input.Substring (init, i - init));
                            } else {
                                indexes[j] = Convert.ToInt32 (GetValueFromString (input.Substring (init, i - init), scope));
                            }
                            j++;
                            continue;
                        }
                    }
                } else
                    continue;
        }
        input = input.Split('[')[0];
        // Ensure all brackets are closed
        if( indexes[1] > -1){
            return SetVarValue (input, value, scope, indexes[0], indexes[1]);
        } else if( indexes[0] > -1){
            return SetVarValue (input, value, scope, indexes[0]);
        }
        return SetVarValue (input, value, scope);
    }
}