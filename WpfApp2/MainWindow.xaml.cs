using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp2.Model;
using WpfApp2.Util;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int indice = -1;
        public List<Peca> ListaPecas { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ListaPecas = new List<Peca>();
            string[] pesquisa = { "Código", "Descrição", "Dimensão" };
            cboPesquisa.ItemsSource = pesquisa;
        }

        private int ValidarInt(string item)
        {
            int result = 0;
            if (int.TryParse(item, out result) && result > 0)
            {
                return result;
            }
            else
            {
                return -1;
            }
        }
        private decimal ValidarDecimal(string item)
        {
            decimal result = 0;
            string item2 = item.Replace(".", ",");
            if (decimal.TryParse(item2, out result) && result > 0)
            {
                return result;
            }
            else
            {
                return -1;
            }

        }

        private void btnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (ValidarInt(txbCodigo.Text) > 0 && ValidarDecimal(txbLargura.Text) > 0 && ValidarDecimal(txbComprimento.Text) > 0)
            {
                Peca novaPeca = new Peca();
                novaPeca.codigoPeca = ValidarInt(txbCodigo.Text);
                novaPeca.comprimentoPeca = ValidarDecimal(txbComprimento.Text);
                novaPeca.larguraPeca = ValidarDecimal(txbLargura.Text);
                novaPeca.descricaoPeca = txbDescricao.Text;
                novaPeca.dimensaoPeca = $"{novaPeca.comprimentoPeca} X {novaPeca.larguraPeca}";

                if (indice == -1)
                {
                    ListaPecas.Add(novaPeca);
                }
                if (indice > -1)
                {
                    ListaPecas.RemoveAt(indice);
                    ListaPecas.Insert(indice, novaPeca);

                }
                ManipuladorArquivos.EscreverArquivo(ListaPecas);
                LimparCampos();
                CarregarListaPecas();
                indice = -1;
                MessageBox.Show("Salvo com sucesso", "Salvar", MessageBoxButton.OK);
            }
            else
            {
                if (ValidarInt(txbCodigo.Text) <= 0)
                {
                    MessageBox.Show("Codigo invalido digite novamente", "Erro", MessageBoxButton.OK);
                    txbCodigo.Text = "";
                }
                if (ValidarDecimal(txbLargura.Text) <= 0)
                {
                    MessageBox.Show("Largura invalida digite novamente", "Erro", MessageBoxButton.OK);
                    txbLargura.Text = "";
                }
                if (ValidarDecimal(txbComprimento.Text) <= 0)
                {
                    MessageBox.Show("Comprimento invalido digite novamente", "Erro", MessageBoxButton.OK);
                    txbComprimento.Text = "";
                }
            }

        }
        private void LimparCampos()
        {
            txbCodigo.Text = "";
            txbDescricao.Text = "";
            txbComprimento.Text = "";
            txbLargura.Text = "";
        }
        private void CarregarListaPecas()
        {
            dataGridPeca.ItemsSource = null;
            dataGridPeca.ItemsSource = ListaPecas;
        }

        private void shown_SourceInitialized(object sender, EventArgs e)
        {
            if (ManipuladorArquivos.LerArquivo() != null)
            {
                ListaPecas = ManipuladorArquivos.LerArquivo();
            }
            CarregarListaPecas();
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            Peca peca = (Peca)dataGridPeca.SelectedItem;
            indice = BuscaItem(peca);
            txbCodigo.Text = peca.codigoPeca.ToString();
            txbDescricao.Text = peca.descricaoPeca;
            txbComprimento.Text = peca.comprimentoPeca.ToString();
            txbLargura.Text = peca.larguraPeca.ToString();

        }
        private int BuscaItem(Peca peca)
        {
            int indice = -1;

            foreach (Peca pecas in ListaPecas)
            {
                indice++;
                if (pecas.codigoPeca == peca.codigoPeca && pecas.descricaoPeca == pecas.descricaoPeca && pecas.dimensaoPeca == peca.dimensaoPeca)
                {
                    break;
                }
            }
            return indice;
        }

        private void btnExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem certeza?", "Pergunta", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ListaPecas.RemoveAt(BuscaItem((Peca)dataGridPeca.SelectedItem));
                ManipuladorArquivos.EscreverArquivo(ListaPecas);
                LimparCampos();
                CarregarListaPecas();
            }
        }

        private IEnumerable<Peca> PesquisaItem(List<Peca> list)
        {
            IEnumerable<Peca> resultado = from item in list
                                          where item.codigoPeca == ValidarInt(txbPesquisar.Text)
                                          select item;
            return resultado;
        }
        private IEnumerable<Peca> PesquisaDescricao(List<Peca> list)
        {
            IEnumerable<Peca> resultado = from item in list
                                          where item.descricaoPeca.Contains(txbPesquisar.Text)
                                          select item;
            return resultado;
        }
        private IEnumerable<Peca> PesquisaDimensao(List<Peca> list)
        {
            IEnumerable<Peca> resultado = from item in list
                                          where item.comprimentoPeca == ValidarDecimal(txbPesquisar.Text) || item.larguraPeca == ValidarDecimal(txbPesquisar.Text)
                                          select item;
            return resultado;
        }



        private void btnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            switch (cboPesquisa.SelectedIndex)
            {
                case 0:
                    dataGridPeca.ItemsSource = PesquisaItem(ListaPecas);
                    break;
                case 1:
                    dataGridPeca.ItemsSource = PesquisaDescricao(ListaPecas);
                    break;
                case 2:
                    dataGridPeca.ItemsSource = PesquisaDimensao(ListaPecas);
                    break;
                default:
                    MessageBox.Show("Seleção inválida", "Erro", MessageBoxButton.OK);
                    break;
            }
        }

        private void btnLimparFiltros_Click(object sender, RoutedEventArgs e)
        {
            CarregarListaPecas();
            txbPesquisar.Text = "";
        }
    }
}
