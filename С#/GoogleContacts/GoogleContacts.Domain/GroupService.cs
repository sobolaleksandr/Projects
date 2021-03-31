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
    public class GroupService
    {
        ContactGroupsResource groupsResource;

        //private static readonly string m_client_secret = "uavwQnDWY6bUEFf75pXtP0m6";
        //private static readonly string m_client_id = "217336154173-tdce9e8b3c9hjfsd9abnfb7q0ef4q9ab.apps.googleusercontent.com";

        public GroupService(string client_secret, string client_id)
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
            PeopleServiceService service = new PeopleServiceService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "My App",
            });

            groupsResource = new ContactGroupsResource(service);
        }

        public async Task<List<GroupModel>> GetAll()
        {
            List<GroupModel> groups = new();

            if (groupsResource != null)
            {
                ListContactGroupsResponse response =
                await TryToGet();

                if (response != null)
                    foreach (ContactGroup group in response.ContactGroups)
                        groups.Add(new GroupModel(group));
            }

            return groups;
        }

        private async Task<ListContactGroupsResponse> TryToGet()
        {
            try
            {
                return await groupsResource.List().ExecuteAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<GroupModel> Create(GroupModel model)
        {
            if (model == null || groupsResource == null)
                return null;

            var request = new CreateContactGroupRequest
            {
                ContactGroup = model.Map()
            };


            ContactGroup response = await TryToCreate(request);

            return response != null ? new GroupModel(response) : null;
        }

        private async Task<ContactGroup> TryToCreate(CreateContactGroupRequest request)
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

        public async Task<GroupModel> Update(GroupModel model)
        {
            if (model == null || groupsResource == null)
                return null;

            var request = new UpdateContactGroupRequest
            {
                ContactGroup = model.Map()
            };

            ContactGroup response = await TryToUpdate(request, model.modelResourceName);

            return response != null ? new GroupModel(response) : null;
        }

        private async Task<ContactGroup> TryToUpdate(UpdateContactGroupRequest request, string property)
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

        public async Task<bool> Delete(GroupModel model)
        {
            if (groupsResource == null)
                return false;

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

        public async Task<ModifyContactGroupMembersResponse> Modify(GroupModel model, List<string> resources)
        {
            if (groupsResource == null)
                 return null;

            var members = groupsResource.Members;

            ModifyContactGroupMembersRequest request = new ModifyContactGroupMembersRequest
            {
                ResourceNamesToAdd = resources
            };

            return await members.Modify(request, model.modelResourceName).ExecuteAsync();
        }
    }
}
