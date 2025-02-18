namespace ModelLibrary
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string? Description { get; set; }

        public Product(string name, double price, string desc)
        {
            this.Name = name;
            this.Price = price;
            this.Description = desc;
        }

        public Product(string name, double price) : this(name, price, string.Empty) { }

        public Product(string name) : this(name, 0.0, string.Empty) { }

        public Product() { }
    }
}
