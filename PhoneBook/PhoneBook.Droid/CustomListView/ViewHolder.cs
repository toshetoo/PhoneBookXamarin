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

namespace PhoneBook.Droid.CustomListView
{
    class ViewHolder
    {
        public TextView NameTxt;
        public ImageView Img;
        //public Button btn;
        public ViewHolder(View v)
        {
            this.NameTxt = v.FindViewById<TextView>(Resource.Id.nameTxt);            
            this.Img = v.FindViewById<ImageView>(Resource.Id.playerImg);
            //this.btn = v.FindViewById<Button>(Resource.Id.btnCall);
        }
    }
}