using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MachineAxisConfigurator;
using MachineAxisConfigurator.Models;
using MachineAxisConfigurator.Services;

namespace MachineAxisConfigurator.UnitTests
{
    [TestFixture]
    public class MachineSettingsReadWriteTests
    {

        [Test]
        public void ReadWrite_MachineSettingsFile_ShouldPreserveData()
        {
            // Arrange
            string fileName = "valid_machine_settings_to_modify.xml";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MachineSettingsFiles", fileName);
            var fileService = new FileService();

            // Act
            var originalSettings = fileService.DeserializeXml<MachineSettings>(filePath);
            var newAxis = new Axis { Name = "Z", Type = "Translation", MinValue = (float)-10.1, MaxValue = (float)10.1 };
            originalSettings.Axes.Add(newAxis);

            // Act
            fileService.SerializeXml(originalSettings, filePath);
            var deserializedSettings = fileService.DeserializeXml<MachineSettings>(filePath);

            // Assert
            Assert.IsNotNull(deserializedSettings);
            Assert.AreEqual("3axisMill", deserializedSettings.Machine.Name);
            Assert.AreEqual(4, deserializedSettings.Axes.Count);
            Assert.AreEqual("Z", deserializedSettings.Axes[3].Name);
            Assert.AreEqual((float)-10.1, deserializedSettings.Axes[3].MinValue);
            Assert.AreEqual((float)10.1, deserializedSettings.Axes[3].MaxValue);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }


        [Test]
        public void Deserialize_InvalidFilePath_ThrowsFileNotFoundException()
        {
            // Arrange
            string fileName = "valid_machine_settings.xml";
            string invalidFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MachineSettings", fileName);        // faulty path
            var fileService = new FileService();

            // Act
            var ex = Assert.Throws<FileNotFoundException>(() =>
            {
                MachineSettings result = fileService.DeserializeXml<MachineSettings>(invalidFilePath);
            });

            // Assert
            Assert.That(ex.Message, Does.Contain("The specified file path does not exist."));  
        }




        [Test]
        public void Deserialize_CorruptXml_WithStringMinValue_ThrowsException()
        {
            // Arrange
            string fileName = "machine_settings_axis_minvalue_string_error.xml";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MachineSettingsFiles", fileName);
            var fileService = new FileService();

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                MachineSettings result = fileService.DeserializeXml<MachineSettings>(filePath);
            });

            // Assert
            Assert.That(ex.Message, Does.Contain("There is an error in XML document"));
        }
    }
}

