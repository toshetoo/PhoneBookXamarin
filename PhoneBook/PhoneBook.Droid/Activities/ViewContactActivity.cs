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

namespace PhoneBook.Droid.Activities
{
    [Activity(Label = "ViewContactActivity")]
    public class ViewContactActivity : ListActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ViewContact);

            TextView lblFirstName = FindViewById<TextView>(Resource.Id.textViewFirstName);
            TextView lblLastName = FindViewById<TextView>(Resource.Id.textViewLastName);
            

            lblFirstName.Text = MainActivity.SelectedContact.FirstName;
            lblLastName.Text = MainActivity.SelectedContact.LastName;

            PhonesRepository phonesRepo = new PhonesRepository();
            MainActivity.SelectedContact.Phones = phonesRepo.GetPhonesByContactID(MainActivity.SelectedContact.ID).ToList();

            ArrayAdapter phoneDetails = new ArrayAdapter(this, Resource.Layout.TextViewItem);
            for (int i = 0; i < MainActivity.SelectedContact.Phones.Count; i++)
            {
                phoneDetails.Add(MainActivity.SelectedContact.Phones[i].Type + " - " + MainActivity.SelectedContact.Phones[i].Number);
            }
            ListAdapter = phoneDetails;

            

            Button btnEditContact = FindViewById<Button>(Resource.Id.btnEditContact);
            
            btnEditContact.Click += delegate
            {
                var editContactIntent = new Intent(this, typeof(AddContactActivity));
                StartActivityForResult(editContactIntent, 1);                
            };

            Button btnAddPhone = FindViewById<Button>(Resource.Id.btnAddPhone);
            btnAddPhone.Click += delegate
            {
                var addPhoneIntent = new Intent(this, typeof(AddPhoneActivity));
                StartActivityForResult(addPhoneIntent, 0);
            };          
            
        }

        protected override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);

            //Android.Widget.Toast.MakeText(this, l.CheckedItemCount.ToString(), ToastLength.Short).Show();
            PhonesRepository phoneRepo = new PhonesRepository();
            
            //phoneRepo.Delete(MainActivity.SelectedContact.Phones[position]);
           // MainActivity.SelectedContact.Phones.Remove(MainActivity.SelectedContact.Phones[position]);
            //Recreate();
        }


        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            
            //Result from editContact button
            if (resultCode == Result.Ok && requestCode == 1)
            {                
                var okMessage = new AlertDialog.Builder(this);
                okMessage.SetMessage("Done!");
                okMessage.SetPositiveButton("OK", delegate { });
                okMessage.Show();
                Recreate();
            }

            //result from addPhone button
            if (resultCode == Result.Ok && requestCode == 0)
            {
                var okMessage = new AlertDialog.Builder(this);
                okMessage.SetMessage("Phone added!");
                okMessage.SetPositiveButton("OK", delegate { });
                okMessage.Show();
                Recreate();
            }
        }
        
    }
}