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
using WpfApp2.ViewModel;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModelParts parts = new ViewModelParts();

        public MainWindow()
        {
            InitializeComponent();
            cboSearch.ItemsSource = parts.ItemCbo();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            parts.SavePart(txbCode.Text, txbDescription.Text, txbLength.Text, txbWidth.Text);
            dataGridParts.ItemsSource = null;
            dataGridParts.ItemsSource = parts.RefreshDataGrid();
            ClearTextBox();
        }
        private void ClearTextBox()
        {
            txbCode.Text = "";
            txbDescription.Text = "";
            txbLength.Text = "";
            txbWidth.Text = "";
        }

        private void shown_SourceInitialized(object sender, EventArgs e)
        {
            parts.LoadList();
            dataGridParts.ItemsSource = parts.RefreshDataGrid();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Part part = (Part)dataGridParts.SelectedItem;
            parts.EditPiece(part);
            if (part != null)
            {
                txbCode.Text = part.codePart.ToString();
                txbDescription.Text = part.descriptionPart;
                txbLength.Text = part.lengthPart.ToString();
                txbWidth.Text = part.widthPart.ToString();
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            parts.DeletePart((Part)dataGridParts.SelectedItem);
            ClearTextBox();
            dataGridParts.ItemsSource = null;
            dataGridParts.ItemsSource = parts.RefreshDataGrid();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dataGridParts.ItemsSource = parts.SearchPart(cboSearch.SelectedIndex, txbSearch.Text);
        }

        private void btnClearFilters_Click(object sender, RoutedEventArgs e)
        {
            dataGridParts.ItemsSource = null;
            dataGridParts.ItemsSource = parts.RefreshDataGrid();
            ClearTextBox();
        }
    }
}
