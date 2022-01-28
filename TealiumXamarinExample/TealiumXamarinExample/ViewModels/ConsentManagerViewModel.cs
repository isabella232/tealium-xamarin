using System;
using System.ComponentModel;
using System.Windows.Input;

using System.Collections.Generic;

using System.Collections.ObjectModel;

using static Tealium.ConsentManager;
using TealiumXamarinExample.Models;
using Tealium;
using Xamarin.Forms;

namespace TealiumXamarinExample.ViewModels
{
    public interface IConsentManagerViewModel : INotifyPropertyChanged, IDisposable
    {
        ConsentStatus UserConsentStatus { get; }
        ConsentCategory[] UserConsentCategories { get; }
        ObservableCollection<CategoryModel> Categories { get; }

        ICommand CallToggleConsented { get; }
        ICommand CallResetConsent { get; }
    }

    public class ConsentManagerViewModel : BaseViewModel, IConsentManagerViewModel
    {
        readonly ITealium tealium;

        ICommand toggleConsented;
        ICommand resetConsented;

        //Binding object for the ListView.
        private ObservableCollection<CategoryModel> categories = new ObservableCollection<CategoryModel>();

        // Switching the switches will auto-save. On cases where Consent has been dropped, we'll need to reset the bound collection to opt people out.
        // However, on refusing/resetting consent, the SDK will auto update the list of Categories, so we don't want to save anything on a binding-updated event.
        bool shouldSave = true;


        public ConsentManagerViewModel()
        {
            Title = "ConsentManager";

            this.tealium = Teal.Helper.DefaultInstance;

            this.toggleConsented = new Command(ExecuteToggleConsented);
            this.resetConsented = new Command(ExecuteResetConsent);

            foreach (var category in ConsentManager.AllCategories)
            {
                CategoryModel model = new CategoryModel(category, IsConsentedToCategory(category));
                model.PropertyChanged += Model_PropertyChanged;
                categories.Add(model);
            }

        }

        #region Getters and Setters

        /// <summary>
        /// Gets whether the Consent Manager is enabled - i.e. has a Consent
        /// Policy enforced.
        /// </summary>
        /// <value>The user consent status.</value>
        public bool IsConsentManagerEnabled
        {
            get => tealium.ConsentManager.IsConsentManagerEnabled;
        }

        /// <summary>
        /// Gets whether the Consent Manager is enabled - i.e. has a Consent
        /// Policy enforced.
        /// </summary>
        /// <value>The user consent status.</value>
        public string IsConsentEnabledString
        {
            get => tealium.ConsentManager.IsConsentManagerEnabled ? "enabled" : "disabled";
        }

        /// <summary>
        /// Gets whether the Consent Manager policy
        /// </summary>
        /// <value>The Consent Policy.</value>
        public string ConsentPolicy
        {
            get => tealium.ConsentManager.Policy?.ToString() ?? "none";
        }

        /// <summary>
        /// Gets the user consent status.
        /// </summary>
        /// <value>The user consent status.</value>
        public ConsentStatus UserConsentStatus
        {
            get => tealium.ConsentManager.UserConsentStatus;
            private set => OnPropertyChanged(nameof(UserConsentStatus));
        }

        /// <summary>
        /// Gets the user consent categories.
        /// </summary>
        /// <value>The user consent categories.</value>
        public ConsentCategory[] UserConsentCategories
        {
            get => tealium.ConsentManager.UserConsentCategories;
            private set
            {
                tealium.ConsentManager.UserConsentCategories = value;
                OnPropertyChanged(nameof(UserConsentStatus));
            }
        }

        /// <summary>
        /// Gets the Model Categories collections.
        /// </summary>
        /// <value>The categories.</value>
        public ObservableCollection<CategoryModel> Categories
        {
            get => this.categories;

        }

        #endregion Getters and Setters

        #region Category Model functions

        /// <summary>
        /// Optin switch handler - controls whether or not these changes should be auto-saved or not.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Only interested in optedin property changing.
            if (e.PropertyName.ToLower() != "optedin") return;

            String message = ((CategoryModel)sender).Name + " Switch " + (((CategoryModel)sender).OptedIn ? "Enabled" : "Disabled");
            System.Diagnostics.Debug.WriteLine(message);

            // Global shouldSave flag to determine whether or not these updates should get pushed back into the SDK.
            if (this.shouldSave)
            {
                UserConsentCategories = GetCategoryModelSelection();
            }
        }

        private void SetCategoryModelSelections(ConsentManager.ConsentCategory[] cats)
        {
            try
            {
                // This could potentially triggere quite a few update events for the bindings...
                // The SDK will automatically take care of any Consent Resets - Categories will be made empty.
                // so we don't want to save each time as it will actually overwrite those changes.
                this.shouldSave = false;

                foreach (var cat in categories)
                {
                    bool IsConsented = false;
                    for (int i = 0; i < cats.Length; i++)
                    {
                        if (cats[i] == cat.Name)
                        {
                            IsConsented = true;
                        }
                    }
                    cat.OptedIn = IsConsented;
                }
            }
            finally
            {
                this.shouldSave = true;
            }
        }

        private ConsentManager.ConsentCategory[] GetCategoryModelSelection()
        {
            // New List to hold out currently selected categories.
            List<ConsentManager.ConsentCategory> selections = new List<ConsentManager.ConsentCategory>();

            foreach (var cat in categories)
            {
                if (cat.OptedIn)
                {
                    // model is opted in; add it to our Selections list.
                    selections.Add(cat.Name);
                }
            }

            // Return our selections or an empty array.
            return selections.Count > 0 ? selections.ToArray() : ConsentManager.NoCategories;
        }

        private bool IsConsentedToCategory(ConsentManager.ConsentCategory name)
        {
            ConsentManager.ConsentCategory[] consents = tealium.ConsentManager.UserConsentCategories;
            for (int i = 0; i < consents.Length; i++)
            {
                if (consents[i] == name)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion Category Model functions

        public void Dispose()
        {
            // throw new NotImplementedException();
        }

        #region Button command bindings

        public ICommand CallToggleConsented { get => this.toggleConsented; }
        void ExecuteToggleConsented()
        {
            switch (tealium.ConsentManager.UserConsentStatus)
            {
                case ConsentStatus.Unknown:
                case ConsentStatus.NotConsented:
                    tealium.ConsentManager.UserConsentStatus = ConsentStatus.Consented;
                    SetCategoryModelSelections(ConsentManager.AllCategories);
                    break;
                case ConsentStatus.Consented:
                    tealium.ConsentManager.UserConsentStatus = ConsentStatus.NotConsented;
                    SetCategoryModelSelections(ConsentManager.NoCategories);
                    break;
            }

            this.UserConsentStatus = tealium.ConsentManager.UserConsentStatus;
        }

        public ICommand CallResetConsent { get => this.resetConsented; }
        void ExecuteResetConsent()
        {
            // update the Tealium SDK values
            this.tealium.ConsentManager.ResetUserConsentPreferences();
            this.UserConsentStatus = tealium.ConsentManager.UserConsentStatus;

            // Update the model as well.
            SetCategoryModelSelections(ConsentManager.NoCategories);
        }

        #endregion Button command bindings

    }
}
