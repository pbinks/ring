using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Kerv.Common;

namespace Kerv.Droid
{
    public class DeviceAdaptor : BaseAdapter<Device>
    {
        public DeviceAdaptor(Context context)
        {
            this.context = context;
            this.devices = new List<Device>();
        }


        private readonly Context context;

        private List<Device> devices;
        public List<Device> Devices {
            get => devices;
            set {
                devices = value;
                NotifyDataSetChanged();
            }
        }

        public override Device this[int position] => devices[position];

        public override int Count => devices.Count;

        public override long GetItemId(int position)
        {
            return devices[position].ID;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null) {
                var inflater =
                    context.GetSystemService(Activity.LayoutInflaterService)
                           as LayoutInflater;
                convertView =
                    inflater.Inflate(Android.Resource.Layout.SimpleSpinnerItem, parent, false);
            }

            TextView descriptionView = convertView.FindViewById<TextView>(Android.Resource.Id.Text1);
            descriptionView.Text = devices[position].ShortDescription;
            return convertView;
        }

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            if (convertView == null)
            {
                var inflater =
                    context.GetSystemService(Activity.LayoutInflaterService)
                           as LayoutInflater;
                convertView =
                    inflater.Inflate(Android.Resource.Layout.SimpleDropDownItem1Line, parent, false);
            }

            TextView descriptionView = convertView.FindViewById<TextView>(Android.Resource.Id.Text1);
            descriptionView.Text = devices[position].Description;
            return convertView;
        }

    }
}
