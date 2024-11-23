using MachineAxisConfigurator.Commands;
using MachineAxisConfigurator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MachineAxisConfigurator.ViewModels
{
    public class EditWindowViewModel : BaseViewModel
    {
        public Axis EditableAxis { get; private set; }
        public ICommand SaveCommand { get; private set; }

        public event Action<Axis> OnAxisEdited;

        public EditWindowViewModel(Axis axis)
        {
            EditableAxis = axis ?? throw new ArgumentNullException(nameof(axis));
            SaveCommand = new RelayCommand(EditAxis);
        }


        private void EditAxis()
        {
            OnAxisEdited?.Invoke(EditableAxis);
        }

    }
}
