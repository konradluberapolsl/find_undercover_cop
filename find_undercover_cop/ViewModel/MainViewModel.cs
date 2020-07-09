
using find_undercover_cop.Model;
using find_undercover_cop.Model.AI;

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
using Convert = find_undercover_cop.Model.Convert;

namespace find_undercover_cop.ViewModel
{
    class MainViewModel: ViewModelBase
    {
        #region Fields


        private LicensePlate currentLicensePlate;
        private string fullLicensePlate;

        private string filePath;
        private string isItCopStatement;
        private string locationName;

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

        public string LocationName
        {
            get => locationName;
            set
            {
                locationName = value;
                onPropertyChanged(nameof(locationName));
            }
        }


        private ImageSource imageIn;
        public ImageSource ImageIn
        {
            get => imageIn;
            set
            {
                imageIn = value;
                onPropertyChanged(nameof(imageIn));
            }
        }

        private ImageSource imageOut;
        public ImageSource ImageOut
        {
            get => imageOut;
            set
            {
                imageOut = value;
                onPropertyChanged(nameof(imageOut));
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
        private ICommand checkLicensePlate;
        public ICommand CheckLicensPlate
        {
            get
            {
                if (checkLicensePlate == null)
                {
                    checkLicensePlate = new RelayCommand(execute =>
                    {
                        if (CurrentLicensePlate.IsUndercoverCop)
                        {
                            //napraw bo nie pokazuje jakim autem jeździ gliniarz
                            IsItCopStatement = $"To gliniarz \n{CurrentLicensePlate.GetCopCar(CurrentLicensePlate.FullLicensePlate)}!";
                        }
                        else
                        {
                            IsItCopStatement = "Masz szczęście, to nie gliniarz!";
                        }

                        LocationName = $"Województow: {CurrentLicensePlate.LocationVoivodeship} \nPowiat: {CurrentLicensePlate.LocationFullName}";


                    }, canExecute => CurrentLicensePlate != null);
                }
                return checkLicensePlate;
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
                        LocationName = null;
                        ImageIn = null;
                        ImageOut = null;
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
            LocationName = "";
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
            ImageIn = new BitmapImage(new Uri(path));


            //
            //detekcja
            //
            try
            {
                Detection detection = new Detection(path);

                Bitmap image = detection.ConvertOutImgToBitmap();

                ImageOut = Convert.ToImageSource(image);

                Recognition recognition = new Recognition(image);

                CurrentLicensePlate = new LicensePlate(recognition.Text);

                Console.WriteLine(recognition.Text);
            }
            catch (Exception e)
            {

                MessageBox.Show(Resources.Resources.SomethingWentWrong);
            }


        }
    }
    #endregion

}