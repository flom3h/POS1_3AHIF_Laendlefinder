using System.Windows;
using System.Windows.Controls;
using Mapsui.UI.Wpf;
using Mapsui.Layers;
using Mapsui.Utilities;
using Mapsui.Projection;
using Mapsui;
using Mapsui.Geometries;

namespace Laendlefinder.Pages;

public partial class MapPage : Page
{
    public static event EventHandler HomeButtonClickedNavHome;
    public static event EventHandler ExploreButtonClickedNavExplore;
    public static event EventHandler CalendarButtonClickedNavCalendar;
    public static event EventHandler FavsButtonClickedNavFavs;
    public static event EventHandler MapButtonClickedNavMap;
    public static event EventHandler ProfileButtonClickedNavProfile;
    private readonly BoundingBox _vorarlbergEnvelope;

    public MapPage()
    {
        InitializeComponent();
        var tileLayer = OpenStreetMap.CreateTileLayer();
        MapView.Map?.Layers.Add(tileLayer);

        var min = SphericalMercator.FromLonLat(9.60, 46.88);
        var max = SphericalMercator.FromLonLat(10.23, 47.60);
        _vorarlbergEnvelope = new BoundingBox(min.X, min.Y, max.X, max.Y);

        MapView.Navigator.NavigateTo(_vorarlbergEnvelope, ScaleMethod.Fit, 0, null);

        MapView.Viewport.ViewportChanged += Viewport_ViewportChanged;
    }

    private bool _isResettingViewport = false;

    private void Viewport_ViewportChanged(object? sender, System.EventArgs e)
    {
        if (_isResettingViewport) return;

        var extent = MapView.Viewport.Extent;
        if (extent.MinX < _vorarlbergEnvelope.MinX ||
            extent.MinY < _vorarlbergEnvelope.MinY ||
            extent.MaxX > _vorarlbergEnvelope.MaxX ||
            extent.MaxY > _vorarlbergEnvelope.MaxY)
        {
            _isResettingViewport = true;
            MapView.Navigator.NavigateTo(_vorarlbergEnvelope, ScaleMethod.Fit, 0, null);
            _isResettingViewport = false;
        }
    }
    
    private void HomeButton_Click(object sender, RoutedEventArgs e)
    {
        HomeButtonClickedNavHome?.Invoke(this, EventArgs.Empty);
    }

    private void ExploreButton_Click(object sender, RoutedEventArgs e)
    {
        ExploreButtonClickedNavExplore?.Invoke(this, EventArgs.Empty);
    }

    private void CalndarButton_Click(object sender, RoutedEventArgs e)
    {
        CalendarButtonClickedNavCalendar?.Invoke(this, EventArgs.Empty);
    }

    private void FavsButton_Click(object sender, RoutedEventArgs e)
    {
        FavsButtonClickedNavFavs?.Invoke(this, EventArgs.Empty);
    }

    private void MapButton_Click(object sender, RoutedEventArgs e)
    {
        MapButtonClickedNavMap?.Invoke(this, EventArgs.Empty);
    }

    private void ProfileButton_Click(object sender, RoutedEventArgs e)
    {
        ProfileButtonClickedNavProfile?.Invoke(this, EventArgs.Empty);
        
        
    }
}