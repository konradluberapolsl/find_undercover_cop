
using find_undercover_cop.Model;
using find_undercover_cop.Model.AI;
using find_undercover_cop.Model.AI.NeutralNetwork;
using find_undercover_cop.ViewModel.BaseClass;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace find_undercover_cop.ViewModel
{
    class MainViewModel: ViewModelBase
    {
        #region Fields

        private Recognition recognition = new Recognition();

        private LicensePlate currentLicensePlate;
        private string fullLicensePlate;

        private string filePath;
        private string isItCopStatement;

        #endregion

        #region Properties

        public LicensePlate CurrentLicensePlate
        {
            get => currentLicensePlate;
            set
            {
                currentLicensePlate = value;
                onPropertyChanged(nameof(CurrentLicensePlate));
            }
        }
        public string FullLicensePlate
        {
            get => fullLicensePlate;
            set
            {
                fullLicensePlate = value;
                onPropertyChanged(nameof(FullLicensePlate));
            }
        }


        public string FilePath
        {
            get => filePath;
            set
            {
                filePath = value;
                onPropertyChanged(nameof(FilePath));
            }
        }
        public string IsItCopStatement
        {
            get => isItCopStatement;
            set
            {
                isItCopStatement = value;
                onPropertyChanged(nameof(isItCopStatement));
            }
        }

        private ImageSource imagePreview;
        public ImageSource ImagePreview
        {
            get => imagePreview;
            set
            {
                imagePreview = value;
                onPropertyChanged(nameof(imagePreview));
            }
        }

        #endregion

        #region Ctor

        public MainViewModel()
        {

        }
        ~MainViewModel()
        {

        }

        #endregion

        #region Commands

        //load by d&d TUTAJ BĘDZIE TRZEBA POKOMBINOWAĆ
        private ICommand loadFileByDragAndDrop;
        public ICommand LoadFileByDragAndDrop
        {
            get
            {
                if (loadFileByDragAndDrop == null)
                {
                    loadFileByDragAndDrop = new RelayCommand(execute =>
                    {
                        //no tutaj będzie problem bo przeceiż jest ten argument, trzeba bedzie ogarnac to dependency najprawdopodobniej
                    }, canExecute => true);
                }
                return loadFileByDragAndDrop;
            }
        }

        //load by button
        private ICommand loadFileByButton;
        public ICommand LoadFileByButton
        {
            get
            {
                if (loadFileByButton == null)
                {
                    loadFileByButton = new RelayCommand(execute =>
                    {
                        LoadFileDialog();
                    }, canExecute => true);
                }
                return loadFileByButton;
            }
        }

        //check if its cop
        private ICommand checkIfItsCop;
        public ICommand CheckIfItsCop
        {
            get
            {
                if (checkIfItsCop == null)
                {
                    checkIfItsCop = new RelayCommand(execute =>
                    {
                        if (CurrentLicensePlate.isUndercoverCop)
                        {
                            //napraw bo nie pokazuje jakim autem jeździ gliniarz
                            IsItCopStatement = $"To gliniarz \n{CurrentLicensePlate.GetCopCar(CurrentLicensePlate.FullLicensePlate)}";
                        }
                        else
                        {
                            IsItCopStatement = "To nie gliniarz";
                        }
                    }, canExecute => CurrentLicensePlate != null);
                }
                return checkIfItsCop;
            }
        }

        //clear
        private ICommand clear;
        public ICommand Clear
        {
            get
            {
                if (clear == null)
                {
                    clear = new RelayCommand(execute =>
                    {
                        CurrentLicensePlate = null;
                        FilePath = null;
                        IsItCopStatement = null;
                        ImagePreview = null;
                    }, canExecute => FilePath != null);
                    //}, canExecute => CurrentLicensePlate != null && FilePath != null);
                }
                return clear;
            }
        }

        #endregion

        #region Methods

        public void LoadFileDialog()
        {

            IsItCopStatement = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
            if (openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
            }
            if (FilePath != null)
            {
                DetectAndRecognizeLetters(FilePath);
            }

        }

        public void DetectAndRecognizeLetters(string path)
        {

            Console.WriteLine(path);

            //podglad
            ImagePreview = new BitmapImage(new Uri(path));

            //
            //detekcja
            //
            Detection detection = new Detection(path);
            List<Bitmap> charBitmaps = detection.Letters;


            //
            //rozpoznanie znakow i dodawanie ich do stringa 
            //
            string charsFromPicture = null;
            charBitmaps.Reverse();
            foreach (var letter in charBitmaps)
            {
                charsFromPicture += recognition.Recognize(letter);
                Console.WriteLine(recognition.Recognize(letter));
            }

            //
            //konstruktor tablicy i wkladamy do niego stringa
            //
            CurrentLicensePlate = new LicensePlate(charsFromPicture);

        }
    }
    #endregion

}