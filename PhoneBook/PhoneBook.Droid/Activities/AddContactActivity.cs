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
    [Activity(Label = "PhoneBook - ADD CONTACT", Icon = "@drawable/book")]
    public class AddContactActivity : Activity
    {
        Contact c = new Contact();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddContact);

            EditText tbFirstName = FindViewById<EditText>(Resource.Id.editTextFirstName);
            EditText tbLastName = FindViewById<EditText>(Resource.Id.editTextLastName);
            EditText tbEmail = FindViewById<EditText>(Resource.Id.editTextEmail);
            Button btnCreate = FindViewById<Button>(Resource.Id.btnCreate);

            if (MainActivity.SelectedContact != null)
            {
                tbFirstName.Text = MainActivity.SelectedContact.FirstName;
                tbLastName.Text = MainActivity.SelectedContact.LastName;
                tbEmail.Text = MainActivity.SelectedContact.Email;
                btnCreate.Text = "Update";

                c.ID = MainActivity.SelectedContact.ID;
            }

            btnCreate.Click += BtnCreate_Click;
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            EditText tbFirstName = FindViewById<EditText>(Resource.Id.editTextFirstName);
            EditText tbLastName = FindViewById<EditText>(Resource.Id.editTextLastName);
            EditText tbEmail = FindViewById<EditText>(Resource.Id.editTextEmail);


            c.FirstName = tbFirstName.Text;
            c.LastName = tbLastName.Text;
            c.Email = tbEmail.Text;
            c.UserID = AuthenticationService.LoggedUser.ID;

            MainActivity.SelectedContact = c;
            MainActivity.SelectedContact.Phones = new PhonesService().GetPhonesByContactID(MainActivity.SelectedContact.ID).ToList();

            ContactsService contactsService = new ContactsService();
            contactsService.Save(c);

            Intent intentResult = new Intent(this, typeof(MainActivity));
            SetResult(Result.Ok, intentResult);
            Finish();
        }
    }
}