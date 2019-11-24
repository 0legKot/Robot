using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RTOS
{
    public static class Extensions
    {
        const string log = "..\\Log.txt";
        public static string MySubstring(this string input, int start, int end)
        {
            return input.Substring(start, end - start);
        }
        private static void WriteLog(string trimmed)
        {
            var prev = File.ReadAllText(log);
            File.WriteAllText(log, prev + "\n" + trimmed);
        }
        public static List<BlockWithSubs> GetBlocks(this string input, out string rest)
        {
            File.WriteAllText(log, "");
            Helper.sw.Restart();
            var currentBlocks = new List<BlockWithSubs>();
            while (true)
            {
                input = input.Trim('\n', '\r', '\t', ' ');
                if (input.StartsWith("for") || input.StartsWith("while") || input.StartsWith("if"))
                {
                    var index = input.IndexOf('{');
                    currentBlocks.Add(new BlockWithSubs(
                        input.MySubstring(0, index),
                        true,
                        GetBlocks(input.Substring(index + 1), out string restOfInput)
                        ));
                    input = restOfInput;
                }
                else if (input.StartsWith("}"))
                {
                    input = input.Substring(2);
                    rest = input;
                    return currentBlocks;
                }
                else if (input == "")
                {
                    rest = input;
                    return currentBlocks;
                }
                else
                {
                    var index = input.IndexOf(';');
                    if (index == -1)
                    {
                        throw new StackOverflowException();
                    }
                    currentBlocks.Add(new BlockWithSubs(
                        input.MySubstring(0, index + 1),
                        false,
                        new List<BlockWithSubs>()
                        ));
                    input = input.Substring(index + 1);
                }
            }
        }
        public static void Execute(this List<BlockWithSubs> blocks)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                var trimmed = blocks[i].Simple.Trim();
                if (blocks[i].Blocks.Count == 0)
                {
                    if (trimmed.Contains(" is"))
                    {
                        var uvar = trimmed.MySubstring(1, trimmed.IndexOf(" is"));
                        Storage.UserVars[uvar] = Helper.Expression(trimmed.MySubstring(trimmed.IndexOf(" is") + 4,trimmed.Length-1));
                        //WriteLog($"{uvar} IS {Storage.UserVars[uvar]} NOW");
                    }
                    else
                    {
                        WriteLog(trimmed);
                    }
                    continue;
                }
                if (trimmed.StartsWith("for"))
                {
                    string range = trimmed.MySubstring(trimmed.IndexOf('(') + 1, trimmed.IndexOf(')'));
                    var uvar = range.MySubstring(0, range.IndexOf(" from"));
                    Storage.UserVars[uvar] = Helper.Expression(range.MySubstring(range.IndexOf(" from") + 6, range.IndexOf(" to")));
                    for (; Storage.UserVars[uvar] <= Helper.Expression(range.Substring(range.IndexOf(" to") + 4)); Storage.UserVars[uvar] = Storage.UserVars[uvar] + 1)
                    {
                        Execute(blocks[i].Blocks);
                    }
                    continue;

                }

                if (trimmed.StartsWith("while"))
                {
                    string cond = trimmed.MySubstring(trimmed.IndexOf('(') + 1, trimmed.IndexOf(')'));
                    while (Helper.Conditions(cond))
                    {
                        Execute(blocks[i].Blocks);
                    }
                    continue;
                }
                if (trimmed.StartsWith("if"))
                {
                    string cond = trimmed.MySubstring(trimmed.IndexOf('(') + 1, trimmed.IndexOf(')'));
                    if (Helper.Conditions(cond))
                    {
                        Execute(blocks[i].Blocks);
                    }
                    continue;
                }
            }
        }
    }
}