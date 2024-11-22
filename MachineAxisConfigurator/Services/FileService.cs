using MachineAxisConfigurator.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MachineAxisConfigurator.Services
{
    public class FileService : IFileService
    {
        private string GetFilePath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "XML Files (*.xml)|*.xml",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                return openFileDialog.FileName;
            }
            return null;
        }


        public MachineSettings LoadXml()
        {
            string path = GetFilePath();
            if (string.IsNullOrEmpty(path)) return null;

            XmlSerializer serializer = new XmlSerializer(typeof(MachineSettings));
            using (StreamReader reader = new StreamReader(path))
            {
                return (MachineSettings)serializer.Deserialize(reader);
            }
        }
    }
}
