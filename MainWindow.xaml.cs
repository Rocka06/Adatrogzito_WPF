using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Adatrogzito_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Data> dataList = new List<Data>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumberOnly(object sender, TextCompositionEventArgs e)
        {
            Regex number = new Regex("[^0-9]+");
            e.Handled = number.IsMatch(e.Text);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(ValidateForm(out string message))
            {
                dataList.Add(new(
                    NameTextBox.Text,
                    int.Parse(AgeTextBox.Text),
                    EmailTextBox.Text,
                    MobileTextBox.Text,
                    AddressTextBox.Text,
                    GenderTextBox.SelectedIndex == 0,
                    new TextRange(DescTextBox.Document.ContentStart, DescTextBox.Document.ContentEnd).Text
                ));
                SaveToFile();
            }
            ErrorLabel.Content = message;
        }

        bool ValidateForm(out string message)
        {
            Regex number = new(@"^\d+$");
            if (NameTextBox.Text.Trim().Length < 3)
            {
                message = "A névnek legalább 3 karakternek kell lennie!";
                return false;
            }

            if(!number.IsMatch(AgeTextBox.Text))
            {
                message = "Az életkornak számnak kell lennie!";
                return false;
            }

            if(int.Parse(AgeTextBox.Text) < 0)
            {
                message = "Az életkornak pozitív egész számnak kell lennie!";
                return false;
            }

            if (!IsValidEmail(EmailTextBox.Text))
            {
                message = "Érvénytelen email cím!";
                return false;
            }

            if (MobileTextBox.Text.Trim().Length < 8)
            {
                message = "A telefonszámnak legalább 8 karakter hosszúnak kell lennie!";
                return false;
            }

            if (AddressTextBox.Text.Trim().Length < 1)
            {
                message = "A telefonszámnak legalább 8 karakter hosszúnak kell lennie!";
                return false;
            }

            message = "";
            return true;
        }

        bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        void SaveToFile()
        {
            using (StreamWriter file = File.CreateText("adatok.csv"))
            {
                foreach (Data data in dataList)
                {
                    file.WriteLine(data.SaveString() + ";");
                }
            }
        }

        List<Data> ReadFromFile()
        {
            List<Data> list = new List<Data>();
            try
            {
                string text = File.ReadAllText("adatok.csv");
                string[] splitText = text.Split(";");

                foreach (string line in splitText)
                {
                    if (line.Trim().Length == 0) continue;
                    list.Add(new(line));
                }
            }
            catch { }
            
            return list;
        }

        void PopulateDataList()
        {
            dataList = ReadFromFile();
            DataList.Items.Clear();
            foreach (Data data in dataList)
            {
                DataList.Items.Add(data);
            }
        }

        private void DataList_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue) return;
            PopulateDataList();
        }

        private void DataList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = DataList.SelectedIndex;
            if (index == -1) return;
            dataList.RemoveAt(index);
            SaveToFile();
            PopulateDataList();
        }
    }
}