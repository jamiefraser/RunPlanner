using RunPlanner.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Data;

namespace RunPlanner.UWP.ValueConverters
{
    public class RunPointsToGeoPath : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var points = value as IEnumerable<RunPoint>;
            if (points.Count() == 0) return null;
            List<BasicGeoposition> geoPoints = new List<BasicGeoposition>();
            foreach(RunPoint p in points)
            {
                geoPoints.Add(new BasicGeoposition()
                {
                    Latitude = p.Latitude,
                    Longitude = p.Longitude
                });
            }
            var path = new Geopath(geoPoints);
            return path;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
