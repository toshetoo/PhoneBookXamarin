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
using PhoneBook.Repositories;
using PhoneBook.Services.EntityServices;

namespace PhoneBook.Droid.Activities
{
    [Activity(Label = "AddPhoneActivity", Icon = "@drawable/book")]
    public class AddPhoneActivity : Activity
    {
        Phone p = new Phone();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddPhone);

            Spinner phoneType = FindViewById<Spinner>(Resource.Id.spinnerPhoneType);
            EditText phoneNumber = FindViewById<EditText>(Resource.Id.editTextPhoneNumber);
            Button btnCreatePhone = FindViewById<Button>(Resource.Id.btnCreatePhone);

            ArrayAdapter phoneTypes = new ArrayAdapter(this, Resource.Layout.TextViewItem);
            phoneTypes.Add("Home");
            phoneTypes.Add("Mobile");
            phoneTypes.Add("Fax");

            phoneType.Adapter = phoneTypes;

            if (ViewContactActivity.SelectedPhone != null)
            {
                phoneNumber.Text = ViewContactActivity.SelectedPhone.Number;
                switch (ViewContactActivity.SelectedPhone.Type)
                {
                    case "Home": phoneType.SetSelection(0); break;
                    case "Mobile": phoneType.SetSelection(1);  break;
                    case "Fax": phoneType.SetSelection(2); break;                    
                }
                btnCreatePhone.Text = "Update";
                p.ID = ViewContactActivity.SelectedPhone.ID;
            }

            btnCreatePhone.Click += BtnCreatePhone_Click;
            

        }

        private void BtnCreatePhone_Click(object sender, EventArgs e)
        {
            Spinner phoneType = FindViewById<Spinner>(Resource.Id.spinnerPhoneType);
            EditText phoneNumber = FindViewById<EditText>(Resource.Id.editTextPhoneNumber);

            p.Type = phoneType.SelectedItem.ToString();
            p.Number = phoneNumber.Text;
            p.ContactID = MainActivity.SelectedContact.ID;

            PhonesService phonesService = new PhonesService();
            phonesService.Save(p);
            MainActivity.SelectedContact.Phones.Clear();
            MainActivity.SelectedContact.Phones = phonesService.GetPhonesByContactID(MainActivity.SelectedContact.ID).ToList();

            Intent intentResult = new Intent(this, typeof(ViewContactActivity));
            SetResult(Result.Ok, intentResult);
            Finish();
        }
    }
}