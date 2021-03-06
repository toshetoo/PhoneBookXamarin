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
using PhoneBook.Repositories;
using PhoneBook.Entities;
using PhoneBook.Services;
using Android.Graphics;
using PhoneBook.Services.EntityServices;
using PhoneBook.Droid.CustomListView;

namespace PhoneBook.Droid.Activities
{
    [Activity(Label = "PhoneBook", Icon = "@drawable/book", Permission = "android.permission.GET_CONTENT")]
    public class MainActivity : ListActivity
    {
        public static Contact SelectedContact;
        int selectedMenuPosition;
        List<Contact> selectedContacts = new List<Contact>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MainScreen);            

            ContactsService contactsService = new ContactsService();
            AuthenticationService.LoggedUser.Contacts = contactsService.GetAllByUserID(AuthenticationService.LoggedUser.ID).ToList();
            //AuthenticationService.LoggedUser.Contacts = AuthenticationService.LoggedUser.Contacts.OrderBy(c=>c.FirstName).ToList();

            Button btnDelete = FindViewById<Button>(Resource.Id.btnDelete);
            Button btnAddContact = FindViewById<Button>(Resource.Id.btnAddContact);
            Button btnEditUser = FindViewById<Button>(Resource.Id.btnEditUser);
            TextView userLabel = FindViewById<TextView>(Resource.Id.textViewUser);
            ImageView userImage = FindViewById<ImageView>(Resource.Id.userImage);
            TextView userMail = FindViewById<TextView>(Resource.Id.textViewEmail);

            userLabel.Text = AuthenticationService.LoggedUser.FirstName + " " + AuthenticationService.LoggedUser.LastName;
            userMail.Text = "Email: " + AuthenticationService.LoggedUser.Email; 


            if (AuthenticationService.LoggedUser.ImageURI != null)
            {
                Android.Net.Uri imageUri = Android.Net.Uri.Parse(AuthenticationService.LoggedUser.ImageURI);
                userImage.SetImageURI(imageUri);

            }

            ListView listViewContacts = FindViewById<ListView>(Resource.Id.listViewContacts);
            listViewContacts.ChoiceMode = ChoiceMode.Multiple;
            listViewContacts.Adapter = RefreshAdapter();            
            
            //trigers the ListViewContacts_ItemLongClick event
            listViewContacts.ItemLongClick += ListViewContacts_ItemLongClick;

            //trigers the ListViewContacts_ItemClick event
            listViewContacts.ItemClick += ListViewContacts_ItemClick;

            //creates an activity to add a new contact
            btnAddContact.Click += BtnAddContact_Click;

            //creates an activity to edit user
            btnEditUser.Click += BtnEditUser_Click;

            //creates an activity to delete contacts
            btnDelete.Click += BtnDelete_Click;

