namespace ModelLibrary
{
    public class Product
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Product(int id, double price, string name, string desc)
        {
            this.Id = id;
            this.Price = price;
            this.Name = name;
            this.Description = desc;
        }

        public Product()
        {
            
        }
    }
}
