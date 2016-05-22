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

namespace PhoneBook.Droid.Activities
{
    [Activity(Label = "PhoneBook")]
    public class MainActivity : ListActivity
    {
        public static Contact SelectedContact;
        List<Contact> selectedContacts = new List<Contact>();
        Button btnDelete;
        ContactsRepository contactsRepo;
        ListView listViewContacts;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.MainScreen);

            btnDelete = FindViewById<Button>(Resource.Id.btnDelete);

            Button btnAddContact = FindViewById<Button>(Resource.Id.btnAddContact);
            TextView userLabel = FindViewById<TextView>(Resource.Id.textViewUser);
            userLabel.Text = "Hello " + AuthenticationService.LoggedUser.FirstName;


            listViewContacts = FindViewById<ListView>(Resource.Id.listViewContacts);
            listViewContacts.ChoiceMode = ChoiceMode.Multiple;
            
            

            contactsRepo = new ContactsRepository();
            AuthenticationService.LoggedUser.Contacts = contactsRepo.GetAllByUserID(AuthenticationService.LoggedUser.ID).ToList();

            ArrayAdapter contactNames = new ArrayAdapter(this, Resource.Layout.TextViewItem);
            RefreshAdapter(contactNames);
            
            listViewContacts.Adapter = contactNames;
            
            //trigers the ListViewContacts_ItemLongClick event
            listViewContacts.ItemLongClick += ListViewContacts_ItemLongClick;

            //trigers the ListViewContacts_ItemClick event
            listViewContacts.ItemClick += ListViewContacts_ItemClick;

            //creates an activity to add a new contact
            btnAddContact.Click += delegate
            {
                var addContactIntent = new Intent(this, typeof(AddContactActivity));
                StartActivityForResult(addContactIntent, 0);
            };

            btnDelete.Click += delegate
            {
                var confirm = new AlertDialog.Builder(this);
                confirm.SetMessage("Are you sure you want to delete " + selectedContacts.Count + " contacts?");
                confirm.SetPositiveButton("Yes",delegate 
                {
                    for (int i = 0; i < selectedContacts.Count; i++)
                    {
                        contactsRepo.Delete(selectedContacts[i]);
                        AuthenticationService.LoggedUser.Contacts.Remove(selectedContacts[i]);
                    }
                    var okMessage = new AlertDialog.Builder(this);
                    okMessage.SetMessage(selectedContacts.Count + " contacts deleted!");
                    okMessage.SetPositiveButton("Okay", delegate { });
                    okMessage.Show();

                    RefreshAdapter(contactNames);
                    listViewContacts.Adapter = contactNames;
                    selectedContacts.Clear();
                    btnDelete.Enabled = false;
                    Recreate();
                });
                confirm.SetNegativeButton("No", delegate { Recreate();  });                
                confirm.Show();
                
                
            };
        }

        private void ListViewContacts_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {          

            if (selectedContacts.Contains(AuthenticationService.LoggedUser.Contacts[e.Position]))
                selectedContacts.Remove(AuthenticationService.LoggedUser.Contacts[e.Position]);            
            else
                selectedContacts.Add(AuthenticationService.LoggedUser.Contacts[e.Position]);

            if (selectedContacts.Count >0)
                btnDelete.Enabled = true;            
            else
                btnDelete.Enabled = false;
            
        }

        private void ListViewContacts_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            SelectedContact = AuthenticationService.LoggedUser.Contacts[e.Position];
            var viewContact = new Intent(this, typeof(ViewContactActivity));
            StartActivity(viewContact);
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
        }

        protected override void OnResume()
        {
            base.OnResume();
            SelectedContact = null;
        }

        void RefreshAdapter(ArrayAdapter contactNames)
        {
            contactNames.Clear();
            for (int i = 0; i < AuthenticationService.LoggedUser.Contacts.Count; i++)
            {
                contactNames.Add(AuthenticationService.LoggedUser.Contacts[i].FirstName + " " + AuthenticationService.LoggedUser.Contacts[i].LastName);
            }
        }
    }
}