using lab2;

string fileName = "script.txt";

using (StreamReader streamReader = new StreamReader(fileName))
{
    string line = string.Empty;
    while (await streamReader.ReadLineAsync() != null)
    {
        line = await streamReader.ReadLineAsync();

        for (int i = 0; i < line.Length; i++)
        {
            Console.WriteLine(line[i]);
            if (line[i] == ' ')
            {
                continue;
            }

            int lexemeEndIndex = 0;

            if (Analizer.Alphabet.Contains(line[i].ToString()))
            {
                for (int j = i + 1; j < line.Length; j++)
                {
                    if (!Analizer.Alphabet.Contains(line[j].ToString()))
                    {
                        lexemeEndIndex = j;
                        break;
                    }
                }

                string substr = line.Substring(i, lexemeEndIndex - i);
                if (substr == "def")
                {
                    lexemeEndIndex = Analizer.FuncNameCheck(line, lexemeEndIndex + 1);
                }
                Analizer.LexemeCheck(substr);
                i = lexemeEndIndex;
            }


            bool isOperator = false;
            foreach (string op in Analizer.operators)
            {
                if (op.Contains(line[i]))
                {
                    lexemeEndIndex = i + 1;

                    if (Analizer.operators.Contains(line[i].ToString() + line[i + 1].ToString()))
                    {
                        lexemeEndIndex = i + 2;
                    }

                    isOperator = true;
                    break;
                }
            }
            if (isOperator)
            {
                string substr = line.Substring(i, lexemeEndIndex - i);
                Analizer.LexemeCheck(substr);
                i += lexemeEndIndex;
                continue;
            }
            if (int.TryParse(line[i].ToString(), out var number))
            {
                string num = line[i].ToString();
                for (int j = i + 1; j < line.Length; j++)
                {
                    if (int.TryParse(num + line[j].ToString(), out var number1))
                    {
                        num += line[j].ToString();
                        continue;
                    }
                    else
                    {
                        Analizer.NumCheck(num);
                    }
                }
            }
        }
    }
}
foreach (var keyValue in Analizer.Result)
{
    if (keyValue.Value.Contains("error"))
    {
        Console.WriteLine($"Error: {keyValue.Key}:\t {keyValue.Value}");
        return;
    }
}

Console.WriteLine("\noperators: ");
foreach (var keyValue in Analizer.ops)
{
    Console.WriteLine($"{keyValue.Key}:\t {keyValue.Value}");
}
Console.WriteLine("\nsp words: ");
foreach (var keyValue in Analizer.spwords)
{
    Console.WriteLine($"{keyValue.Key}:\t {keyValue.Value}");
}
Console.WriteLine("\nvariables: ");
foreach (var keyValue in Analizer.vars)
{
    Console.WriteLine($"{keyValue.Key}:\t {keyValue.Value}");
}
Console.WriteLine("\nfunctions: ");
foreach (var keyValue in Analizer.funcs)
{
    Console.WriteLine($"{keyValue.Key}:\t {keyValue.Value}");
}



