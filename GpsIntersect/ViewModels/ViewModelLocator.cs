using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GpsIntersect.Dialogs;
using GpsIntersect.Xml;

namespace GpsIntersect.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Register dependencies.
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<IOpenFileDialog, OpenFileDialog>();
            SimpleIoc.Default.Register<ISaveFileDialog, SaveFileDialog>();
            SimpleIoc.Default.Register<IIMessageBoxDialog, MessageBoxDialog>();
            SimpleIoc.Default.Register<IGpxReader, GpxReader>();
            SimpleIoc.Default.Register<IGpxWriter, GpxWriter>();
            SimpleIoc.Default.Register<IKmlReader, KmlReader>();
            SimpleIoc.Default.Register<IKmlWriter, KmlWriter>();
            SimpleIoc.Default.Register<IXmlIntersectionWriter, XmlIntersectionWriter>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
    }
}