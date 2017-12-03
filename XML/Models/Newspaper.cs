using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace XML.Models
{
    public class Newspaper : IEntity
    {
        [XmlAttribute("ISSN")]
        public int Id { get; set; }

        [XmlElement(IsNullable = false)]
        public string Title { get; set; }

        [XmlElement]
        public string City { get; set; }

        [XmlElement]
        public int Year
        {
            get
            {
                return this.Year;
            }
            set
            {
                if (new DateTime().Year > value)
                {
                    this.Year = value;
                }
            }
        }

        [XmlElement]
        public int NumberOfPages { get; set; }

        [XmlElement]
        public string Annotation { get; set; }

        [XmlElement(IsNullable = false)]
        public int Issue { get; set; }

        [XmlElement]
        public DateTime Date { get; set; }

        [XmlAttribute]
        public string Description { get; set; }
    }
}
