using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;

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

            //tylko jeden element
            listFiles.Items.Clear();

            foreach (string s in droppedFiles)
            {
                listFiles.Items.Add(s);
            }

        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            listFiles.Items.Clear();
        }

        private void buttonTemp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                listFiles.Items.Clear();
                listFiles.Items.Add(openFileDialog.FileName);
            }
        }
    }
}
