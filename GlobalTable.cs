class GlobalTable {
    public string name {get; set;}
    public string type {get; set;}
    public bool isFinal {get; set;}
    public GlobalTable(string name, string type, bool isFinal){
        this.name = name;
        this.type = type;
        this.isFinal = isFinal; 
    }   
    
    public override string? ToString(){
        return "{"+name+", "+type+"}";
    }
}