using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FleetingTime.MathOperationForString;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ExpressionArray();
            Console.Read();
        }

        private static void ExpressionArray()
        {
            string[] expressionArray = new[]
                        {
                "Model.Principal+Model.Interest+Model.Principal*Model.Rate+(Model.Principal+Model.Interest+(Model.Principal+Model.Interest))+(Model.Principal+Model.Interest)+(Model.Principal+Model.Interest)",
            };
            foreach (var expression in expressionArray)
            {
                var model = new { Principal = 100, Interest = 20, Rate = 0.5 };
                Console.WriteLine("源字符串：" + expression);
                Console.WriteLine("实体转换后数学等式：" + MathOperation.GetExpression(expression, model));
                double result = MathOperation.MathExpression(expression, model);
                Console.Write("算法结果：");
                Console.WriteLine(result);
            }
            Console.Write("实际结果：");
            Console.WriteLine(100 + 20 + 100 * 0.5 + (100 + 20 + (100 + 20)) + (100 + 20) + (100 + 20));
        }
    }
}
