using System;
using System.ComponentModel;

using Tealium;

namespace ExampleAppConsentMgr
{
    public class CategoryModel : INotifyPropertyChanged
    {
        private ConsentManager.ConsentCategory name;
        private bool optedIn;

        public CategoryModel(ConsentManager.ConsentCategory name, bool optedIn)
        {
            this.name = name;
            this.optedIn = optedIn;
        }

        public ConsentManager.ConsentCategory Name
        {
            get => this.name;
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        public bool OptedIn
        {
            get => this.optedIn;
            set
            {
                if (this.optedIn != value)
                {
                    this.optedIn = value;
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(OptedIn)));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
