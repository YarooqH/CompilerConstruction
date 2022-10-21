public class TokenObj
{
    public string lineNum { get; set; }
    public string word { get; set; }
    public TokenClass? classPart { get; set; }

    public TokenObj(string lineNum, TokenClass? classPart, string word)
    {
        this.lineNum = lineNum;
        this.classPart = classPart;
        this.word = word;
    }

    public override string ToString()
    {
        return word.ToString() + " \t\t\t" + classPart.ToString() + " \t\t\t" + lineNum;
    }
    
}
