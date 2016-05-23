using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using PhoneBook.Repositories;
using PhoneBook.Entities;
using PhoneBook.Services;

namespace PhoneBook.Droid.Activities
{
	[Activity (Label = "PhoneBook", MainLauncher = true, Icon = "@drawable/book")]
	public class LoginActivity : Activity
	{
	    protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
            Xamarin.Forms.Forms.Init(this, bundle);          
            
            SetContentView (Resource.Layout.Login);            
            
            Button btnLogin = FindViewById<Button>(Resource.Id.btnLogin);
            Button btnRegister = FindViewById<Button>(Resource.Id.btnRegister);
            EditText tbUsername = FindViewById<EditText>(Resource.Id.editTextUsername);
            EditText tbPassword = FindViewById<EditText>(Resource.Id.editTextPassword);

            //trigers the login button event
            btnLogin.Click += BtnLogin_Click;

            //trigers the register button event
            btnRegister.Click += BtnRegister_Click;
		}

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            var registerActivity = new Intent(this, typeof(RegisterActivity));
            StartActivityForResult(registerActivity, 0);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            EditText tbUsername = FindViewById<EditText>(Resource.Id.editTextUsername);
            EditText tbPassword = FindViewById<EditText>(Resource.Id.editTextPassword);

            var dialog = new AlertDialog.Builder(this);
            AuthenticationService.AuthenticateUser(tbUsername.Text, tbPassword.Text);
            if (AuthenticationService.LoggedUser != null)
            {
                var mainIntent = new Intent(this, typeof(MainActivity));
                StartActivity(mainIntent);
            }
            else
            {
                dialog.SetMessage("Wrong username/password!");
                dialog.SetPositiveButton("Okay!", delegate { });
                dialog.Show();
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok && requestCode == 0)
            {
                var dialog = new AlertDialog.Builder(this);
                dialog.SetMessage("Registration successful!");
                dialog.SetPositiveButton("Okay!", delegate { });
                dialog.Show();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            EditText tbUsername = FindViewById<EditText>(Resource.Id.editTextUsername);
            EditText tbPassword = FindViewById<EditText>(Resource.Id.editTextPassword);
            tbUsername.Text = "";
            tbPassword.Text = "";
            AuthenticationService.LoggedUser = null;

        }
    }
}


