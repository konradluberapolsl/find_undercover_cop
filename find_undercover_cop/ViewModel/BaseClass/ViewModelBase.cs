using System.ComponentModel;

namespace find_undercover_cop.ViewModel.BaseClass
{
    class ViewModelBase: INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void onPropertyChanged(params string[] namesOfProperties)
        {
            if (PropertyChanged != null)
            {

                foreach (var prop in namesOfProperties)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
                }
            }
        }
    }
}