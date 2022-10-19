using System.Collections;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

public class LexicalAnalyzer
{
    Regex? regEx;
    Hashtable hashTable;
    ArrayList tokensLi;

    int lastLine;

    public LexicalAnalyzer(){
        tokensLi = new ArrayList();
        hashTable = new Hashtable();

        hashTable.Add("+", TokenClass.PM);
        hashTable.Add("-", TokenClass.PM);
        hashTable.Add("*", TokenClass.MDM);
        hashTable.Add("/", TokenClass.MDM);
        hashTable.Add("%", TokenClass.MDM);
        hashTable.Add("!", TokenClass.NOT);
        hashTable.Add(">", TokenClass.COMP);
        hashTable.Add("<", TokenClass.COMP);
        hashTable.Add("<=", TokenClass.COMP);
        hashTable.Add(">=", TokenClass.COMP);
        hashTable.Add("==", TokenClass.COMP);
        hashTable.Add("!=", TokenClass.COMP);
        hashTable.Add("=", TokenClass.ASIGN);
        hashTable.Add("&&", TokenClass.AND);
        hashTable.Add("||", TokenClass.OR);
        hashTable.Add("return", TokenClass.RETURN);
        hashTable.Add("returns", TokenClass.RETURNS);
        hashTable.Add("(", TokenClass.ORB);
        hashTable.Add(")", TokenClass.CRB);
        hashTable.Add("[", TokenClass.OSB);
        hashTable.Add("]", TokenClass.CRB);
        hashTable.Add("{",TokenClass.OCB);
        hashTable.Add("}", TokenClass.CCB);
        hashTable.Add("if", TokenClass.IF);
        hashTable.Add("else", TokenClass.ELSE);
        hashTable.Add("for", TokenClass.FOR);
        hashTable.Add("while", TokenClass.WHILE);
        hashTable.Add("do", TokenClass.DO);
        hashTable.Add("break", TokenClass.BREAK);
        hashTable.Add("continue", TokenClass.CONT);
        hashTable.Add("num", TokenClass.DT);
        hashTable.Add("bool", TokenClass.DT);
        hashTable.Add("string", TokenClass.DT);
        hashTable.Add(";", TokenClass.SEMICOL);
        hashTable.Add(":", TokenClass.COL);
        hashTable.Add(",",TokenClass.COM);
        hashTable.Add(">>", TokenClass.EXT);
        hashTable.Add("null", TokenClass.NULL);
        hashTable.Add("abstract", TokenClass.ABS);
        hashTable.Add("avail", TokenClass.AVAIL);
        hashTable.Add("new", TokenClass.NEW);
        hashTable.Add("class", TokenClass.CLASS);
        hashTable.Add("const", TokenClass.CONST);
        hashTable.Add("let", TokenClass.LET);
        hashTable.Add("current", TokenClass.CURR);
        hashTable.Add("super", TokenClass.PARENT);
        hashTable.Add("public", TokenClass.AM);
        hashTable.Add("scoped", TokenClass.AM);
        hashTable.Add("protected", TokenClass.AM);
    }

    public ArrayList generateTokens(ArrayList wordsList){
         foreach (string[] wordData in wordsList){
            if (hashTable.ContainsKey(wordData[0])) { 
                tokensLi.Add(new TokenObj(wordData[1], (TokenClass?)hashTable[wordData[0]], wordData[0])); 
                continue; 
            } else if (Char.IsNumber(wordData[0][0])) {
                regEx = new Regex(@"^[0-9]+$");
                if (regEx.IsMatch(wordData[0])) { 
                    tokensLi.Add(new TokenObj(wordData[1], TokenClass.NUM, wordData[0])); 
                    continue; 
                }
                regEx = new Regex(@"^[0-9]*[.][0-9]+$");
                if (regEx.IsMatch(wordData[0])) { 
                    tokensLi.Add(new TokenObj(wordData[1], TokenClass.NUM, wordData[0])); 
                    continue; 
                } else { 
                    tokensLi.Add(new TokenObj(wordData[1], TokenClass.ERR, wordData[0])); 
                    continue; 
                }
            } else if (wordData[0][0] == '.') {
                if (wordData[0].Length == 1) { 
                    tokensLi.Add(new TokenObj(wordData[1], TokenClass.DOT, wordData[0])); 
                    continue; 
                }
                regEx = new Regex(@"^[0-9]*[.][0-9]+$");
                if (regEx.IsMatch(wordData[0])) { 
                    tokensLi.Add(new TokenObj(wordData[1], TokenClass.NUM, wordData[0]));
                    continue; 
                }
                else { 
                    tokensLi.Add(new TokenObj(wordData[1], TokenClass.ERR, wordData[0])); 
                    continue; 
                }
            } else if (wordData[0][0] == '"') {
                regEx = new Regex("^[\"]([\\\\][abfnrtv0\"\'\\\\]|[^(\"\'\\\\)]|[()])*[\"]$");
                if (regEx.IsMatch(wordData[0])) { 
                    tokensLi.Add(new TokenObj(wordData[1], TokenClass.SC, wordData[0])); 
                    continue;
                } else { 
                    tokensLi.Add(new TokenObj(wordData[1], TokenClass.ERR, wordData[0])); 
                    continue; 
                }
            } else if (wordData[0][0] == '\'') {
                regEx = new Regex("^[\']([\\\\][abfnrtv0\"\'\\\\]|[^(\"\'\\\\)]|[()])[\']$");
                if (regEx.IsMatch(wordData[0])) { 
                    tokensLi.Add(new TokenObj(wordData[1], TokenClass.CC, wordData[0])); 
                    continue;
                } else { 
                    tokensLi.Add(new TokenObj(wordData[1], TokenClass.ERR, wordData[0]));
                    continue; 
                }
            } else {
                regEx = new Regex("^([a-zA-Z_$][a-zA-Z\\d_$]*)$");
                if (regEx.IsMatch(wordData[0])) { 
                    tokensLi.Add(new TokenObj(wordData[1], TokenClass.ID, wordData[0])); 
                    continue; 
                } else { 
                    tokensLi.Add(new TokenObj(wordData[1], TokenClass.ERR, wordData[0]));
                    continue; 
                }}}
            // int lastline = wordsList.Length
            tokensLi.Add(new TokenObj("$", TokenClass.PROGEND, lastLine.ToString()));
        return tokensLi;
    }

    public void writeTokensToFile(string txt)
        { 
            using (StreamWriter sw = File.AppendText(@".\files\LexicalAnalyzerTokenOutput.txt"))
            {   
                sw.WriteLine(txt);
            }
        }

}
