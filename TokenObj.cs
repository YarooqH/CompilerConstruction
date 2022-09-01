public class TokenObj
{
    string lineNum { get; set; }
    string word { get; set; }
    TokenClass? classPart { get; set; }

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
