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

namespace PhoneBook.Droid.CustomListView
{
    class GroupsViewAdapter : ArrayAdapter
    {
        private Context c;
        private List<Group> groups;
        private LayoutInflater inflater;
        private int resource;
        public GroupsViewAdapter(Context context, int resource, List<Group> groups) : base(context, resource, groups)
        {
            this.c = context;
            this.resource = resource;
            this.groups = groups;
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
                NameTxt = { Text = groups[position].GroupName }
            };
            if (groups[position].ImageURI != null)
            {
                Android.Net.Uri imageURI = Android.Net.Uri.Parse(groups[position].ImageURI);
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