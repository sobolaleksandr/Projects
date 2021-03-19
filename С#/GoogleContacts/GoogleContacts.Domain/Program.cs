using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleContacts.Domain
{
    public static class GoogleService
    {
        static PeopleServiceService service;
        static ContactGroupsResource groupsResource;

        private static readonly string m_client_secret = "uavwQnDWY6bUEFf75pXtP0m6";
        private static readonly string m_client_id = "217336154173-tdce9e8b3c9hjfsd9abnfb7q0ef4q9ab.apps.googleusercontent.com";

        public static void Initialize()
        {
            // Create OAuth credential.
            UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = m_client_id,
                    ClientSecret = m_client_secret
                },
                new[] { "https://www.googleapis.com/auth/contacts" },
                "user",
                CancellationToken.None).Result;

            // Create the service.
            service = new PeopleServiceService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "My App",
            });

            groupsResource = new ContactGroupsResource(service);
        }

        public static async Task<List<GroupModel>> GetGroups()
        {
            ListContactGroupsResponse response;

            try
            {
                response = await groupsResource.List().ExecuteAsync();
            }
            catch
            {
                return null;
            }

            List<GroupModel> groups = new();

            foreach (ContactGroup group in response.ContactGroups)
                groups.Add(new GroupModel(group));

            return groups;
        }

        public static async Task<GroupModel> CreateGroup(GroupModel model)
        {
            if (model == null)
                return null;

            var request = new CreateContactGroupRequest
            {
                ContactGroup = model.Map()
            };

            ContactGroup response;

            try
            {
                response = await groupsResource.Create(request).ExecuteAsync();
            }
            catch
            {
                return null;
            }

            return new GroupModel(response);
        }

        public static async Task<GroupModel> UpdateGroup(GroupModel model)
        {
            if (model == null)
                return null;

            var request = new UpdateContactGroupRequest
            {
                ContactGroup = model.Map()
            };

            ContactGroup response;

            try 
            { 
                response = await groupsResource.Update(request, model.modelResourceName).ExecuteAsync();
            }
            catch
            {
                return null;
            }

            return new GroupModel(response);
        }

        public static async Task<List<PersonModel>> GetContacts(string personFields)
        {
            PeopleResource.ConnectionsResource.ListRequest peopleRequest =
              service.People.Connections.List("people/me");
            peopleRequest.PersonFields = personFields;
            ListConnectionsResponse connectionsResponse = await peopleRequest.ExecuteAsync();
            IList<Person> connections = connectionsResponse.Connections;

            List<PersonModel> contacts = new();

            foreach (var person in connections)
                contacts.Add(new PersonModel(person));

            return contacts;
        }

        

        

        public static async Task<bool> DeleteGroup(GroupModel model)
        {
            try
            {
                await groupsResource.Delete(model.modelResourceName).ExecuteAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<ModifyContactGroupMembersResponse> ModifyGroup(string groupResourceName, List<string> resources)
        {
            var members = groupsResource.Members;

            ModifyContactGroupMembersRequest request = new ModifyContactGroupMembersRequest
            {
                ResourceNamesToAdd = resources
            };

            return await members.Modify(request, groupResourceName).ExecuteAsync();
        }
        

        public static async Task<PersonModel> CreateContact(PersonModel personModel)
        {
            var createdContact = await service.People.CreateContact(personModel.Map()).ExecuteAsync();

            return new PersonModel(createdContact);
        }
        //todo:get and update (coz etag after update changes)
        public static async Task<PersonModel> UpdateContact(PersonModel personModel, string personFields)
        {
            Person person = personModel.Map();
            var updateRequest = service.People.UpdateContact(person, personModel.modelResourceName);
            updateRequest.UpdatePersonFields = personFields;
            var updatedContact = await updateRequest.ExecuteAsync();

            return new PersonModel(updatedContact);
        }

        public static async Task<bool> TryToDeleteContact(PersonModel personModel)
        {
            try
            {
                await service.People.DeleteContact(personModel.modelResourceName).ExecuteAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<List<PersonModel>> SearchContact(string query, string readMask)
        {
            var request = service.People.SearchContacts();
            request.Query = query;
            request.ReadMask = readMask;
            var response = await request.ExecuteAsync();

            List<PersonModel> personModels = new List<PersonModel>();

            foreach (var result in response.Results)
                personModels.Add(new PersonModel(result.Person));

            return personModels;
        }


    }

    class Program
    {


        static async Task Main(string[] args)
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
        Console.WriteLine();
        }

}
}
