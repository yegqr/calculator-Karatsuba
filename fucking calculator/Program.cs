public class Program
{
    static int priority(string symbol)
    {
        var dict = new Dictionary<string, int>()
        {
            { "+", 0 }, { "-", 0 }, { "/", 1 }, { "*", 1 }, { "^", 2 } , {"(" , -1}
        };
        return dict[symbol];
    }

    public static List<string> tokenize(string input)
    {
        var tokenized = new List<string>();
        var typesOperations = new List<string>() { "+", "-", "/", "*", "(", ")", "^" };
        var previousDigit = true;
        var   temporary = new List<string>();

        foreach (var element in input)
        {
            if (element == ' ')
            {
                continue;
            }
            else if (char.IsLetter(element))
            {
                previousDigit = false;
                if (temporary.Count > 0)
                {
                    var bufer = string.Join("", temporary);
                    tokenized.Add(bufer);
                    temporary.Clear();
                }
            }

            else if (char.IsDigit(element) || element == '.')
            {
                previousDigit = true;
                temporary.Add(element.ToString());
            }

            else if (typesOperations.Contains(element.ToString()))
            {
                if (temporary.Count != 0)
                {
                    var bufer = string.Join("", temporary);
                    tokenized.Add(bufer);
                    temporary.Clear();
                }

                previousDigit = false;
                tokenized.Add(element.ToString());
            }
        }
        var residue = string.Join("", temporary);
        if (residue.Length > 0)
        {
            tokenized.Add(residue);
        }
        
        return tokenized;
    }

    public static List<string> ToPostfix(List<string> tokenized)
    {
        var operators = new Stack<string>();
        var output = new List<string>();

        foreach (string token in tokenized)
        {
            if (float.TryParse(token, out float res))
            {
                output.Add(res.ToString());
            }
            else if (token == "(")
            {
                operators.Push(token);
            }
            else if (token == ")")
            {
                while (operators.Count > 0 && operators.Peek() != "(")
                {
                    output.Add(operators.Pop());
                }
                if (operators.Count > 0 && operators.Peek() == "(")
                {
                    operators.Pop();
                }
            }
            else
            {
                while (operators.Count > 0 && priority(operators.Peek()) >= priority(token))
                {
                    output.Add(operators.Pop().ToString());
                }
                operators.Push(token);
            }
        }

        while (operators.Count > 0)
        {
            output.Add(operators.Pop());
        }

        return output;
    }
    
    public static float Calculating(List<string> Postfix)
    {
        var final = new Stack<float>();

        foreach (var element in Postfix)
        {
            if (float.TryParse(element, out float res))
            {
                final.Push(res);
            }
            else
            {
                var num1 = final.Pop();
                var num2 = final.Pop();
                
                if (element == "+")
                {
                    final.Push(num2 + num1);
                }
                else if (element == "-")
                {
                    final.Push(num2 - num1);
                }
                else if (element == "*")
                {
                    final.Push(num2 * num1);
                }
                else if (element == "/")
                {
                    final.Push(num2 / num1);
                }
                else if (element == "^")
                {
                    float box = 1;
                    for (int i = 0; i < num1; i++)
                    {
                        box *= num2;
                    }
                    final.Push(box);
                }
            }
        }
        return final.Pop();
    }
}