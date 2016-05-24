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
    class PhonesViewAdapter: ArrayAdapter
    {
        private Context c;
        private List<Phone> phones;
        private LayoutInflater inflater;
        private int resource;
        public PhonesViewAdapter(Context context, int resource, List<Phone> phones) : base(context, resource, phones)
        {
            this.c = context;
            this.resource = resource;
            this.phones = phones;
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
                NameTxt = { Text = phones[position].Number }
            };
            switch (phones[position].Type)
            {
                case "Mobile": holder.Img.SetImageResource(Resource.Drawable.mobile); break;
                case "Home": holder.Img.SetImageResource(Resource.Drawable.home); break;
                case "Fax": holder.Img.SetImageResource(Resource.Drawable.def); break;
                default:
                    break;
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