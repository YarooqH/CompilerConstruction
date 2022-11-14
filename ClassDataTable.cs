class ClassDataTable{
    public string name {get; set;}
    public string type {get; set;}
    public string accessModifier {get; set;}
    // public bool isStatic {get; set;}

    public bool isFinal {get; set;}

    public bool isAbstract {get; set;}

    public ClassDataTable(string _name, string _type, string _accessModifier, bool _final, bool _abstract){
        this.name = _name;
        this.type = _type;
        this.accessModifier = _accessModifier;
        // this.isStatic = _static;
        this.isFinal = _final;
        this.isAbstract = _abstract;
    }

     public override string? ToString()
    {
        return "{"+name+", "+type+", "+"AM: "+accessModifier.ToString()+", isFinal: " + isFinal.ToString() + ", isAbstract: " + isAbstract.ToString() + "}";
    }
}