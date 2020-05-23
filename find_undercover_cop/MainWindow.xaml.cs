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

            foreach (string s in droppedFiles)
            {
                TextBlockCop.Text = "";
                TextBoxPath.Text = s;
                if (Path.GetExtension(s) == ".txt")
                {
                    string[] textFromFile = File.ReadAllLines(s);
                    TextBlockLicensePlate.Text = textFromFile[0];
                }
                else
                {
                    TextBlockLicensePlate.Text = "";
                }
            }
        }

        //private void addFiles_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        //    openFileDialog.Filter = "Text files (*.txt) | *.txt;";
        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        TextBlockCop.Text = "";
        //        TextBoxPath.Text = openFileDialog.FileName;
        //        if (Path.GetExtension(openFileDialog.FileName) == ".txt")
        //        {
        //            string[] textFromFile = File.ReadAllLines(openFileDialog.FileName);
        //            TextBlockLicensePlate.Text = textFromFile[0];
        //        }
        //    }
        //}

        //private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        //{
        //    if (TextBlockLicensePlate.Text != "")
        //    {
        //        LicensePlate p = new LicensePlate(TextBlockLicensePlate.Text);
        //        if (p.isUnderCoverCop)
        //        {
        //            TextBlockCop.Text = "to jes glina: \n" + p.GetCopCar(p.FullLicensePlate, p.isUnderCoverCop);
        //        }
        //        else
        //        {
        //            TextBlockCop.Text = "to nie jest glina";
        //        }
        //    }
        //    else
        //    {
        //        TextBlockLicensePlate.Text = "";
        //    }
        //}
    }
}
