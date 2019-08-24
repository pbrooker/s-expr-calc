using System;
using System.Collections;
using System.Collections.Generic;

namespace s_expr_calc
{
    // The calculations
    public static class Calculator
    {
        public static Dictionary<string, Func<double, double, double>> functions = new Dictionary<string, Func<double, double, double>>()
        {
            {"add",new Func<double,double,double>(Add)},
            {"multiply",new Func<double,double,double>(Multiply)}
        };

        static double Add(double x, double y)
        {
            return x + y;
        }

        static double Multiply(double x, double y)
        {
            return x * y;
        }
    }

    // This is the heart of determining what is in the s-expression
    public class ParseUserExpression
    {
        public ParseUserExpression()
        {
        }

        public ParseUserExpression(String userExpression)
        {
            UserExpression = userExpression;
        }

        public string UserExpression { get; }


        // The intent is to find the first occurrance of a right bracket, and
        // then find the closest left bracket to get the enclosed expression
        // Provided a properly formed expression, there will only be one calcuation.
        // This continues until the left bracket index is 0 at which time the
        // calculations are complete
        public Int32 Parse(string UserExpression)
        {
            UserExpression = UserExpression.Replace("(", "( ").Replace(")", " )");
            while (UserExpression.Contains(')'))
            {
                int right_bracket = UserExpression.IndexOf(')', 0);
                int left_bracket = UserExpression.LastIndexOf('(', right_bracket);
                int length = right_bracket - left_bracket;

                string operation = UserExpression.Substring(left_bracket, length+1);

                EvaluateDetermined evaluate = new EvaluateDetermined();

                Int32 value = evaluate.Evaluate(operation.Trim());

                if (left_bracket == 0)
                {
                    return value;
                }
                else
                {
                    UserExpression = UserExpression.Replace(operation, value.ToString());
                }
            }
            return Convert.ToInt32(UserExpression);                  
        }
    }

    public class EvaluateDetermined
    {
        // read each individual expression and use the Calculator to determine result.
        // The result is returned to replace the expression in the string.
        public int Evaluate(String operation)
        {
            Dictionary<string, int> operations = new Dictionary <string, int>();
           
            if (operations.ContainsKey(operation))
            {
                return Convert.ToInt32(operations[operation]);
            }

            String[] expression = operation.Split(' ');
            int calculated = 0;
            try
            {
                calculated = (int)Calculator.functions[expression[1]](Convert.ToInt32(expression[2]), Convert.ToInt32(expression[3]));
            } catch (IndexOutOfRangeException)
            {
                throw new IndexOutOfRangeException();
            }
            operations.Add(operation, calculated);

            return calculated;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please Enter an S-Expression");
            string UserInput = Console.ReadLine();
            ParseUserExpression parse = new ParseUserExpression();

            Int32 result = parse.Parse(UserInput);

            Console.WriteLine(result);
        }
    }
}
