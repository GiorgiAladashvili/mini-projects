using System.Data;
using G17_ProductImport.Models;
using Microsoft.Data.SqlClient;

namespace G17_ProductImport
{
	public class DatabaseImporter
	{
		private readonly string _connectionString;

		public event Action? ImportStart;
		public event Action? ImportComplete;
		public event Action<Category,Product>? ImportProgress;

		
		public DatabaseImporter(string connectionString)
		{
			_connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
		}

		public void ImportData(IEnumerable<Category> categories)
		{
			using var connection = new SqlConnection(_connectionString);
			using var command = connection.CreateCommand();
			SetupCommand(command);

			connection.Open();
			using var transaction = connection.BeginTransaction();
			try
			{
				ImportStart?.Invoke();
				foreach (var category in categories)
				{
					foreach (var product in category.Products)
					{
						ImportProgress?.Invoke(category,product);
						command.Parameters["@CategoryName"].Value = category.Name;
						command.Parameters["@CategoryIsActive"].Value = category.IsActive;
						command.Parameters["@ProductCode"].Value = product.Code;
						command.Parameters["@ProductName"].Value = product.Name;
						command.Parameters["@ProductPrice"].Value = product.Price;
						command.Parameters["@ProductIsActive"].Value = product.IsActive;
						command.Transaction = transaction;
						command.ExecuteNonQuery();
					}
				}
				transaction.Commit();
				ImportComplete?.Invoke();
			}
			catch(SqlException _ )
			{
				transaction.Rollback();
				throw;
			}
		}


		private static void SetupCommand(SqlCommand command)
		{
			command.CommandText = "sp_ImportProduct";
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add("@CategoryName", SqlDbType.NVarChar, 100);
			command.Parameters.Add("@CategoryIsActive", SqlDbType.Bit);
			command.Parameters.Add("@ProductCode", SqlDbType.VarChar, 10);
			command.Parameters.Add("@ProductName", SqlDbType.NVarChar, 100);
			command.Parameters.Add("@ProductPrice", SqlDbType.Money);
			command.Parameters.Add("@ProductIsActive", SqlDbType.Bit);
		}
	}
}
