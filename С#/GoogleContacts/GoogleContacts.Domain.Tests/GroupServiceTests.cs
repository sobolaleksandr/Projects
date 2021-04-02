using System;
using System.Collections.Generic;
using GoogleContacts.Domain;
using Xunit;
using System.Threading.Tasks;
using System.Linq;
using Xunit.Sdk;
using Xunit.Abstractions;
using System.Threading;

namespace GoogleContacts.Domain.Tests
{
    public class GroupServiceTests
    {
        private const string NAME = "TestName";
        private const string CHANGED_NAME = "ChangedGroupName";
        private const string CLIENT_SECRET = "uavwQnDWY6bUEFf75pXtP0m6";
        private const string CLIENT_ID = "217336154173-tdce9e8b3c9hjfsd9abnfb7q0ef4q9ab.apps.googleusercontent.com";

        static readonly GroupService groupService = new(CLIENT_SECRET,CLIENT_ID);
        static GroupModel groupModel = new(NAME);

        //todo modify tests
        [Fact]
        public async Task CRUD_Group()
        {
            await GetAll_Initialized_ReturnsGroups();
            Thread.Sleep(3000);
            await Create_WithGroup_ReturnsGroup();
            Thread.Sleep(3000);
            await Update_WithGroup_ReturnsGroup();
            Thread.Sleep(3000);
            await Delete_WithGroup_ReturnsTrue();
        }

        internal static async Task GetAll_Initialized_ReturnsGroups()
        {
            List<GroupModel> groups = await groupService.GetAll();

            Assert.Equal(8, groups.Count);
        }

        internal static async Task Create_WithGroup_ReturnsGroup()
        {
            var createResult = await groupService.Create(groupModel);

            Assert.Equal(NAME, createResult.modelFormattedName);

            groupModel = createResult;
        }

        internal static async Task Update_WithGroup_ReturnsGroup()
        {
            groupModel.modelFormattedName = CHANGED_NAME;

            var result = await groupService.Update(groupModel);

            Assert.Equal(groupModel.modelFormattedName, result.modelFormattedName);

            groupModel = result;
        }

        //todo getContacts to add in group
        internal static async Task Modify_WithGroup_ReturnsTrue()
        {
            var groupModel = (await groupService.GetAll())
                            .Where(g => g.modelFormattedName == CHANGED_NAME)
                            .SingleOrDefault();
            groupModel.modelFormattedName = CHANGED_NAME;

            var result = await groupService.Update(groupModel);

            Assert.Equal(groupModel.modelFormattedName, result.modelFormattedName);
        }

        internal static async Task Delete_WithGroup_ReturnsTrue()
        {
            bool deleteResult = await groupService.Delete(groupModel);

            Assert.True(deleteResult);
        }

        [Fact]
        public async Task GetAll_WithAPIError_ReturnsEmptyList()
        {
            var service = new GroupService("","");

            List<GroupModel> groups = await service.GetAll();

            Assert.Equal(new(), groups);
        }

        [Fact]
        public async Task Create_WithNull_ReturnsNull()
        {
            var service = new GroupService(CLIENT_SECRET,CLIENT_ID);
            GroupModel groupModel = null;

            GroupModel group = await service.Create(groupModel);

            Assert.Null(group);
        }

        [Fact]
        public async Task Create_WithAPIError_ReturnsNull()
        {
            var service = new GroupService("", "");
            GroupModel groupModel = new("TestName");

            GroupModel group = await service.Create(groupModel);

            Assert.Null(group);
        }

        [Fact]
        public async Task Update_WithNull_ReturnsNull()
        {
            var service = new GroupService(CLIENT_SECRET, CLIENT_ID);
            GroupModel groupModel = null;

            GroupModel group = await service.Update(groupModel);

            Assert.Null(group);
        }

        [Fact]
        public async Task Update_WithAPIError_ReturnsNull()
        {
            var service = new GroupService("", "");
            GroupModel groupModel = new("TestName");

            GroupModel group = await service.Update(groupModel);

            Assert.Null(group);
        }

        [Fact]
        public async Task Delete_WithAPIError_ReturnsNull()
        {
            var service = new GroupService("", "");
            GroupModel groupModel = new("TestName");

            bool result = await service.Delete(groupModel);

            Assert.False(result);
        }

        [Fact]
        public async Task Modify_WithAPIError_ReturnsNull()
        {
            var service = new GroupService("", "");
            GroupModel groupModel = new("TestName");

            var result = await service.Modify(groupModel, new());

            Assert.Null(result);
        }
    }
}
