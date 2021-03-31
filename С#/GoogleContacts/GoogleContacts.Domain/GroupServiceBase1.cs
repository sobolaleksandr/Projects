namespace GoogleContacts.Domain
{
    public static class GroupServiceBase1
    {

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
    }
}