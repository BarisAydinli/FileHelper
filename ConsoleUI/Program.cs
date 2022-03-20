using FileHelper;
class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new EntityDal<Product>(Path.Combine(@"C:\Projects\CSharpProjects\FileHelper\ConsoleUI\bin\Debug\net6.0\abc.txt"));
            manager.Delete(new Product { Id = 1});
        }
    }
}