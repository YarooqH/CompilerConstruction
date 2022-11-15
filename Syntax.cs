using System.Collections;
using System.Collections.Generic;
public class Syntax
{
    SemanticAnalyzer SE = new SemanticAnalyzer();
    Dictionary<string, List<string[]>> rules;
    List<TokenObj> tokens;
    List<TokenObj> pTokens;
    bool bracktCheck = true;
    // HashSet<string> bTokens;
    int index = 0;
    // bool expmode = false;
    // LexicalAnalyzer la = new LexicalAnalyzer();
    public Syntax(List<TokenObj> tokens)
    {
        this.rules = new Dictionary<string,List<string[]>>();
        this.tokens = tokens;
        this.pTokens = new List<TokenObj>();
        // this.bTokens = new HashSet<string>() {  };
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
        
        // System.Console.WriteLine("=======================");

        foreach (String[] pr in productionRules)        
        {
            // System.Console.WriteLine(" % " + currentNT + " -> " );
            int prev = index;
            int j = 0;
            for (; j < pr.Length; j++) {
                String element = pr[j];                
                // System.Console.WriteLine("\nCFG Terminal " + element + " of :" + currentNT );
                if (element[0] == '^') {
                    ++index;
                    return true;
                }
                if (element[0] == '<') {
                    // System.Console.WriteLine("into => " + element);
                    if (!helper(element)){
                        // System.Console.WriteLine("tracking back, Did not match");
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
                    // System.Console.WriteLine("token type = " + tokens[index].classPart.ToString());
                    // System.Console.WriteLine("word = " + tokens[index].word);
                    
                    if (string.Equals(element, tokens[index].classPart.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        checkScope();
                        if(tokens[index].classPart != TokenClass.CCB){
                            pTokens.Add(tokens[index]);
                        }

                        // if(tokens[index].classPart == TokenClass.SEMICOL || tokens[index].classPart == TokenClass.OCB){
                        //     if (!secheck()) { return false; }
                        //     pTokens.Clear();
                        // }

                        if (tokens[index].classPart == TokenClass.OCB && tokens[index - 1].classPart == TokenClass.ASIGN) {
                            bracktCheck =false;
                        }

                         if (pTokens.Count > 1)
                        {
                            if (tokens[index].classPart == TokenClass.CCB &&
                                pTokens[1].classPart != TokenClass.CLASS &&
                                pTokens[0].classPart != TokenClass.FUNC &&
                                pTokens[0].classPart != TokenClass.WHILE &&
                                pTokens[0].classPart != TokenClass.IF &&
                                pTokens[0].classPart == TokenClass.MAIN) { pTokens.Add(tokens[index]); }

                            if (tokens[index].classPart == TokenClass.SEMICOL ||
                                (tokens[index].classPart == TokenClass.OCB &&
                                (pTokens[0].classPart == TokenClass.MAIN ||
                                 pTokens[1].classPart == TokenClass.CLASS ||
                                 pTokens[0].classPart == TokenClass.FUNC ||
                                 pTokens[0].classPart == TokenClass.WHILE ||
                                 pTokens[0].classPart == TokenClass.IF)))
                            {
                                if (!secheck()) return false;

                                pTokens.Clear();
                                bracktCheck = true;
                            }
                        }

                        // Console.Write(tokens[index]);
                        // if (tokens[index].classPart == TokenClass.SC || tokens[index].classPart == TokenClass.OCB)
                        // {
                        //     if (!secheck()) { return false; }
                        // }
                        index++;
                        // System.Console.WriteLine("Matched Terminal = " + element);
                    } else {
                        break;
                    }
                }
            }
            if (j == pr.Length){
                // System.Console.WriteLine("Successfully parsed from here");
                return true;
            } 
            // else {
            //     index = prev;
            // }

        }
        return false;
    }

    private bool secheck()
    {
        printpTokens();

        if (pTokens[1].classPart == TokenClass.CLASS || pTokens[2].classPart == TokenClass.CLASS) { return classSE(); }

        else if (pTokens[0].classPart == TokenClass.AM
                || pTokens[0].classPart == TokenClass.ID
                || pTokens[0].classPart == TokenClass.DT || pTokens[0].classPart == TokenClass.LET || pTokens[0].classPart == TokenClass.CONST) { return VariableSE(); }

        else if (pTokens[0].classPart == TokenClass.FUNC || pTokens[0].classPart == TokenClass.MAIN) { return FuncSE(); }

        // else if (pTokens[0].classPart == TokenClass.RETURN) { return ReturnSE(); } //to be implemented

        // else if (pTokens[0].classPart == TokenClass.IF || pTokens[0].classPart == TokenClass.WHILE) { return If_WhileSE(); } //to be implemented

        // else if (pTokens[0].classPart == TokenClass.LK) { return true; }

        // else { return SimpleStatementSE();  } //to be implemented

        return true;
        void printpTokens()
        {
            System.Console.WriteLine("\n\t^^^^-Parsed Tokens List-^^^^");
            pTokens.ForEach(Console.Write); System.Console.WriteLine("");
        }
    }

     private bool FuncSE()
    {
        string name = "", type = "", stype = "", sname = "", am = "";

        bool sta = false, final = false, abstrac = false;

        for (int i = 0; i < pTokens.Count; i++)
        {
            // if((pTokens[i].classPart == TokenClass.ID || pTokens[i].classPart == TokenClass.EXECUTE) && pTokens[i+1].classPart == TokenClass.ORB ){
            //     name = pTokens[i].word;
            // }
            // uppar alternate check hai
            if (pTokens[i].classPart == TokenClass.ORB)
            {
                name = pTokens[i - 1].word;
            }
            else if (pTokens[i].classPart == TokenClass.ID && pTokens[i + 1].classPart != TokenClass.ORB)
            {
                sname = pTokens[i].word;
            }
            else if (pTokens[i].classPart == TokenClass.COL)
            {
                type += "->" + pTokens[i + 1].word;
                i++;
            }
            else if (pTokens[i].classPart == TokenClass.OSB || pTokens[i].classPart == TokenClass.CSB)
            {
                type += pTokens[i].word;
                stype += pTokens[i].word;
            }
            else if (pTokens[i].classPart == TokenClass.AM)
            {
                am = pTokens[i].word;
            }
            else if (pTokens[i].classPart == TokenClass.AVAIL)
            {
                sta = true;
            }
            else if (pTokens[i].classPart == TokenClass.ABS)
            {
                abstrac = true;
            }
            else if (pTokens[i].classPart == TokenClass.CONST)
            {
                final = true;
            }

            else if (pTokens[i].classPart == TokenClass.COM || pTokens[i].classPart == TokenClass.CRB)
            {
                if (pTokens[i].classPart == TokenClass.COM) type += ",";
                if (sname != "")
                {
                    if (!SE.insertFunctionTable(sname, stype)) return false;
                    sname = "";
                    stype = "";
                }
            }
            else if ((pTokens[i].classPart == TokenClass.DT || pTokens[i].classPart == TokenClass.ID) && pTokens[i + 1].classPart == TokenClass.ID)
            {
                if (pTokens[i].classPart == TokenClass.ID)
                {
                    MainTable? mt = SE.lookUpMT(pTokens[i].word);

                    if (mt == null) { System.Console.WriteLine("\nNo refference found for: " + pTokens[i].word + " on lineNum: " + pTokens[i].lineNum); Environment.Exit(0); }

                    if (mt.typeModifier == "ABSTRACT") { System.Console.WriteLine("\nCan't make objects for abstract class: " + pTokens[i].word + "on lineNum: " + pTokens[i].lineNum); Environment.Exit(0); }
                }
                type += pTokens[i].word;
                stype = pTokens[i].word;
            }
            else if (pTokens[i].classPart == TokenClass.OCB || pTokens[i].classPart == TokenClass.SEMICOL)
            {
                if (pTokens[1].classPart != TokenClass.AM)
                {
                    if (!SE.insertGlobalData(name, type, sta, final)) { System.Console.WriteLine("\nFunction RE-deleared on lineNum: " + pTokens[i].lineNum); Environment.Exit(0); }
                }
                else
                {
                    if (!type.Contains("->") && SE.currentClassName != name) { System.Console.WriteLine("\nInValid Constructor Declaration on lineNum: " + pTokens[i].lineNum); Environment.Exit(0); }

                    if (!type.Contains("->")) type += "->";

                    if (!SE.insertClassTable(name, type, am, sta, final, abstrac)) { System.Console.WriteLine("\nFunction RE-deleared on lineNum: " + pTokens[i].lineNum); Environment.Exit(0); }
                }
            }
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

        if (SE.lookUpMT(name) != null) { System.Console.WriteLine("Re-Decleared class:" + name + " at lineNum: " + tokens[index - 1].lineNum); return false; }

        else if (SE.lookUpMT(ext) == null && ext != "") { System.Console.WriteLine("Parent class : " + ext + " isn't Decleared"); return false; }

        else if (SE.lookUpMT(ext) != null)
        {
            if (SE.lookUpMT(ext)?.typeModifier == "CONST") { System.Console.WriteLine("Parent class : " + ext + " is Decleared as FINAL class"); return false; }
        }
        // pTokens.Clear();
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

        // System.Console.WriteLine(SE.currentScope.ToString());

        foreach (int val in SE.currentScope) System.Console.Write(val + ",");

        System.Console.Write(" ]");
        System.Console.WriteLine();
    }
    string? getType(List<TokenObj> expression)
    {
        return null;
    }

     private bool SimpleStatementSE()
    {
        throw new NotImplementedException();
    }

    private bool If_WhileSE()
    {
        throw new NotImplementedException();
    }

    private bool ReturnSE()
    {
        throw new NotImplementedException();
    }

    private bool VariableSE()
    {
        string name = "", type = "", am = "", valueType = "";

        bool sta = false, final = false;

        for (int i = 0; i < pTokens.Count; i++)
        {
            if (pTokens[i].classPart == TokenClass.ASIGN) { 
                // valueType = getExpType(i + 1); 
            }

            else if (pTokens[i].classPart == TokenClass.AM) { am = pTokens[i].word; }

            else if (pTokens[i].classPart == TokenClass.AVAIL) { sta = true; }

            else if (pTokens[i].classPart == TokenClass.CONST) { final = true; }

            else if (pTokens[i].classPart == TokenClass.OSB || pTokens[i].classPart == TokenClass.CSB) { type += pTokens[i].word; }

            else if (pTokens[i].classPart == TokenClass.DT) { type = pTokens[i].word; name = pTokens[i + 1].word; i++; }

            else if (pTokens[i].classPart == TokenClass.ID && pTokens[i + 1].classPart == TokenClass.ID)
            {
                MainTable? mt = SE.lookUpMT(pTokens[i].word);

                if (mt == null) { System.Console.WriteLine("\nNo refference found for: " + pTokens[i].word + " on lineNum: " + pTokens[i].lineNum); Environment.Exit(0); }

                if (mt.typeModifier == "ABSTRACT") { System.Console.WriteLine("\nCan't make objects for abstract class: " + pTokens[i].word + "on lineNum: " + pTokens[i].lineNum); Environment.Exit(0); }

                type = pTokens[i].word;
                name = pTokens[i + 1].word;
                i++;
            }
            
            else if (pTokens[i].classPart == TokenClass.SEMICOL)
            {
                // comentted until expmethod is not writtten
                // if (!type.Contains(valueType))
                // {
                //     System.Console.WriteLine("\nType Mismatch"+ " on lineNum: " + pTokens[i].lineNum);                    
                // }
                if (SE.currentScope.Count == 0)
                {
                    if (!SE.insertGlobalData(name, type, sta, final)) { System.Console.WriteLine("\nariable RE-deleared on lineNum: " + pTokens[i].lineNum); Environment.Exit(0); }
                }
                // else if (SE.currentClassName != null && pTokens[0].classPart == TokenClass.CLASS)
                else if (SE.currentClassName != null && pTokens[0].classPart == TokenClass.AM)
                {
                    if (!SE.insertClassTable(name, type, am, sta, final, false)) { System.Console.WriteLine("\nariable RE-deleared on lineNum: " + pTokens[i].lineNum); Environment.Exit(0); }
                }
                else
                {
                    if (!SE.insertFunctionTable(name, type)) { System.Console.WriteLine("\nariable RE-deleared on lineNum: " + pTokens[i].lineNum); Environment.Exit(0); }
                }
            }
        }
        return true;
    }

    // private bool updateExpMode()
    // {
    //     if (tokens[index].classPart.ToString() == "ORB" || tokens[index].classPart.ToString() == "ASI" || tokens[index].classPart.ToString() == "OSB") { expmode = true; }
    //     else if (tokens[index].classPart.ToString() == "CRB" || tokens[index].classPart.ToString() == "SC" || tokens[index].classPart.ToString() == "CSB") { expmode = false; getType(expression); }
    //     return expmode;
    // }

}