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
using Android.Graphics;

namespace PhoneBook.Droid.CustomListView
{
    class ContactsViewAdapter : ArrayAdapter
    {
        private Context c;
        private List<Contact> contacts;
        private LayoutInflater inflater;
        private int resource;
        public ContactsViewAdapter(Context context, int resource, List<Contact> contacts) : base(context, resource, contacts)
        {
            this.c = context;
            this.resource = resource;
            this.contacts = contacts;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            
            if (inflater == null)
            {
                inflater = (LayoutInflater)c.GetSystemService(Context.LayoutInflaterService);
            }
            if (convertView == null)
            {
                convertView = inflater.Inflate(resource, parent, false);
            }
            ViewHolder holder = new ViewHolder(convertView)
            {
                NameTxt = { Text = contacts[position].FirstName }
            };
            if (contacts[position].ImageURI != null)
            {
                Android.Net.Uri imageURI = Android.Net.Uri.Parse(contacts[position].ImageURI);
                holder.Img.SetImageURI(imageURI);
            }
            else
            {
                holder.Img.SetImageResource(Resource.Drawable.def);
            }
            
            //convertView.SetBackgroundColor(Color.Rgb(44,44,44));
            if (position % 2 == 0)
            {
                //convertView.SetBackgroundColor(Color.LightGreen);
            }
            return convertView;
        }
        
    }
}