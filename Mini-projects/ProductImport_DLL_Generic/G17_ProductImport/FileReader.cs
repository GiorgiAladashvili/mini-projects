using G17_ProductImport.Models;

namespace G17_ProductImport
{
	public class FileReader
	{
		private readonly string _filePath;

		public FileReader(string filePath)
		{
			_filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
			if (!File.Exists(_filePath)) throw new FileNotFoundException("File not found", _filePath);
		}

        public IEnumerable<Category> GetData()
        {
            var categories = new Dictionary<string, Category>();
            using var reader = new StreamReader(_filePath);

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var parts = line.Split('\t');

                var categoryName = parts[0];
                var isActive = parts[1] == "1";
                var productCode = parts[2];
                var productName = parts[3];
                var price = decimal.Parse(parts[4]);
                var productIsActive = parts[5] == "1";

                if (!categories.TryGetValue(categoryName, out Category category))
                {
                    category = new Category
                    {
                        Name = categoryName,
                        IsActive = isActive,
                    };
                    categories.Add(categoryName, category);
                }

                var product = new Product
                {
                    Code = productCode,
                    Name = productName,
                    Price = price,
                    IsActive = productIsActive
                };

                category.Products.Add(product);
            }
            return categories.Values;
        }
    }
}
