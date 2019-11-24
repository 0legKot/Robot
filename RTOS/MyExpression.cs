using System.Linq;

namespace RTOS
{
    public class MyExpression
    {
        string res = "";
        public MyExpression(string input) 
        {
            var trimmed = input.Trim();
            var operator_number_to_logic = new char[] { '>', '<', '=', '!' };
            var exprsparsed = trimmed.Split(operator_number_to_logic);
            if (exprsparsed.Where(x=>x.Trim().Length>0).ToArray().Length > 1)
                for (int i=0;i<exprsparsed.Length;i++)
                {
                    var expr = exprsparsed[i];
                    res += $"Expr%{new MyExpression(expr)}% ";
                    if (i != exprsparsed.Length - 1)
                        res += $" NumberOper%{trimmed.Where(x=>operator_number_to_logic.Contains(x)).ToArray()[i]}% ";
                }
            else res += $"{exprsparsed.FirstOrDefault()} ";
        }
        public override string ToString()
        {
            return res;
        }
        public string Execute() 
        {
            return res;
        }
    }
}