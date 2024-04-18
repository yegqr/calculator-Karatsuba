partial class start
{
    static void Main()
    {
        while (true)
        {
            Console.Write($"> ");
            string equation = Console.ReadLine();
            if (equation == "stop")
            {
                break;
            }
            
            List<string> tokens = Program.tokenize(equation);
            List<string> postfix = Program.ToPostfix(tokens);
            float result = Program.Calculating(postfix);
            
            Console.WriteLine($"< {result}");
        }
    }
}