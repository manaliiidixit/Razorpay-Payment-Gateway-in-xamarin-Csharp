using Android.App;
using Android.OS;
using Android.Widget;
using SQLite;
using Android.Database.Sqlite;
using Android.Telecom;

namespace DemoApp
{
    [Activity(Label = "Main")]
    public class MainActivity : Activity
    {
        EditText user_name, address, city, mobileno, user_id;
        Button btnsubmit, btnshow, btnupdate;

       
        string Country_Code = "";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.MainLayout);
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Test.db3");
          
        }

        private void Btnsubmit_Click(object sender, System.EventArgs e)
        {
            
            

        }

        private void Btnshow_Click(object sender, System.EventArgs e)
        {
            

           
        }

        private void Btnupdate_Click(object sender, System.EventArgs e)
        {
            
        }
       
    }
}