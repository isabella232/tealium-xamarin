using System;
using System.ComponentModel;
using Tealium;

namespace TealiumXamarinExample.Models
{

    public class CategoryModel : ModelObject
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
                    RaisePropertyChanged(nameof(Name));
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
                    RaisePropertyChanged(nameof(OptedIn));
                }
            }
        }


    }

    public class ModelObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
