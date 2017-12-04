using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using XML;
using XML.Models;
using XML.Service;

namespace XMLTests
{
    [TestFixture]
    public class Tests
    {

        [TestCase(null, typeof(ArgumentException))]
        [TestCase("", typeof(ArgumentException))]
        public void CreateRepositoryWithEmptyPath(string path, Type exception)
        {
           Assert.Throws(exception, () => new XMLRepository(path));
        }

        [TestCase("aaaaa.xml", typeof(IOException))]
        public void NotExistingPathExeption(string repositoryName, Type exception)
        {
            XMLRepository repository = new XMLRepository(repositoryName);
            Assert.Throws(exception, () => repository.Load());
        }
        
        [TestCase("C:/Users/Viktar_Varanko/Source/Repos/Module06.BasicXML/XMLTests/test.xml", null, typeof(ArgumentNullException))]
        public void SaveDataExeption(string path, Catalog catalog, Type exception)
        {
            XMLRepository repository = new XMLRepository(path);
            Assert.Throws(exception, () => repository.Save(catalog));
        }
        [TestCase(null, null, typeof(IOException))]
        public void CreateServiceWithNullRepository(string filePath, Catalog catalog, Type exception)
        {
            Assert.Throws(exception, () => new XMLService());
        }

        [TestCase("C:/Users/Viktar_Varanko/Source/Repos/Module06.BasicXML/XMLTests/test.xml")]
        public void AddNewEntity(string repositoryName)
        {
            XMLService service = new XMLService(repositoryName);
            bool operationResult = service.AddEntity(new Book() {
                Id = 365461,
                Title = "New Book",
                City = "Minsk",
                Annotation = "aaaaddhdhaaa",
                Authors = new List<string> { "Victor" },
                Publisher = "EPAM",
                Description = "Book" });
            Assert.AreEqual(operationResult, true);
        }

        [TestCase("C:/Users/Viktar_Varanko/Source/Repos/Module06.BasicXML/XMLTests/test.xml", typeof(ArgumentNullException))]
        public void AddNewNullEntityExeption(string repositoryName, Type exception)
        {
            XMLService service = new XMLService(repositoryName);
            Assert.Throws(exception, () => service.AddEntity(null));
        }
    }
}
