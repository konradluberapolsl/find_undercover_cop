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
            string newPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\database\list_of_markings.txt"));
            string[] test = File.ReadAllLines(newPath);
            MessageBox.Show(test[0]);
        }
    }
}
