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
    [Activity(Label = "Register")]
    public class RegisterActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Register);

            EditText tbFirstName = FindViewById<EditText>(Resource.Id.editTextFirstName);
            EditText tbLastName = FindViewById<EditText>(Resource.Id.editTextLastName);
            EditText tbUsername = FindViewById<EditText>(Resource.Id.editTextUsername);
            EditText tbPassword = FindViewById<EditText>(Resource.Id.editTextPassword);

            Button btnRegister = FindViewById<Button>(Resource.Id.btnRegister);

            btnRegister.Click += delegate
            {
                User u = new User();
                u.FirstName = tbFirstName.Text;
                u.LastName = tbLastName.Text;
                u.Username = tbUsername.Text;
                u.Password = tbPassword.Text;

                UsersRepository repo = new UsersRepository();
                repo.Save(u);

                var okMessage = new AlertDialog.Builder(this);
                okMessage.SetMessage("User registered successfully!");
                okMessage.SetPositiveButton("OK!", delegate { });
                okMessage.Show();

                //returns result success when the intent is finished
                Intent intentResult = new Intent(this, typeof(LoginActivity));
                SetResult(Result.Ok, intentResult);
                Finish();

            };
        }
        
    }
}