            //adds an image to the ImageView
            userImage.Click += UserImage_Click;

        }

        private void UserImage_Click(object sender, EventArgs e)
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(
                Intent.CreateChooser(imageIntent, "Select photo"), 1);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            var confirm = new AlertDialog.Builder(this);
            confirm.SetMessage("Are you sure you want to delete " + selectedContacts.Count + " contacts?");
            confirm.SetPositiveButton("Yes", delegate
            {
                ContactsService contactsService = new ContactsService();
                Button btnDelete = FindViewById<Button>(Resource.Id.btnDelete);
                for (int i = 0; i < selectedContacts.Count; i++)
                {
                    contactsService.Delete(selectedContacts[i]);
                    AuthenticationService.LoggedUser.Contacts.Remove(selectedContacts[i]);
                }
                var okMessage = new AlertDialog.Builder(this);
                okMessage.SetMessage(selectedContacts.Count + " contacts deleted!");
                okMessage.SetPositiveButton("Okay", delegate { });
                okMessage.Show();

                ListView listViewContacts = FindViewById<ListView>(Resource.Id.listViewContacts);
                listViewContacts.Adapter = RefreshAdapter();
                selectedContacts.Clear();
                btnDelete.Enabled = false;
                Recreate();
            });
            confirm.SetNegativeButton("No", delegate { Recreate(); });
            confirm.Show();
        }

        private void BtnEditUser_Click(object sender, EventArgs e)
        {
            var editUserIntent = new Intent(this, typeof(RegisterActivity));
            StartActivityForResult(editUserIntent, 2);
        }

        private void BtnAddContact_Click(object sender, EventArgs e)
        {
            var addContactIntent = new Intent(this, typeof(AddContactActivity));
            StartActivityForResult(addContactIntent, 0);
        }

        private void ListViewContacts_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Button btnDelete = FindViewById<Button>(Resource.Id.btnDelete);
            if (selectedContacts.Contains(AuthenticationService.LoggedUser.Contacts[e.Position]))
            {
                selectedContacts.Remove(AuthenticationService.LoggedUser.Contacts[e.Position]);
                e.View.SetBackgroundColor(default(Color));              
            }
            else
            {
                selectedContacts.Add(AuthenticationService.LoggedUser.Contacts[e.Position]);
                e.View.SetBackgroundColor(Color.Rgb(169, 169, 169));
            }

            if (selectedContacts.Count > 0)
                btnDelete.Enabled = true;
            else
                btnDelete.Enabled = false;
        }

        private void ListViewContacts_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            
            selectedMenuPosition = e.Position;
            RegisterForContextMenu(e.View);
            PopupMenu menu = new PopupMenu(this, e.View);
            menu.Inflate(Resource.Menu.ContactsMenuOptions);
            menu.Show();

            menu.MenuItemClick += Menu_MenuItemClick;
            
        }

        private void Menu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            //Android.Widget.Toast.MakeText(this, e.Item.TitleFormatted, ToastLength.Short).Show();
            switch (e.Item.TitleFormatted.ToString())
            {
                case "View":
                    SelectedContact = AuthenticationService.LoggedUser.Contacts[selectedMenuPosition];
                    SelectedContact.Phones = new PhonesService().GetPhonesByContactID(SelectedContact.ID).ToList();
                    var viewContact = new Intent(this, typeof(ViewContactActivity));
                    StartActivity(viewContact);
                    break;
                case "Edit":
                    SelectedContact = AuthenticationService.LoggedUser.Contacts[selectedMenuPosition];
                    var editContactIntent = new Intent(this, typeof(AddContactActivity));
                    StartActivityForResult(editContactIntent, 3);                    
                    break;
                case "Manage groups":
                    SelectedContact = AuthenticationService.LoggedUser.Contacts[selectedMenuPosition];
                    var manageGroupsIntent = new Intent(this, typeof(ManageContactGroupsActivity));
                    StartActivity(manageGroupsIntent);
                    break;
                default:
                    break;
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            //returns the result from the btnAddContact activity
            if (resultCode == Result.Ok && requestCode == 0)
            {
                var okMessage = new AlertDialog.Builder(this);
                okMessage.SetMessage("Contact saved!");
                okMessage.SetPositiveButton("OK", delegate { });
                okMessage.Show();
                SelectedContact = null;
                this.Recreate();
            }
            //returns result from the btnEditUser
            if (resultCode == Result.Ok && requestCode == 2)
            {
                var okMessage = new AlertDialog.Builder(this);
                okMessage.SetMessage("User updated!");
                okMessage.SetPositiveButton("OK", delegate { });
                okMessage.Show();
                this.Recreate();
            }
            //returns result from image selection
            if (resultCode == Result.Ok && requestCode == 1)
            {
                var imageView =
                    FindViewById<ImageView>(Resource.Id.userImage);
                imageView.SetImageURI(data.Data);
                AuthenticationService.LoggedUser.ImageURI = data.DataString;
                UsersService usersService = new UsersService();
                usersService.Save(AuthenticationService.LoggedUser);
            }

            //Result from editContact button
            if (resultCode == Result.Ok && requestCode == 3)
            {
                var okMessage = new AlertDialog.Builder(this);
                okMessage.SetMessage("Done!");
                okMessage.SetPositiveButton("OK", delegate { });
                okMessage.Show();
                Recreate();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            SelectedContact = null;
            //refreshes the listed contacts
            ListView listViewContacts = FindViewById<ListView>(Resource.Id.listViewContacts);
            listViewContacts.ChoiceMode = ChoiceMode.Multiple;
            listViewContacts.Adapter = RefreshAdapter();
        }

        ContactsViewAdapter RefreshAdapter()
        {
            AuthenticationService.LoggedUser.Contacts.Clear();
            ContactsService contactsService = new ContactsService();
            AuthenticationService.LoggedUser.Contacts = contactsService.GetAllByUserID(AuthenticationService.LoggedUser.ID).ToList();

            ContactsViewAdapter contactNames = new ContactsViewAdapter(this, Resource.Layout.ViewModel, AuthenticationService.LoggedUser.Contacts);
            return contactNames;
        }
    }
}