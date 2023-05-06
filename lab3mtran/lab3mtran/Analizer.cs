using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab3mtran
{
    public static class Analizer
    {

        public static List<string> vars = new List<string>();

        public static List<string> functions = new List<string>();

        public static List<string> ops = new List<string>()
        {
            "+" ,"*=" , "/=" , "-" , "=" , "==" , ">=" ,
            ">" , "<" , "<=" , "/" , "*" , "**" , "%" ,
            "!=" , "-=" ,"(" , ")",  "%=" , "+=" ,

        };

        public static List<string> noParamsSpWords = new List<string>()
        {
            "while" , "range" ,
            "def" , "lambda",
            "for" , "if" ,
            "elif" , "return",
        };

        public static List<string> Alphabet = new List<string>()
        {
            "a" , "A" , "b" , "B" , "c" , "C" , "d" , "D" , "e" , "E" , "f" , "F" , "g" , "G" ,
            "h" , "H" , "i" , "I" , "j" , "J" , "k" , "K" , "l" , "L" , "m" , "M" , "n" , "N" ,
            "o" , "O" , "p" , "P" , "q" , "Q" , "r" , "R" , "s" , "S" , "t" , "T" , "U" , "u" ,
            "v" , "V" , "w", "W" , "x" , "X" , "y" , "Y" , "z" , "Z"
        };

        public static List<string> spWords = new List<string>(new List<string> { "else", "break", "continue" });

        public static bool NumCheck(string num)
        {   
            try
            {
                int.Parse(num);
                float.Parse(num);
                return true;

            }
            catch( Exception e)
            {
                return false;
            }
        }

        public static int FuncCheck(string line, int index, string space)
        {
            string substr = line.Substring(index, line.Substring(index).IndexOf(' '));
            functions.AddRange(new List<string> { substr });

            ParamsCheck(line.Substring(line.IndexOf('('), line.IndexOf(')') - line.IndexOf('(')), space);
            Console.WriteLine(String.Concat(space, "def ") + substr);
            return line.Substring(index).IndexOf(' ');
        }

        public static string ParamsCheck(string line, string space)
        {
            line = line.Replace("(", string.Empty).Replace(")", string.Empty).Replace(":", string.Empty);
            var values = new List<string>();
            var operators = new List<string>();

            if (line == string.Empty) return "no parameters";

            while (line[line.Length - 1].Equals( ' ')) {
                line = line.Remove(line.Length - 1, 1);
                if (line == string.Empty) return "no parameters";
            }

            for (int index = 0; index < line.Length; index++)
            {
                string substr = line.Substring(index);
                int endIndex = substr.IndexOf(" ");

                if (endIndex == -1 && !(vars == null)) endIndex = line.Length - index;

                substr = line.Substring(index, endIndex);
                var a = string.Empty;
                if (ops.Contains(substr))
                {
                    operators.AddRange(new List<string> { substr });
                    index = index + endIndex;
                    continue;
                }
                else {
                    index = index + endIndex;
                    continue;
                }
            }

            List<string> opWithPriority = new List<string>();
            var value1 = String.Concat(space, values[0]);
            var op = String.Concat(space, "    ") + operators[0];
            var value2 = String.Concat(space, values[1]);

            if (operators[0] == "=" && String.Join(string.Empty, operators).Length == 1)
                Console.WriteLine($"{value1}\n {op}\n {value2}\n");
            else 
            {
                if (operators[0] == "=" && !vars.Contains(values[0]))
                {
                    var newstr = string.Empty;
                    vars.AddRange(new List<string> { values[0] });
                }
                for (int index = 0; index < String.Join(string.Empty, opWithPriority).Length; index++)
                {
                    if (!values[index].Equals("none") && !NumCheck(values[index]) && !vars.Contains(values[index]))
                    {
                        return $"error";
                    }
                    Console.WriteLine(String.Concat(space, values[index]));

                    var toPrint = String.Concat(space, "    ") + opWithPriority[index];
                    Console.WriteLine(toPrint);

                    bool check = index + 1 < String.Join(string.Empty, values).Length;
                    check = check && !(values[index + 1].Equals("none")); 
                    if (!check) Console.WriteLine(space);
                    else if (check == true)
                    {
                        toPrint = space + values[index + 1];
                        Console.WriteLine(toPrint);
                        if (index > 0 && String.Join(string.Empty, opWithPriority).Length > 1)
                        {
                            string temp = values[index - 1];
                            values[index - 1] = "none";
                            space = String.Concat(space, "    ");
                        }
                    }
                }
                return "no parameters";
            }
            return "no parameters";
        }

        public static string LineAnalizer(string line, string space)
        {
            int k = 0;
            while (line[k].Equals(" "))
            {
                k = k + 1;
                space = String.Concat(space, " ");
            }

            line = line.Substring(k);

            for (int index = 0; index < line.Length; index++)
            {
                int endIndex = line.Substring(index).IndexOf(' ');
                if (line.Substring(index).IndexOf(' ') == -1)
                {
                    endIndex = line.Length - index;
                }
                string substr = line.Substring(index, line.Substring(index).IndexOf(' '));
                if (substr == "def")
                {
                    if (!line[line.Length - 1].Equals(":"))
                    {
                        return $"error";
                    }
                    int temp = FuncCheck(line, index + line.Substring(index).IndexOf(' ') + 1, space);
  
                    index = index + temp;
                    return space;
                }
                var result = String.Concat(space, substr);
                var newline = (line.Substring(line.IndexOf(')') + 1));

                if (spWords.Contains(substr))
                {
                    Console.WriteLine(result);
                    return space;
                }
                if (spWords.Contains(substr))
                {
                    Console.WriteLine(String.Concat(space, substr));
                    return space;
                }
                if (functions.Contains(substr) && !(vars == null))
                {
                    Console.WriteLine(result);
                    if (!(line.IndexOf(')') == line.Length - 1))
                    {
                        LineAnalizer(newline, space);
                    }
                    return space;
                }
            }

            return space;
        }

    }
}
