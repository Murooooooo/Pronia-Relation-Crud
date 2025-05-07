namespace WebApplication8.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string? PhotoUrl { get; set; }
        public int ProductId { get; set; }
        public  Product? Product { get; set; }
    }
}
