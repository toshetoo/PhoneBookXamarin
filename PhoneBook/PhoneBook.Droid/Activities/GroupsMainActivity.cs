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
using PhoneBook.Services;
using PhoneBook.Entities;
using PhoneBook.Droid.CustomListView;
using Android.Graphics;

namespace PhoneBook.Droid.Activities
{
    [Activity(Label = "GroupsMainActivity")]
    public class GroupsMainActivity : Activity
    {
        public static Entities.Group SelectedGroup;
        List<Group> selectedGroups = new List<Group>();
        List<Group> allGroups;
        int selectedMenuPosition;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            SetContentView(Resource.Layout.GroupsMainScreen);
            GroupsService groupsService = new GroupsService();
            ListView groupsListView = FindViewById<ListView>(Resource.Id.GroupsListView);
            groupsListView.ChoiceMode = ChoiceMode.Multiple;
            Button btnAddGroup = FindViewById<Button>(Resource.Id.btnAddGroup);

           allGroups = groupsService.GetAll().ToList();

            GroupsViewAdapter adapter = new GroupsViewAdapter(this, Resource.Layout.ViewModel,allGroups);
            groupsListView.Adapter = adapter;


            btnAddGroup.Click += BtnAddGroup_Click;

            groupsListView.ItemClick += GroupsListView_ItemClick;

            groupsListView.ItemLongClick += GroupsListView_ItemLongClick;    
            
        }

        private void GroupsListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            selectedMenuPosition = e.Position;
            RegisterForContextMenu(e.View);
            PopupMenu menu = new PopupMenu(this, e.View);
            menu.Inflate(Resource.Menu.GroupsMenuOptions);
            menu.Show();

            menu.MenuItemClick += Menu_MenuItemClick;
        }

        private void Menu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            switch (e.Item.TitleFormatted.ToString())
            {
                case "View":
                    SelectedGroup = allGroups[selectedMenuPosition];
                    SelectedGroup.Contacts = new ContactsService().GetAllByGroupID(SelectedGroup.ID).ToList();
                    var viewGroup = new Intent(this, typeof(ViewGroupActivity));
                    StartActivity(viewGroup);
                    break;
                case "Edit":
                    SelectedGroup = allGroups[selectedMenuPosition];
                    var editGroup = new Intent(this, typeof(AddGroupActivity));
                    StartActivityForResult(editGroup, 1);
                    break;                
            }
        }

        private void GroupsListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Button btnDelete = FindViewById<Button>(Resource.Id.btnDelete);
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

            if (selectedGroups.Count > 0)
                btnDelete.Enabled = true;
            else
                btnDelete.Enabled = false;
        }

        private void BtnAddGroup_Click(object sender, EventArgs e)
        {
            var addGroupIntent = new Intent(this, typeof(AddGroupActivity));
            StartActivityForResult(addGroupIntent, 0);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            //result from AddGroup button
            if (resultCode == Result.Ok && requestCode == 0)
            {
                var okMessage = new AlertDialog.Builder(this);
                okMessage.SetMessage("Group added!");
                okMessage.SetPositiveButton("OK", delegate { });
                okMessage.Show();
                this.Recreate();
            }

            //Result from editGroup button
            if (resultCode == Result.Ok && requestCode == 1)
            {
                var okMessage = new AlertDialog.Builder(this);
                okMessage.SetMessage("Done!");
                okMessage.SetPositiveButton("OK", delegate { });
                okMessage.Show();
                Recreate();
            }
        }
    }
}