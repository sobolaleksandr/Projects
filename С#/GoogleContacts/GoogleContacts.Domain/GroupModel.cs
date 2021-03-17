using Google.Apis.PeopleService.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleContacts.Domain
{
    public class GroupModel
    {
        public string modelResourceName;
        public string modelFormattedName;
        public int modelMemberCount;

        private readonly string modelEtag;

        public ContactGroup Map()
        {
            return new ContactGroup
            {
                Name = modelFormattedName,
                ETag = modelEtag
            };
        }

        public GroupModel(ContactGroup group)
        {
            modelResourceName = group.ResourceName;
            modelEtag = group.ETag;
            modelFormattedName = group.FormattedName ?? "";
            modelMemberCount = group.MemberCount ?? 0;
        }

        public GroupModel(string name)
        {
            modelFormattedName = name;
        }   
}

}
