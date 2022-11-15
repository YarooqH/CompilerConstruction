using System;
using System.Collections;

class SemanticAnalyzer {
    public Dictionary<string, MainTable> mainTable;
    public Dictionary<string, FunctionDataTable> functionTable;
    public Dictionary<string, GlobalTable> globalTable;
    public int scopeNum = 0;
    public string? currentClassName = null; 
    public List<int> currentScope = new List<int>();
    // public Stack currentScope = new Stack();

    public SemanticAnalyzer(){
        globalTable = new Dictionary<string, GlobalTable>();
        mainTable = new Dictionary<string, MainTable>();
        functionTable = new Dictionary<string, FunctionDataTable>();
    }
    
    // public void createScope(){
    //     scopeNum++;
    //     currentScope.Push(scopeNum);
    // }

    // public void destroyScope(){
    //     currentScope.Pop();
    // }
    public bool insertGlobalData(string name, string type, bool sta, bool final)
    {
        if (globalTable.ContainsKey(name)) return false;
        globalTable[name] = new GlobalTable(name, type,sta, final);
        printGTable();
        return true;

    }
    public bool insertMainTable(string name, string type, string typeModifier, string extendingClass){
        if(mainTable.ContainsKey(name)){
            return false;
        }
        mainTable[name] = new MainTable(name, type, typeModifier, extendingClass);
        printMainTable();
        return true;
    }


    public bool insertFunctionTable(string name, string type){
        if (functionTable.ContainsKey(name)) {
            System.Console.WriteLine("Variable already decleared in Scope");
            return false;
        }
        functionTable[name + scopeNum.ToString()] = new FunctionDataTable(name, type, scopeNum);
        printFTable();
        return true;
    }
    public bool insertClassTable(string name, string type, string accessModifier, bool isStatic,bool isFinal, bool isAbstract){
        string key = (name + ':' + type).Split(">>")[0];

        if(currentClassName != null){
            if (mainTable[currentClassName].classDT.ContainsKey(key) || mainTable[currentClassName].classDT.ContainsKey(name)){
                return false;
            } else if (type.Contains("->")){
                mainTable[currentClassName].classDT[key] = new ClassDataTable(name, type, accessModifier, isStatic, isFinal, isAbstract);
                printCTable();
                return true;
            } else {
                mainTable[currentClassName].classDT[name] = new ClassDataTable(name, type, accessModifier, isStatic, isFinal, isAbstract);
                printCTable();
                return true;
            } 
        } else {
                return false;
        }

    }
    public MainTable? lookUpMT(string name){
        if(mainTable.ContainsKey(name)){
            return mainTable[name];
        }
        return mainTable[name];
    }

    public FunctionDataTable lookUpFT(string name, int scope){
        for(int i = 0;  i < functionTable.Count; i++){
            if (functionTable.ContainsKey(name + scope.ToString())){
                return functionTable[name + scope.ToString()];
            }
        }
        return null;
    }

    public ClassDataTable? lookUpCT(string name, string type, string currentClassName){
        if(mainTable[currentClassName].classDT.ContainsKey((name + ':' + type).Split(">>")[0])){
            return mainTable[currentClassName].classDT[(name + ':' + type).Split(">>")[0]];
        } else {
            return null;
        }
    }

    public string? compatibility(string left, string right, string op){
        if (left == "string" && right == "string")
        {
            if (op == "+") { return "string"; }
            else if (op == "==" || op == "!=") { return "boolean"; }
        }
        else if (left != "string" && right != "string")
        {
            if (op == "<" || op == ">" || op == "==" || op == "<=" || op == ">=" || op == "!=") { return "boolean"; }
            else if (left == "number" && right == "number") { return "number"; }
            // else if (right != "char" && left != "char") { return "number"; }
        }
        return null;
    }

    public void createScope() { scopeNum += 1; currentScope.Add(scopeNum); }
    public void destroyScope() { currentScope.RemoveAt(currentScope.Count - 1); }
      private void printMainTable()
    {
        System.Console.WriteLine("\n\t^^^^SE_Main_Data_Table^^^^");
        foreach (KeyValuePair<string, MainTable> kvp in mainTable)
        {
            System.Console.WriteLine(string.Format("classID = {0}, Value = {1}", kvp.Key, kvp.Value.ToString()));
        }
        System.Console.WriteLine("\n");
    }

    private void printGTable()
    {
        System.Console.WriteLine("\n\t^^^^SE_Global_Data_Table^^^^");
        foreach (KeyValuePair<string, GlobalTable> kvp in globalTable)
        {
            System.Console.WriteLine(string.Format("ID = {0}, Value = {1}", kvp.Key, kvp.Value.ToString()));
        }
        System.Console.WriteLine("\n");
    }    
    private void printCTable()
    {
        System.Console.WriteLine("\n\t^^^^SE_Class_Data_Table^^^^");

        foreach (KeyValuePair<string, ClassDataTable> kvp in mainTable[currentClassName].classDT)
        {
            System.Console.WriteLine(string.Format("classID = {0}, Value = {1}", kvp.Key, kvp.Value.ToString()));
        }
        System.Console.WriteLine("\n");
    }
    private void printFTable()
    {
        System.Console.WriteLine("\n\t^^^^SE_Function_Data_Table^^^^");
        
        // System.Console.WriteLine(function_table.Count);
        foreach (KeyValuePair<string, FunctionDataTable> kvp in functionTable)
        {
            System.Console.WriteLine(string.Format("classID = {0}, Value = {1}", kvp.Key, kvp.Value.ToString()));
        }
        System.Console.WriteLine("\n");
    }
}