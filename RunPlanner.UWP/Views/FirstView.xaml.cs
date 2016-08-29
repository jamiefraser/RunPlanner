using MvvmCross.WindowsUWP.Views;
using RunPlanner.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MvvmCross.Binding.Parse.Binding.Lang;
using MvvmCross.Binding.BindingContext;
using RunPlanner.Core.Entities;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Storage.Streams;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace RunPlanner.UWP.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FirstView : MvxWindowsPage
    {
        FirstViewModel vm;
        public FirstView()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            vm = this.ViewModel as FirstViewModel;
            vm.PropertyChanged += Vm_PropertyChanged;
        }

        private async void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentLocation")
            {

                this.RouteMap.Center = new Geopoint(new BasicGeoposition() { Latitude = vm.CurrentLocation.Latitude, Longitude = vm.CurrentLocation.Longitude });
                MapIcon currentlocation = new MapIcon()
                {
                    Location = this.RouteMap.Center,
                    Image= RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/location.png"))
                };
                currentlocation.Visible = true;
                currentlocation.CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible;
                this.RouteMap.MapElements.Add(currentlocation);
                await RouteMap.TryZoomToAsync(18);
                vm.PropertyChanged -= Vm_PropertyChanged;
                vm.PropertyChanged += HandlePinsChanged;
            }
        }

        private async void HandlePinsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Pins") return;
            for (int i = 0; i < RouteMap.MapElements.Count; i++)
            {
                if (RouteMap.MapElements[i].GetType().Equals(typeof(MapPolyline)))
                {
                    RouteMap.MapElements.Remove(RouteMap.MapElements[i]);
                }
            }
            var points = vm.Pins;
            if (points.Count() <=1) return;
            List<BasicGeoposition> geoPoints = new List<BasicGeoposition>();
            List<Geopoint> routePoints = new List<Geopoint>();
            foreach (RunPoint p in points.Where(point => point != null))
            {
                geoPoints.Add(new BasicGeoposition()
                {
                    Latitude = p.Latitude,
                    Longitude = p.Longitude
                });
            }
            foreach(var gp in geoPoints)
            {
                routePoints.Add(new Geopoint(gp));
            }
            var path = new Geopath(geoPoints);
            var route = await MapRouteFinder.GetWalkingRouteFromWaypointsAsync(routePoints);
            vm.ProposedRunLengthInKilometers = Math.Round(route.Route.LengthInMeters / 1000,2);
            /*List<BasicGeoposition> finalRoutePoints = new List<BasicGeoposition>();
            foreach(var leg in route.Route.Legs)
            {
                foreach(var p in leg.Path.Positions)
                {
                    finalRoutePoints.Add(p);
                }
            }
            var finalRoutePath = new Geopath(finalRoutePoints);*/
            MapPolyline line = new MapPolyline() { Path = route.Route.Path};
            line.StrokeColor = Windows.UI.Color.FromArgb(255, 255, 25, 150);
            line.StrokeDashed = false;
            line.StrokeThickness = 10;
            RouteMap.MapElements.Add(line);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            vm.PropertyChanged -= Vm_PropertyChanged;
        }
        public Geocoordinate Centre
        {
            get { return (Geocoordinate)GetValue(CentreProperty); }
            set { SetValue(CentreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Centre.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CentreProperty =
            DependencyProperty.Register("Centre", typeof(Geocoordinate), typeof(FirstView), new PropertyMetadata(null, new PropertyChangedCallback(OnCenterChanged)));

        private static void OnCenterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Geocoordinate c = e.NewValue as Geocoordinate;
            Geopoint point = new Geopoint(new BasicGeoposition() { Altitude = c.Point.Position.Altitude, Latitude = c.Point.Position.Latitude, Longitude = c.Point.Position.Longitude});
            (d as FirstView).RouteMap.Center = point;
        }
        private void map_Tapped(MapControl sender, MapInputEventArgs args)
        {
            System.Diagnostics.Debug.WriteLine(sender.GetType().Name);
            var selected = args.Location.Position;
            RunPoint p = new RunPoint()
            {
                Latitude = selected.Latitude,
                Longitude = selected.Longitude
            };
            vm.AddPointCommand.Execute(p);
        }
    }
}
