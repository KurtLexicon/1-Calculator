using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;

namespace Calc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Console.WriteLine("Enter an expression containing 2 operands and 1 operator (e.g. 42 + 666)");
                Console.WriteLine("Leave blank and just press ENTER for terminating the program");
                string str = Console.ReadLine();
                if (String.IsNullOrWhiteSpace(str)) break;
                try
                {
                    Expression expr = Expression.Parse(str);
                    string strExpr = expr.ToString();
                    double result = expr.calculate();
                    Console.WriteLine(String.Format("{0} = {1}", strExpr, result));
                }
                catch (Expression.ParseException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine();
            }
        }
    }



    class Expression
    {
        public class ParseException: Exception {
            public ParseException(string msg) : base(msg) { }
        }

        private double a { get; set; }
        private double b { get; set; }
        private string op { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1} {2}", a, op, b);
        }

        static public Expression Parse(string str)
        {

            string pattern = @"\s*([+-]?\d*[,\.]?\d+)\s*([+*/\-])\s*([+-]?\d*[,\.]?\d+)\s*";
            Match match = Regex.Match(str, pattern);
            
            if (!match.Success && match.Groups.Count != 4)
                throw new ParseException("Expression is not in correct format");

            // We want that the user does not need bother which decimal separator is allowed,
            // For this reason we always convert , to . and use InvariantCulture
            double a = double.Parse(match.Groups[1].Value.Replace(',', '.'), CultureInfo.InvariantCulture);
            string op = match.Groups[2].Value;
            double b = double.Parse(match.Groups[3].Value.Replace(',', '.'), CultureInfo.InvariantCulture);
            
            if (op == "/" && b == 0.0)
                throw new ParseException("Division with 0 is not possible");
            
            return new Expression(a, op, b);
        }

        private Expression(double a, string op, double b)
        {
            this.a = a;
            this.b = b;
            this.op = op;
        }

        public double calculate()
        {
            switch(op)
            {
                case "*":
                    return Mul(a, b);
                case "+":
                    return Add(a, b);
                case "-":
                    return Sub(a, b);
                case "/":
                    return Div(a, b);
                default:
                    throw new Exception("Invalid operator");
            }
        }

        private double Add(double a, double b)
        {
            return a + b;
        }

        private double Sub(double a, double b)
        {
            return a - b;
        }

        private double Mul(double a, double b)
        {
            return a * b;
        }

        private double Div(double a, double b)
        {
            return a / b;
        }
    }
}
