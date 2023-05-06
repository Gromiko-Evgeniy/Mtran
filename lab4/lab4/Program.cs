using lab4;
var path = "script.txt";

using (var stremReader = new StreamReader(path))
{
    string space = string.Empty;
    string line = string.Empty;

    var i = 0;

    while ((line = await stremReader.ReadLineAsync()) != null)
    {
        if (line == "") {
            i++;
            continue;
        }

        space = Analizer.Analize(line, space);
        if (space.Substring(0, 5).Equals( "error" ) && space.Length >= 5)
        {
            Console.WriteLine("error in line" + (i + 1).ToString());
            return;
        }
        i++;
    }
}

