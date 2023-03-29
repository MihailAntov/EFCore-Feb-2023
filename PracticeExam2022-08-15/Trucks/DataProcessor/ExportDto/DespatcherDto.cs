using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Trucks.DataProcessor.ExportDto
{
    [XmlType("Despatcher")]
    public class DespatcherDto
    {
        [XmlAttribute("TrucksCount")]
        public int TrucksCount { get; set; }


        [XmlElement("DespatcherName")]
        public string DespatcherName { get; set; } = null!;

        [XmlArray("Trucks")]
        public TruckDto[] Trucks { get; set; } = null!;
    }
}
