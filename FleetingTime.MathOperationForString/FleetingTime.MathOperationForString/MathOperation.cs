using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FleetingTime.MathOperationForString
{
    public static class MathOperation
    {
        public static string GetExpression<T>(string expression, T tModel)
        {
            PropertyInfo[] properties = tModel.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            string newExpression = expression;
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(tModel, null);
                newExpression = newExpression.Replace($"Model.{name}", value.ToString());
            }
            return newExpression;
        }
        public static double MathExpression<T>(string expression, T tModel)
        {
            PropertyInfo[] properties = tModel.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            string newExpression = expression;
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(tModel, null);
                newExpression = newExpression.Replace($"Model.{name}", value.ToString());
            }
            return MathExpression(newExpression);
        }

        public static double MathExpression(string expression)
        {
            return Convert.ToDouble(MathExpressionResult(expression.Replace(" ", "")).Replace('~', '-'));
        }

        private static string MathExpressionResult(string expression)
        {
            if (expression.Contains('('))
            {
                Stack<char> charStack = new Stack<char>();
                StringBuilder newExpression = new StringBuilder();
                foreach (var item in expression)
                {
                    if (item == ')')
                    {
                        while (charStack.Any())
                        {
                            char c = charStack.Pop();
                            if (c == '(')
                            {
                                string result = MathExpressionResult(newExpression.ToString());
                                foreach (var rItem in result)
                                {
                                    charStack.Push(rItem);
                                }
                                newExpression.Clear();
                                break;
                            }
                            newExpression.Insert(0, c);
                        }
                        continue;
                    }
                    charStack.Push(item);
                }
                while (charStack.Any())
                {
                    newExpression.Insert(0, charStack.Pop());
                }
                return MathExpressionResult(newExpression.ToString());
            }
            double numStr = MathExpressionResult_Child(expression);
            return numStr < 0 ? numStr.ToString(CultureInfo.InvariantCulture).Replace("-", "~") : numStr.ToString(CultureInfo.InvariantCulture);
        }

        private static double MathExpressionResult_Child(string expression)
        {
            string tempExpression = expression.Replace('+', ';').Replace('-', ';').Replace('*', ';').Replace('/', ';');
            string[] numStrings = tempExpression.Split(';');
            Queue<double> numQueue = new Queue<double>();
            foreach (var numStr in numStrings)
            {
                numQueue.Enqueue(Convert.ToDouble(numStr.Replace('~', '-')));
            }
            tempExpression = expression;
            Queue<char> operatingQueue = new Queue<char>();
            for (char cOperating = ' '; cOperating != char.MinValue;)
            {
                cOperating = tempExpression.FirstOrDefault(o => o == '+' || o == '-' || o == '*' || o == '/');
                if (cOperating != char.MinValue)
                {
                    int index = tempExpression.IndexOf(cOperating);
                    tempExpression = tempExpression.Remove(index, 1);
                    operatingQueue.Enqueue(cOperating);
                }
            }
            Stack<double> numStack = new Stack<double>();
            Stack<char> oStack = new Stack<char>();
            for (int i = 1; operatingQueue.Count + numQueue.Count > 0; i++)
            {
                if (i % 2 == 1)
                {
                    numStack.Push(numQueue.Dequeue());
                }
                else
                {
                    char operating = operatingQueue.Dequeue();
                    switch (operating)
                    {
                        case '+':
                            {
                                oStack.Push(operating);
                                break;
                            }
                        case '-':
                            {
                                oStack.Push(operating);
                                break;
                            }
                        case '*':
                            {
                                numStack.Push(numStack.Pop() * numQueue.Dequeue());
                                i++;
                                break;
                            }
                        case '/':
                            {
                                numStack.Push(numStack.Pop() / numQueue.Dequeue());
                                i++;
                                break;
                            }
                    }
                }
            }
            Stack<double> newNumStack = new Stack<double>();
            Stack<char> newOpStack = new Stack<char>();
            while (oStack.Any())
            {
                newOpStack.Push(oStack.Pop());
            }
            while (numStack.Any())
            {
                newNumStack.Push(numStack.Pop());
            }
            while (newOpStack.Any())
            {
                switch (newOpStack.Pop())
                {
                    case '+':
                        {
                            double firstNum = newNumStack.Pop();
                            double secondNum = newNumStack.Pop();
                            newNumStack.Push(firstNum + secondNum);
                            break;
                        }
                    case '-':
                        {
                            double firstNum = newNumStack.Pop();
                            double secondNum = newNumStack.Pop();
                            newNumStack.Push(firstNum - secondNum);
                            break;
                        }
                }
            }
            return newNumStack.Pop();
        }
    }
}