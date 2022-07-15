using System.ComponentModel;

namespace VSI.EDGEAXConnector.ECommerceDataModels
{
    public partial class EComCategory
    {
        private int category_idField;
        private EComCategory[] childrenField;
        private int is_activeField;
        private int levelField;
        private string nameField;
        private int parent_idField;
        private int positionField;

        /// <remarks />
        public int category_id
        {
            get { return category_idField; }
            set
            {
                category_idField = value;
                RaisePropertyChanged("category_id");
            }
        }

        /// <remarks />
        public int parent_id
        {
            get { return parent_idField; }
            set
            {
                parent_idField = value;
                RaisePropertyChanged("parent_id");
            }
        }

        /// <remarks />
        public string name
        {
            get { return nameField; }
            set
            {
                nameField = value;
                RaisePropertyChanged("name");
            }
        }

        /// <remarks />
        public int is_active
        {
            get { return is_activeField; }
            set
            {
                is_activeField = value;
                RaisePropertyChanged("is_active");
            }
        }

        /// <remarks />
        public int position
        {
            get { return positionField; }
            set
            {
                positionField = value;
                RaisePropertyChanged("position");
            }
        }

        /// <remarks />
        public int level
        {
            get { return levelField; }
            set
            {
                levelField = value;
                RaisePropertyChanged("level");
            }
        }

        /// <remarks />
        public EComCategory[] children
        {
            get { return childrenField; }
            set
            {
                childrenField = value;
                RaisePropertyChanged("children");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            var propertyChanged = PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}