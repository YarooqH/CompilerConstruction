using System;
using System.Collections;

class SemanticAnalyzer {
    public Dictionary<string, MainTable> mainTable;
    public Dictionary<string, FunctionDataTable> functionTable;
    public int scopeNum = 0;

    public string currentClassName; 
    Stack currentScope = new Stack();

    public SemanticAnalyzer(){
        mainTable = new Dictionary<string, MainTable>();
        functionTable = new Dictionary<string, FunctionDataTable>();
    }

    public void createScope(){
        scopeNum++;
        currentScope.Push(scopeNum);
    }

    public void destroyScope(){
        currentScope.Pop();
    }

    bool insertMainTable(string name, string type, string typeModifier, string extendingClass){
        if(mainTable.ContainsKey(name)){
            return false;
        }
        mainTable[name] = new MainTable(name, type, typeModifier, extendingClass);
        return true;
    }

    MainTable? lookUpMT(string name){
        if(mainTable.ContainsKey(name)){
            return mainTable[name];
        }
        return mainTable[name];
    }

    bool insertFunctionTable(string name, string type, int scope){
        if (functionTable.ContainsKey(name)) {
            return false;
        }

        functionTable[name + scope.ToString()] = new FunctionDataTable(name, type, scope);
        return true;
    }

    FunctionDataTable lookUpFT(string name, int scope){
        for(int i = 0;  i < functionTable.Count; i++){
            if (functionTable.ContainsKey(name + scope.ToString())){
                return functionTable[name + scope.ToString()];
            }
        }
        return null;
    }

    bool insertClassTable(string name, string type, string accessModifier, bool isFinal, bool isAbstract, string currentClassName){
        string key = (name + ':' + type).Split(">>")[0];

        if (mainTable[currentClassName].classDT.ContainsKey(key) || mainTable[currentClassName].classDT.ContainsKey(name)){
            return false;
        } else if (type.Contains("->")){
            mainTable[currentClassName].classDT[key] = new ClassDataTable(name, type, accessModifier, isFinal, isAbstract);
            return true;
        } else {
            mainTable[currentClassName].classDT[name] = new ClassDataTable(name, type, accessModifier, isFinal, isAbstract);
            return true;
        }
    }

    // ClassDataTable? lookUpCT(string name, string currentClassName){
    //     if(mainTable[currentClassName].classDT.ContainsKey(name)){
    //         return mainTable[currentClassName].classDT[name];
    //     } else {
    //         return null;
    //     }
    // }
    ClassDataTable? lookUpCT(string name, string type, string currentClassName){
        if(mainTable[currentClassName].classDT.ContainsKey((name + ':' + type).Split(">>")[0])){
            return mainTable[currentClassName].classDT[(name + ':' + type).Split(">>")[0]];
        } else {
            return null;
        }
    }
}