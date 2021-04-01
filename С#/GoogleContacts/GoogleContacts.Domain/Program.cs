using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleContacts.Domain
{
    public class ContactService
    {
        readonly PeopleServiceService service;

        //private readonly string m_client_secret = "uavwQnDWY6bUEFf75pXtP0m6";
        //private readonly string m_client_id = "217336154173-tdce9e8b3c9hjfsd9abnfb7q0ef4q9ab.apps.googleusercontent.com";

        public ContactService(string client_secret, string client_id)
        {
            // Create OAuth credential.
            UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = client_id,
                    ClientSecret = client_secret
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
        }

        public async Task<List<PersonModel>> GetAll(string personFields)
        {
            List<PersonModel> contacts = new();
            ListConnectionsResponse connectionsResponse;

            PeopleResource.ConnectionsResource.ListRequest peopleRequest =
              service.People.Connections.List("people/me");
            peopleRequest.PersonFields = personFields;

            try
            {
                connectionsResponse = await peopleRequest.ExecuteAsync();
            }
            catch
            {
                return contacts;
            }

            IList<Person> connections = connectionsResponse.Connections;

            if (connections != null)
                foreach (var person in connections)
                    contacts.Add(new PersonModel(person));

            return contacts;
        }


        public async Task<PersonModel> Create(PersonModel personModel)
        {
            if (personModel == null)
                return null;

            var createdContact = await service.People.CreateContact(personModel.Map()).ExecuteAsync();

            return new PersonModel(createdContact);
        }

        //todo:get and update (coz etag after update changes)
        public  async Task<PersonModel> Update(PersonModel personModel, string personFields)
        {
            Person person = personModel.Map();
            var updateRequest = service.People.UpdateContact(person, personModel.modelResourceName);
            updateRequest.UpdatePersonFields = personFields;

            var updatedContact = await updateRequest.ExecuteAsync();

            return new PersonModel(updatedContact);
        }

        public async Task<bool> TryToDelete(PersonModel personModel)
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

        public async Task<List<PersonModel>> SearchContact(string query, string readMask)
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

            //GoogleService googleService = new();

            //GoogleService.Initialize();
            //List<GroupModel> groups = await googleService.GetGroups();
            //GroupModel group = await GoogleContacts.CreateGroup(groupModel);
            //var modGroup = await GoogleContacts.ModifyGroup("contactGroups/2f4d42e08a6f5e7f",resources);
            //groupModel.modelResourceName = "contactGroups/2f4d42e08a6f5e7f";
            //var updated = await GoogleContacts.UpdateGroup(groups.FirstOrDefault());
            //GoogleContacts.CreateContact(personModel);
            //var model = (await GoogleService.GetContacts(personFields)).FirstOrDefault();
            //model.modelEmail = "JohnD@yahoo.com";
            //var model = (await GoogleContacts.SearchContact(query, properties)).FirstOrDefault();
            //await GoogleContacts.UpdateContact(model, personFields);
            //await GoogleContacts.TryToDeleteContact(model);
        Console.WriteLine();
        }

}
}
