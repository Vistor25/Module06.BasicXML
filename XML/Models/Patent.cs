using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace XML.Models
{
    public class Patent:IEntity
    {
        [XmlElement(IsNullable = false)]
        public string Title { get; set; }

        [XmlElement]
        public List<string> Creators { get; set; }

        [XmlElement]
        public string Country { get; set; }

        [XmlElement(IsNullable = false)]
        public int RegistrationNumber { get; set; }

        [XmlElement]
        public DateTime DateOfRequest { get; set; }

        [XmlElement]
        public DateTime PublicationDate { get; set; }

        [XmlElement(IsNullable = false)]
        public int NumberOfPages { get; set; }

        [XmlElement]
        public string Annotation { get; set; }

        [XmlAttribute]
        public string Description { get; set; }
    }
}
