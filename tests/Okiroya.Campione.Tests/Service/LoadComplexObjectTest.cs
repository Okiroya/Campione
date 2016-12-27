using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Okiroya.Campione.DataAccess;
using Okiroya.Campione.Service;
using Okiroya.Campione.SystemUtility.DI;
using Okiroya.Campione.Tests.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Okiroya.Campione.Tests.Service
{
    [TestClass]
    public class LoadComplexObjectTest
    {
        [TestMethod]
        public void Select_Dynamic_Type_From_Service()
        {
            //Arrange
            string testName = "some some";
            int testId = 1;

            string commandName = "command1";

            RegisterDependencyContainer<IDataService>.Reset();

            var mockDataService = new Mock<IDataService>();

            mockDataService
                .Setup(
                    p => p.ExecuteQuery(
                        It.Is<string>(q => q.Equals(commandName, StringComparison.OrdinalIgnoreCase)),
                        null))
                .Returns(
                    () =>
                    {
                        var dataItems = new List<DataItem>();

                        var metas = new List<Tuple<string, Type, object>>();
                        metas.Add(Tuple.Create<string, Type, object>("Id", typeof(int), testId));
                        metas.Add(Tuple.Create<string, Type, object>("Name", typeof(string), testName));
                        dataItems.Add(new DataItem { Items = metas.Select(triple => new DataItemElement(triple)) });

                        return new DataQueryResult(dataItems.AsEnumerable(), new Dictionary<string, object>(), new DataServiceStatistics());
                    }
                );

            RegisterDependencyContainer<IDataService>.Register(commandName, mockDataService.Object);

            //Act
            dynamic person = EntityServiceFacade.GetItem(commandName);

            //Assert
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.EntityTypeSysName);
            Assert.AreEqual(testId, person.Id);
            Assert.AreEqual(testName, person.Name);
        }

        [TestMethod]
        public void Select_Known_Type_From_Service()
        {
            //Arrange
            string testName = "some some";
            int testId = 1;
            long testForeignId = 2L;

            string commandName = "command2";

            RegisterDependencyContainer<IDataService>.Reset();

            var mockDataService = new Mock<IDataService>();

            mockDataService
                .Setup(
                    p => p.ExecuteQuery(
                        It.Is<string>(q => q.Equals(commandName, StringComparison.OrdinalIgnoreCase)),
                        null))
                .Returns(
                    () =>
                    {
                        var dataItems = new List<DataItem>();

                        var metas = new List<Tuple<string, Type, object>>();
                        metas.Add(Tuple.Create<string, Type, object>("Id", typeof(int), testId));
                        metas.Add(Tuple.Create<string, Type, object>("Name", typeof(string), testName));
                        metas.Add(Tuple.Create<string, Type, object>("SomeForeignId", typeof(long), testForeignId));
                        dataItems.Add(new DataItem { Items = metas.Select(triple => new DataItemElement(triple)) });

                        return new DataQueryResult(dataItems.AsEnumerable(), new Dictionary<string, object>(), new DataServiceStatistics());
                    }
                );

            RegisterDependencyContainer<IDataService>.Register(commandName, mockDataService.Object);

            //Act
            var person = EntityServiceFacade<ComplexTestObject, int>.GetItem(commandName);

            //Assert
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.EntityTypeSysName);
            Assert.AreEqual(testId, person.Id);
            Assert.AreEqual(testName, person.Name);
            Assert.AreEqual(testForeignId, person.SomeForeignId);
        }

        [TestMethod]
        public void Select_Known_Type_From_Service_With_Mapper()
        {
            //Arrange
            string testName = "some some";
            int testId = 1;
            long testForeignId = 2L;

            string commandName = "command3";

            RegisterDependencyContainer<IDataService>.Reset();

            var mockDataService = new Mock<IDataService>();

            mockDataService
                .Setup(
                    p => p.ExecuteQuery(
                        It.Is<string>(q => q.Equals(commandName, StringComparison.OrdinalIgnoreCase)),
                        null))
                .Returns(
                    () =>
                    {
                        var dataItems = new List<DataItem>();

                        var metas = new List<Tuple<string, Type, object>>();
                        metas.Add(Tuple.Create<string, Type, object>("AppId", typeof(int), testId));
                        metas.Add(Tuple.Create<string, Type, object>("Name", typeof(string), testName));
                        metas.Add(Tuple.Create<string, Type, object>("SomeForeignId", typeof(long), testForeignId));
                        dataItems.Add(new DataItem { Items = metas.Select(triple => new DataItemElement(triple)) });

                        return new DataQueryResult(dataItems.AsEnumerable(), new Dictionary<string, object>(), new DataServiceStatistics());
                    }
                );
            
            RegisterDependencyContainer<IDataService>.Register(commandName, mockDataService.Object);

            var mockDataServiceMapper = new Mock<IDataServiceMapper>();

            mockDataServiceMapper.Setup(p => p.MapTo(It.IsAny<string>()))
                .Returns(
                    (string p) =>
                    {
                        return p.Equals("Id", StringComparison.OrdinalIgnoreCase) ?
                            "AppId" :
                            p;
                    });

            RegisterDependencyContainer<IDataServiceMapper>.RegisterFor(typeof(ComplexTestObject), mockDataServiceMapper.Object);

            //Act
            var person = EntityServiceFacade<ComplexTestObject, int>.GetItem(commandName);

            //Assert
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.EntityTypeSysName);
            Assert.AreEqual(testId, person.Id);
            Assert.AreEqual(testName, person.Name);
            Assert.AreEqual(testForeignId, person.SomeForeignId);
        }
    }
}
