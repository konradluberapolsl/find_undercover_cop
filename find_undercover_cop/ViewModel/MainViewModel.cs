using find_undercover_cop.Model;
using find_undercover_cop.ViewModel.BaseClass;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace find_undercover_cop.ViewModel
{
    class MainViewModel: ViewModelBase
    {
        #region Fields

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
                            //temporary solution 
                            //with get it from txt 
                            //until we will have AI which get it from JPG

                            IsItCopStatement = "";
                            OpenFileDialog openFileDialog = new OpenFileDialog();
                            //openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                            openFileDialog.Filter = "Text files (*.txt) | *.txt;";
                            if (openFileDialog.ShowDialog() == true)
                            {
                                if (Path.GetExtension(openFileDialog.FileName) == ".txt")
                                {
                                    string[] textFromFile = File.ReadAllLines(openFileDialog.FileName);
                                    CurrentLicensePlate = new LicensePlate(textFromFile[0]);
                                    FilePath = openFileDialog.FileName;
                                }
                            }
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
                            IsItCopStatement = $"To gliniarz {CurrentLicensePlate.GetCopCar(FullLicensePlate)}";
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
                    }, canExecute => CurrentLicensePlate != null && FilePath != null);
                }
                return clear;
            }
        }

        #endregion
    }
}
