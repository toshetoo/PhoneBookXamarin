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
	[Activity (Label = "PhoneBook", MainLauncher = true, Icon = "@drawable/icon")]
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

            btnLogin.Click += delegate
            {
                AuthenticationService.AuthenticateUser(tbUsername.Text,tbPassword.Text);
                if (AuthenticationService.LoggedUser != null)
                {
                    var dialog = new AlertDialog.Builder(this);
                    dialog.SetMessage("Login successful!");
                    dialog.SetPositiveButton("Okay!", delegate { });
                    dialog.Show();
                    
                    var mainIntent = new Intent(this, typeof(MainActivity));
                    StartActivity(mainIntent);
                }
            };

            btnRegister.Click += delegate
            {
                var registerActivity = new Intent(this,typeof(RegisterActivity));
                StartActivityForResult(registerActivity, 0);
               
            };
		}

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
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


