namespace E_Commers.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Adderss { get; set; }

        public ICollection<Product> Products { get; } = new List<Product>();


    }
}
