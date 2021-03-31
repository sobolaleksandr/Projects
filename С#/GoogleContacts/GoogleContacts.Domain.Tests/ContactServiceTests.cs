using System;
using System.Collections.Generic;
using GoogleContacts.Domain;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using Xunit.Sdk;
using Xunit.Abstractions;

namespace GoogleContacts.Domain.Tests
{
    public class ContactServiceTests
    {
        private const string PERSON_FIELDS = "names,emailAddresses,phoneNumbers,organizations,memberships";
        private const string NAME = "John";
        private const string FAMILYNAME = "Doe";
        private const string EMAIL = "JohnD@yahoo.com";
        private const string PHONE_NUMBER = "+7800553535";
        private const string PROPERTIES = "names,emailAddresses";
        private const string CLIENT_SECRET = "uavwQnDWY6bUEFf75pXtP0m6";
        private const string CLIENT_ID = "217336154173-tdce9e8b3c9hjfsd9abnfb7q0ef4q9ab.apps.googleusercontent.com";

        //todo modify tests
        //[Fact]
        //public async Task CRUD_Group()
        //{
        //    groupService1.Initialize();

        //    await GetAll_Initialized_ReturnsGroups();

        //    await Create_WithGroup_ReturnsGroup();

        //    await Update_WithGroup_ReturnsGroup();

        //    await Delete_WithGroup_ReturnsTrue();
        //}

        //internal static async Task GetAll_Initialized_ReturnsGroups()
        //{
        //    List<GroupModel> groups = await groupService1.GetAll();

        //    Assert.Equal(10, groups.Count);
        //}

        //internal static async Task Create_WithGroup_ReturnsGroup()
        //{
        //    GroupModel groupModel = new(GROUP_NAME);

        //    var createResult = await groupService1.Create(groupModel);

        //    Assert.Equal(GROUP_NAME, createResult.modelFormattedName);
        //}

        //internal static async Task Update_WithGroup_ReturnsGroup()
        //{
        //    var groupModel = (await groupService1.GetAll())
        //        .Where(g => g.modelFormattedName == GROUP_NAME)
        //        .SingleOrDefault();
        //    groupModel.modelFormattedName = CHANGED_GROUP_NAME;

        //    var result = await groupService1.Update(groupModel);

        //    Assert.Equal(groupModel.modelFormattedName, result.modelFormattedName);
        //}
        ////todo getContacts to add in group
        //internal static async Task Modify_WithGroup_ReturnsTrue()
        //{
        //    var groupModel = (await groupService1.GetAll())
        //                    .Where(g => g.modelFormattedName == CHANGED_GROUP_NAME)
        //                    .SingleOrDefault();
        //    groupModel.modelFormattedName = CHANGED_GROUP_NAME;

        //    var result = await groupService1.Update(groupModel);

        //    Assert.Equal(groupModel.modelFormattedName, result.modelFormattedName);
        //}

        //internal static async Task Delete_WithGroup_ReturnsTrue()
        //{
        //    var groupModel = (await groupService1.GetAll())
        //        .Where(g => g.modelFormattedName == CHANGED_GROUP_NAME).SingleOrDefault();

        //    bool deleteResult = await groupService1.Delete(groupModel);

        //    Assert.True(deleteResult);
        //}

        [Fact]
        public async Task GetAll_WithApiError_ReturnsEmptyList()
        {
            var service = new ContactService("","");

            List<PersonModel> result = await service.GetAll(PERSON_FIELDS);

            Assert.Equal(new(), result);
        }

        [Fact]
        public async Task CreateAndDelete_WithGoodModel_ReturnsPerson()
        {
            var service = new ContactService(CLIENT_SECRET, CLIENT_ID);
            var person = new PersonModel(NAME, FAMILYNAME, EMAIL, PHONE_NUMBER);

            PersonModel createResult = await service.Create(person);

            Assert.Equal(NAME, createResult.modelGivenName);
            Assert.Equal(FAMILYNAME, createResult.modelFamilyName);
            Assert.Equal(EMAIL, createResult.modelEmail);
            Assert.Equal(PHONE_NUMBER, createResult.modelPhoneNumber);

            person.modelResourceName = createResult.modelResourceName;

            List<PersonModel> result = await service.GetAll(PERSON_FIELDS);

            Assert.Single(result);
            Assert.Equal(person.modelResourceName, result[0].modelResourceName);

            bool deleteResult = await service.TryToDeleteContact(person);

            Assert.True(deleteResult);
        }

        [Fact]
        public async Task Create_WithNull_ReturnsNull()
        {
            var service = new ContactService(CLIENT_SECRET,CLIENT_ID);
            PersonModel personModel = null;

            PersonModel result = await service.Create(personModel);

            Assert.Null(result);
        }

        //[Fact]
        //public async Task Create_NotInitialized_ReturnsNull()
        //{
        //    var service = new GroupService();
        //    GroupModel groupModel = new("TestName");

        //    GroupModel group = await service.Create(groupModel);

        //    Assert.Null(group);
        //}

        //[Fact]
        //public async Task Update_WithNull_ReturnsNull()
        //{
        //    var service = new GroupService();
        //    service.Initialize();
        //    GroupModel groupModel = null;

        //    GroupModel group = await service.Update(groupModel);

        //    Assert.Null(group);
        //}

        //[Fact]
        //public async Task Update_NotInitialized_ReturnsNull()
        //{
        //    var service = new GroupService();
        //    GroupModel groupModel = new("TestName");

        //    GroupModel group = await service.Update(groupModel);

        //    Assert.Null(group);
        //}

        //[Fact]
        //public async Task Delete_NotInitialized_ReturnsNull()
        //{
        //    var service = new GroupService();
        //    GroupModel groupModel = new("TestName");

        //    bool result = await service.Delete(groupModel);

        //    Assert.False(result);
        //}

        //[Fact]
        //public async Task Modify_NotInitialized_ReturnsNull()
        //{
        //    var service = new GroupService();
        //    GroupModel groupModel = new("TestName");

        //    var result = await service.Modify(groupModel, new());

        //    Assert.Null(result);
        //}
    }
}
