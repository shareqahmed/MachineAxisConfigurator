using MachineAxisConfigurator.Commands;
using MachineAxisConfigurator.Models;
using MachineAxisConfigurator.Services;
using MachineAxisConfigurator.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;

namespace MachineAxisConfigurator.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IFileService _fileService;
        public ICommand OpenAddWindowCommand { get; private set; }
        public ICommand OpenEditWindowCommand { get; private set; }
        public ICommand LoadCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteAxisCommand { get; private set; }

        //private FileService fileService = new FileService();
        private string XmlPath { get; set; }

        #region Constructor
        public MainWindowViewModel(IFileService fileService)
        {
            _fileService = fileService;
            LoadCommand = new RelayCommand(LoadMachineSettings);
            SaveCommand = new RelayCommand(SaveMachineSettings);
            OpenAddWindowCommand = new RelayCommand(ExecuteOpenAddWindow);
            OpenEditWindowCommand = new RelayCommand(EditAxis, CanEditAxis);
            DeleteAxisCommand = new RelayCommand(DeleteAxis, CanDeleteAxis);
        }
        #endregion Constructor


        private MachineSettings _machineSettings = new MachineSettings();
        public MachineSettings MachineSettings
        {
            get
            {
                return _machineSettings;
            }
            set
            {
                if (_machineSettings != value)
                {
                    _machineSettings = value;
                    OnPropertyChanged(nameof(MachineSettings));
                }
            }
        }

        private Axis _selectedAxis;
        public Axis SelectedAxis
        {
            get
            {
                return _selectedAxis;
            }
            set
            {
                if (_selectedAxis != value)
                {
                    _selectedAxis = value;
                    OnPropertyChanged(nameof(SelectedAxis));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region Load and Save Machine Settings
        private void LoadMachineSettings()
        {
            XmlPath = _fileService.GetFilePath();
            try
            {
                MachineSettings = _fileService.DeserializeXml<MachineSettings>(XmlPath);
                MessageBox.Show("File loaded successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveMachineSettings()
        {
            try
            {
                _fileService.SerializeXml(MachineSettings, XmlPath);
                MessageBox.Show("File saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion Load and Save Machine Settings


        #region Delete Axis
        private void DeleteAxis()
        {
            if (SelectedAxis != null && MachineSettings.Axes.Contains(SelectedAxis))
            {
                MachineSettings.Axes.Remove(SelectedAxis);
            }
        }

        private bool CanDeleteAxis()
        {
            return SelectedAxis != null;
        }

        #endregion Delete Axis


        #region Add Axis
        private void ExecuteOpenAddWindow()
        {
            AddWindowViewModel viewModel = new AddWindowViewModel();
            viewModel.OnAxisAdded += AddAxisToMachineSettings;
            AddWindow addWindow = new AddWindow
            {
                DataContext = viewModel
            }; 
            addWindow.Show();
        }

        public void AddAxisToMachineSettings(Axis newAxis)
        {
           
            if (MachineSettings.Axes.Any(axis => axis.Name.Equals(newAxis.Name, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("An axis with the same name already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MachineSettings.Axes.Add(newAxis);
                MessageBox.Show("New axis added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion Add Axis


        #region Edit Axis
        private void EditAxis()
        {
            if (SelectedAxis != null && MachineSettings.Axes.Contains(SelectedAxis))
            {
                ExecuteOpenEditWindow(SelectedAxis);
            }
        }

        private bool CanEditAxis()
        {
            return SelectedAxis != null;
        }

        private void ExecuteOpenEditWindow(Axis selectedAxis)
        {
            EditWindowViewModel viewModel = new EditWindowViewModel(selectedAxis);
            viewModel.OnAxisEdited += UpdateAxisToMachineSettings;
            EditWindow editWindow = new EditWindow
            {
                DataContext = viewModel
            };
            editWindow.Show();
        }

        public void UpdateAxisToMachineSettings(Axis updatedAxis)
        {
            var axis = MachineSettings.Axes.FirstOrDefault(a => a.Name == updatedAxis.Name);
            
            if (axis != null)
            {
                axis.Type = updatedAxis.Type;
                axis.MinValue = updatedAxis.MinValue;
                axis.MaxValue = updatedAxis.MaxValue;
                OnPropertyChanged(nameof(MachineSettings));
                MessageBox.Show("An axis has been edited.", "Error", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        #endregion Edit Axis 
    }
}
