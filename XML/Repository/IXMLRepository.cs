using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XML.Models;

namespace XML
{
    public interface IXMLRepository
    {
        void Save(Catalog catalog);
        FileStream Load();
    }
}
