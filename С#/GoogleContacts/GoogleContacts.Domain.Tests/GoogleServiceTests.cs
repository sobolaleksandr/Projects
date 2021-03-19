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
    [TestCaseOrderer(nameof(PriorityOrderer), nameof(Tests))]
    public class GoogleServiceTests
    {
        readonly GroupModel groupModel;
        private const string groupName = "TestName";
        private const string changedGroupName = "ChangedGroupName";

        public GoogleServiceTests()
        {
            GoogleService.Initialize();
            groupModel = new(groupName);
        }

        [Fact, TestPriority(0)]
        public async Task CreateGroup_WithGroup_ReturnsGroup()
        {
            var result = await GoogleService.CreateGroup(groupModel);

            Assert.Equal(groupName, result.modelFormattedName);
        }

        [Fact, TestPriority(1)]
        public async Task UpdateGroup_WithGroup_ReturnsGroup()
        {
            GroupModel groupModel = (await GoogleService.GetGroups()).FirstOrDefault();

            groupModel.modelFormattedName = changedGroupName;

            groupModel = await GoogleService.UpdateGroup(groupModel);

            Assert.Equal(changedGroupName, groupModel.modelFormattedName);
        }

        [Fact, TestPriority(2)]
        public async Task DeleteGroup_WithGroup_ReturnsGroup()
        {
            GroupModel groupModel = (await GoogleService.GetGroups()).FirstOrDefault();

            bool result = await GoogleService.DeleteGroup(groupModel);

            Assert.True(result);
        }

        //[Fact, TestPriority(3)]
        //public async Task GetGroups_Initialized_ReturnsGroups()
        //{
        //    List<GroupModel> groups = await GoogleService.GetGroups();

        //    Assert.Equal(11, groups.Count);
        //}

        //[Fact]
        //public async Task GetGroups_NotInitialized_ReturnsNull()
        //{
        //    List<GroupModel> groups = await GoogleService.GetGroups();

        //    Assert.Null(groups);
        //}


        //[Fact]
        //public async Task CreateGroup_WithNull_ReturnsNull()
        //{
        //    GoogleService.Initialize();
        //    GroupModel groupModel = null;

        //    GroupModel group = await GoogleService.CreateGroup(groupModel);

        //    Assert.Null(group);
        //}

        //[Fact]
        //public async Task CreateGroup_NotInitialized_ReturnsNull()
        //{
        //    GroupModel groupModel = new("TestName");

        //    GroupModel group = await GoogleService.CreateGroup(groupModel);

        //    Assert.Null(group);
        //}

        internal static async Task TestAsync()
        {
            PersonModel personModel = new PersonModel("John", "Doe", "JohnD@yahoo.com", "+7800553535");
            GroupModel groupModel = new GroupModel("testGroup2");
            string query = "Ринат";// "JohnD@yahoo.com";
            string properties = "names,emailAddresses";// "emailAddresses";
            string personFields = "names,emailAddresses,phoneNumbers,organizations,memberships";
            List<string> resources = new List<string> { "people/c8717037971012891222" };
            GoogleService.Initialize();
            List<GroupModel> groups = await GoogleService.GetGroups();
            //GroupModel group = await GoogleContacts.CreateGroup(groupModel);
            //var modGroup = await GoogleContacts.ModifyGroup("contactGroups/2f4d42e08a6f5e7f",resources);
            //groupModel.modelResourceName = "contactGroups/2f4d42e08a6f5e7f";
            //var updated = await GoogleContacts.UpdateGroup(groups.FirstOrDefault());
            //GoogleContacts.CreateContact(personModel);
            var model = (await GoogleService.GetContacts(personFields)).FirstOrDefault();
            //model.modelEmail = "JohnD@yahoo.com";
            //var model = (await GoogleContacts.SearchContact(query, properties)).FirstOrDefault();
            //await GoogleContacts.UpdateContact(model, personFields);
            //await GoogleContacts.TryToDeleteContact(model);
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestPriorityAttribute : Attribute
    {
        public int Priority { get; private set; }

        public TestPriorityAttribute(int priority) => Priority = priority;
    }

    public class PriorityOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(
            IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            string assemblyName = typeof(TestPriorityAttribute).AssemblyQualifiedName!;
            var sortedMethods = new SortedDictionary<int, List<TTestCase>>();
            foreach (TTestCase testCase in testCases)
            {
                int priority = testCase.TestMethod.Method
                    .GetCustomAttributes(assemblyName)
                    .FirstOrDefault()
                    ?.GetNamedArgument<int>(nameof(TestPriorityAttribute.Priority)) ?? 0;

                GetOrCreate(sortedMethods, priority).Add(testCase);
            }

            foreach (TTestCase testCase in
                sortedMethods.Keys.SelectMany(
                    priority => sortedMethods[priority].OrderBy(
                        testCase => testCase.TestMethod.Method.Name)))
            {
                yield return testCase;
            }
        }

        private static TValue GetOrCreate<TKey, TValue>(
            IDictionary<TKey, TValue> dictionary, TKey key)
            where TKey : struct
            where TValue : new() =>
            dictionary.TryGetValue(key, out TValue result)
                ? result
                : (dictionary[key] = new TValue());
    }
}
