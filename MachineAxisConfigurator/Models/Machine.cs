using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MachineAxisConfigurator.Models
{
    public class Machine
    {
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "Type")]
        public string Type { get; set; }
    }
}
