using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TestApplicationSIBERS.Commands;

namespace TestApplicationSIBERS.ViewModels
{
    public abstract class WorkspaceViewModel : ViewModelBase
    {
        DelegateCommand _closeCommand;

        protected WorkspaceViewModel()
        {
        }

        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new DelegateCommand(OnRequestClose);

                return _closeCommand;
            }
        }

        public event EventHandler RequestClose;

        void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
