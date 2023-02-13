using TealiumXamarinExample.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Tealium;
using Xamarin.Forms;
using TealiumXamarinExample.Teal;

namespace TealiumXamarinExample.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string email;
        private string visitorId;
        public Command LoginCommand { get; }
        public Command ClearStoredVisitorIdsCommand { get; }
        public Command ResetVisitorIdCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
            ClearStoredVisitorIdsCommand = new Command(ClearStoredVisitorIds);
            ResetVisitorIdCommand = new Command(ResetVisitorId);
            Email = (string)Helper.DefaultInstance.GetFromDataLayer("email");
            this.VisitorId = Helper.DefaultInstance.GetVisitorId();
            Helper.DefaultInstance.AddVisitorIdListener((id) =>
            {
                this.VisitorId = id;
            });
        }

        private void OnLoginClicked(object obj)
        {
            if (email == null || email == "")
            {
                Helper.DefaultInstance.RemoveFromDataLayer(new List<string> { "email" });
            }
            else
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("email", email);
                Helper.DefaultInstance.AddToDataLayer(dict, Expiry.Forever);
            }
        }

        private void ClearStoredVisitorIds(object obj)
        {
            Helper.DefaultInstance.ClearStoredVisitorIds();
        }

        private void ResetVisitorId(object obj)
        {
            Helper.DefaultInstance.ResetVisitorId();
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string VisitorId
        {
            get => visitorId;
            set => SetProperty(ref visitorId, value);
        }
    }
}
