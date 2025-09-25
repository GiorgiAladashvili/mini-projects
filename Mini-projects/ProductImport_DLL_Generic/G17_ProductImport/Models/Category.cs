namespace G17_ProductImport.Models
{
	public class Category
	{
		public string Name { get; set; } = null!;
		public bool IsActive { get; set; }
		public List<Product> Products { get; } = new();
	}
}
