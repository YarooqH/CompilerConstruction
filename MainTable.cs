using System.Collections;

class MainTable {
    public Dictionary<string, ClassDataTable> classDT {get; set;}
    public string name {get; set;}
    public string type {get; set;}
    public string typeModifier {get; set;}
    public string extendingClass {get; set;}

    public MainTable(string name, string type, string typeModifier, string extendingClass){
        this.name = name;
        this.type = type;
        this.typeModifier = typeModifier;
        this.extendingClass = extendingClass;

        classDT = new Dictionary<string, ClassDataTable>();
    }

    public override string? ToString()
    {
        return "{"+name+", "+type+", "+"typeModifier: "+typeModifier.ToString()+ ", extendingClass: " + extendingClass + "}";
    }

}