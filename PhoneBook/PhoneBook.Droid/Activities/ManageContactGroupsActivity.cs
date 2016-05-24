using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PhoneBook.Services.EntityServices;
using PhoneBook.Droid.CustomListView;
using PhoneBook.Entities;
using Android.Graphics;

namespace PhoneBook.Droid.Activities
{
    [Activity(Label = "SelectUserGroupsActivity")]
    public class ManageContactGroupsActivity : Activity
    {
        List<Group> groups;
        List<Group> selectedGroups = new List<Group>();
        List<Group> allGroups;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ManageContactGroups);

            GroupsService groupsService = new GroupsService();
            groups = groupsService.GetAll().ToList();
            allGroups = groupsService.GetAll().ToList();
            MainActivity.SelectedContact.Groups = groupsService.GetAllByContactID(MainActivity.SelectedContact.ID).ToList();

            ListView groupsListView = FindViewById<ListView>(Resource.Id.listViewGroups);
            Button btnUpdate = FindViewById<Button>(Resource.Id.btnUpdateGroups);
            
            GroupsViewAdapter adapter = new GroupsViewAdapter(this, Resource.Layout.ViewModel, allGroups);
            groupsListView.Adapter = adapter;
            groupsListView.ChoiceMode = ChoiceMode.Multiple;

            //populates all user contact and selects the ones that the contact participates
            groupsListView.ChildViewAdded += GroupsListView_ChildViewAdded;

            //selects and deselects groups /*ONCLICK BUG */
            groupsListView.ItemClick += GroupsListView_ItemClick;
            
            //refreshes the groups of the current contact
            btnUpdate.Click += BtnUpdate_Click;
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            ContactGroupsService groupsService = new ContactGroupsService();
            List<ContactGroup> allContactGroups = groupsService.GetAll().ToList();

            for (int i = 0; i < allContactGroups.Count; i++)
            {
                for (int j = 0; j < selectedGroups.Count; j++)
                {
                    if (allContactGroups[i].GroupID == selectedGroups[j].ID && allContactGroups[i].ContactID == MainActivity.SelectedContact.ID)
                    {
                        groupsService.Delete(allContactGroups[i]);
                    }
                }
            }

            for (int i = 0; i < selectedGroups.Count; i++)
            {
                for (int j = 0; j < allContactGroups.Count; j++)
                {
                    if (!(allContactGroups[j].GroupID == selectedGroups[i].ID && allContactGroups[j].ContactID == MainActivity.SelectedContact.ID))
                    {
                        ContactGroup cg = new ContactGroup();
                        cg.GroupID = selectedGroups[i].ID;
                        cg.ContactID = MainActivity.SelectedContact.ID;
                        groupsService.Save(cg);
                    }
                }
            }            
        }

        private void GroupsListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {            
            if (selectedGroups.Contains(allGroups[e.Position]))
            {
                selectedGroups.Remove(allGroups[e.Position]);
                e.View.SetBackgroundColor(default(Color));
            }
            else
            {
                selectedGroups.Add(allGroups[e.Position]);
                e.View.SetBackgroundColor(Color.Rgb(169, 169, 169));
            }
        }

        private void GroupsListView_ChildViewAdded(object sender, ViewGroup.ChildViewAddedEventArgs e)
        {
            for (int i = 0; i < groups.Count; i++)
            {
                for (int j = 0; j < MainActivity.SelectedContact.Groups.Count; j++)
                {
                    if (MainActivity.SelectedContact.Groups[j].ID == groups[i].ID)
                    {
                        e.Child.Selected = true;
                        e.Child.SetBackgroundColor(Color.Rgb(169, 169, 169));
                        selectedGroups.Add(groups[i]);
                        groups.Remove(groups[i]);
                        break;                                
                    }
                    else
                    {
                        e.Child.Selected = false;
                        e.Child.SetBackgroundColor(default(Color));
                    }
                }
                if (e.Child.Selected)
                {
                    break;
                }                
                
            }
        }
    }
}