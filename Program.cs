using System.Collections;
using System;

WordBreaker WB = new WordBreaker();
ArrayList wordsLi = WB.getWords();
WB.writeWordsToFile();
LexicalAnalyzer LA = new LexicalAnalyzer();

// foreach (string[] item in wordsLi)
// {
//     Console.WriteLine(item[0] + " " + item[1]);
// }
ArrayList tokenLi = LA.generateTokens(wordsLi);

foreach (TokenObj TOKEN in tokenLi)
{
    // Console.WriteLine(TOKEN.ToString());
    LA.writeTokensToFile(TOKEN.ToString());
    // LA.ExampleAsync();
}


