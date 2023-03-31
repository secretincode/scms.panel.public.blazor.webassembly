namespace scms.panel.b.Models.Products;

public class Product
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public int? Stock { get; set; }

    public int? Price { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
