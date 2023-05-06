using lab3mtran;

string path = "script.txt";

using (StreamReader reader = new StreamReader(path))
{
    string? line;
    int index = 0;
    var newstr = string.Empty;
    string space = "";
    while ((line = await reader.ReadLineAsync()) != null)
    {
        if (line == "") continue;

        space = Analizer.LineAnalizer(line, space);
        if (space.Substring(0, 5) == "error")
        {
            Console.WriteLine($"error found");
            return;
        }

        index++;
    }
}