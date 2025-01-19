namespace DistributedCaching.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }

        public Product()
        {
        }

        public Product(string name, string description, decimal price)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
        }

    }
    public record ProductCreationDto(string Name, string Description, decimal Price);
}
