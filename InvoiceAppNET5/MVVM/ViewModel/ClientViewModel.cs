using InvoiceAppNET5.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAppNET5.MVVM.ViewModel
{
    class ClientViewModel : ObservableObject
    {
        public RelayCommand ViewInvoicesCommand { get; set; }
        private MainViewModel _mainViewModel;
        public ClientViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            ViewInvoicesCommand = new RelayCommand(o =>
            {
                _mainViewModel.CurrentView = _mainViewModel.InvoicesVM;
            });
        }
    }
}
