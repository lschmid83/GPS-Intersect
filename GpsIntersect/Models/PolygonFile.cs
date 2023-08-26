using GalaSoft.MvvmLight;
using GpsIntersect.Maths;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace GpsIntersect.Models
{
	public class PolygonFile : ViewModelBase
    {
		[XmlIgnore]
		public string Filename { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public ObservableCollection<Polygon> Polygons { get; set; }

		[XmlIgnore]
		public bool IsExpanded
		{
			get { return isExpanded; }
			set
			{
				if (value != isExpanded)
				{
					isExpanded = value;
					RaisePropertyChanged("IsExpanded");
				}
			}
		}
		private bool isExpanded;

		public PolygonFile() { }

		public PolygonFile(string filename, string name, string description, ObservableCollection<Polygon> polygons)
		{
			Filename = filename;
			Name = name;
			Description = description;
			Polygons = polygons;
		}

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
