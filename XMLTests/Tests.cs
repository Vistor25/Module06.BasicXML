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
        public void CreateRepositoryWithoutPath_Exception(string path, Type exception)
        {
           Assert.Throws(exception, () => new XMLRepository(path));
        }

        [TestCase("test.xml", typeof(IOException))]
        public void GetStreamToRepository_RepositoryNotExsist_Exception(string repositoryName, Type exception)
        {
            XMLRepository repository = new XMLRepository(repositoryName);
            Assert.Throws(exception, () => repository.Load());
        }

        //Should be created repository
        [TestCase("C:/Users/Viktar_Varanko/Source/Repos/Module06.BasicXML/XMLTests/test.xml")]
        public void GetStreamToRepository_ReturnStreamToRepository(string path)
        {
            XMLRepository repository = new XMLRepository(path);
            Stream stream = repository.Load();
            Assert.AreNotEqual(stream, null);
        }

        //Should be created repository
        [TestCase("C:/Users/Viktar_Varanko/Source/Repos/Module06.BasicXML/XMLTests/test.xml", null, typeof(ArgumentNullException))]
        public void SaveDataInRepository_Exception(string path, Catalog catalog, Type exception)
        {
            XMLRepository repository = new XMLRepository(path);
            Assert.Throws(exception, () => repository.Save(catalog));
        }
        [TestCase(null, null, typeof(IOException))]
        public void CreatingService_RepositoryNotFound_Exception(string filePath, Catalog catalog, Type exception)
        {
            Assert.Throws(exception, () => new XMLService());
        }

        //Should be created repository
        [TestCase("C:/Users/Viktar_Varanko/Source/Repos/Module06.BasicXML/XMLTests/test.xml")]
        public void AddNewValueToRepository_SuccessAddedNewItem(string repositoryName)
        {
            XMLService service = new XMLService(repositoryName);
            bool operationResult = service.AddEntity(new Book() {
                Id = 365461,
                Title = "New Book",
                City = "Minsk",
                Annotation = "aaaaaaa",
                Authors = new List<string> { "Victor" },
                Publisher = "EPAM",
                Description = "Book" });
            Assert.AreEqual(operationResult, true);
        }

        //Should be created repository
        [TestCase("C:/Users/Viktar_Varanko/Source/Repos/Module06.BasicXML/XMLTests/test.xml", typeof(ArgumentNullException))]
        public void AddNewValueToRepository_FailedAddedNewItem(string repositoryName, Type exception)
        {
            XMLService service = new XMLService(repositoryName);
            Assert.Throws(exception, () => service.AddEntity(null));
        }
    }
}
