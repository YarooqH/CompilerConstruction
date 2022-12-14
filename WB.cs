using System.Collections;

public class WB
{
    private char[] breakers = { 
    '(',')','[',']','{','}',';',':',',','+','-','*','/','%','<','>','=','!',' ','\'','"', '@', '&', '|'
    };
    private string word = "";
    private List<string[]> words;
    private string[] lines;
    private char ch;
    private int i;
    private int lineNo;
    // COUNSTRUCTOR

    public WB() {
        words = new List<string[]>();

        lines = System.IO.File.ReadAllLines(@".\files\input.txt");
    }
    
    public WB (string path)
    {
        words = new List<string[]>();

        lines = System.IO.File.ReadAllLines(@path);
    }
    // GETTERS
    public List<string[]> GetWords()
    {
        BreakIntoWords();

        return words;
    }
    private void BreakIntoWords()
    {
        bool flag = false;
        foreach (string line in lines)
        {
            lineNo += 1;
            string l = line + " ";
            for (i = 0; i <= l.Length - 1; i++)
            {
                if (l[i] == '#') break;                              

                if ((l[i] == '/' && l[i+1] == '*') || flag == true)                    
                {
                    flag = Check_Comment_Status(l, flag);           
                    if (flag) break;
                }

                if (l[i] == '.')
                {
                    bool isNumeric;
                    if (!WordIsEmpty()) isNumeric = int.TryParse(word, out _);
                    else isNumeric = true;

                    if (isNumeric)
                    {

                        isNumeric = int.TryParse(l[i + 1].ToString(), out _);
                        if (isNumeric)
                        {
                            AddCharacter(l[i]);
                            while (!breakers.Contains(l[i]) && l[i] != '.')
                            {
                                AddCharacter(l[i]);
                            }
                            createWord(word);
                            i--;
                            continue;
                        }
                        else
                        {
                            if (WordIsEmpty())
                            {
                                AddCharacter(l[i]);
                                createWord(word);
                            }
                            else{
                                createWord(word);
                                AddCharacter(l[i]);
                                createWord(word);
                            }

                            i--;
                            continue;
                        }
                    }
                    else
                    {
                        createWord(word);
                        isNumeric = int.TryParse(l[i + 1].ToString(), out _);
                        if (isNumeric)
                        {
                            AddCharacter(l[i]);
                            AddCharacter(l[i + 1]);
                            i--;
                            continue;
                        }
                        createWord(l[i].ToString());
                        continue;
                    }
                }

                if (breakers.Contains(l[i]))
                {
                    if (l[i] == '"')
                    {
                        if (!WordIsEmpty()) { createWord(word); AddCharacter(l[i]); }

                        else { AddCharacter(l[i]); }

                        ch = ' ';

                        while (ch != '"' && i < l.Length - 1)
                        {
                            if (l[i] == '\\')
                            {
                                AddCharacter(l[i]);
                                if (i == l.Length - 1) break;     //Case: {"\ }

                                AddCharacter(l[i]);
                                ch = ' ';
                                continue;                        //Added continue for this case { "\" }
                            }
                            AddCharacter(l[i]);
                        }
                        createWord(word);
                        i--;
                        continue;
                    }

                    if (l[i] == '\'')
                    {
                        int count = 0;
                        if (!WordIsEmpty()) { createWord(word); AddCharacter(l[i]); }

                        else { AddCharacter(l[i]); }
                        count += 1;
                        ch = ' ';
                        while (ch != '\'' && i < l.Length - 1 && count != 3)
                        {
                            if (l[i] == '\\')
                            {
                                AddCharacter(l[i]);
                                count += 1;
                                if (i == l.Length - 1) break;     //Case: {"\ }

                                AddCharacter(l[i]);
                                ch = ' ';
                                continue;                        //Added continue for this case { "\" }
                            }
                            AddCharacter(l[i]);
                            count += 1;
                        }
                        createWord(word);
                        i--;
                        continue;
                    }

                    if (!WordIsEmpty()) createWord(word);

                    if (l[i] == ' ') continue;

                    if (Check_RO(l)) continue;

                    createWord(l[i].ToString());
                    continue;
                }

                word = word + l[i];
            }
        }

    }
    private void AddCharacter(char character)
    {
        ch = character;
        word += character;
        i++;
    }
    private void createWord(string w)
    {
        words.Add(new string[] { w, lineNo.ToString() });
        word = "";
    }
    private bool WordIsEmpty()
    {
        if (word == "") return true;
        return false;
    }
    private bool Check_Comment_Status(string l, bool flag)
    {
        int index;
        if (flag == true) {
            index = l.IndexOf("/", i);   
        }
        else{
            index = l.IndexOf("*/", i + 1);
        }

        if (index == -1){ 
            return true;
        }                                                  
        i = index + 1;
        return false;
    }
    // private bool Check_RO(string l)
    // {
    //     if ((l[i] == '>' || l[i] == '<' || l[i] == '=' || l[i] == '!') && l[i + 1] == '=')
    //     {
    //         createWord(l[i].ToString() + l[i + 1].ToString());
    //         i++;
    //         return true;
    //     }
    //     else return false;
    // }

     private bool Check_RO(string singleWord)
    {
        if(singleWord[i] == '>' && singleWord[i + 1] == '>'){
            // Console.WriteLine("BANANA");
            createWord(singleWord[i].ToString() + singleWord[i+1].ToString());
            i++;
            return true;
        }
        else if ((singleWord[i] == '>' || singleWord[i] == '<' || singleWord[i] == '=' || singleWord[i] == '!') && singleWord[i + 1] == '=')
        {
            // Console.WriteLine("BANANA Milshake");
            createWord(singleWord[i].ToString() + singleWord[i + 1].ToString());
            i++;
            return true;
        } 
        // else if(singleWord[i] == '>' && singleWord[i + 1] == '>'){
        //     createWord(singleWord[i].ToString() + singleWord[i+1].ToString());
        //     i++;
        //     return true;
        // }
        else return false;
    }

}