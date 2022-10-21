using System.Collections;
using System;

// WordBreaker WB = new WordBreaker();
// ArrayList wordsLi = WB.getWords();
// WB.writeWordsToFile();
// LexicalAnalyzer LA = new LexicalAnalyzer();

// foreach (string[] item in wordsLi)
// {
//     Console.WriteLine(item[0] + " " + item[1]);
// }

// ArrayList tokenLi = LA.generateTokens(wordsLi);

// foreach (TokenObj TOKEN in tokenLi)
// {
    // Console.WriteLine(TOKEN.ToString());
    // LA.writeTokensToFile(TOKEN.ToString());
    // LA.ExampleAsync();
// }

// List<string[]> words;

// SyntaxAnalyzer SA = new SyntaxAnalyzer(tokenLi);

static async Task ExampleAsync(List<TokenObj> tokens)
{

    using StreamWriter file = new(@".\files\WordBreakerOutput.txt");

    foreach (TokenObj item in tokens)
    {
        // Console.WriteLine(item.ToString());
        await file.WriteLineAsync(item.ToString());
    }
}


WB breaker = new WB();
LA la = new LA();

List<TokenObj> tokens = la.GetTokens(breaker.GetWords());

await ExampleAsync(tokens);

SyntaxAnalyzer sa = new SyntaxAnalyzer(tokens);

System.Console.WriteLine(sa.checkSyntax());


// Console.WriteLine( SA.start());
