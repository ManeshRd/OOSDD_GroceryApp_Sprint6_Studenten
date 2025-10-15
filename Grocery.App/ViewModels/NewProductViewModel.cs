using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System;
using System.Threading.Tasks;
using Grocery.App.Views;

namespace Grocery.App.ViewModels
{
    public partial class NewProductViewModel : BaseViewModel
    {
        private readonly IProductService _productrepository;
        private readonly IAuthService _authservice;

        [ObservableProperty]
        Client client;

        [ObservableProperty]
        private string name = string.Empty;

        [ObservableProperty]
        private int stock;

        [ObservableProperty]
        private DateOnly shelfLife = DateOnly.FromDateTime(DateTime.Now.AddMonths(1));

        [ObservableProperty]
        private decimal price;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public NewProductViewModel(IProductService productrepository, IAuthService authservice, GlobalViewModel global)
        {
            _productrepository = productrepository;
            _authservice = authservice;
            client = global.Client;
        }

        [RelayCommand]
        public async Task ShowNewProducts()
        {
            if (client.Role == Role.Admin) await Shell.Current.GoToAsync(nameof(NewProductView), true);
        }

        [RelayCommand]
        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private async Task SaveProduct()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorMessage = "Productnaam is verplicht";
                return;
            }
            if (Stock <= 0)
            {
                ErrorMessage = "Voorraad moet groter zijn dan 0";
                return;
            }
            if (Price <= 0)
            {
                ErrorMessage = "Prijs moet groter zijn dan 0";
                return;
            }
            

            try
            {
                var newProduct = new Product(0, Name, Stock, ShelfLife, Price);
                _productrepository.Add(newProduct);
                ErrorMessage = string.Empty;
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij opslaan: {ex.Message}";
            }
        }
    }
}