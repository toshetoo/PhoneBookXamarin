using PhoneBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook.Services.EntityServices
{
    public class GroupsService:BaseService<Group>
    {
        public GroupsService():base()
        {

        }

        public List<Group> GetAllByContactID(int id)
        {
            List<ContactGroup> contactGroups = new ContactGroupsService().GetAll().ToList();            
            List<int> groupIds = new List<int>();
            for (int i = 0; i < contactGroups.Count; i++)
            {
                if (contactGroups[i].ContactID == id)
                {
                    groupIds.Add(contactGroups[i].GroupID);
                }
            }

            List<Group> toReturn = new List<Group>();
            GroupsService service = new GroupsService();
            for (int i = 0; i < groupIds.Count; i++)
            {
                toReturn.Add(service.GetById(groupIds[i]));
            }

            return toReturn;
            //return repo.GetAll().Where(g => g.Contacts.Any(c => c.Groups.Select(gr => gr.ID).Contains(id))).ToList();
            
        }
    }
}
