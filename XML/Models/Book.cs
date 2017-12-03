using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace XML.Models
{
    [XmlRoot]
    public class Book : IEntity
    {
        [XmlAttribute("ISBN")]
        public int Id { get; set; }

        [XmlElement(IsNullable = false)]
        public string Title { get; set; }

        [XmlArray("Authors")]
        [XmlArrayItem("Author", typeof(string))]
        public List<string> Authors { get; set; }

        [XmlElement]
        public string City { get; set; }

        [XmlElement]
        public string Publisher { get; set; }

        [XmlElement(IsNullable = false)]
        public int NumberOfPages { get; set; }

        [XmlElement]
        public string Annotation { get; set; }

        [XmlAttribute]
        public string Description { get; set; }
    }
}
