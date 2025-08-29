namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var countries = new List<Cntry>();

            var Cntry = countries.FirstOrDefault(x => x.Name == "eg");
            Console.WriteLine(Cntry);
        }
    }
    public class Cntry
    {
        public string Name { get; set; }
    }
}
