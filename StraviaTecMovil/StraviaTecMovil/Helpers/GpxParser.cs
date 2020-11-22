using StraviaTecMovil.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace StraviaTecMovil.Helpers
{
    public class PuntoComparator : IComparer<Punto>
    {
        public int Compare(Punto x, Punto y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            if (x.Orden < y.Orden)
            {
                return -1;
            }
            else if (x.Orden > y.Orden)
            {
                return 1;
            }
            return 0;
        }
    }

    public static class GpxParser
    {
        public static XmlDocument currentDoc;
        public static XmlNamespaceManager nsManager;
        public static XmlElement root;

        public static void SetCurrentDoc(XmlDocument doc)
        {
            if (doc == null)
                return;

            currentDoc = doc;
            nsManager = new XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("def", "http://www.topografix.com/GPX/1/1");
            nsManager.AddNamespace("gpxtpx", "http://www.garmin.com/xmlschemas/TrackPointExtension/v1");

            root = doc.DocumentElement;
        }
        public static DateTime GetDateTime()
        {
            if (currentDoc == null)
                return DateTime.Now;
            
            var docTime = root.SelectSingleNode(@"./def:metadata/def:time[1]", nsManager);

            return DateTime.Parse(docTime.InnerText);
        }

        public static string GetName()
        {
            if (currentDoc == null)
                return null;
            
            
            var docName = root.SelectSingleNode(@"./def:trk/def:name[1]", nsManager);
            return docName.InnerText;
        }

        public static List<Punto> ParseDoc(int idRecorrido)
        {
            if (currentDoc == null)
                return new List<Punto>();

            var pointList = new List<Punto>();
            
            //var docName = root.SelectSingleNode(@"./def:trk/def:name[1]", nsManager);
            var segments = root.SelectNodes(@"./def:trk/def:trkseg", nsManager);

            for (int i = 0; i < segments.Count; i++)
            {
                var seg = segments[i];
                var points = seg.SelectNodes(@"def:trkpt", nsManager);

                for (int j = 0; j < points.Count; j++)
                {
                    var pt = points[j];
                    var lat = float.Parse(pt.Attributes["lat"].Value, CultureInfo.InvariantCulture);
                    var lng = float.Parse(pt.Attributes["lon"].Value, CultureInfo.InvariantCulture);
                    var time = DateTime.Parse(pt.SelectSingleNode(@"def:time[1]", nsManager).InnerText);
                    var elev = float.Parse(pt.SelectSingleNode(@"def:ele[1]", nsManager).InnerText, CultureInfo.InvariantCulture);

                    pointList.Add(new Punto
                    {
                        Id_recorrido = idRecorrido,
                        Segmento = i,
                        Orden = j,
                        Tiempo = time,
                        Latitud = lat,
                        Longitud = lng,
                        Elevacion = elev
                    });
                }
            }
            return pointList;
        }

        public static double GetTotalTime(List<Punto> points)
        {
            if (points.Count() == 0)
                return 0.0;

            points.OrderBy((punto) => punto.Orden);

            var first = points.First();
            var last = points.Last();

            var initialTime = first.Tiempo;
            var finalTime = last.Tiempo;

            TimeSpan duration = finalTime - initialTime;

            return duration.TotalMinutes;
        }

        public static double PointsToDistanceInKm(List<Punto> points)
        {
            // points.OrderBy((punto) => punto.Orden);
            points.Sort(new PuntoComparator());

            Punto lastPoint;
            double totalDistance = 0.0;
            var size = points.Count();

            for (int i = 1; i < size; i++)
            {
                lastPoint = points[i - 1];
                var currentPoint = points[i];

                totalDistance += ComputeDistanceInKm(lastPoint.Latitud, currentPoint.Latitud, lastPoint.Longitud, currentPoint.Longitud);
            }

            return totalDistance;
        }

        private static double ComputeDistanceInKm(double lat1, double lat2, double lng1, double lng2)
        {
            var earthRadiusInKm = 6371;
            var dLat = DegreeToRad(lat2 - lat1);
            var dLng = DegreeToRad(lng2 - lng1);
            lat1 = DegreeToRad(lat1);
            lat2 = DegreeToRad(lat2);

            var a1 = Math.Sin((double)dLat / 2);
            var b1 = Math.Sin((double)dLng / 2);
            var a = a1 * a1 + b1 * b1 * Math.Cos((double)lat1) * Math.Cos((double)lat2);
            var b = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var c = earthRadiusInKm * b;
            return c;
        }

        private static double DegreeToRad(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}
