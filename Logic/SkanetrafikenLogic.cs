using System;
using System.Collections.Generic;
using System.Linq;
using c_sharp_playground.Models.Skanetrafiken;

namespace c_sharp_playground.Logic
{
    public static class SkanetrafikenLogic
    {
        public static TrainDelaysResponse GetDelaysResponse(GetJourneyResult getJourneyResponse, GetMeansOfTransportResult getMeansOfTransportResult)
        {
            var response = new TrainDelaysResponse();
            response.StartPoint = getJourneyResponse.Journeys.FirstOrDefault()?.RouteLinks.First().From.Name;
            response.EndPoint = getJourneyResponse.Journeys.FirstOrDefault()?.RouteLinks.Last().To.Name;
            response.Journeys = new List<Journey>();

            var replacementBusId = getMeansOfTransportResult.LineTypes.Where(line => line.Name.Contains("Tågbuss", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault().Id;

            // Process all journeys returned from api
            foreach (var result in getJourneyResponse.Journeys)
            {
                var journey = new Journey();
                journey.LineTypes = new List<string>();

                journey.Switches = result.NoOfChanges;

                var departureLink = result.RouteLinks.FirstOrDefault();
                var departureRealTime = departureLink.RealTime.RealTimeInfo?.FirstOrDefault();

                var arrivalLink = result.RouteLinks.LastOrDefault();
                var arrivalRealTime = arrivalLink.RealTime.RealTimeInfo?.FirstOrDefault();

                journey.Departure = new JourneyPoint
                {
                    Name = departureLink.From.Name,
                    Time = departureLink.DepDateTime,
                    Deviation = departureRealTime?.DepTimeDeviation ?? 0
                };
                journey.Arrival = new JourneyPoint
                {
                    Name = arrivalLink.To.Name,
                    Time = arrivalLink.ArrDateTime,
                    Deviation = arrivalRealTime?.ArrTimeDeviation ?? 0
                };

                journey.Delayed = journey.Departure.Deviation > 0 || journey.Arrival.Deviation > 0;

                // Check all of the individual trips for cancellations or replacement buses
                foreach (var link in result.RouteLinks)
                {
                    if (link.Line.LineTypeId == replacementBusId)
                    {
                        journey.ReplacementBuses = true;
                    }

                    var lineTypeName = link.Line.LineTypeName;
                    if (!journey.LineTypes.Contains(lineTypeName))
                    {
                        journey.LineTypes.Add(lineTypeName);
                    }

                    // Check for real time information
                    var realTime = link.RealTime.RealTimeInfo?.FirstOrDefault();
                    if (realTime == null)
                    {
                        journey.NoRealTimeAvailable = true;
                    }
                    else if (realTime.Canceled)
                    {
                        journey.Cancelled = true;
                    }
                }

                response.Journeys.Add(journey);
            }

            return response;
        }
    }
}
