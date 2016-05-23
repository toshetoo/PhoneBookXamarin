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
using PhoneBook.Entities;
using PhoneBook.Services;
using PhoneBook.Repositories;
using PhoneBook.Services.EntityServices;

namespace PhoneBook.Droid.Activities
{
    [Activity(Label = "AddContactActivity", Icon = "@drawable/book")]
    public class AddContactActivity : Activity
    {
        Contact c = new Contact();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddContact);

            EditText tbFirstName = FindViewById<EditText>(Resource.Id.editTextFirstName);
            EditText tbLastName = FindViewById<EditText>(Resource.Id.editTextLastName);
            Button btnCreate = FindViewById<Button>(Resource.Id.btnCreate);

            if (MainActivity.SelectedContact != null)
            {
                tbFirstName.Text = MainActivity.SelectedContact.FirstName;
                tbLastName.Text = MainActivity.SelectedContact.LastName;
                btnCreate.Text = "Update";

                c.ID = MainActivity.SelectedContact.ID;
            }

            btnCreate.Click += BtnCreate_Click;
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            EditText tbFirstName = FindViewById<EditText>(Resource.Id.editTextFirstName);
            EditText tbLastName = FindViewById<EditText>(Resource.Id.editTextLastName);

            MainActivity.SelectedContact = new Contact();
            MainActivity.SelectedContact.ID = c.ID;
            MainActivity.SelectedContact.FirstName = tbFirstName.Text;
            MainActivity.SelectedContact.LastName = tbLastName.Text;
            MainActivity.SelectedContact.Phones = new PhonesService().GetPhonesByContactID(MainActivity.SelectedContact.ID).ToList();

            c.FirstName = tbFirstName.Text;
            c.LastName = tbLastName.Text;
            c.UserID = AuthenticationService.LoggedUser.ID;

            ContactsService contactsService = new ContactsService();
            contactsService.Save(c);

            Intent intentResult = new Intent(this, typeof(MainActivity));
            SetResult(Result.Ok, intentResult);
            Finish();
        }
    }
}