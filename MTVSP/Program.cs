using System;

namespace MTVSP
{
    internal class Program
    { 
        
        
        static void Main()
        {
            Console.WriteLine("Please enter the number of threads followed by the number of messages ie, 10 100");
            while (true)
            {
                try
                {
                    var input = Console.ReadLine();
                    
                    if (!string.IsNullOrEmpty(input))
                    {
                        InputProcessor ip = new InputProcessor();
                        ip.Start(input);
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
           
        }
    }
}
