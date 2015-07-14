using System;
using System.Collections.Generic;
using System.IO;
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

namespace OfficerMate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random RNG;
        private readonly string officerMatePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\OfficerMate";
        private readonly string templatesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\OfficerMate\Templates";

        public MainWindow()
        {
            InitializeComponent();
            InitializeFileStructure();
            RNG = new Random();
        }

        private void InitializeFileStructure()
        {
            if (!Directory.Exists(officerMatePath))
            {
                Directory.CreateDirectory(officerMatePath);
            }
            if (!Directory.Exists(templatesPath))
            {
                Directory.CreateDirectory(templatesPath);
            }
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            if (SloganInputBox.Text.Length == 0)
            {
                return;
            }
            var temp = new ListBoxItem();
            temp.Content = SloganInputBox.Text;
            temp.Background = Brushes.Yellow;
            SloganListBox.Items.Add(temp);
            SloganInputBox.Text = string.Empty;
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            while (SloganListBox.SelectedIndex > -1)
            {
                SloganListBox.Items.RemoveAt(SloganListBox.SelectedIndex);
            }
        }

        private void CopySloganButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(string.Join(Environment.NewLine, SloganListBox.SelectedItems.Cast<ListBoxItem>().Select((x) => x.Content)));
        }

        private void RandomSloganButton_Click(object sender, RoutedEventArgs e)
        {
            SloganListBox.SelectedIndex = RNG.Next(SloganListBox.Items.Count);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.FileName = "Recruitment"; // Default file name
            saveDialog.DefaultExt = ".txt"; // Default file extension
            saveDialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            saveDialog.InitialDirectory = templatesPath;
            bool? result = saveDialog.ShowDialog();

            if (result == true)
            {
                string savePath = saveDialog.FileName;
                List<ListBoxItem> listBoxItems = SloganListBox.Items.Cast<ListBoxItem>().ToList();

                File.WriteAllText(savePath, string.Join(Environment.NewLine, listBoxItems.Select(x => x.Content)));

                foreach (var boxItem in listBoxItems)
                {
                    boxItem.Background = null;
                }
            }
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            var loadDialog = new Microsoft.Win32.OpenFileDialog();
            loadDialog.FileName = "Recruitment"; // Default file name
            loadDialog.DefaultExt = ".txt"; // Default file extension
            loadDialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension
            loadDialog.InitialDirectory = templatesPath;
            bool? result = loadDialog.ShowDialog();

            if (result == true)
            {
                string loadPath = loadDialog.FileName;

                string text = "";
                using (StreamReader newReader = File.OpenText(loadPath))
                {
                    text = newReader.ReadToEnd();
                }

                string[] tokens = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                while (SloganListBox.Items.Count > 0)
                {
                    SloganListBox.Items.RemoveAt(SloganListBox.Items.Count - 1);
                }
                foreach (var s in tokens)
                {
                    var temp = new ListBoxItem();
                    temp.Content = s;
                    SloganListBox.Items.Add(temp);
                }
            }
        }


    }
}
