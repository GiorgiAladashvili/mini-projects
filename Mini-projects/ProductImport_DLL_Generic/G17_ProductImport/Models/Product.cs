namespace G17_ProductImport.Models;

public class Product
{
	public string Code { get; set; } = null!;
	public string Name { get; set; } = null!;
	public decimal Price { get; set; }
	public bool IsActive { get; set; }
}