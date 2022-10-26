using System.Collections;
using System.Collections.Generic;
public class SyntaxAnalyzer
{
    List<TokenObj> tokens;
    Dictionary<string,List<string[]>> rules;
    int index = 0;
    LexicalAnalyzer la = new LexicalAnalyzer();
    public SyntaxAnalyzer(List<TokenObj> tokens)
    {
        this.rules = new Dictionary<string,List<string[]>>();
        this.tokens = tokens;
        this.getRules();
        // this.printRules();
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
        int index = 0;
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
        System.Console.WriteLine(rules[currentNT]);

        List<String[]> productionRules = rules[currentNT];
        System.Console.WriteLine("=======================");

        foreach (String[] pr in productionRules)        
        {
            System.Console.WriteLine("% " + currentNT + " -> " );
            int prev = index;
            int j = 0;
            for (; j < pr.Length; j++) {
                String element = pr[j];                
                System.Console.WriteLine("\nElement :" + element + "' { of :" + currentNT + "}");
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
                    System.Console.WriteLine("HERE IN TERMINAL");
                    System.Console.WriteLine("tokens.get(index).type =" + tokens[index].classPart.ToString() + "'");
                    System.Console.WriteLine("tokens.get(index).type =" + tokens[index].word + "'");
                    // string a = la.ht.Contains();

                    Console.WriteLine(element + " : "  + tokens[index].classPart.ToString() );
                    
                    if (string.Equals(element, tokens[index].classPart.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        index++;
                        System.Console.WriteLine("Matched Terminal =" + element);
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

}