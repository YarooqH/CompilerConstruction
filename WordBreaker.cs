using System.Collections;
using System;
using System.Text;
public class WordBreaker
{
    private char[] wordBreakers = { 
        '(',')','[',']','{','}',';',':',',','+','-','*','/','%','<','>','=','!',' ','\'','"', '@', '&', '|'
    };
    private int i, lineNo;
    private string word = "";
    private ArrayList wordsLi;
    // private List<string[]> wordsLI;
    private string[] lines;
    private char ch;

    public WordBreaker(){
        wordsLi = new ArrayList();
        lines = System.IO.File.ReadAllLines(@".\files\input.txt");
    }
    public ArrayList getWords(){
        breakIntoWords();
        return wordsLi;
    }

    private void breakIntoWords() {
        bool commentStatus = false;
        foreach (string line in lines){
            lineNo += 1;
            string singleWord = line + " ";
            for (i = 0; i <= singleWord.Length - 1; i++){                                                        
                if (singleWord[i] == '#') break;                       
                if ((singleWord[i] == '/' && singleWord[i+1] == '*') || commentStatus == true)
                {                                                                       
                    commentStatus = isComment(singleWord, commentStatus);
                    if (commentStatus) break;
                }

                if (singleWord[i] == '.')
                {
                    bool isNumeric;
                    if (!isEmpty()) isNumeric = int.TryParse(word, out _);
                    else isNumeric = true;

                    if (isNumeric)
                    {
                        insertWord(singleWord[i]);
                        isNumeric = int.TryParse(singleWord[i].ToString(), out _);
                        if (isNumeric)
                        {
                            while (!wordBreakers.Contains(singleWord[i]) && singleWord[i] != '.')
                            {
                                insertWord(singleWord[i]);
                            }
                            createWord(word);
                            i--;
                            continue;
                        }
                        else
                        {
                            createWord(word);
                            insertWord(singleWord[i]);
                            i--;
                            continue;
                        }
                    }
                    else
                    {
                        createWord(word);
                        isNumeric = int.TryParse(singleWord[i + 1].ToString(), out _);
                        if (isNumeric)
                        {
                            insertWord(singleWord[i]);
                            insertWord(singleWord[i + 1]);
                            continue;
                        }
                        createWord(singleWord[i].ToString());
                        continue;
                    }
                }

                if (wordBreakers.Contains(singleWord[i]))
                {
                    if (singleWord[i] == '"')
                    {
                        if (!isEmpty()) { createWord(word); insertWord(singleWord[i]); }

                        else { insertWord(singleWord[i]); }

                        ch = ' ';

                        while (ch != '"' && i < singleWord.Length - 1)
                        {
                            if (singleWord[i] == '\\')
                            {
                                insertWord(singleWord[i]);
                                if (i == singleWord.Length - 1) break;

                                insertWord(singleWord[i]);
                                ch = ' ';
                                continue;              
                            }
                            insertWord(singleWord[i]);
                        }
                        createWord(word);
                        i--;
                        continue;
                    }
                    
                    if (singleWord[i] == '\'')
                    {
                        int count = 0;
                        if (!isEmpty()) { createWord(word); insertWord(singleWord[i]); }

                        else { insertWord(singleWord[i]); }
                        count += 1;
                        ch = ' ';
                        while (ch != '\'' && i < singleWord.Length - 1 && count != 3)
                        {
                            if (singleWord[i] == '\\')
                            {
                                insertWord(singleWord[i]);
                                count += 1;
                                if (i == singleWord.Length - 1) break;     //Case: {"\ }

                                insertWord(singleWord[i]);
                                ch = ' ';
                                continue;                        //Added continue for this case { "\" }
                            }
                            insertWord(singleWord[i]);
                            count += 1;
                        }
                        createWord(word);
                        i--;
                        continue;
                    }

                    if (!isEmpty()) createWord(word);

                    if (singleWord[i] == ' ') continue;

                    if (isOP(singleWord)) continue;

                    createWord(singleWord[i].ToString());
                    continue;
                }

                word = word + singleWord[i];
            }
        }

    }
    private void insertWord(char character)
    {
        ch = character;
        word += character;
        i++;
    }
    private void createWord(string w)
    {
        wordsLi.Add(new string[]{w,lineNo.ToString()});
        word = "";
    }
    private bool isEmpty()
    {
        if (word == "") return true;
        return false;
    }
    private bool isComment(string singleWord, bool commentStatus)
    {
        int index;
        if (commentStatus == true) {
            index = singleWord.IndexOf("/", i);   
        }
        else{
            index = singleWord.IndexOf("*/", i + 1);
        }

        if (index == -1){ 
            return true;
        }                                                  
        i = index + 1;
        return false;
    }
    private bool isOP(string singleWord)
    {
        if ((singleWord[i] == '>' || singleWord[i] == '<' || singleWord[i] == '=' || singleWord[i] == '!') && singleWord[i + 1] == '=')
        {
            createWord(singleWord[i].ToString() + singleWord[i + 1].ToString());
            i++;
            return true;
        } else if(singleWord[i] == '>' && singleWord[i + 1] == '>'){
            createWord(singleWord[i].ToString() + singleWord[i+1].ToString());
            i++;
            return true;
        }
        else return false;
    }

    public void writeWordsToFile(){      
        using (FileStream sw = File.Create(@".\files\WordBreakerOutput.txt"))
        {   
            foreach (string[] item in wordsLi)
            {
                byte[] txt = new UTF8Encoding(true).GetBytes(item[0] + " \t\t\t" + item[1]+'\n');
                sw.Write(txt, 0, txt.Length);
                // sw.Write(item[0] + " \t\t\t" + item[1]);                                
            }
        }
    }
}
