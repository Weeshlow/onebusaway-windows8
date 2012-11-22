﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneBusAway.Model
{
    /// <summary>
    /// Data structure that represents a route and it's scheduled time vs predicted arrival time.
    /// </summary>
    public class TrackingData : BindableBase
    {
        private string routeId;
        private string tripId;
        private int scheduledArrivalInMinutes;
        private int predictedArrivalInMinutes;

        public TrackingData()
        {
        }

        /// <summary>
        /// Creates the tracking data out of an arrival and departure element.
        /// </summary>
        public TrackingData(DateTime serverTime, XElement arrivalAndDepartureElement)
        {
            this.RouteId = arrivalAndDepartureElement.GetFirstElementValue<string>("routeId");
            this.TripId = arrivalAndDepartureElement.GetFirstElementValue<string>("tripId");

            DateTime scheduledArrivalDateTime = arrivalAndDepartureElement.GetFirstElementValue<long>("scheduledArrivalTime").ToDateTime();
            this.ScheduledArrivalInMinutes = (scheduledArrivalDateTime - serverTime).Minutes;

            long predictedTime = arrivalAndDepartureElement.GetFirstElementValue<long>("predictedArrivalTime");
            if (predictedTime > 0)
            {
                DateTime predictedArrivalDateTime = predictedTime.ToDateTime();
                this.PredictedArrivalInMinutes = (predictedArrivalDateTime - serverTime).Minutes;
            }
            else
            {
                this.PredictedArrivalInMinutes = this.scheduledArrivalInMinutes;
            }
        }

        public string RouteId
        {
            get
            {
                return this.routeId;
            }
            set
            {
                SetProperty(ref this.routeId, value);
            }
        }

        public string TripId
        {
            get
            {
                return this.tripId;
            }
            set
            {
                SetProperty(ref this.tripId, value);
            }
        }

        public int ScheduledArrivalInMinutes
        {
            get
            {
                return this.scheduledArrivalInMinutes;
            }
            set
            {
                SetProperty(ref this.scheduledArrivalInMinutes, value);
            }
        }

        public int PredictedArrivalInMinutes
        {
            get
            {
                return this.predictedArrivalInMinutes;
            }
            set
            {
                SetProperty(ref this.predictedArrivalInMinutes, value);
            }
        }
    }
}