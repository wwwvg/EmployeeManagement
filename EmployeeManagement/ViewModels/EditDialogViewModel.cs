using CommunityToolkit.Mvvm.ComponentModel;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeManagement.ViewModels
{
	public class EditDialogViewModel : ObservableObject, IDialogAware
    {
        public EditDialogViewModel()
        {

        }

        public DialogCloseListener RequestClose => throw new NotImplementedException();

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
