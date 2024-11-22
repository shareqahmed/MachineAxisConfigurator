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
using System.Windows.Input;
using System.Xml.Serialization;

namespace MachineAxisConfigurator.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand OpenAddWindowCommand { get; private set; }
        public ICommand LoadCommand { get; }
        public ICommand SaveCommand { get; }

        public ICommand DeleteAxisCommand { get; private set; }

        private string XmlPath { get; set; }

        public MainWindowViewModel()
        {
            OpenAddWindowCommand = new RelayCommand(ExecuteOpenAddWindow);
            LoadCommand = new RelayCommand(LoadXml);
            SaveCommand = new RelayCommand(SaveXml);

            DeleteAxisCommand = new RelayCommand(DeleteAxis, CanDeleteAxis);
        }


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
        private void LoadXml()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            bool? result = openFileDialog.ShowDialog();


            if (result == true)
            {
                XmlPath = openFileDialog.FileName;

                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(MachineSettings));
                    using (StreamReader reader = new StreamReader(XmlPath))
                    {
                        MachineSettings = (MachineSettings)serializer.Deserialize(reader);
                    }
                    MessageBox.Show("File loaded successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }

            }
        }


        private void SaveXml()
        {

            if (string.IsNullOrEmpty(XmlPath))
            {
                return;
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MachineSettings));
                using (StreamWriter writer = new StreamWriter(XmlPath))
                {
                    serializer.Serialize(writer, MachineSettings);
                }
                MessageBox.Show("File saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

















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


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
