using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace XML.Models
{
    [XmlRoot(IsNullable = false)]
    public class Catalog
    {
        [XmlAttribute]
        public string Library { get; set; }

        [XmlAttribute]
        public DateTime CreationDate { get; set; }

        [XmlElement("Book")]
        public List<Book> Books { get; set; }

        [XmlElement("Magazin")]
        public List<Newspaper> Newspapers { get; set; }

        [XmlElement("Patent")]
        public List<Patent> Patents { get; set; }
    }
}
