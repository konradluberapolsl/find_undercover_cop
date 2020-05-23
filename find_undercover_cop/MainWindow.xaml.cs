using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using find_undercover_cop.Model;

namespace find_undercover_cop
{
    public partial class MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //wstępne ogarnięcie drag and dropa

        private void DDplace_Drop(object sender, DragEventArgs e)
        {
            string[] droppedFiles = null;


            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                droppedFiles = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            }

            if ((droppedFiles == null) || (!droppedFiles.Any()))
            {
                return;
            }

            //dopisywanie do listy/textboxa

            //tylko jeden element
            //listFiles.Items.Clear();
            foreach (string s in droppedFiles)
            {
                //listFiles.Items.Add(s);
                TextBlockCop.Text = "";
                TextBoxPath.Text = s;
            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            //listFiles.Items.Clear();
            TextBoxPath.Text = "";
            TextBlockCop.Text = "";
        }

        private void buttonTemp_Click(object sender, RoutedEventArgs e)
        {
            LicensePlate p = new LicensePlate(TextBlockLicensePlate.Text);
            MessageBox.Show($"tablica rej \nfull plate: {p.FullLicensePlate}\nloc shortcut: {p.LocationShortcut}\nloc fullname: {p.LocationFullName}\nloc voivod: {p.LocationVoivodeship}\nrand chars: {p.RandomCharacters}");
        }

        private void addFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                //listFiles.Items.Clear();
                //listFiles.Items.Add(openFileDialog.FileName);
                TextBlockCop.Text = "";
                TextBoxPath.Text = openFileDialog.FileName;
            }
        }

        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            LicensePlate p = new LicensePlate(TextBlockLicensePlate.Text);
            if (p.isUnderCoverCop)
            {
                TextBlockCop.Text = "to jes glina";
            }
            else
            {
                TextBlockCop.Text = "to nie jest glina";
            }
        }
    }
}
