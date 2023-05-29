using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DemoApp
{
    [Activity(Label = "cameraActivity")]
    public class cameraActivity : Activity
    {
        Button btncamera;
        ImageView img;
        private static  int CAMERA_REQUEST = 1888;
        
        private static  int MY_CAMERA_PERMISSION_CODE = 100;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.cameraLayout);

            btncamera = FindViewById<Button>(Resource.Id.button1);
        //   img = FindViewById<Button>(Resource.Id.txtimg);

            btncamera.Click += Btncamera_Click;
            
        }

        private void Btncamera_Click(object sender, EventArgs e)
        {

            StartActivity(typeof(MainActivity));

            Intent cameraIntent = new Intent(Android.Provider.MediaStore.ActionImageCapture);// Android.provider.MediaStore.ACTION_IMAGE_CAPTURE);
                StartActivityForResult(cameraIntent, CAMERA_REQUEST);
            
        }
        
    }
}