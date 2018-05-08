using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using UI.Services;

namespace UI.ViewModels
{
    class DeleteTransactionViewModel : ViewModelBase
    {
        public int deletableItemID;
        public DelegateCommand DeleteCommand { get; }

        public DeleteTransactionViewModel()
        {
            DeleteCommand = new DelegateCommand(DeleteAsync);
        }
        private async void DeleteAsync()
        {
            var service = new TransactionManager();
            bool result = await service.DeleteTransactionConfirmedAsync(deletableItemID);
        }
    }
}
