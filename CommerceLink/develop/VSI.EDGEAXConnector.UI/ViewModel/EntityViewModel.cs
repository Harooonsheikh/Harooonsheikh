using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.UI.Helpers;

namespace VSI.EDGEAXConnector.UI.ViewModel
{
    public class EntityViewModel : BaseViewModel
    {
        private Type _selectedDestEntity;
        private Type _selectedSrcEntity;
        private Transformer _transformer;

        public EntityViewModel(MapDirection type)
        {
            try
            {
                this.Transformer = new Transformer();
                this.SourceProperties = new ObservableCollection<PropertyInfo>();
                this.Direction = type;
                ConfigureMap();
            }
            catch (Exception ex)
            {
                ExceptionHelper.ShowMessage(ex);
                CustomLogger.LogException(ex, "Console");
            }
        }

        public ObservableCollection<Type> DestEntities { get; set; }
        public ObservableCollection<Type> SrcEntities { get; set; }
        public ObservableCollection<PropertyInfo> SourceProperties { get; set; }
        public MapDirection Direction { get; set; }

        public Transformer Transformer
        {
            get { return _transformer; }
            set
            {
                _transformer = value;
                RaisePropertyChanged("Transformer");
            }
        }

        public Type SelectedDestEntity
        {
            get { return _selectedDestEntity; }
            set
            {
                _selectedDestEntity = value;
                this.Transformer.DestinationClass = value;
                this.Transformer.Properties = new ObservableCollection<TransformerProperty>();
                if (_selectedDestEntity != null)
                {
                    _selectedDestEntity.GetProperties()
                        .ToList()
                        .ForEach(p => { this.Transformer.Properties.Add(new TransformerProperty(p)); });
                    RaisePropertyChanged("Transformer");
                    RaisePropertyChanged("SelectedEntity");
                }
            }
        }

        public Type SelectedSrcEntity
        {
            get { return _selectedSrcEntity; }
            set
            {
                _selectedSrcEntity = value;
                this.Transformer.SourceClass = value;
                this.SourceProperties.Clear();
                if (_selectedSrcEntity != null)
                {
                    _selectedSrcEntity.GetProperties().ToList().ForEach(p => { this.SourceProperties.Add(p); });
                    RaisePropertyChanged("SelectedSrcEntity");

                }
            }
        }

        public void ConfigureMap()
        {
            if (this.Direction == MapDirection.ErpToEcom)
            {
                IClassInfo connectorInfo = new VSI.EDGEAXConnector.ECommerceDataModels.ClassInfo();
                this.DestEntities = new ObservableCollection<Type>(connectorInfo.GetClassesInfo().ToList().OrderBy(c=>c.Name).ToList());
                connectorInfo = new VSI.EDGEAXConnector.ERPDataModels.ClassInfo();
                this.SrcEntities = new ObservableCollection<Type>(connectorInfo.GetClassesInfo().ToList().OrderBy(c=>c.Name).ToList());
            }
            else if (this.Direction == MapDirection.EcomToErp)
            {
                IClassInfo connectorInfo = new VSI.EDGEAXConnector.ERPDataModels.ClassInfo();
                this.DestEntities = new ObservableCollection<Type>(connectorInfo.GetClassesInfo().ToList().OrderBy(c=>c.Name).ToList());
                connectorInfo = new VSI.EDGEAXConnector.ECommerceDataModels.ClassInfo();
                this.SrcEntities = new ObservableCollection<Type>(connectorInfo.GetClassesInfo().ToList().OrderBy(c=>c.Name).ToList());
            }
        }
    }
}