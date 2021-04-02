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
        private const string NEW_NAME = "Johny";
        private const string FAMILY_NAME = "Doe";
        private const string EMAIL = "JohnD@yahoo.com";
        private const string PHONE_NUMBER = "+7800553535";
        private const string CLIENT_SECRET = "uavwQnDWY6bUEFf75pXtP0m6";
        private const string CLIENT_ID = "217336154173-tdce9e8b3c9hjfsd9abnfb7q0ef4q9ab.apps.googleusercontent.com";

        readonly ContactService service = new(CLIENT_SECRET, CLIENT_ID);
        PersonModel person = new(NAME, FAMILY_NAME, EMAIL, PHONE_NUMBER);

        [Fact]
        public async Task CRUD_Contacts()
        {
            await Create_ReturnsPerson();
            await GetAll_ReturnsPeople();
            await Update_ReturnsPerson();
            await Delete_ReturnsTrue();
        }

        internal async Task Create_ReturnsPerson()
        {
            PersonModel createResult = await service.Create(person);

            Assert.Equal(NAME, createResult.modelGivenName);
            Assert.Equal(FAMILY_NAME, createResult.modelFamilyName);
            Assert.Equal(EMAIL, createResult.modelEmail);
            Assert.Equal(PHONE_NUMBER, createResult.modelPhoneNumber);

            person = createResult;
        }

        private async Task Delete_ReturnsTrue()
        {
            var deleteResult = await service.TryToDelete(person);

            Assert.True(deleteResult);
        }
        
        //нестабильно работает
        private async Task Search_ReturnsPerson()
        {
            var searchResult = await service.Search(NEW_NAME, PERSON_FIELDS);

            Assert.Single(searchResult);
            Assert.Equal(NEW_NAME, searchResult[0].modelGivenName);
            Assert.Equal(EMAIL, searchResult[0].modelEmail);
            Assert.Equal(PHONE_NUMBER, searchResult[0].modelPhoneNumber);
            Assert.Equal(person.modelResourceName, searchResult[0].modelResourceName);
        }

        private async Task Update_ReturnsPerson()
        {
            person.modelGivenName = NEW_NAME;

            var updateResult = await service.Update(person, PERSON_FIELDS);

            Assert.Equal(NEW_NAME, updateResult.modelGivenName);

            person = updateResult;
        }

        private async Task GetAll_ReturnsPeople()
        {
            List<PersonModel> result = await service.GetAll(PERSON_FIELDS);

            Assert.Single(result);
            Assert.Equal(person.modelResourceName, result[0].modelResourceName);
            person = result[0];
        }
    }
}
