﻿using System.Collections.Generic;

namespace Playground.Models.Timeline
{
    public class SemanticTimeline
    {
        public List<Timelineobject> timelineObjects { get; set; }
    }

    public class Timelineobject
    {
        public Activitysegment activitySegment { get; set; }
        public Placevisit placeVisit { get; set; }
    }

    public class Activitysegment
    {
        public Location startLocation { get; set; }
        public Location endLocation { get; set; }
        public Duration duration { get; set; }
        public int distance { get; set; }
        public string activityType { get; set; }
        public string confidence { get; set; }
        public Activity[] activities { get; set; }
        public Waypointpath waypointPath { get; set; }
        public Simplifiedrawpath simplifiedRawPath { get; set; }
        public Transitpath transitPath { get; set; }
    }

    public class Duration
    {
        public long startTimestampMs { get; set; }
        public long endTimestampMs { get; set; }
    }

    public class Waypointpath
    {
        public Waypoint[] waypoints { get; set; }
    }

    public class Waypoint
    {
        public int latE7 { get; set; }
        public int lngE7 { get; set; }
    }

    public class Simplifiedrawpath
    {
        public Point[] points { get; set; }
    }

    public class Point
    {
        public int latE7 { get; set; }
        public int lngE7 { get; set; }
        public string timestampMs { get; set; }
        public int accuracyMeters { get; set; }
    }

    public class Transitpath
    {
        public Transitstop[] transitStops { get; set; }
        public string name { get; set; }
        public string hexRgbColor { get; set; }
    }

    public class Transitstop
    {
        public int latitudeE7 { get; set; }
        public int longitudeE7 { get; set; }
        public string placeId { get; set; }
        public string name { get; set; }
    }

    public class Activity
    {
        public string activityType { get; set; }
        public float probability { get; set; }
    }

    public class Placevisit
    {
        public Location location { get; set; }
        public Duration duration { get; set; }
        public string placeConfidence { get; set; }
        public int centerLatE7 { get; set; }
        public int centerLngE7 { get; set; }
        public Childvisit[] childVisits { get; set; }
    }

    public class Location
    {
        public int latitudeE7 { get; set; }
        public int longitudeE7 { get; set; }
        public string placeId { get; set; }
        public string address { get; set; }
        public string name { get; set; }
        public string semanticType { get; set; }
        public Sourceinfo sourceInfo { get; set; }
    }

    public class Sourceinfo
    {
        public int deviceTag { get; set; }
    }

    public class Childvisit
    {
        public Location location { get; set; }
        public Duration duration { get; set; }
        public string placeConfidence { get; set; }
        public int centerLatE7 { get; set; }
        public int centerLngE7 { get; set; }
    }
}
