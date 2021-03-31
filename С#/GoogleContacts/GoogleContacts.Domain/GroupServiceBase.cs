namespace GoogleContacts.Domain
{
    public static class GroupServiceBase
    {

        private static readonly string m_client_secret = "uavwQnDWY6bUEFf75pXtP0m6";
        private static readonly string m_client_id = "217336154173-tdce9e8b3c9hjfsd9abnfb7q0ef4q9ab.apps.googleusercontent.com";
        static PeopleServiceService service;
        static ContactGroupsResource groupsResource;


        public static async Task<PersonModel> CreateContact(PersonModel personModel)
        {
            var createdContact = await service.People.CreateContact(personModel.Map()).ExecuteAsync();

            return new PersonModel(createdContact);
        }

        public static async Task<GroupModel> CreateGroup(GroupModel model)
        {
            if (model == null)
                return null;

            var request = new CreateContactGroupRequest
            {
                ContactGroup = model.Map()
            };

            ContactGroup response = await TryToCreateGroup(request);

            return response != null ? new GroupModel(response) : null;
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

        public static async Task<List<GroupModel>> GetGroups()
        {
            List<GroupModel> groups = new();

            if (groupsResource != null)
            {
                ListContactGroupsResponse response =
                await TryGetGroups();

                if (response != null)
                    foreach (ContactGroup group in response.ContactGroups)
                        groups.Add(new GroupModel(group));
            }

            return groups;
        }

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

        public static async Task<ModifyContactGroupMembersResponse> ModifyGroup(string groupResourceName, List<string> resources)
        {
            var members = groupsResource.Members;

            ModifyContactGroupMembersRequest request = new ModifyContactGroupMembersRequest
            {
                ResourceNamesToAdd = resources
            };

            return await members.Modify(request, groupResourceName).ExecuteAsync();
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
        //todo:get and update (coz etag after update changes)
        public static async Task<PersonModel> UpdateContact(PersonModel personModel, string personFields)
        {
            Person person = personModel.Map();
            var updateRequest = service.People.UpdateContact(person, personModel.modelResourceName);
            updateRequest.UpdatePersonFields = personFields;
            var updatedContact = await updateRequest.ExecuteAsync();

            return new PersonModel(updatedContact);
        }

        public static async Task<GroupModel> UpdateGroup(GroupModel model)
        {
            if (model == null)
                return null;

            var request = new UpdateContactGroupRequest
            {
                ContactGroup = model.Map()
            };

            ContactGroup response = await TryToUpdateGroup(request, model.modelResourceName);

            return response != null ? new GroupModel(response) : null;
        }

        private static async Task<ListContactGroupsResponse> TryGetGroups()
        {
            try
            {
                return await groupsResource?.List().ExecuteAsync();
            }
            catch
            {
                return null;
            }
        }

        private static async Task<ContactGroup> TryToCreateGroup(CreateContactGroupRequest request)
        {
            try
            {
                return await groupsResource.Create(request).ExecuteAsync();
            }
            catch
            {
                return null;
            }
        }

        private static async Task<ContactGroup> TryToGet(UpdateContactGroupRequest request, string property)
        {
            try
            {
                return await groupsResource.Update(request, property).ExecuteAsync();
            }
            catch
            {
                return null;
            }
        }

        private static async Task<ContactGroup> TryToUpdateGroup(UpdateContactGroupRequest request, string property)
        {
            try
            {
                return await groupsResource.Update(request, property).ExecuteAsync();
            }
            catch
            {
                return null;
            }
        }
    }
}