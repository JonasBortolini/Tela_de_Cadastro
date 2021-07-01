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
        int index = -1;
        public List<Part> ListParts { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ListParts = new List<Part>();
            string[] CboSearch = { "Código", "Descrição", "Dimensão" };
            cboSearch.ItemsSource = CboSearch;
        }

        private int ConvertToInt(string item)
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
        private decimal ConvertToDecimal(string item)
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ConvertToInt(txbCode.Text) > 0 && ConvertToDecimal(txbWidth.Text) > 0 && ConvertToDecimal(txbLength.Text) > 0)
            {
                Part newPart = new Part();
                newPart.codePart = ConvertToInt(txbCode.Text);
                newPart.lengthPart = ConvertToDecimal(txbLength.Text);
                newPart.widthPart = ConvertToDecimal(txbWidth.Text);
                newPart.descriptionPart = txbDescription.Text;
                newPart.dimensionPart = $"{newPart.lengthPart} X {newPart.widthPart}";

                if (index == -1)
                {
                    ListParts.Add(newPart);
                }
                if (index > -1)
                {
                    ListParts.RemoveAt(index);
                    ListParts.Insert(index, newPart);

                }
                ManipuladorArquivos.EscreverArquivo(ListParts);
                ClearTextBox();
                LoadListToDataGrid();
                index = -1;
                MessageBox.Show("Salvo com sucesso", "Salvar", MessageBoxButton.OK);
            }
            else
            {
                if (ConvertToInt(txbCode.Text) <= 0)
                {
                    MessageBox.Show("Codigo invalido digite novamente", "Error", MessageBoxButton.OK);
                    txbCode.Text = "";
                }
                if (ConvertToDecimal(txbWidth.Text) <= 0)
                {
                    MessageBox.Show("Largura invalida digite novamente", "Error", MessageBoxButton.OK);
                    txbWidth.Text = "";
                }
                if (ConvertToDecimal(txbLength.Text) <= 0)
                {
                    MessageBox.Show("Comprimento invalido digite novamente", "Error", MessageBoxButton.OK);
                    txbLength.Text = "";
                }
            }

        }
        private void ClearTextBox()
        {
            txbCode.Text = "";
            txbDescription.Text = "";
            txbLength.Text = "";
            txbWidth.Text = "";
        }
        private void ClearTextBoxSearch() 
        {
            txbSearch.Text = "";
        }
        private void LoadListToDataGrid()
        {
            dataGridParts.ItemsSource = null;
            dataGridParts.ItemsSource = ListParts;
        }

        private void shown_SourceInitialized(object sender, EventArgs e)
        {
            if (ManipuladorArquivos.LerArquivo() != null)
            {
                ListParts = ManipuladorArquivos.LerArquivo();
            }
            LoadListToDataGrid();
        }
        private void EditPiece(Part peca) 
        {
            if (peca != null)
            {
                txbCode.Text = peca.codePart.ToString();
                txbDescription.Text = peca.descriptionPart;
                txbLength.Text = peca.lengthPart.ToString();
                txbWidth.Text = peca.widthPart.ToString();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditPiece((Part)dataGridParts.SelectedItem);
            index = GetIndex((Part)dataGridParts.SelectedItem);
        }
        private int GetIndex(Part peca)
        {
            int indice = -1;
            if (peca != null)
            {
                foreach (Part pecas in ListParts)
                {
                    indice++;
                    if (pecas.codePart == peca.codePart && pecas.descriptionPart == pecas.descriptionPart && pecas.dimensionPart == peca.dimensionPart)
                    {
                        break;
                    }
                }
            }
            return indice;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Tem certeza?", "Pergunta", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ListParts.RemoveAt(GetIndex((Part)dataGridParts.SelectedItem));
                ManipuladorArquivos.EscreverArquivo(ListParts);
                ClearTextBox();
                LoadListToDataGrid();
            }
        }

        private IEnumerable<Part> GetItem(List<Part> list)
        {
            IEnumerable<Part> result = from item in list
                                          where item.codePart == ConvertToInt(txbSearch.Text)
                                          select item;
            return result;
        }
        private IEnumerable<Part> GetDescription(List<Part> list)
        {
            IEnumerable<Part> result = from item in list
                                          where item.descriptionPart.Contains(txbSearch.Text)
                                          select item;
            return result;
        }
        private IEnumerable<Part> GetDimension(List<Part> list)
        {
            IEnumerable<Part> result = from item in list
                                          where item.lengthPart == ConvertToDecimal(txbSearch.Text) || item.widthPart == ConvertToDecimal(txbSearch.Text)
                                          select item;
            return result;
        }



        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            switch (cboSearch.SelectedIndex)
            {
                case 0:
                    dataGridParts.ItemsSource = GetItem(ListParts);
                    break;
                case 1:
                    dataGridParts.ItemsSource = GetDescription(ListParts);
                    break;
                case 2:
                    dataGridParts.ItemsSource = GetDimension(ListParts);
                    break;
                default:
                    MessageBox.Show("Seleção inválida", "Error", MessageBoxButton.OK);
                    break;
            }
        }

        private void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            LoadListToDataGrid();
            ClearTextBoxSearch();
        }
    }
}
