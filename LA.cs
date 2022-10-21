using System.Collections;
using System.Text.RegularExpressions;

class LA
{
    List<TokenObj> tokens;
    public Dictionary<string,TokenClass> ht;
    Regex? regex;
    public LA()
    {
        tokens = new List<TokenObj>();
        ht = new Dictionary<string,TokenClass>()
        {
            {"+", TokenClass.PM}, {"-", TokenClass.PM}, 
            {"*", TokenClass.MDM},{"/", TokenClass.MDM},
            {"%", TokenClass.MDM},{"!", TokenClass.NOT},
            {">", TokenClass.COMP},{"<", TokenClass.COMP},
            {"<=", TokenClass.COMP},{">=", TokenClass.COMP},
            {"==", TokenClass.COMP},{"!=", TokenClass.COMP},
            {"=", TokenClass.ASIGN},{"&&", TokenClass.AND},
            {"||", TokenClass.OR},{"return", TokenClass.RETURN},
            {"returns", TokenClass.RETURNS},{"(", TokenClass.ORB},
            {")", TokenClass.CRB},{"[", TokenClass.OSB},
            {"]", TokenClass.CRB},{"{",TokenClass.OCB},
            {"}", TokenClass.CCB}, {"if", TokenClass.IF},
            {"else", TokenClass.ELSE},{"for", TokenClass.FOR},
            {"while", TokenClass.WHILE}, {"do", TokenClass.DO},
            {"break", TokenClass.BREAK}, {"continue", TokenClass.CONT},
            {"num", TokenClass.DT}, {"bool", TokenClass.DT},
            {"string", TokenClass.DT}, {";", TokenClass.SEMICOL},
            {":", TokenClass.COL}, {",",TokenClass.COM},
            {">>", TokenClass.EXT}, {"null", TokenClass.NULL}, 
            {"abstract", TokenClass.ABS}, {"avail", TokenClass.AVAIL},
            {"new", TokenClass.NEW}, {"class", TokenClass.CLASS}, 
            {"const", TokenClass.CONST}, {"let", TokenClass.LET}, 
            {"current", TokenClass.CURR}, {"super", TokenClass.PARENT}, 
            {"public", TokenClass.AM}, {"scoped", TokenClass.AM},
            {"protected", TokenClass.AM}

        };
    

    }


    public List<TokenObj> GetTokens(List<string[]> words)
    {
        foreach (string[] w in words)
        {
            if (ht.ContainsKey(w[0])) { tokens.Add(new TokenObj(w[1], ht[w[0]], w[0])); continue; }

            else if (Char.IsNumber(w[0][0]))
            {
                regex = new Regex(@"^[0-9]+$");
                if (regex.IsMatch(w[0])) { tokens.Add(new TokenObj(w[1], TokenClass.NUM, w[0])); continue; }

                regex = new Regex(@"^[0-9]*[.][0-9]+$");
                if (regex.IsMatch(w[0])) { tokens.Add(new TokenObj(w[1], TokenClass.NUM, w[0])); continue; }

                else { tokens.Add(new TokenObj(w[1], TokenClass.ERR, w[0])); continue; }
            }
            else if (w[0][0] == '.')
            {
                if (w[0].Length == 1) { tokens.Add(new TokenObj(w[1], TokenClass.DOT, w[0])); continue; }

                regex = new Regex(@"^[0-9]*[.][0-9]+$");
                if (regex.IsMatch(w[0])) { tokens.Add(new TokenObj(w[1], TokenClass.NUM, w[0])); continue; }

                else { tokens.Add(new TokenObj(w[1], TokenClass.ERR, w[0])); continue; }

            }
            else if (w[0][0] == '"')
            {
                regex = new Regex("^[\"]([\\\\][abfnrtv0\"\'\\\\]|[^(\"\'\\\\)]|[()])*[\"]$");
                if (regex.IsMatch(w[0])) { tokens.Add(new TokenObj(w[1], TokenClass.SC, w[0].Trim('"'))); continue; }

                else { tokens.Add(new TokenObj(w[1], TokenClass.ERR, w[0])); continue; }
            }
            else if (w[0][0] == '\'')
            {
                regex = new Regex("^[\']([\\\\][abfnrtv0\"\'\\\\]|[^(\"\'\\\\)]|[()])[\']$");
                if (regex.IsMatch(w[0])) { tokens.Add(new TokenObj(w[1], TokenClass.CC, w[0].Trim('\''))); continue; }

                else { tokens.Add(new TokenObj(w[1], TokenClass.ERR, w[0])); continue; }

            }
            else
            {
                regex = new Regex("^([a-zA-Z_$][a-zA-Z\\d_$]*)$");
                if (regex.IsMatch(w[0])) { tokens.Add(new TokenObj(w[1], TokenClass.ID, w[0])); continue; }

                else { tokens.Add(new TokenObj(w[1], TokenClass.ERR, w[0])); continue; }
            }
        }
        return tokens;
    }
}