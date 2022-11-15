class GlobalTable {
    public string name {get; set;}
    public string type {get; set;}
    public bool isFinal {get; set;}
    public bool isAvail {get; set;}
    public GlobalTable(string name, string type, bool isAvail, bool isFinal){
        this.name = name;
        this.type = type;
        this.isAvail = isAvail;
        this.isFinal = isFinal; 
    }   
    
    public override string? ToString()
    {
        return "{"+name+", "+type+", "+"static: "+isAvail.ToString()+"}";
    }
}