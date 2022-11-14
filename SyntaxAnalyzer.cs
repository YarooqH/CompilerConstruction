using System.Collections;
using System.Collections.Generic;
public class SyntaxAnalyzer
{
    SemanticAnalyzer SE = new SemanticAnalyzer();
    Dictionary<string, List<string[]>> rules;
    List<TokenObj> tokens;
    List<TokenObj> pTokens;
    HashSet<string> bTokens;
    int index = 0;
    bool expmode = false;
    // LexicalAnalyzer la = new LexicalAnalyzer();
    public SyntaxAnalyzer(List<TokenObj> tokens)
    {
        this.rules = new Dictionary<string,List<string[]>>();
        this.tokens = tokens;
        this.getRules();
    }
    private void getRules()
    {
        foreach (string line in System.IO.File.ReadLines(@".\files\CFG.txt"))
        {
            if (line == "") { continue; }
            if (line[0] == '#') { continue; }
            string[] arr = line.Split("->");
            if (rules.ContainsKey(arr[0].Trim())) rules[arr[0].Trim()].Add(arr[1].Trim().Split(" "));   
            else
            {
                List<string[]> val = new List<string[]>();
                val.Add(arr[1].Trim().Split(" "));
                // Console.WriteLine(arr[1]);
                rules.Add(arr[0].Trim(), val);
            }
        }
    }
   
    public void printRules()
    {
        string[] keys = new string[rules.Keys.Count];
        rules.Keys.CopyTo(keys, 0);
        index = 0;
        foreach (List<string[]> items in rules.Values)
        {
            System.Console.Write($"{keys[index]} -> ");
            index += 1;
            System.Console.Write("[");
            foreach (string[] item in items)
            {
                System.Console.Write("[");
                foreach (string s in item)
                {
                    System.Console.Write(s);
                    System.Console.Write(",");
                }
                System.Console.Write("]");

            }
            System.Console.Write("]\n");
        }
    }
    public bool checkSyntax()
    {
        if (helper("<START>") && index >= tokens.Count)
        {
            System.Console.WriteLine("INDEX == " + index);
            return true;
        }
        else
        {
            System.Console.WriteLine("INDEX == " + index);
            return false;
        }
    }
    private bool helper(String currentNT)
    {
        // System.Console.WriteLine(rules[currentNT]);

        List<String[]> productionRules = rules[currentNT];
        System.Console.WriteLine("=======================");

        foreach (String[] pr in productionRules)        
        {
            System.Console.WriteLine(" % " + currentNT + " -> " );
            int prev = index;
            int j = 0;
            for (; j < pr.Length; j++) {
                String element = pr[j];                
                System.Console.WriteLine("\nCFG Terminal " + element + " of :" + currentNT );
                if (element[0] == '^') {
                    ++index;
                    return true;
                }
                if (element[0] == '<') {
                    System.Console.WriteLine("into => " + element);
                    if (!helper(element)){
                        System.Console.WriteLine("tracking back, Did not match");
                        index = prev;
                        break;
                    }
                }
                else if (element.Length == 1 && element[0] == 'E'){
                    continue;
                }
                else
                {
                    // System.Console.WriteLine("HERE IN TERMINAL");
                    System.Console.WriteLine("token type = " + tokens[index].classPart.ToString());
                    System.Console.WriteLine("word = " + tokens[index].word);
                    
                    if (string.Equals(element, tokens[index].classPart.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        checkScope();
                        pTokens.Add(tokens[index]);
                        if (tokens[index].classPart == TokenClass.SC || tokens[index].classPart == TokenClass.OCB)
                        {
                            if (!secheck()) { return false; }
                        }
                        index++;
                        System.Console.WriteLine("Matched Terminal = " + element);
                    } else {
                        break;
                    }
                }
            }
            if (j == pr.Length){
                System.Console.WriteLine("Successfully parsed from here");
                return true;
            } else {
                index = prev;
            }

        }
        return false;
    }

    private bool secheck()
    {
        if (pTokens[1].classPart == TokenClass.CLASS || pTokens[2].classPart == TokenClass.CLASS)
        {
            return classSE();
        }
        return true;
    }

    private bool classSE()
    {
        string name = "";
        string type = "";
        string tm = "";
        string ext = "";
        int i = 0;
        for (; i < pTokens.Count; i++)
        {
            if (pTokens[i].classPart == TokenClass.CLASS)
            {
                i++;
                type = "CLASS";
                name = pTokens[i].word;
            }

            else if (pTokens[i].classPart == TokenClass.CONST || pTokens[i].classPart == TokenClass.ABS) tm = pTokens[i].classPart.ToString();

            else if (pTokens[i].classPart == TokenClass.EXT)
            {
                i++;
                for (; i < pTokens.Count; i++)
                {
                    if (pTokens[i].classPart == TokenClass.OCB) continue;

                    else if (pTokens[i].classPart == TokenClass.COM) ext += ",";

                    else ext += pTokens[i].word.ToString();
                }
            }
        }

        if (SE.lookUpMT(name) != null) { System.Console.WriteLine("Re-Decleared class:" + name + " at lineNo: " + tokens[index - 1].lineNum); return false; }

        else if (SE.lookUpMT(ext) == null && ext != "") { System.Console.WriteLine("Parent class : " + ext + " isn't Decleared"); return false; }

        else if (SE.lookUpMT(ext) != null)
        {
            if (SE.lookUpMT(ext)?.typeModifier == "CONST") { System.Console.WriteLine("Parent class : " + ext + " is Decleared as FINAL class"); return false; }
        }
        pTokens.Clear();
        SE.currentClassName = name;
        return SE.insertMainTable(name, type, tm, ext);
    }

     public void checkScope()
    {
        System.Console.WriteLine("Matched Terminal = " + tokens[index].classPart.ToString());

        if (tokens[index].classPart == TokenClass.CLASS) { SE.currentScope.Add(0); printScopeStack(); }

        else if (tokens[index].classPart.ToString() == "ORB" && (tokens[index - 1].classPart.ToString() == "ID" || tokens[index - 1].classPart.ToString() == "EXECUTE")) { SE.createScope(); printScopeStack(); }

        else if (tokens[index].classPart.ToString() == "OCB" && tokens[index - 1].classPart.ToString() == "CRB") { SE.createScope(); printScopeStack(); }

        else if (tokens[index].classPart.ToString() == "CCB") { SE.destroyScope(); printScopeStack(); }
    }
    private void printScopeStack()
    {
        System.Console.WriteLine("Scope Count " + SE.scopeNum);
        System.Console.WriteLine("--scope stack--");
        System.Console.Write("[ ");

        foreach (int val in SE.currentScope) System.Console.Write(val + ",");

        System.Console.Write(" ]");
        System.Console.WriteLine();
    }
    string? getType(List<TokenObj> expression)
    {

        return null;
    }

    // private bool updateExpMode()
    // {
    //     if (tokens[index].classPart.ToString() == "ORB" || tokens[index].classPart.ToString() == "ASI" || tokens[index].classPart.ToString() == "OSB") { expmode = true; }
    //     else if (tokens[index].classPart.ToString() == "CRB" || tokens[index].classPart.ToString() == "SC" || tokens[index].classPart.ToString() == "CSB") { expmode = false; getType(expression); }
    //     return expmode;
    // }

}