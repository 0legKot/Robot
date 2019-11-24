using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RTOS
{
    public static class ext {
        public static string MySubstring(this string input, int start, int end)
        {
            return input.Substring(start, end-start);
        }
    }
    public static class Storage {
        static Dictionary<string, int> UserVars = new Dictionary<string, int>();
    }
    internal class MyBlock
    {
        const string log = "..\\Log.txt";
        string[] res;
        public MyBlock(string input) {
            while (-1 != input.IndexOf('}')) {

                int end = input.IndexOf('}');
                int start = 0;
                for (int i = end; i >0 ; i--)
                {
                    if (input[i] == '{') {
                        start = i;
                        break;
                    }
                }
                var input2 = input.Substring(0, start);
                var tmp = new MyBlock(input.MySubstring(start + 1, end)).Execute();
                foreach (var bl in tmp)
                {
                    
                    input2 += bl+" ";
                }
                 input2 += (input+" ").Substring(end + 1);
                input = input2;
            }
            var withoutBlocks = input.Split(';').ToList();
            res = new string[withoutBlocks.Count];
            for (int i=0;i<withoutBlocks.Count;i++)
            {
                res[i] += "\n";
                var trimmed = withoutBlocks[i].Trim();
                if (trimmed == "") continue;
                if (trimmed.StartsWith("for"))
                {
                    string range = trimmed.MySubstring(trimmed.IndexOf('(')+1, trimmed.IndexOf(')'));
                    var uvar = new MyExpression(range.MySubstring(0, range.IndexOf(" from")));
                    var from = new MyExpression(range.MySubstring(range.IndexOf(" from")+6, range.IndexOf(" to")));
                    var to = new MyExpression(range.Substring(range.IndexOf(" to") + 4));
                    string forblocks = (trimmed+" ").Substring(trimmed.IndexOf(')')+1);
                    res[i] += $"Var({uvar}) from({from}) to({to}) ForBlocks@{forblocks}@";
                    continue;
                }
                if (trimmed.StartsWith("while"))
                {
                    var operator_logic_to_logic = new char[] { '&', '|' };
                    string cond = trimmed.MySubstring(trimmed.IndexOf('(') + 1, trimmed.IndexOf(')'));
                    var conds = cond.Split(operator_logic_to_logic);
                    res[i] += "while ";
                    for (int j = 0; j < conds.Length; j++)
                    {
                        var expr = conds[j];
                        res[i] += new MyExpression(expr);
                        if (j != conds.Length - 1)
                            res[i] += $" LogicOper%{cond.Where(x => operator_logic_to_logic.Contains(x)).ToArray()[j]}% ";
                    }
                    string whileblocks = (trimmed + " ").Substring(trimmed.IndexOf(')') + 1);
                    res[i] += $"WhileBlocks@{whileblocks}@";
                    continue;
                }
                if (trimmed.StartsWith("if"))
                {
                    var operator_logic_to_logic = new char[] { '&', '|' };
                    string cond = trimmed.MySubstring(trimmed.IndexOf('(') + 1, trimmed.IndexOf(')'));
                    var conds = cond.Split(operator_logic_to_logic);
                    res[i] += "if ";
                    for(int j=0;j<conds.Length;j++)
                    {
                        var expr = conds[j];
                        res[i] += new MyExpression(expr);
                        if (j != conds.Length - 1)
                            res[i] += $" LogicOper%{cond.Where(x => operator_logic_to_logic.Contains(x)).ToArray()[j]}% ";
                    }
                    string ifblocks = (trimmed + " ").Substring(trimmed.IndexOf(')') + 1);
                    res[i] += $"IfBlocks@{ifblocks}@";
                    
                    continue;
                }
                var prev = File.ReadAllText(log);
                File.WriteAllText(log, prev+"\n"+trimmed);
                res[i] += trimmed;
            }
        }
        public List<string> Execute() {
            return res.ToList();
        }
    }
}