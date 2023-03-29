
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class ImportCoachDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(2),MaxLength(40)]
        public string Name { get; set; } = null!;
        [Required]
        [XmlElement("Nationality")]
        public string Nationality { get; set; } = null!;
        [XmlArray("Footballers")]
        public FootballerImportDto[] Footballers { get; set; } 
    }

  //  <Coaches>
  //<Coach>
  //  <Name>Bruno Genesio</Name>
  //  <Nationality>France</Nationality>
  //  <Footballers>
  //    <Footballer>
  //      <Name>Benjamin Bourigeaud</Name>
  //      <ContractStartDate>22/03/2020</ContractStartDate>
  //      <ContractEndDate>24/02/2025</ContractEndDate>
  //      <BestSkillType>2</BestSkillType>
  //      <PositionType>2</PositionType>
  //    </Footballer>
  //    <Footballer>
  //      <Name>Martin Terrier</Name>
  //      <ContractStartDate>29/12/2021</ContractStartDate>
  //      <ContractEndDate>16/06/2024</ContractEndDate>
  //      <BestSkillType>2</BestSkillType>
  //      <PositionType>3</PositionType>
  //    </Footballer>
  //  </Footballers>
  //</Coach>
}
