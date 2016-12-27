using Microsoft.VisualStudio.TestTools.UnitTesting;
using Okiroya.Campione.DataAccess;
using Okiroya.Campione.DataAccess.EntityFrameworkCore;
using Okiroya.Campione.Service;
using Okiroya.Campione.SystemUtility.DI;
using Okiroya.Campione.Tests.Internal.DataAccess;
using System;
using System.Collections.Generic;

namespace Okiroya.Campione.Tests.DataAccess
{
    [TestClass]
    public class SaveSelectTests
    {
        [TestMethod]
        public void Add_Than_Select()
        {
            //Assign
            RegisterDependencyContainer<IDataService>.SetDefault(
                new TestInMemoryEntityFrameworkDataService(
                    new TestInMemoryDbContext(
                        "test",
                        (options) => { })));

            RegisterDependencyContainer<IDbContextCommand<TestInMemoryDbContext>>.Register(InsertDataServiceCommand.Name, new InsertDataServiceCommand());
            RegisterDependencyContainer<IDbContextCommand<TestInMemoryDbContext>>.Register(SelectDataServiceCommand.Name, new SelectDataServiceCommand());

            //Act

            EntityServiceFacade<TestEntity, int>.ExecuteCommand(
                InsertDataServiceCommand.Name,
                new Dictionary<string, object>().FluentIt(
                    p =>
                    {
                        p.Add("Id", 1);
                        p.Add("Name", "First");
                    }));

            var result = EntityServiceFacade<TestEntity, int>.GetItem(
                SelectDataServiceCommand.Name,
                new Dictionary<string, object>().FluentIt(
                    p =>
                    {
                        p.Add("Name", "First");
                    }));

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("First", result.Name);
        }
    }
}
