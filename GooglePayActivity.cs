using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoApp
{
    [Activity(Label = "GooglePayActivity")]
    public class GooglePayActivity : Activity
    {
        Button btnpay;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.googlePayLayout);
            btnpay = FindViewById<Button>(Resource.Id.btnpay);
            btnpay.Click += Btnpay_Click;
         
           
        }

        private void Btnpay_Click(object sender, EventArgs e)
        {
            try
            {
                string Amount = "1.0";
                long tslong = JavaSystem.CurrentTimeMillis() / 1000;
                //string transaction_ref_id = Guid.NewGuid().ToString().Substring(0, 8) + "UPI";
                string transaction_ref_id = "UPI"+ Guid.NewGuid().ToString().Substring(0, 8);
                string transaction_ref = Guid.NewGuid().ToString().Substring(0, 10);
                using (var uri = new Android.Net.Uri.Builder()
                    .Scheme("upi")
                    .Authority("pay")
                    .AppendQueryParameter("pa", "") // upi
                    .AppendQueryParameter("pn", "") // name
                    .AppendQueryParameter("pn", "Payment")
                    .AppendQueryParameter("mc", "BCR2DN6CN3VST")
                    .AppendQueryParameter("tr", transaction_ref_id)
                    .AppendQueryParameter("tn", "")
                    .AppendQueryParameter("am", Amount)
                    .AppendQueryParameter("cu", "INR")
                    .Build())
                {
                    Intent = new Intent(Intent.ActionView);
                    Intent.SetData(uri);
                    if (IsAppInstalledInYourPhone("com.google.android.apps.nbu.paisa.user"))
                    {
                        Intent.SetPackage("com.google.android.apps.nbu.paisa.user");
                        StartActivityForResult(Intent,9999);
                    }
                    else
                    {
                        var package = PackageName;
                        ShowToast("GPay is not installed in your device");
                    }
                }

            }
            catch (System.Exception ex)
            {
                ShowToast("Payment Failed");
            }
        }

        private bool IsAppInstalledInYourPhone(string packageName)
        {
            PackageManager pm = this.PackageManager;
            bool Installed = false;
            try
            {
                pm.GetPackageInfo(packageName, PackageInfoFlags.Activities);
                Installed = true;
            }
            catch (PackageManager.NameNotFoundException ex)
            {
                Installed = false;
            }
            return Installed;
        }
        private void ShowToast(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long).Show();
        }
        protected override void OnActivityResult(int RequestCode,Result resultcode,Intent data)
        {
            base.OnActivityResult(RequestCode, resultcode, data);
            {
                if(RequestCode == 9999)
                {
                   if(data.GetStringExtra("Status").Contains("Success"))
                    {
                        ShowToast("Success");
                    }
                    else
                    {
                        ShowToast("Payment Failed");
                    }
                }
            }
        }
    }
}