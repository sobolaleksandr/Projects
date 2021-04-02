using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleContacts.Domain
{
    public class ContactService
    {
        readonly PeopleServiceService service;

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

            if (connectionsResponse?.Connections != null)
                foreach (var person in connectionsResponse.Connections)
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

        //нестабильно работает
        public async Task<List<PersonModel>> Search(string query, string readMask)
        {
            var request = service.People.SearchContacts();
            request.Query = query;
            request.ReadMask = readMask;

            List<PersonModel> personModels = new();
            SearchResponse response;

            try
            {
                response = await request.ExecuteAsync();
            }
            catch
            {
                return personModels;
            }

            if (response?.Results != null)
                foreach (var result in response.Results)
                    personModels.Add(new PersonModel(result.Person));

            return personModels;
        }
    }
}
