using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

using Tealium;

namespace ExampleAppConsentMgr
{
    public class ConsentManagerModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ConsentManager.ConsentStatus selectedStatus;
        private ObservableCollection<CategoryModel> selectedCategories;
        private bool isDirty;

        public ConsentManagerModel()
        {
            selectedCategories = new ObservableCollection<CategoryModel>();
            if (TealiumConsts.DefaultInstance.ConsentManager.IsConsentManagerEnabled)
            {
                SelectedStatus = TealiumConsts.DefaultInstance.ConsentManager.UserConsentStatus;

                foreach (var category in ConsentManager.AllCategories)
                {
                    CategoryModel model = new CategoryModel(category, IsConsentedToCategory(category));
                    model.PropertyChanged += Model_PropertyChanged;
                    selectedCategories.Add(model);
                }

                IsDirty = false;
            }
        }

        /// <summary>
        /// Handles the property changed event of each CategoryModel
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CategoryModel cat = (CategoryModel)sender;
            // If the selected status isnt Consented, then we should automatically update that.
            // There's no sense in opting into categories without Consenting.
            if (cat.OptedIn && selectedStatus != ConsentManager.ConsentStatus.Consented)
            {
                selectedStatus = ConsentManager.ConsentStatus.Consented;
                OnPropertyChanged(nameof(SelectedStatus));
            }
            IsDirty = true;

        }

        #region Getters/Setters

        /// <summary>
        /// Gets the consented status currently persisted in Tealium.
        /// </summary>
        /// <value>The consented status.</value>
        public ConsentManager.ConsentStatus ConsentedStatus
        {
            get
            {
                return TealiumConsts.DefaultInstance.ConsentManager.UserConsentStatus;
            }
        }

        /// <summary>
        /// Gets the consented categories persisted in Tealium.
        /// </summary>
        /// <value>The consented categories.</value>
        public ConsentManager.ConsentCategory[] ConsentedCategories
        {
            get
            {
                return TealiumConsts.DefaultInstance.ConsentManager.UserConsentCategories;
            }
        }

        /// <summary>
        /// Gets the consented categories persisted in Tealium as a String.
        /// </summary>
        /// <value>The consented categories.</value>
        public String ConsentedCategoriesString
        {
            get
            {
                return TealiumConsts.DefaultInstance.ConsentManager.UserConsentCategories.Length > 0 ? String.Join(", ", TealiumConsts.DefaultInstance.ConsentManager.UserConsentCategories) : "None";
            }
        }

        /// <summary>
        /// Gets or sets the selected status on the Model - call Save method to persist.
        /// </summary>
        /// <value>The selected status.</value>
        public ConsentManager.ConsentStatus SelectedStatus
        {
            get
            {
                return selectedStatus;
            }
            set
            {
                if (selectedStatus != value)
                {
                    IsDirty = true;
                    selectedStatus = value;
                    if (value == ConsentManager.ConsentStatus.NotConsented || value == ConsentManager.ConsentStatus.Unknown)
                    {
                        SelectedCategories = ConsentManager.NoCategories;
                    }
                    else if (value == ConsentManager.ConsentStatus.Consented)
                    {
                        SelectedCategories = ConsentManager.AllCategories;
                    }

                    OnPropertyChanged(nameof(SelectedStatus));
                }
            }
        }

        /// <summary>
        /// Gets an Observable Collection representing the selected Categories in the model.
        /// </summary>
        /// <value>The selected categories view.</value>
        public ObservableCollection<CategoryModel> SelectedCategoriesView
        {
            get
            {
                return selectedCategories;
            }
        }

        /// <summary>
        /// Gets or sets the selected categories on the Model - call Save method to persist.
        /// </summary>
        /// <value>The selected categories.</value>
        public ConsentManager.ConsentCategory[] SelectedCategories
        {
            get
            {
                List<ConsentManager.ConsentCategory> cats = new List<ConsentManager.ConsentCategory>();
                foreach (var cat in selectedCategories)
                {
                    if (cat.OptedIn)
                    {
                        cats.Add(cat.Name);
                    }
                }
                return cats.ToArray();
            }
            set
            {
                foreach (var cat in selectedCategories)
                {
                    cat.OptedIn = Array.IndexOf(value, cat.Name) > -1;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:ExampleAppConsentMgr.ConsentManagerModel"/> is dirty.
        /// </summary>
        /// <value><c>true</c> if model is dirty; otherwise, <c>false</c>.</value>
        public bool IsDirty
        {
            get
            {
                return isDirty;
            }
            private set
            {
                isDirty = value;
                OnPropertyChanged("IsDirty");
            }
        }

        #endregion Getters/Setters

        #region Persistence Methods

        /// <summary>
        /// Resets the Consent status/categories persisted in Tealium.
        /// Also resets the unsaved values on the Model.
        /// </summary>
        public void Reset()
        {
            TealiumConsts.DefaultInstance.ConsentManager.ResetUserConsentPreferences();

            SelectedStatus = TealiumConsts.DefaultInstance.ConsentManager.UserConsentStatus;
            UpdateSelectedCategories();

            OnPropertyChanged(nameof(ConsentedStatus));
            OnPropertyChanged(nameof(ConsentedCategoriesString));

            IsDirty = false;
        }

        /// <summary>
        /// Persists any unsaved changes on the Model into the Tealium Instance.
        /// Only after calling this method will the changes be written, and therefore
        /// your tracking will be unaffected until after this method.
        /// </summary>
        public void Save()
        {
            if (SelectedStatus != TealiumConsts.DefaultInstance.ConsentManager.UserConsentStatus && !ArraysAreEqual(TealiumConsts.DefaultInstance.ConsentManager.UserConsentCategories, SelectedCategories))
            {
                TealiumConsts.DefaultInstance.ConsentManager.UserConsentStatusWithCategories(SelectedStatus, SelectedCategories);
            }
            else if (SelectedStatus != TealiumConsts.DefaultInstance.ConsentManager.UserConsentStatus)
            {
                TealiumConsts.DefaultInstance.ConsentManager.UserConsentStatus = SelectedStatus;
            }
            else if (!ArraysAreEqual(SelectedCategories, TealiumConsts.DefaultInstance.ConsentManager.UserConsentCategories))
            {
                TealiumConsts.DefaultInstance.ConsentManager.UserConsentCategories = SelectedCategories;
            }

            OnPropertyChanged(nameof(ConsentedStatus));
            OnPropertyChanged(nameof(ConsentedCategoriesString));

            IsDirty = false;
        }

        #endregion Persistence Methods

        #region Helper Methods

        /// <summary>
        /// Helper method to raise a PropertyChanged event for binding purposes.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected void OnPropertyChanged(String propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Utility method for comparing the underlying Tealium Consent and the Unsaved Model Consent categories
        /// </summary>
        /// <returns><c>true</c>, if arrays are equal, <c>false</c> otherwise.</returns>
        /// <param name="arr1">Arr1.</param>
        /// <param name="arr2">Arr2.</param>
        private bool ArraysAreEqual(ConsentManager.ConsentCategory[] arr1, ConsentManager.ConsentCategory[] arr2)
        {
            // Lengths don't match, they can't be equal.
            if (arr1.Length != arr2.Length)
            {
                return false;
            }

            foreach (var c in arr1)
            {
                if (Array.IndexOf(arr2, c) == -1)
                {
                    // category not found in second array.
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Helper method to confirm if the persisted category is consented to.
        /// </summary>
        /// <returns><c>true</c>, if the category is consented to, <c>false</c> otherwise.</returns>
        /// <param name="name">Name.</param>
        private bool IsConsentedToCategory(ConsentManager.ConsentCategory name)
        {
            ConsentManager.ConsentCategory[] consents = TealiumConsts.DefaultInstance.ConsentManager.UserConsentCategories;
            for (int i = 0; i < consents.Length; i++)
            {
                if (consents[i] == name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Resynchronises the unsaved model categories with the persisted values.
        /// Useful after a Reset has taken place.
        /// </summary>
        private void UpdateSelectedCategories()
        {
            foreach (var cat in selectedCategories)
            {
                cat.OptedIn = IsConsentedToCategory(cat.Name);
            }
        }

        #endregion Helper Methods

    }
}
