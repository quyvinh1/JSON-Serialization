using Microsoft.Win32;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
namespace ManageProductsApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}
		ManageProducts product = new ManageProducts();
		private void Window_Loaded(object sender, RoutedEventArgs e) => LoadProducts();
		private void LoadProducts()
		{
			lvProducts.ItemsSource = product.GetProducts();
		}

		private void btnInsert_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var Product = new Product
				{
					ProductID = int.Parse(txtProductID.Text),
					ProductName = txtProductName.Text,
				};
				product.InsertProduct(Product);
				LoadProducts();
			} catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Insert Product");
			}
		}

		private void btnUpdate_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var Product = new Product
				{
					ProductID = int.Parse(txtProductID.Text),
					ProductName = txtProductName.Text
				};
				product.UpdateProduct(Product);
				LoadProducts();
			} catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Update Product");
			}
		}

		private void btnDelete_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var Product = new Product
				{
					ProductID = int.Parse(txtProductID.Text)
				};
				product.DeleteProduct(Product);
				LoadProducts();
			} catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "Delete Product");
			}
		}

		private void btnLoad_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				OpenFileDialog openFileDialog = new OpenFileDialog
				{
					Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
					DefaultExt = ".json",
					Title = "Import Product List"
				};
				if (openFileDialog.ShowDialog() == true)
				{
					string filePath = openFileDialog.FileName;
					ImportProductsFromFile(filePath);
					MessageBox.Show("Product list imported successfully.");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}");
			}
		}

		private void btnSaveToFile_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SaveFileDialog saveFileDialog = new SaveFileDialog
				{
					Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
					FileName = "ProductList.json",
					DefaultExt = ".json"
				};
				if (saveFileDialog.ShowDialog() == true)
				{
					string filePath = saveFileDialog.FileName;

					ExportProductsToFile(filePath);

					MessageBox.Show("Product list exported successfully.");
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error: {ex.Message}");
			}
		}
		private void ImportProductsFromFile(string filePath)
		{
			try
			{
				string jsonData = File.ReadAllText(filePath);

				List<Product> importedProducts = JsonSerializer.Deserialize<List<Product>>(jsonData);

				if (importedProducts != null && importedProducts.Count > 0)
				{
					product.products.Clear();
					foreach (var productItem in importedProducts)
					{
						product.InsertProduct(productItem);
					}
					lvProducts.ItemsSource = null;
					lvProducts.ItemsSource = product.GetProducts();
				}
			}
			catch (Exception ex)
			{
				throw new Exception($"Error importing product list: {ex.Message}");
			}
		}
		private void ExportProductsToFile(string filePath)
		{
			try
			{
				string jsonData = JsonSerializer.Serialize(product.GetProducts(),
					new JsonSerializerOptions { WriteIndented = true });

				File.WriteAllText(filePath, jsonData);
			}
			catch (Exception ex)
			{
				throw new Exception($"Error exporting product list: {ex.Message}");
			}
		}
	}
}