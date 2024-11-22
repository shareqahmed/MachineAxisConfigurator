using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MachineAxisConfigurator.Models
{
    [XmlRoot(ElementName = "MachineSettings")]
    public class MachineSettings
    {
        public Machine Machine { get; set; }
        public ObservableCollection<Axis> Axes { get; set; } = new ObservableCollection<Axis>();
    }
}

