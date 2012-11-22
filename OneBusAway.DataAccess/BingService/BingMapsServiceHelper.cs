﻿using OneBusAway.Model.BingService;
using OneBusAway.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneBusAway.DataAccess.BingService
{
    /// <summary>
    /// Class that provides helper methods to interface with the Bing Maps Service
    /// </summary>
    public class BingMapsServiceHelper
    {
        /// <summary>
        /// Queries the Bing Maps Service with a given search query and returns a list of locations that match the query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async Task<List<Location>> GetLocationByQuery(string query)
        {
            return await GetLocationByQuery(query, Confidence.Low);
        }

        /// <summary>
        /// Queries the Bing Maps Service with a given search query, confidence and userLocation and returns a list of locations that match the query
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="minimumConfidence">Minimum confidence by which to filter the search results</param>
        /// <param name="userPosition">User's location that is used to improve search results</param>
        /// <returns></returns>
        public static async Task<List<Location>> GetLocationByQuery(string query, Confidence minimumConfidence, Windows.Devices.Geolocation.Geoposition userPosition = null)
        {
            if (string.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException(query);
            }

            Dictionary<string, string> queryParameters = Helpers.GetBasicParameters();
            queryParameters.Add(Constants.Parameter_Query, query);

            if (userPosition != null)
            {
                queryParameters.Add(Constants.Parameter_UserLocation, userPosition.Coordinate.Latitude + "," + userPosition.Coordinate.Longitude);
            }

            string url = Helpers.CreateServiceUrl(Constants.BingLocationServiceBaseAddress, queryParameters);

            Response response = await Helpers.GetJsonResponse<Response>(url);

            return Helpers.FilterResults<Location>(response, minimumConfidence);            
        }        
    }
}