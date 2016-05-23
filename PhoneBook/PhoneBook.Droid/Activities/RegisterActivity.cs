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
using PhoneBook.Services;
using PhoneBook.Services.EntityServices;

namespace PhoneBook.Droid.Activities
{
    [Activity(Label = "Register", Icon = "@drawable/book")]
    public class RegisterActivity : Activity
    {
        User u = new User();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Register);

            EditText tbFirstName = FindViewById<EditText>(Resource.Id.editTextFirstName);
            EditText tbLastName = FindViewById<EditText>(Resource.Id.editTextLastName);
            EditText tbUsername = FindViewById<EditText>(Resource.Id.editTextUsername);
            EditText tbPassword = FindViewById<EditText>(Resource.Id.editTextPassword);

            Button btnRegister = FindViewById<Button>(Resource.Id.btnRegister);


            if (AuthenticationService.LoggedUser != null)
            {
                tbFirstName.Text = AuthenticationService.LoggedUser.FirstName;
                tbLastName.Text = AuthenticationService.LoggedUser.LastName;
                tbUsername.Text = AuthenticationService.LoggedUser.Username;
                tbPassword.Text = AuthenticationService.LoggedUser.Password;
                u.ID = AuthenticationService.LoggedUser.ID;
                btnRegister.Text = "Update";
            }

            
            btnRegister.Click += BtnRegister_Click;


        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            EditText tbFirstName = FindViewById<EditText>(Resource.Id.editTextFirstName);
            EditText tbLastName = FindViewById<EditText>(Resource.Id.editTextLastName);
            EditText tbUsername = FindViewById<EditText>(Resource.Id.editTextUsername);
            EditText tbPassword = FindViewById<EditText>(Resource.Id.editTextPassword);

            u.FirstName = tbFirstName.Text;
            u.LastName = tbLastName.Text;
            u.Username = tbUsername.Text;
            u.Password = tbPassword.Text;

            AuthenticationService.LoggedUser = u;

            UsersService usersService = new UsersService();
            usersService.Save(u);

            var okMessage = new AlertDialog.Builder(this);
            okMessage.SetMessage("User registered successfully!");
            okMessage.SetPositiveButton("OK!", delegate { });
            okMessage.Show();

            //returns result success when the intent is finished
            Intent intentResult = new Intent(this, typeof(LoginActivity));
            SetResult(Result.Ok, intentResult);
            Finish();
        }
    }
}