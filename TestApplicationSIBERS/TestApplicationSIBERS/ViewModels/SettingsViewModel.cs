using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TestApplicationSIBERS.Commands;
using Dal.Nhibernate;

namespace TestApplicationSIBERS.ViewModels
{
    public class SettingsViewModel : WorkspaceViewModel
    {
        private ICommand _createDB;
        public ICommand CreateDB
        {
            get
            {
                if (_createDB == null)
                {
                    _createDB = new DelegateCommand(CreateDataBase);
                }
                return _createDB;
            }
        }

        private void CreateDataBase()
        {
            NhHelper.Instance.CreateDB();
        }


        public string DisplayName
        {
            get { return "Настройки программы"; }
        }
    }
}
