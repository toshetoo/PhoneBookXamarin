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

namespace PhoneBook.Droid.Activities
{
    [Activity(Label = "AddPhoneActivity")]
    public class AddPhoneActivity : Activity
    {
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

            btnCreatePhone.Click += delegate
            {
                Phone p = new Phone();
                p.Type = phoneType.SelectedItem.ToString();
                p.Number = phoneNumber.Text;
                p.ContactID = MainActivity.SelectedContact.ID;

                PhonesRepository phonesRepo = new PhonesRepository();
                phonesRepo.Save(p);

                Intent intentResult = new Intent(this, typeof(ViewContactActivity));
                SetResult(Result.Ok, intentResult);
                Finish();
            };
            

        }
    }
}