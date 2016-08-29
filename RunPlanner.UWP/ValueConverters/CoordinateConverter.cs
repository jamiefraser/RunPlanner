using MvvmCross.Plugins.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Data;

namespace RunPlanner.UWP.ValueConverters
{
    public class CoordinateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            MvxCoordinates val = value as MvxCoordinates;
            if (val == null) return null;
            Geopoint point = new Geopoint(new BasicGeoposition() { Altitude = val.Altitude.HasValue ? val.Altitude.Value : 0d, Latitude = val.Latitude, Longitude = val.Longitude });
            return point;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Geopoint val = value as Geopoint;
            MvxCoordinates coordinate = new MvxCoordinates()
            {
                Latitude = val.Position.Latitude,
                Longitude = val.Position.Longitude,
                Altitude = val.Position.Altitude
            };
            return coordinate;
        }
    }
}
