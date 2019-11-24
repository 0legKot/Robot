using System;
using System.Diagnostics;
using System.Linq;

namespace RTOS
{
    public static class Helper
    {
        public static readonly Stopwatch sw = new Stopwatch();
        public static bool Conditions(string cond) {
            cond = cond.Trim();
            var input = cond;
            while (-1 != input.IndexOf(']'))
            {

                int end = input.IndexOf(']');
                int start = 0;
                for (int i = end; i > 0; i--)
                {
                    if (input[i] == '[')
                    {
                        start = i;
                        break;
                    }
                }
                var input2 = input.Substring(0, start);
                input2 += Conditions(input.MySubstring(start + 1, end)) ? "1=1" : "1=0";
                input2 += (input + " ").Substring(end + 1);
                input = input2;
            }
            cond = input;
            var operator_logic_to_logic = new char[] { '&', '|' };
            var conds = cond.Split(operator_logic_to_logic);
            bool final = Condition(conds[0]);
            for (int j = 1; j < conds.Length; j++)
            {
                var right = Condition(conds[j]);
                var oper = cond.Where(x => operator_logic_to_logic.Contains(x)).ToArray()[j-1];
                switch (oper)
                {
                    case '&':
                        final = final && right;
                        break;
                    case '|':
                        final = final || right;
                        break;
                }
             
            }
            return final;

        }
        public static bool Condition(string input)
        {
            var trimmed = input.Trim();
            var operator_number_to_logic = new char[] { '>', '<', '=', '!' };
            var exprsparsed = trimmed.Split(operator_number_to_logic);
            var left = Expression(exprsparsed[0]);
            var right = Expression(exprsparsed[1]);
            var oper = trimmed.Where(x => operator_number_to_logic.Contains(x)).ToArray()[0];
            switch (oper)
            {
                case '>':
                    return left > right;
                case '<':
                    return left < right;
                case '=':
                    return left == right;
                case '!':
                    return left != right;
            }
            return true;
        }
        public static int Expression(string input1)
        {
            var trimmed = input1.Trim();
            var input = trimmed;
            while (-1 != input.IndexOf(']'))
            {

                int end = input.IndexOf(']');
                int start = 0;
                for (int i = end; i > 0; i--)
                {
                    if (input[i] == '[')
                    {
                        start = i;
                        break;
                    }
                }
                var input2 = input.Substring(0, start);
                input2 += Expression(input.MySubstring(start + 1, end));
                input2 += (input + " ").Substring(end + 1);
                input = input2;
            }
            trimmed = input;
            var operator_number_to_number = new char[] { '+', '-', '*', '/' };
            var exprsparsed = trimmed.Split(operator_number_to_number).Select(x=>x.Trim()).ToArray();
            if (exprsparsed.Length == 0)
                return GetValue(trimmed);
            var final = GetValue(exprsparsed[0]);
            for (int j = 1; j < exprsparsed.Length; j++)
            {
                var right = GetValue(exprsparsed[j]);
                var oper = trimmed.Where(x => operator_number_to_number.Contains(x)).ToArray()[0];
                switch (oper)
                {
                    case '+':
                        final += right;
                        break;
                    case '-':
                        final -= right;
                        break;
                    case '*':
                        final *= right;
                        break;
                    case '/':
                        final /= right;
                        break;
                }
            }
            return final;
        }
        public static int GetValue(string something)
        {

            something = something.Trim();
            if (something == "get_current_time")
                return (int)sw.ElapsedMilliseconds;
            if (something == "get_damage_level")
                return Math.Min((SituationInfo.GetHandX() < 45&&SituationInfo.GetHandY() < 15)?SituationInfo.humanMap[SituationInfo.GetHandX(), SituationInfo.GetHandY()] :-1,0); 
            if (something.StartsWith("$"))
            {
                return Storage.UserVars[something.Substring(1)];
            }
            else return int.Parse(something);
        }
    }
}