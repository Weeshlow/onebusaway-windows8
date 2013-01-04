﻿using OneBusAway.Model;
using OneBusAway.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OneBusAway.Controls
{
    public sealed partial class TripTimelineControl : UserControl
    {
        public TripTimelineControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Called when the user selects a stop.
        /// </summary>
        private void OnStopClicked(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as TripTimelineControlViewModel;
            if (viewModel != null)
            {
                var stop = ((Button)sender).DataContext as TripStop;
                if (stop != null)
                {
                    viewModel.SelectNewStop(stop);
                }
            }
        }
    }
}
