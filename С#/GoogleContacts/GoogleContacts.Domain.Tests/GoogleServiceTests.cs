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
    public class GoogleServiceTests
    {
        private const string GROUP_NAME = "TestName";
        const string CHANGED_GROUP_NAME = "ChangedGroupName";

        public async Task CRUD_Group()
        {
            GroupService.Initialize();

            await GetGroups_Initialized_ReturnsGroups();

            await CreateGroup_WithGroup_ReturnsGroup();

            await UpdateGroup_WithGroup_ReturnsGroup();

            await DeleteGroup_WithGroup_ReturnsTrue();
        }

        internal static async Task GetGroups_Initialized_ReturnsGroups()
        {
            List<GroupModel> groups = await GroupService.GetGroups();

            Assert.Equal(10, groups.Count);
        }

        internal static async Task CreateGroup_WithGroup_ReturnsGroup()
        {
            GroupModel groupModel = new(GROUP_NAME);

            var createResult = await GroupService.CreateGroup(groupModel);

            Assert.Equal(GROUP_NAME, createResult.modelFormattedName);
        }

        internal static async Task UpdateGroup_WithGroup_ReturnsGroup()
        {
            var groupModel = (await GroupService.GetGroups())
                .Where(g => g.modelFormattedName == GROUP_NAME).SingleOrDefault();
            groupModel.modelFormattedName = CHANGED_GROUP_NAME;

            groupModel = await GroupService.UpdateGroup(groupModel);

            Assert.Equal(CHANGED_GROUP_NAME, groupModel.modelFormattedName);
        }

        internal static async Task DeleteGroup_WithGroup_ReturnsTrue()
        {
            var groupModel = (await GroupService.GetGroups())
                .Where(g => g.modelFormattedName == CHANGED_GROUP_NAME).SingleOrDefault();

            bool deleteResult = await GroupService.DeleteGroup(groupModel);

            Assert.True(deleteResult);
        }

        [Fact]
        public async Task GetGroups_NotInitialized_ReturnsNull()
        {
            List<GroupModel> groups = await GroupService.GetGroups();

            Assert.Equal(new(), groups);
        }


        [Fact]
        public async Task CreateGroup_WithNull_ReturnsNull()
        {
            GroupService.Initialize();
            GroupModel groupModel = null;

            GroupModel group = await GroupService.CreateGroup(groupModel);

            Assert.Null(group);
        }

        [Fact]
        public async Task CreateGroup_NotInitialized_ReturnsNull()
        {
            GroupModel groupModel = new("TestName");

            GroupModel group = await GroupService.CreateGroup(groupModel);

            Assert.Null(group);
        }

        internal static async Task TestAsync()
        {
            PersonModel personModel = new PersonModel("John", "Doe", "JohnD@yahoo.com", "+7800553535");
            GroupModel groupModel = new GroupModel("testGroup2");
            string query = "Ринат";// "JohnD@yahoo.com";
            string properties = "names,emailAddresses";// "emailAddresses";
            string personFields = "names,emailAddresses,phoneNumbers,organizations,memberships";
            List<string> resources = new List<string> { "people/c8717037971012891222" };
            //GoogleService.Initialize();
            //List<GroupModel> groups = await GoogleService.GetGroups();
            ////GroupModel group = await GoogleContacts.CreateGroup(groupModel);
            ////var modGroup = await GoogleContacts.ModifyGroup("contactGroups/2f4d42e08a6f5e7f",resources);
            ////groupModel.modelResourceName = "contactGroups/2f4d42e08a6f5e7f";
            ////var updated = await GoogleContacts.UpdateGroup(groups.FirstOrDefault());
            ////GoogleContacts.CreateContact(personModel);
            //var model = (await GoogleService.GetContacts(personFields)).FirstOrDefault();
            //model.modelEmail = "JohnD@yahoo.com";
            //var model = (await GoogleContacts.SearchContact(query, properties)).FirstOrDefault();
            //await GoogleContacts.UpdateContact(model, personFields);
            //await GoogleContacts.TryToDeleteContact(model);
        }
    }
}
