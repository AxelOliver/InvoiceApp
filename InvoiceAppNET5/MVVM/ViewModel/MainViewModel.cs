using InvoiceAppNET5.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAppNET5.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand InvoicesViewCommand { get; set; }
        public RelayCommand ClientsViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public InvoicesViewModel InvoicesVM { get; set; }
        public ClientViewModel ClientVM { get; set; }
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            InvoicesVM = new InvoicesViewModel();
            ClientVM = new ClientViewModel(this);

            CurrentView = HomeVM;

            HomeViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeVM;
            });
            InvoicesViewCommand = new RelayCommand(o =>
            {
                CurrentView = InvoicesVM;
            });
            ClientsViewCommand = new RelayCommand(o =>
            {
                CurrentView = ClientVM;
            });
        }

    }
}
