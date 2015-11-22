namespace Oocx.ACME.CLRConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine($"acme.exe v{typeof(Program).Assembly.GetName()?.Version}");
            var program = new Oocx.ACME.Console.Program();
            program.Main(args);
        }
    }
}
