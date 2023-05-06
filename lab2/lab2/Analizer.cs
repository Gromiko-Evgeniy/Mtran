
namespace lab2
{
    public static class Analizer
    {
        public static Dictionary<string, string> Result = new Dictionary<string, string>();

        public static List<string> spWords = new List<string>()
        {
            "while" , "range" ,
            "def" , "lambda",
            "for" , "if" ,
            "elif" , "else" ,
            "break" , "continue" ,
            "return",
        };

        public static List<string> Alphabet = new List<string>()
        {
            "a" , "A" , "b" , "B" , "c" , "C" , "d" , "D" , "e" , "E" , "f" , "F" , "g" , "G" ,
            "h" , "H" , "i" , "I" , "j" , "J" , "k" , "K" , "l" , "L" , "m" , "M" , "n" , "N" ,
            "o" , "O" , "p" , "P" , "q" , "Q" , "r" , "R" , "s" , "S" , "t" , "T" , "U" , "u" ,
            "v" , "V" , "w", "W" , "x" , "X" , "y" , "Y" , "z" , "Z"
        };

        public static List<string> operators = new List<string>()
        {
            "+" ,"*=" , "/=" , "-" , "=" , "==" , ">=" ,
            ">" , "<" , "<=" , "/" , "*" , "**" , "%" ,
            "!=" , "-=" ,"(" , ")",  "%=" , "+=" ,
        };


        public static Dictionary<string, string> ops = new Dictionary<string, string>();
        public static Dictionary<string, string> spwords = new Dictionary<string, string>();
        public static Dictionary<string, string> vars = new Dictionary<string, string>();
        public static Dictionary<string, string> funcs = new Dictionary<string, string>();
        public static Dictionary<string, string> nums = new Dictionary<string, string>();
        public static Dictionary<string, string> bools = new Dictionary<string, string>();
        public static Dictionary<string, string> strs = new Dictionary<string, string>();

        public static List<string> functions = new List<string>();

        public static bool FindItem(string value)
        {
            if (operators.Contains(value))
            {
                ops[value] = value + " operator";
                return true;
            }

            if (spWords.Contains(value))
            {
                spwords[value] = value + " sp word";
                return true;
            }

            foreach (var item in spWords)
            {
                if (Math.Abs(item.Length - value.Length) > 2) continue;
                int count = 0;

                if (count < value.Length - 1)
                {
                    Result[value] = value + " error " + item + " sp word";
                    return true;
                }
            }

            foreach (var item in operators)
            {
                int count = 0;
                if (item.Length > value.Length) {
                    count = item.Length;
                }
                else count = value.Length;
                
                if (count == 5)
                {
                    Result[value] = value + " error " + item + " operator";
                    return true;
                }
            }
            return false;
        }

        public static bool BoolCheck(string value)
        {
            bool check = value == "True";
            check = check || value == "False";
            var result = value.Concat("    bool").ToString();
            if (check)
            {
                bools[value] = result;
                return true;
            }
            else return false;
        }

        public static bool StringCheck(string value)
        {
            var end = value.LastIndexOf('"');
            var start = value.IndexOf('"');
            if (!(end == start)) return true;
            else return false;
        }

        public static bool NumCheck(string num)
        {
            try
            {
                int.Parse(num);
                float.Parse(num);
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static int FuncNameCheck(string line, int i)
        {
            int endOfName = 0;
            //= line.Substring(i).IndexOf(' ');
            for (int j = 0; j < line.Length; j++)
            {
                if (!Alphabet.Contains(line[j].ToString()))
                {
                    endOfName = j; break;
                }
            }
            string str = line.Substring(i, endOfName);
            functions.Add(str);
            funcs[str] = str + " func";
            return i + str.Length;
        }


        public static void LexemeCheck(string lexeme)
        {
            lexeme = lexeme.Remove(lexeme.IndexOf(',') == -1 ? 0 : lexeme.IndexOf(','));
            lexeme = lexeme.Remove(lexeme.IndexOf(':') == -1 ? 0 : lexeme.IndexOf(':'));

            if (lexeme == "") return;

            if (NumCheck(lexeme)) return;

            if (FindItem(lexeme)) return;

            if (BoolCheck(lexeme)) return;

            if (StringCheck(lexeme)) return;

            if (functions.Contains(lexeme))
            {
                funcs[lexeme] = lexeme + " func";
                return;
            };

            vars[lexeme] = lexeme + " variable";

        }
    }
}
