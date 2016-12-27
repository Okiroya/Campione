using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Okiroya.Campione.SystemUtility.DI;
using Okiroya.Campione.Tests.Internal;
using System;

namespace Okiroya.Campione.Tests.DI
{
    [TestClass]
    public class RegisterDependencyContainerTests
    {
        [TestMethod]
        public void Resolve_After_Register()
        {
            //Arrange
            var mockService = new Mock<IDependencyContainerTestInterface>();
            mockService.Setup(p => p.Name).Returns("mock1");

            RegisterDependencyContainer<IDependencyContainerTestInterface>.Register("test1", mockService.Object);

            //Act
            var result = RegisterDependencyContainer<IDependencyContainerTestInterface>.Resolve("test1");

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("mock1", result.Name);
        }

        [TestMethod]
        public void IsRegistered_After_Register()
        {
            //Arrange
            var mockService = new Mock<IDependencyContainerTestInterface>();
            mockService.Setup(p => p.Name).Returns("mock2");

            RegisterDependencyContainer<IDependencyContainerTestInterface>.Register("test2", mockService.Object);

            //Act
            var result = RegisterDependencyContainer<IDependencyContainerTestInterface>.IsRegistered("test2");

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(RegisterDependencyException))]
        public void Resolve_Exception_No_Defaults()
        {
            RegisterDependencyContainer<IDependencyContainerTestInterface>.Resolve("test3");
        }

        [TestMethod]
        public void Resolve_After_Set_Default()
        {
            //Arrange
            var mockService = new Mock<IDependencyContainerTestInterface>();
            mockService.Setup(p => p.Name).Returns("mock3");

            RegisterDependencyContainer<IDependencyContainerTestInterface>.SetDefault(mockService.Object);

            //Act
            var result = RegisterDependencyContainer<IDependencyContainerTestInterface>.Resolve("test4");

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("mock3", result.Name);
        }

        [TestMethod]
        public void Resolve_After_Set_Defaults_Without_Name()
        {
            //Arrange
            var mockService = new Mock<IDependencyContainerTestInterface>();
            mockService.Setup(p => p.Name).Returns("mock4");

            RegisterDependencyContainer<IDependencyContainerTestInterface>.SetDefault(mockService.Object);

            //Act
            var result = RegisterDependencyContainer<IDependencyContainerTestInterface>.Resolve();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("mock4", result.Name);
        }

        //[TestMethod]
        //public void Resolve_After_Set_Configuration()
        //{
        //    //Arrange
        //    var configurationCollection = new RegisterDependencyContainerConfigurationCollection();

        //    configurationCollection.Add(
        //        new RegisterDependencyContainerConfigurationElement
        //        {
        //            Name = "Test1",
        //            DependencyType = "IDependencyContainerTestInterface",
        //            ServiceType = "Okiroya.Campione.Tests.DI.TestDependencyContainerTestService, Okiroya.Campione.Tests",
        //            Enabled = true
        //        });

        //    RegisterDependencyContainerConfigurationManager.Config = new RegisterDependencyContainerConfigurationHandler()
        //    {
        //        Dependencies = configurationCollection
        //    };

        //    //Act
        //    var result = RegisterDependencyContainer<IDependencyContainerTestInterface>.Resolve();

        //    //Assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void Resolve_After_Set_Configuration_For_Typed_Service()
        //{
        //    Resolve_After_Set_Configuration();

        //    //Arrange
        //    var configurationCollection = new RegisterDependencyContainerConfigurationCollection();

        //    configurationCollection.Add(
        //        new RegisterDependencyContainerConfigurationElement
        //        {
        //            Name = "Test2",
        //            DependencyType = "IDependencyTestInterface`1",
        //            ServiceType = "Okiroya.Campione.Tests.DI.DependencyTestService`1, Okiroya.Campione.Tests",
        //            Enabled = true
        //        });

        //    RegisterDependencyContainerConfigurationManager.Config = new RegisterDependencyContainerConfigurationHandler()
        //    {
        //        Dependencies = configurationCollection
        //    };

        //    //Act
        //    var result = RegisterDependencyContainer<IDependencyTestInterface<int>>.Resolve();

        //    //Assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void Resolve_After_Set_Configuration_For_Predefined_Service()
        //{
        //    //Arrange
        //    var configurationCollection = new RegisterDependencyContainerConfigurationCollection();

        //    configurationCollection.Add(
        //        new RegisterDependencyContainerConfigurationElement
        //        {
        //            Name = "Test3",
        //            DependencyType = "Okiroya.Campione.Tests.DI.IDependencyTestInterface`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]",
        //            ServiceType = "Okiroya.Campione.Tests.DI.PredefinedDependencyTestService, Okiroya.Campione.Tests",
        //            Enabled = true
        //        });

        //    RegisterDependencyContainerConfigurationManager.Config = new RegisterDependencyContainerConfigurationHandler()
        //    {
        //        Dependencies = configurationCollection
        //    };

        //    //Act
        //    var result = RegisterDependencyContainer<IDependencyTestInterface<int>>.Resolve();

        //    //Assert
        //    Assert.IsNotNull(result);
        //}

        [TestMethod]
        public void Resolve_After_Register_Type_For_Type()
        {
            //Arrange
            RegisterDependencyContainer<IDependencyTestInterface<int>>.RegisterFor(typeof(IDependencyTestInterface<>), typeof(DependencyTestService<>));

            //Act
            var result = RegisterDependencyContainer<IDependencyTestInterface<int>>.Resolve();

            //Assert
            Assert.IsNotNull(result);
        }
    }
}
