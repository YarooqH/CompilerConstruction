class FunctionDataTable{
    public string name {get; set;}
    public string type {get; set;}
    public int scope {get; set;}
    public FunctionDataTable(string name, string type, int scope){
        this.name = name;
        this.type = type;
        this.scope = scope;
    }
}