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
        //public MainWindow()
        //{
        //    InitializeComponent();
        //}
        //public static readonly RoutedEvent DropFileEvent = EventManager.RegisterRoutedEvent(nameof(DropFile), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MainWindow));
        //public event RoutedEventHandler DropFile
        //{
        //    add { AddHandler(DropFileEvent, value); }
        //    remove { RemoveHandler(DropFileEvent, value); }
        //}

        //private void DDplace_Drop(object sender, DragEventArgs e) => RaiseEvent(new RoutedEventArgs(DropFileEvent));

        //private void DDplace_Drop(object sender, DragEventArgs e)
        //{
        //    string[] droppedFiles = null;
        //    if (e.Data.GetDataPresent(DataFormats.FileDrop))
        //    {
        //        droppedFiles = e.Data.GetData(DataFormats.FileDrop, true) as string[];
        //    }

        //    if ((droppedFiles == null) || (!droppedFiles.Any()))
        //    {
        //        return;
        //    }

        //    foreach (string s in droppedFiles)
        //    {
        //        TextBlockCop.Text = "";
        //        TextBoxPath.Text = s;
        //        if (Path.GetExtension(s) == ".txt")
        //        {
        //            string[] textFromFile = File.ReadAllLines(s);
        //            TextBlockLicensePlate.Text = textFromFile[0];
        //        }
        //        else
        //        {
        //            TextBlockLicensePlate.Text = "";
        //        }
        //    }

        //}
    }
}
