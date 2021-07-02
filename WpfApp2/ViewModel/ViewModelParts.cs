using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using WpfApp2.Model;
using WpfApp2.Util;

namespace WpfApp2.ViewModel
{
    public class ViewModelParts
    {
        public int index = -1;
        public List<Part> ListParts = new List<Part>();

        public string[] ItemCbo()
        {
            string[] CboSearch = { "Código", "Descrição", "Dimensão" };
            return CboSearch;
        }

        public int ConvertToInt(string item)
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
        public decimal ConvertToDecimal(string item)
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

        public Part NewPart(string code, string description, string length, string width)
        {
            Part newPart = new Part();
            newPart.codePart = ConvertToInt(code);
            newPart.lengthPart = ConvertToDecimal(length);
            newPart.widthPart = ConvertToDecimal(width);
            newPart.descriptionPart = description;
            newPart.dimensionPart = $"{newPart.lengthPart} X {newPart.widthPart}";
            return newPart;
        }
        public void SavePart(string code, string description, string length, string width)
        {
            if (ConvertToInt(code) > 0 && ConvertToDecimal(length) > 0 && ConvertToDecimal(width) > 0)
            {
                Part newPart = NewPart(code, description, length, width);

                if (index == -1)
                {
                    ListParts.Add(newPart);
                }
                if (index > -1)
                {
                    ListParts.RemoveAt(index);
                    ListParts.Insert(index, newPart);

                }
                Helper.Serialize(ListParts);
                index = -1;
                MessageBox.Show("Salvo com sucesso", "Salvar", MessageBoxButton.OK);
            }
            else
            {
                MessageError(code, length, width);
            }
        }
        private void MessageError(string code, string length, string width)
        {
            if (ConvertToInt(code) <= 0)
            {
                MessageBox.Show("Codigo invalido digite novamente", "Error", MessageBoxButton.OK);

            }
            if (ConvertToDecimal(width) <= 0)
            {
                MessageBox.Show("Largura invalida digite novamente", "Error", MessageBoxButton.OK);

            }
            if (ConvertToDecimal(length) <= 0)
            {
                MessageBox.Show("Comprimento invalido digite novamente", "Error", MessageBoxButton.OK);

            }
        }
        public void LoadList()
        {
            if (Helper.Deserialize() != null) 
            {
                ListParts = Helper.Deserialize();
            }
        }

        public List<Part> RefreshDataGrid()
        {
            return ListParts;
        }
        public void EditPiece(Part part)
        {
            index =GetIndex(part);
        }

        private int GetIndex(Part peca)
        {
            int index = -1;
            if (peca != null) 
            {
                index = ListParts.IndexOf(peca);
            }
            return index;
        }

        public void DeletePart(Part part)
        {
            if (MessageBox.Show("Tem certeza?", "Pergunta", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ListParts.RemoveAt(GetIndex(part));
                Helper.Serialize(ListParts);

            }
        }

        private IEnumerable<Part> GetCode(List<Part> list, string search)
        {
            IEnumerable<Part> result = from item in list
                                       where item.codePart == ConvertToInt(search)
                                       select item;
            return result;
        }
        private IEnumerable<Part> GetDescription(List<Part> list, string search)
        {
            IEnumerable<Part> result = from item in list
                                       where item.descriptionPart.Contains(search)
                                       select item;
            return result;
        }
        private IEnumerable<Part> GetDimension(List<Part> list, string search)
        {
            IEnumerable<Part> result = from item in list
                                       where item.lengthPart == ConvertToDecimal(search) || item.widthPart == ConvertToDecimal(search)
                                       select item;
            return result;
        }

        public IEnumerable<Part> SearchPart(int Index, string search)
        {
            switch (Index)
            {
                case 0:
                    return GetCode(ListParts, search);
                case 1:
                    return GetDescription(ListParts, search);
                case 2:
                    return GetDimension(ListParts, search);
            }
            return ListParts;
        }


    }
}
