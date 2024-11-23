using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MachineAxisConfigurator;
using MachineAxisConfigurator.Models;

namespace MachineAxisConfigurator.UnitTests
{
    [TestFixture]
    public class MachineSettingsReadWriteTests
    {

        #region Reading Machine Settings
        [Test]
        public void Deserialize_ValidXml_ReturnsCorrectMachineSettings()

        {
            // Arrange
            string xmlInput = @"<?xml version='1.0' encoding='utf-8'?>
                                <MachineSettings>
                                    <Machine Name='3D Axis Mill Works' Type='Mill' />
                                    <Axes>
                                        <Axis Name='X' Type='Translation' MinValue='-100' MaxValue='100' />
                                        <Axis Name='Y' Type='Translation' MinValue='-100' MaxValue='100' />
                                    </Axes>
                                </MachineSettings>";

            var serializer = new XmlSerializer(typeof(MachineSettings));
            
            // Act
            MachineSettings result;
            using (TextReader reader = new StringReader(xmlInput))
            {
                result = (MachineSettings)serializer.Deserialize(reader);
            }

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("3D Axis Mill Works", result.Machine.Name);
            Assert.AreEqual(2, result.Axes.Count);
        }



        [Test]
        public void Deserialize_MalformedXml_ThrowsException()
        {
            // Arrange
            string malformedXml = @"<MachineSettings>
                                    <Machine Name='3D Axis Mill Works' Type='Mill'>
                                    <Axes>
                                        <Axis Name='X' Type='Translation' MinValue='-100' MaxValue='100' />
                                    </Axes>
                                </MachineSettings>";

            var serializer = new XmlSerializer(typeof(MachineSettings));

            // Act 
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                using (TextReader reader = new StringReader(malformedXml))
                {
                    var result = (MachineSettings)serializer.Deserialize(reader);
                }
            });

            // Assert
            Assert.That(ex.Message, Does.Contain("There is an error in XML document")); // Check if the error message contains specific details about the malformed part
        }



        [Test]
        public void Deserialize_CorruptXml_WithStringMinValue_ThrowsException()
        {
            // Arrange
            string corruptXml = @"<MachineSettings>
                                    <Machine Name='3D Axis Mill Works' Type='Mill'>
                                     <Axes>
                                        <Axis Name='X' Type='Translation' MinValue='abc' MaxValue='100' /> <!-- Non-numeric MinValue -->
                                     </Axes>
                                   </MachineSettings>";

            var serializer = new XmlSerializer(typeof(MachineSettings));

            // Act
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                using (var reader = new StringReader(corruptXml))
                {
                    var result = (MachineSettings)serializer.Deserialize(reader);
                }
            });

            // Assert
            Assert.That(ex.Message, Does.Contain("There is an error in XML document"), "Expected specific exception message about XML error.");
        }

        #endregion Reading Machine Settings


    }
}

