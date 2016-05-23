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
using PhoneBook.Services.EntityServices;

namespace PhoneBook.Droid.Activities
{
    [Activity(Label = "PhoneBook", Icon = "@drawable/book")]
    public class ViewContactActivity : ListActivity
    {
        
        List<Phone> selectedPhones = new List<Phone>();
        
        public static Phone SelectedPhone;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ViewContact);

            TextView lblContactName = FindViewById<TextView>(Resource.Id.textViewContactName);
            Button btnDelete = FindViewById<Button>(Resource.Id.btnDelete);
            Button btnEditContact = FindViewById<Button>(Resource.Id.btnEditContact);
            Button btnAddPhone = FindViewById<Button>(Resource.Id.btnAddPhone);
            ImageView contactImageView = FindViewById<ImageView>(Resource.Id.ContactImageView);

            if (MainActivity.SelectedContact.ImageURI != null)
            {
                Android.Net.Uri imageUri = Android.Net.Uri.Parse(MainActivity.SelectedContact.ImageURI);
                contactImageView.SetImageURI(imageUri);
            }            

            ListView listViewPhones = FindViewById<ListView>(Resource.Id.listViewPhones);
            listViewPhones.ChoiceMode = ChoiceMode.Multiple;
            ArrayAdapter phoneDetails = RefreshAdapter();
            listViewPhones.Adapter = phoneDetails;

            lblContactName.Text = MainActivity.SelectedContact.FirstName + " " + MainActivity.SelectedContact.LastName;

            PhonesService phonesService = new PhonesService();
            MainActivity.SelectedContact.Phones = phonesService.GetPhonesByContactID(MainActivity.SelectedContact.ID).ToList();

            //launches the EditContact activity
            btnEditContact.Click += BtnEditContact_Click;
            //Launches the AddPhone activity
            btnAddPhone.Click += BtnAddPhone_Click;

            //launches an activity to edit phones
            listViewPhones.ItemLongClick += ListViewPhones_ItemLongClick;
            
            //Selects items to be deleted
            listViewPhones.ItemClick += ListViewPhones_ItemClick;
            //creates an activity to delete phones
            btnDelete.Click += BtnDelete_Click;
            //launches an intent to select an image
            contactImageView.Click += ContactImageView_Click;

        }

        private void ContactImageView_Click(object sender, EventArgs e)
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);
            StartActivityForResult(
                Intent.CreateChooser(imageIntent, "Select photo"), 3);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            ListView listViewPhones = FindViewById<ListView>(Resource.Id.listViewPhones);
            Button btnDelete = FindViewById<Button>(Resource.Id.btnDelete);
            PhonesService phonesService = new PhonesService();

            var confirm = new AlertDialog.Builder(this);
            confirm.SetMessage("Are you sure you want to delete " + selectedPhones.Count + " phones?");
            confirm.SetPositiveButton("Yes", delegate
            {
                for (int i = 0; i < selectedPhones.Count; i++)
                {
                    phonesService.Delete(selectedPhones[i]);
                    MainActivity.SelectedContact.Phones.Remove(selectedPhones[i]);
                }
                var okMessage = new AlertDialog.Builder(this);
                okMessage.SetMessage(selectedPhones.Count + " phones deleted!");
                okMessage.SetPositiveButton("Okay", delegate { });
                okMessage.Show();


                listViewPhones.Adapter = RefreshAdapter(); ;
                selectedPhones.Clear();
                btnDelete.Enabled = false;
                Recreate();
            });
            confirm.SetNegativeButton("No", delegate { Recreate(); });
            confirm.Show();
        }

        private void BtnAddPhone_Click(object sender, EventArgs e)
        {
            var addPhoneIntent = new Intent(this, typeof(AddPhoneActivity));
            StartActivityForResult(addPhoneIntent, 0);
        }

        private void BtnEditContact_Click(object sender, EventArgs e)
        {
            var editContactIntent = new Intent(this, typeof(AddContactActivity));
            StartActivityForResult(editContactIntent, 1);
        }

        private ArrayAdapter RefreshAdapter()
        {
            ArrayAdapter phoneDetails = new ArrayAdapter(this, Resource.Layout.TextViewItem);
            for (int i = 0; i < MainActivity.SelectedContact.Phones.Count; i++)
            {
                phoneDetails.Add(MainActivity.SelectedContact.Phones[i].Type + " - " + MainActivity.SelectedContact.Phones[i].Number);
            }
            return phoneDetails;
        }

        private void ListViewPhones_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Button btnDelete = FindViewById<Button>(Resource.Id.btnDelete);
            if (selectedPhones.Contains(MainActivity.SelectedContact.Phones[e.Position]))
                selectedPhones.Remove(MainActivity.SelectedContact.Phones[e.Position]);
            else
                selectedPhones.Add(MainActivity.SelectedContact.Phones[e.Position]);

            if (selectedPhones.Count > 0)
                btnDelete.Enabled = true;
            else
                btnDelete.Enabled = false;
        }

        private void ListViewPhones_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            
            SelectedPhone = MainActivity.SelectedContact.Phones[e.Position];
            var editPhoneIntent = new Intent(this, typeof(AddPhoneActivity));
            StartActivityForResult(editPhoneIntent,2);
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
                okMessage.SetPositiveButton("OK", delegate { Recreate(); });
                

                ListView listViewPhones = FindViewById<ListView>(Resource.Id.listViewPhones);
                listViewPhones.ChoiceMode = ChoiceMode.Multiple;
                ArrayAdapter phoneDetails = RefreshAdapter();
                listViewPhones.Adapter = phoneDetails;

                okMessage.Show();
            }

            //result from editPhoneButton
            if (resultCode==Result.Ok && requestCode == 2)
            {
                var okMessage = new AlertDialog.Builder(this);
                okMessage.SetMessage("Phone updated!");
                okMessage.SetPositiveButton("OK", delegate { });
                Recreate();
                okMessage.Show();
                
            }

            //result from image selection
            if (resultCode == Result.Ok && requestCode == 3)
            {
                var imageView =
                    FindViewById<ImageView>(Resource.Id.ContactImageView);
                imageView.SetImageURI(data.Data);
                MainActivity.SelectedContact.ImageURI = data.DataString;
                ContactsService contactsService = new ContactsService();
                contactsService.Save(MainActivity.SelectedContact);
            }
        }        
        
    }
}