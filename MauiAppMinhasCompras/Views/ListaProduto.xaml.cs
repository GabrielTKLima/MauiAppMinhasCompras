using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
   
    public ListaProduto()
	{
		InitializeComponent();

        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        // Atualiza a lista ao exibir a página
        lista.Clear();
        List<Produto> tmp = await App.Db.GetAll();
        tmp.ForEach(i => lista.Add(i));
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());

		} catch (Exception ex)
		{
			DisplayAlert("Ops", ex.Message, "OK"); 
		}
    }
    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        string q = e.NewTextValue;

        lista.Clear();

        List<Produto> tmp = await App.Db.Search(q);

        tmp.ForEach(i => lista.Add(i));
    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        double soma = lista.Sum(i => i.Total);

        string msg = $"O total é {soma:C}";

        DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private void MenuItem_Clicked(object sender, EventArgs e)
    {

    }

    private async void OnEditSwipeItemInvoked(object sender, EventArgs e)
    {
        try
        {
            var swipe = sender as SwipeItem;
            var produto = swipe?.BindingContext as Produto;
            if (produto == null) return;

            await Navigation.PushAsync(new Views.EditarProduto(produto));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {
        try
        {
            var swipe = sender as SwipeItem;
            var produto = swipe?.BindingContext as Produto;
            if (produto == null) return;

            bool confirm = await DisplayAlert("Confirmar", "Deseja excluir este produto?", "Sim", "Não");
            if (!confirm) return;

            await App.Db.Delete(produto.Id);
            lista.Remove(produto);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}