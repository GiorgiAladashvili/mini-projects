using DatabaseHelper.SQL;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace G17_ProductImport
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var fileReader = new FileReader(@"C:\Users\User\source\2024 Projects\ProductImport-Files\Products.txt");
			var categories = fileReader.GetData();

			SqlDatabase connection = new SqlDatabase("Server=LAPTOP-F9NIKSL0\\MSSQLSERVER2022;Database=G17_Products;Integrated Security=True; trustservercertificate = true; pooling = true;");

			try
			{
				connection.OpenConnection();
				connection.BeginTransaction();

				foreach (var category in categories)
				{
					foreach (var product in category.Products)
					{
						connection.ExecuteNonQuery("sp_ImportProduct", CommandType.StoredProcedure,
							new SqlParameter("@CategoryName", category.Name),
							new SqlParameter("@CategoryIsActive", category.IsActive),
							new SqlParameter("@ProductCode", product.Code),
							new SqlParameter("@ProductName", product.Name),
							new SqlParameter("@ProductPrice", product.Price),
							new SqlParameter("@ProductIsActive", product.IsActive));
					}
				}

				connection.CommitTransaction();
			}
			catch (SqlException _)
			{
				connection.RollbackTransaction();
				throw;
			}
			finally
			{
				connection.CloseConnection();
			}
		}
	}
}
