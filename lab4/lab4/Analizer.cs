using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab4
{
    internal class Analizer
    {
        public static List<string> ops = new List<string>()
        {
            "+" ,"*=" , "/=" , "-" , "=" , "==" , ">=" ,
            ">" , "<" , "<=" , "/" , "*" , "**" , "%" ,
            "!=" , "-=" ,"(" , ")",  "%=" , "+=" ,
        };

        public static List<string> noParamsSpWords = new()
        {
            "while" , "range" ,
            "def" , "lambda",
            "for" , "if" ,
            "elif" , "return",
        };

        public static List<string> Alphabet = new()
        {
            "a" , "A" , "b" , "B" , "c" , "C" , "d" , "D" , "e" , "E" , "f" , "F" , "g" , "G" ,
            "h" , "H" , "i" , "I" , "j" , "J" , "k" , "K" , "l" , "L" , "m" , "M" , "n" , "N" ,
            "o" , "O" , "p" , "P" , "q" , "Q" , "r" , "R" , "s" , "S" , "t" , "T" , "U" , "u" ,
            "v" , "V" , "w", "W" , "x" , "X" , "y" , "Y" , "z" , "Z"
        };

        public static List<string> spWords = new()
        {
             "else" , "break" , "continue" ,
        };

        public static List<Tuple<string, string>> vars = new();

        public static List<Tuple<string, int>> funcs = new();

        public static List<string> ParamFuncCheck(string line)
        {
            var p = new List<string>();
            for (int l = 0; l < line.Length; l++)
            {
                var substr = line.Substring(l);
                int endIndex = substr.IndexOf(',');
                if (endIndex < 0)
                {
                    endIndex = line.Length;
                    endIndex -= l;
                }
                string s = line.Substring(l, endIndex);
                if (s.Length != 0 && !s.Equals(" "))
                {
                    p.AddRange( new List<string>() { s });
                }
                l += endIndex;
            }
            return p;
        }
        public static bool BoolCheck(string value)
        {
            if (value == "True" || value == "False")
            {
                return true;
            }
            return false;
        }

        public static bool StringCheck(string value)
        {
            if (value.LastIndexOf('"') != value.IndexOf('"'))
            {
                return true;
            }
            return false;
        }

        public static bool NumCheck(string num)
        {
            if (!int.TryParse(num, out var number) || !float.TryParse(num, out var number2))
            {
                return false;
            }
            return true;
        }

        public static int FuncNameCheck(string line, int i, string space)
        {
            var str = line.Substring(i);
            int endOfName = str.IndexOf(' ');
            str = line.Substring(i, endOfName);

            if (line.IndexOf('(') < 0 || line.IndexOf(')') < 0)
            {
                return -1;
            }
            line = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1);
            List<string> p = ParamFuncCheck(line);
            funcs.AddRange(new List<Tuple<string, int>> { new Tuple<string, int>(str, p.Count) });
            foreach (var item in p)
            {
                GetParams(item, space);
            }
            Console.WriteLine(string.Concat(space, "def ") + str);
            return endOfName;
        }

        public static string GetParams(string line, string space)
        {
            line = line.Replace(")", string.Empty);
            line = line.Replace(":", string.Empty);
            line = line.Replace("(", string.Empty);

            while (line.First() == ' ')
            {
                line = line.Remove(0, 1);
            }
            while (line[line.Length - 1].Equals(' '))
            {
                line = line.Remove(line.Length - 1, 1);
            }

            var values = new List<string>();
            var operators = new List<string>();

            for (int i = 0; i < line.Length; i++)
            {
                var str = line.Substring(i);
                int endIndex = str.IndexOf(' ');
                if (endIndex < 0)
                {
                    endIndex = line.Length;
                    endIndex -= i;
                }
                str = string.Empty;
                str = line.Substring(i, endIndex);

                if (ops.Contains(str))
                {
                    operators.AddRange(new List<string>() { str });
                }
                if (!ops.Contains(str))
                {
                    if ( values.Count < operators.Count)
                    {
                        values.AddRange(new List<string>() { "null" });
                    }
                    values.AddRange(new List<string>() { str });
                }

                i = i + endIndex;
            }

            List<string> opWithPriority = new List<string>();

            foreach (var item in ops)
            {
                foreach (var i in operators)
                {
                    if (i != item) continue;
                    else
                    {
                        opWithPriority.AddRange(new List<string>() { i });
                        break;
                    }
                }
            }


            if (operators.Count == 1 && operators.First() == "=")
            {
                if (vars.FirstOrDefault(func => func.Item1 == values[1]) is null && TypeCheck(values[1]).Equals("none"))
                {
                    return $"error (undefined) : {values[1]}";
                }
                string type = TypeCheck(values[1]);
                Console.WriteLine(string.Concat(space, values[1]));
                Console.WriteLine(string.Concat(space, "    ") + operators.First());
                Console.WriteLine(string.Concat(space, values.First()));

                if (vars.FirstOrDefault(func => func.Item1 == values.First()) is not null)
                {
                    int index = vars.FindIndex(func => func.Item1 == values.First());
                    vars.RemoveAt(index);
                    vars.AddRange(new List<Tuple<string, string>> { new Tuple<string, string>(values.First(), type) });
                }

                if (vars.FirstOrDefault(func => func.Item1 == values.First()) is null)
                {
                    vars.AddRange(new List<Tuple<string, string>> { new Tuple<string, string>(values.First(), type) });
                } 
                

            }
            else
            {
                if (vars.FirstOrDefault(func => func.Item1 == values.First()) == null && operators.First().Equals("="))
                {
                    vars.AddRange(new List<Tuple<string, string>> { new Tuple<string, string>(values.First(), "none") });
                }
                for (int i = 0; i < opWithPriority.Count; i++)
                {
                    string type1, type2 = string.Empty;
                    int index = operators.IndexOf(opWithPriority[i]);
                    type2 = "none"; type1 = "none";
                    if (!values[index].Equals("null"))
                    {
                        var first = vars.FirstOrDefault(func => func.Item1 == values.First());
                        if (first is not null)
                        {
                            string type = TypeCheck(values[1]);
                            int j = vars.FindIndex(func => func.Item1 == values.First());
                            vars.RemoveAt(j);
                            vars.AddRange(new List<Tuple<string, string>> { new Tuple<string, string>(values.First(), type) });
                        }
                        type1 = TypeCheck(values[index]);
                        first = vars.FirstOrDefault(func => func.Item1 == values[index]);
                        if (first is null && TypeCheck(values[index]).Equals("none"))
                        {
                            return "error (undefined) :".Concat(values[index].ToString()).ToString();
                        }
                        Console.WriteLine(string.Concat(space, values[index]));
                    }
                    Console.WriteLine(string.Concat(space, "    ") + opWithPriority[i]);
                    if (index + 1 < values.Count && !values[index + 1].Equals("null"))
                    {
                        if (TypeCheck(values[index + 1]).Equals("none") && vars.FindIndex(func => func.Item1 == values[index + 1]) < 0)
                        {
                            return $"error (undefined) : {values[index + 1]}";
                        }
                        type2 = TypeCheck(values[index + 1]);
                        if(!type2.Equals(type1) && (!type1.Equals("none") && !type2.Equals("none")))
                        {
                            type1 = type1.Equals("num") ? "int/float" : type1;
                            type2 = type2.Equals("num") ? "int/float" : type2;

                            return $"error (nonsupported) : {type1} {opWithPriority[i]} {type2}";
                        }
                    }
                }
            }
            if (operators.Count == 0)
            {
                if (vars.FirstOrDefault(func => func.Item1 == line) is null && TypeCheck(line).Equals("none"))
                {
                    vars.AddRange(new List<Tuple<string, string>> { new Tuple<string, string>(line, "none") });
                }
                Console.WriteLine(string.Concat(space, line));
            }
            return "0";
        }
        public static string TypeCheck(string values)
        {
            string type = string.Empty;

            if (vars.FirstOrDefault(f => f.Item1 == values) is not null)
            {
                type = vars.FirstOrDefault(f => f.Item1 == values).Item2;
                return type;
            }

            return NumCheck(values) ? "num" :
                   BoolCheck(values) ? "bool" :
                   StringCheck(values) ? "string" :
                   "none";
        }

        public static string Analize(string line, string space, bool recurs = false)
        {
            if (!recurs)
            {
                int k = 0;
                string currspace = string.Empty;
                while (line[k].Equals(' '))
                {
                    currspace = string.Concat(currspace, ' ');
                    k++;
                }
                if (currspace.Length % 4 < 0 || currspace.Length % 4 > 0)
                {
                    return $"error in spacing";
                }
                space = currspace;
                line = line.Substring(k);
            }
            string check = string.Empty;
            for (int i = 0; i < line.Length; i++)
            {
                var substr = line.Substring(i);
                int endIndex = substr.IndexOf(' ');

                endIndex = endIndex < 0 ? line.Length - i : substr.IndexOf(' ');

                string str = line.Substring(i, endIndex);
                if (str.Equals("def"))
                {

                    int nameEndIndex = FuncNameCheck(line, i + endIndex + 1, space) + endIndex;
                    if (nameEndIndex - endIndex < 0)  {
                        return $"error (no bracket)";
                    }
                    if (nameEndIndex < 0)
                    {
                        return $"error in parameters";
                    }
                    if (!line[line.Length - 1].Equals(':'))
                    {
                        return $"error (no colon)";
                    }
                    
                    i = nameEndIndex + i;
                    return space;
                }
                if (noParamsSpWords.Contains(str))
                {
                    if (!str.Equals("return") && !line[line.Length - 1].Equals(':'))
                    {
                        return $"error (no colon)";
                    }
                    check = Analize(line.Substring(i + endIndex + 1), space, true);
                    if (check.Substring(0, 5) == "error" && !(check.Length < 5) && check.First() == 'e')
                    {
                        return check;
                    }
                    Console.WriteLine(string.Concat(space, str));
                    return space;
                }
                if (funcs.FirstOrDefault(func => func.Item1 == str) is not null)
                {
                    if (line.IndexOf('(') < 0 || line.IndexOf(')') < 0)
                    {
                        return "error (no bracket)";
                    }
                    var start = line.IndexOf('(') + 1;
                    var end = line.IndexOf(')') - line.IndexOf('(') - 1;
                    string temp = line.Substring(start, end);
                    var f = funcs.FirstOrDefault(func => func.Item1 == str);
                    var p = ParamFuncCheck(temp);
                    check = Analize(temp, space, true);
                    if (check.Substring(0, 5) == "error" && check.First() == 'e' && !(check.Length < 5))
                    {
                        return check;
                    }
                    if (f.Item2 > p.Count)
                    {
                        return $"error in parameters";
                    }
                    Console.WriteLine(string.Concat(space, str));
                    if (line.IndexOf(')') < line.Length - 1 || line.IndexOf(')') > line.Length - 1)
                    {
                        Analize(line.Substring(line.IndexOf(')') + 1), space, true);
                    }
                    return space;
                }
                if (noParamsSpWords.Contains(str))
                {
                    Console.WriteLine(string.Concat(space, str));
                    return space;
                }
            }
            check = GetParams(line, space);
            if (!check.Equals("0"))
            {
                return check;
            }
            return space;
        }
    }
}
