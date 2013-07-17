<<<<<<< HEAD
﻿using OneBusAway.DataAccess;
using OneBusAway.Model;
using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OneBusAway.PageControls
{
    /// <summary>
    /// Page control for the trip details page.
    /// </summary>
    public sealed partial class TripDetailsPageControl : UserControl, IPageControl
    {
        /// <summary>
        /// The view model for the trip details.
        /// </summary>
        private TripDetailsPageControlViewModel viewModel;

        /// <summary>
        /// Creates the control.
        /// </summary>
        public TripDetailsPageControl()
        {
            this.InitializeComponent();

            this.viewModel = new TripDetailsPageControlViewModel();
            this.viewModel.MapControlViewModel.StopSelected += OnMapControlViewModelStopSelected;
            this.viewModel.TripTimelineControlViewModel.StopSelected += OnTripTimelineControlViewModelStopSelected;
        }

        /// <summary>
        /// Returns the view model.
        /// </summary>
        public PageViewModelBase ViewModel
        {
            get 
            {
                return this.viewModel;
            }
        }

        /// <summary>
        /// Initializes the page.
        /// </summary>
        public async Task InitializeAsync(object parameter)
        {
            var tripViewModel = this.viewModel.TripTimelineControlViewModel;
            var mapViewModel = this.viewModel.MapControlViewModel;

            TrackingData trackingData = (TrackingData)parameter;
            tripViewModel.TrackingData = trackingData;

            // get the trip details:
            await tripViewModel.GetTripDetailsAsync();
            tripViewModel.SelectStop(trackingData.StopId);

            // Copy bus data into the map control:
            mapViewModel.BusStops = null;
            mapViewModel.BusStops = new BusStopList(tripViewModel.TripDetails.TripStops.Cast<Stop>());
            mapViewModel.SelectStop(trackingData.StopId, true);
            await mapViewModel.FindRouteShapeAsync(trackingData.RouteId, trackingData.StopId);
        }

        /// <summary>
        /// Restores the page.
        /// </summary>
        public Task RestoreAsync()
        {
            this.viewModel.MapControlViewModel.MapView.AnimateChange = true;
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Refresh the trip details.
        /// </summary>
        public async Task RefreshAsync()
        {
            try
            {
                await this.viewModel.TripTimelineControlViewModel.GetTripDetailsAsync();
            }
            catch (ObaException)
            {
                // This trip is over. Just ignore the exception, the user will see that the bus is at the end.
            }
        }

        /// <summary>
        /// Update the selected stop on the map view model.
        /// </summary>
        private void OnMapControlViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            this.viewModel.TripTimelineControlViewModel.SelectStop(e.SelectedStopId);
            this.tripTimelineControl.ScrollToSelectedTripStop();
        }

        /// <summary>
        /// Update the selected stop on the trip details view model.
        /// </summary>
        private void OnTripTimelineControlViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            var mapViewModel = this.viewModel.MapControlViewModel;
            mapViewModel.SelectStop(e.SelectedStopId);

            var mapCenter = mapViewModel.MapView;
            mapViewModel.MapView = new MapView(new Model.Point(e.Latitude, e.Longitude), mapCenter.ZoomLevel, true);
        }

        /// <summary>
        /// Pages should be represent themselves as a string of parameters.
        /// </summary>
        public PageInitializationParameters GetParameters()
        {
            throw new NotImplementedException();
        }
    }
}
=======
﻿using OneBusAway.DataAccess;
using OneBusAway.DataAccess.ObaService;
using OneBusAway.Model;
using OneBusAway.ViewModels;
using OneBusAway.ViewModels.PageControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OneBusAway.PageControls
{
    /// <summary>
    /// Page control for the trip details page.
    /// </summary>
    public sealed partial class TripDetailsPageControl : UserControl, IPageControl
    {
        /// <summary>
        /// The view model for the trip details.
        /// </summary>
        private TripDetailsPageControlViewModel viewModel;

        /// <summary>
        /// Creates the control.
        /// </summary>
        public TripDetailsPageControl()
        {
            this.InitializeComponent();

            this.viewModel = new TripDetailsPageControlViewModel();
            this.viewModel.MapControlViewModel.StopSelected += OnMapControlViewModelStopSelected;
            this.viewModel.TripTimelineControlViewModel.StopSelected += OnTripTimelineControlViewModelStopSelected;
        }

        /// <summary>
        /// Returns the view model.
        /// </summary>
        public PageViewModelBase ViewModel
        {
            get 
            {
                return this.viewModel;
            }
        }

        /// <summary>
        /// Initializes the page.
        /// </summary>
        public async Task InitializeAsync(object parameter)
        {
            var tripViewModel = this.viewModel.TripTimelineControlViewModel;
            var mapViewModel = this.viewModel.MapControlViewModel;

            TrackingData trackingData = (TrackingData)parameter;
            tripViewModel.TrackingData = trackingData;

            // get the trip details:
            await tripViewModel.GetTripDetailsAsync();
            tripViewModel.SelectStop(trackingData.StopId);

            // Copy bus data into the map control:
            mapViewModel.BusStops = null;
            mapViewModel.BusStops = new BusStopList(tripViewModel.TripDetails.TripStops.Cast<Stop>());
            mapViewModel.SelectStop(trackingData.StopId, true);
            await mapViewModel.FindRouteShapeAsync(trackingData.RouteId, trackingData.StopId);
        }

        /// <summary>
        /// Restores the page.
        /// </summary>
        public Task RestoreAsync()
        {
            this.viewModel.MapControlViewModel.MapView.AnimateChange = true;
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Refresh the trip details.
        /// </summary>
        public async Task RefreshAsync()
        {
            try
            {
                await this.viewModel.TripTimelineControlViewModel.GetTripDetailsAsync();

                // Find the closest stop in the trip timeline:
                var closestStop = (from tripStop in this.viewModel.TripTimelineControlViewModel.TripDetails.TripStops
                                   where tripStop.IsClosestStop
                                   select tripStop).FirstOrDefault();

                // OBA doesn't always know where the bus is:
                foreach (var busStop in this.viewModel.MapControlViewModel.BusStops)
                {
                    busStop.IsClosestStop = (closestStop != null && string.Equals(closestStop.StopId, busStop.StopId, StringComparison.OrdinalIgnoreCase));
                }
            }
            catch (ObaException)
            {
                // This trip is over. Just ignore the exception, the user will see that the bus is at the end.
            }
        }

        /// <summary>
        /// Update the selected stop on the map view model.
        /// </summary>
        private void OnMapControlViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            this.viewModel.TripTimelineControlViewModel.SelectStop(e.SelectedStopId);
            this.tripTimelineControl.ScrollToSelectedTripStop();
        }

        /// <summary>
        /// Update the selected stop on the trip details view model.
        /// </summary>
        private void OnTripTimelineControlViewModelStopSelected(object sender, StopSelectedEventArgs e)
        {
            var mapViewModel = this.viewModel.MapControlViewModel;
            mapViewModel.SelectStop(e.SelectedStopId);

            var mapCenter = mapViewModel.MapView;
            mapViewModel.MapView = new MapView(new Model.Point(e.Latitude, e.Longitude), mapCenter.ZoomLevel, true);
        }

        /// <summary>
        /// Pages should be represent themselves as a string of parameters.
        /// </summary>
        public PageInitializationParameters GetParameters()
        {
            throw new NotImplementedException();
        }
    }
}
>>>>>>> 5fa5eaa... Re-arranging the view models by moving page controls & user control view models into sudifferent namepsaces / folders.
