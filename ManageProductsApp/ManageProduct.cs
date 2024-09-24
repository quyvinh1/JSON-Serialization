using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManageProductsApp
{
	public record Product
	{
		public int ProductID { get; set; }
		public string ProductName { get; set; }
	}
	public class ManageProducts
	{
		string fileName = "ProductList.json";
		public List<Product> products = new List<Product>();	
		public List<Product> GetProducts()
		{
			GetDataFromFile();
			return products;
		}
		public void StoreToFile()
		{
			try
			{
				string jsonData = JsonSerializer.Serialize(products,
					new JsonSerializerOptions { WriteIndented = true });
				File.WriteAllText(fileName, jsonData);
			} catch(Exception ex) {
				throw new Exception(ex.Message);
			}
		}
		public void GetDataFromFile()
		{
			try
			{
				if(File.Exists(fileName))
				{
					string jsonData = File.ReadAllText(fileName);
					products = JsonSerializer.Deserialize<List<Product>>(jsonData);
				}
			} catch (Exception ex) { throw new Exception(ex.Message); }
		}
		public void InsertProduct(Product Product)
		{
			try
			{
				Product p = products.SingleOrDefault(p => p.ProductID == Product.ProductID);
				if(p != null)
				{
					throw new Exception("This product already exists.");
				}
				products.Add(Product);
				StoreToFile();
			} catch (Exception ex) { throw new Exception( ex.Message); }
		}
		public void UpdateProduct(Product Product)
		{
			try
			{
				Product p = products.SingleOrDefault(p => p.ProductID == Product.ProductID);
				if(p == null)
				{
					throw new Exception("This product did not exist.");
				} else
				{
					p.ProductName = Product.ProductName;
					StoreToFile();
				}
			} catch (Exception ex) { throw new Exception(ex.Message); }
		}
		public void DeleteProduct(Product Product)
		{
			try
			{
				Product p = products.SingleOrDefault(p => p.ProductID == Product.ProductID);
				if(p == null)
				{
					throw new Exception("This product did not exist. ");
				} else
				{
					products.Remove(p);
					StoreToFile();
				}
			} catch(Exception ex) { throw new Exception(ex.Message); }
		}
	}
}
