using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class EditarProduto : ContentPage
{
	Produto current;

	public EditarProduto(Produto p)
	{
		InitializeComponent();
		current = p;

		// Preenche campos com os valores do produto
		txt_descricao.Text = current.Descricao;
		txt_quantidade.Text = current.Quantidade.ToString();
		txt_preco.Text = current.Preco.ToString();
	}

	private async void ToolbarItem_Clicked(object sender, EventArgs e)
	{
		try
		{
			current.Descricao = txt_descricao.Text;
			current.Quantidade = Convert.ToDouble(txt_quantidade.Text);
			current.Preco = Convert.ToDouble(txt_preco.Text);

			await App.Db.Update(current);

			await DisplayAlert("Sucesso", "Produto atualizado.", "OK");
			await Navigation.PopAsync();
		}
		catch (Exception ex)
		{
			await DisplayAlert("Ops", ex.Message, "OK");
		}
	}
}