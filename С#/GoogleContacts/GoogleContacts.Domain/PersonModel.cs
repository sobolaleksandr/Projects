using Google.Apis.PeopleService.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleContacts.Domain
{
    public class PersonModel
    {
        public string modelResourceName;

        public string modelEtag;
        public string modelGivenName;
        public string modelFamilyName;
        public string modelPhoneNumber;
        public  string modelEmail;
        private string modelOrganization;
        private ContactGroupMembership modelMembership;

        public Person Map()
        {
            return new Person
            {
                ResourceName = modelResourceName,
                ETag = modelEtag,
                Names = new List<Name> { new Name {GivenName = modelGivenName, FamilyName = modelFamilyName } },
                PhoneNumbers = new List<PhoneNumber> { new PhoneNumber { Value = modelPhoneNumber} },
                EmailAddresses = new List<EmailAddress> { new EmailAddress { Value = modelEmail} },
                Organizations = new List<Organization> { new Organization { Name  = modelOrganization } }
            };
        }

        public PersonModel(Person person)
        {
            var name = person.Names?.FirstOrDefault();
            var email = person.EmailAddresses?.FirstOrDefault();
            var phoneNumber = person.PhoneNumbers?.FirstOrDefault();
            var organization = person.Organizations?.FirstOrDefault();
            var membership = person.Memberships?.FirstOrDefault();

            modelResourceName = person.ResourceName;
            modelEtag = person.ETag;

            modelGivenName = name?.GivenName ?? "";
            modelFamilyName = name?.FamilyName ?? "";
            modelPhoneNumber = phoneNumber?.Value ?? "";
            modelEmail = email?.Value ?? "";
            modelOrganization = organization?.Name ?? "";
            modelMembership = membership.ContactGroupMembership;
        }

        public PersonModel(string givenName, string familyName, string email, string phoneNumber)
        {
            modelGivenName = givenName;
            modelFamilyName = familyName;
            modelPhoneNumber = phoneNumber;
            modelEmail = email;
        }
    }

}
