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
    public class AddWindowViewModel : BaseViewModel
    {
        public event Action<Axis> OnAxisAdded;
        public ICommand SaveCommand { get; private set; }
        public Axis AxisToAdd { get; private set; } = new Axis();
        public AddWindowViewModel()
        {
            SaveCommand = new RelayCommand(SaveAxis);
        }

        private void SaveAxis()
        {
            Axis newAxis = AxisToAdd;
            OnAxisAdded?.Invoke(newAxis);         
        }

    }
}
