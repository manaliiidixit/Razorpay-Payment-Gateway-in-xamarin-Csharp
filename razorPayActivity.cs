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
using Com.Razorpay;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Org.Json;
using Xamarin.Forms;
using Button = Xamarin.Android;

namespace DemoApp
{
    [Activity(Label = "razorPayActivity", MainLauncher = true)]
    public class razorPayActivity : Activity
    {
        Android.Widget.Button btnpay;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.razorpayLayout);
            btnpay = FindViewById<Android.Widget.Button>(Resource.Id.btnpay);
            btnpay.Click += Btnpay_Click;
           /* MessagingCenter.Subscribe<razorPayLoad>(this, "PayNow", (payload) =>
            {
                string username = "rzp_test_co1QTfvqLJyWXn";
                string password = "iAhjtNtHYHrQOQPE09X5XBGC";
                payviaRazor(payload, username, password);
            });
           */ //  MessagingCenter.Subscribe<razorPayLoad>(this,"payNow",(paylods)
        }

        private void Btnpay_Click(object sender, EventArgs e)
        {
            string usernm = "rzp_test_W5KzRbFecawzcB";
            string password = "bLX8NnLSzZ6piM0A99OT0zIk";
            razorPayLoad payLoad = new razorPayLoad();
            payLoad.amount = 1000;
            payLoad.currency = "INR";
            payLoad.receipt = RandomString(12);
            payviaRazor(payLoad, usernm, password);
            
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void OnPaymentError(int p0, string p1, PaymentData p2)
        {
           // throw new NotImplementedException();
        }

        public void OnPaymentSuccess(string p0, PaymentData p1)
        {
           // throw new NotImplementedException();
        }
        public async void payviaRazor(razorPayLoad payLoad,string username , string password)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.razorpay.com/v1/orders"))
                {
                    var plaintext = Encoding.UTF8.GetBytes($"{username}:{password}");
                    var baseAuthKey = Convert.ToBase64String(plaintext);

                    request.Headers.TryAddWithoutValidation("authorization", $"Basic {baseAuthKey}");
                    request.Headers.TryAddWithoutValidation("cache-control", "no-cache");
                    request.Headers.TryAddWithoutValidation("postman-token", "8a432bfe-ffc1-c811-ad89-5842e99e7584");

                    string jsonData = JsonConvert.SerializeObject(payLoad);

                    request.Content = new StringContent(jsonData);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

                    var response = await httpClient.SendAsync(request);
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    razorResponse razorRes = JsonConvert.DeserializeObject<razorResponse>(jsonResponse);
                    if(!string.IsNullOrEmpty(razorRes.id))
                    {
                        Checkout checkout = new Checkout();
                        checkout.SetImage(0);
                        checkout.SetKeyID(username);
                        try
                        {
                            JSONObject options = new JSONObject();

                            options.Put("name", "");
                            options.Put("description", "Reference No. #123456");
                            options.Put("image", "https://s3.amazonaws.com/rzp-mobile/images/rzp.png");
                            options.Put("order_id", razorRes.id);//from response of step 3.
                            options.Put("theme.color", "#3399cc");
                            options.Put("currency", "INR");
                            options.Put("amount", payLoad.amount);//pass amount in currency subunits
                            options.Put("prefill.email", "");
                            options.Put("prefill.contact", "");
                            JSONObject retryObj = new JSONObject();
                            retryObj.Put("enabled", true);
                            retryObj.Put("max_count", 4);
                            options.Put("retry", retryObj);

                            checkout.Open(this, options);

                        }
                        catch (Exception e)
                        {
                          //  Log.e(TAG, "Error in starting Razorpay Checkout", e);
                        }

                    }
                    else
                    {
                        Toast.MakeText(this, "Payment Failed", ToastLength.Long).Show();
                    }
                }
            }
        }
    }
}