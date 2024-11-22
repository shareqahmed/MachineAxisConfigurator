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

        public AddWindowViewModel()
        {
            SaveCommand = new RelayCommand(SaveAxis);
        }

        private void SaveAxis()
        {

            Axis newAxis = new Axis
            {
                Name = AxisName,
                MinValue = MinValue,
                MaxValue = MaxValue
            };

            // Invoke the event
            OnAxisAdded?.Invoke(newAxis);

        }


        private string _axisName;
        public string AxisName
        {
            get
            {
                return _axisName;
            }
            set
            {
                if (_axisName != value)
                {
                    _axisName = value;
                    OnPropertyChanged(nameof(AxisName));
                }
            }
        }

        private float _minValue;
        public float MinValue
        {
            get
            {
                return _minValue;
            }
            set
            {
                if (_minValue != value)
                {
                    _minValue = value;
                    OnPropertyChanged(nameof(MinValue));
                }
            }
        }

        private float _maxValue;
        public float MaxValue
        {
            get
            {
                return _maxValue;
            }
            set
            {
                if (_maxValue != value)
                {
                    _maxValue = value;
                    OnPropertyChanged(nameof(MaxValue));
                }
            }
        }
    }
}
