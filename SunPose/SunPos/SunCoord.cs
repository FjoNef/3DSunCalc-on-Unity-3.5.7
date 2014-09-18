using System;
using System.Collections.Generic;
using System.Text;

//----------------------------------------------------------------------
//Все формулы взяты отсюда: http://pvcdrom.pveducation.org/RU/index.html
//----------------------------------------------------------------------
namespace SunPos
{
    public class SunCoord
    {

        // Количество дней с 1 января 2000 года
        double ElapsedJulianDays;
        // Время в десятичных долях
        double DecimalHours;
        // Эклиптическая долгота
        double EclipticLongitude;
        // Эклиптическое наклонение
        double EclipticObliquity;
        // Прямое восхождение
        double RightAscension;
        // Склонение
        double Declination;
        // Полярный угол (Азимут)
        double Azimuth;
        // Зенитный угол
        double ZenithAngle;
        // Угол возвышения
        double Elevation;

        // Вспомогательные переменные
        double Y;
        double X;

        // ----Постоянные----
        // Радиус Земли
        double EarthMeanRadius = 6371.01;
        // Астрономическая единица
        double AstronomicalUnit = 149597890;
        // Радиан
        double rad = Math.PI / 180;

        private void ElJuliDate(int hour, int minut, int second, int day, int month, int year)
        {
            double julianDate;
		    long aux1;
		    long aux2;
		    // Calculate time of the day in UT decimal hours
            DecimalHours = (double)hour + ((double)minut + (double)second / 60.0) / 60.0;
		    // Calculate current Julian Day
		    aux1 =(month-14)/12;
            aux2 = (1461 * (year + 4800 + aux1)) / 4 + (367 * (month - 2 - 12 * aux1)) / 12 - (3 * ((year + 4900 + aux1) / 100)) / 4 + day - 32075;
            julianDate = (double)(aux2) - 0.5 + DecimalHours / 24.0;
		    // Calculate difference between current Julian Day and JD 2451545.0 
            ElapsedJulianDays = julianDate - 2451545.0;
        }

        // Расчет эклиптических координат
        private void EclipticCoordinates()
        {
            double meanLongitude;
            double meanAnomaly;
            double omega;
            omega = 2.1429 - 0.0010394594 * ElapsedJulianDays;
            meanLongitude = 4.8950630 + 0.017202791698 * ElapsedJulianDays;
            meanAnomaly = 6.2400600 + 0.0172019699 * ElapsedJulianDays;
            EclipticLongitude = meanLongitude + 0.03341607 * Math.Sin(meanAnomaly) + 0.00034894 * Math.Sin(2.0 * meanAnomaly) - 0.0001134 - 0.0000203 * Math.Sin(omega);
            EclipticObliquity = 0.4090928 - 6.2140e-9 * ElapsedJulianDays + 0.0000396 * Math.Cos(omega);
        }

        // Вычисление астрономических координат
        private void CelestialCoordinates()
        {
            Y = Math.Cos(EclipticObliquity) * Math.Sin(EclipticLongitude);
            X = Math.Cos(EclipticLongitude);
            RightAscension = Math.Atan2(Y, X);
            if (RightAscension < 0.0)
                RightAscension = RightAscension + 2 * Math.PI;
            Declination = Math.Asin(Math.Sin(EclipticObliquity) * Math.Sin(EclipticLongitude));
        }

        //Расчет азимута
        private void AzimuthCalc(double longitude, double latitude)
        {
            double greenwichMeanSiderealTime;
            double localMeanSiderealTime;
            double latitudeInRadians;
            double hourAngle;
            greenwichMeanSiderealTime = 6.6974243242 + 0.0657098283 * ElapsedJulianDays + DecimalHours;
            localMeanSiderealTime = (greenwichMeanSiderealTime * 15 + longitude) * rad;
            hourAngle = localMeanSiderealTime - RightAscension;
            latitudeInRadians = latitude * rad;
            Y = -Math.Sin(hourAngle);
            X = Math.Tan(Declination) * Math.Cos(latitudeInRadians) - Math.Sin(latitudeInRadians) * Math.Cos(hourAngle);
            Azimuth = Math.Atan2(Y, X);
            if (Azimuth < 0.0)
                Azimuth = Azimuth + 2.0*Math.PI;
            Azimuth = Azimuth / rad;
        }

        //Расчет зенитного угла
        private void ZenAngCalc(double longitude, double latitude)
        {
            double greenwichMeanSiderealTime;
            double localMeanSiderealTime;
            double latitudeInRadians;
            double hourAngle;
            double parallax;
            greenwichMeanSiderealTime = 6.6974243242 + 0.0657098283 * ElapsedJulianDays + DecimalHours;
            localMeanSiderealTime = (greenwichMeanSiderealTime * 15 + longitude) * rad;
            hourAngle = localMeanSiderealTime - RightAscension;
            latitudeInRadians = latitude * rad;
            ZenithAngle = (Math.Acos(Math.Cos(latitudeInRadians) * Math.Cos(hourAngle) * Math.Cos(Declination) + Math.Sin(Declination) * Math.Sin(latitudeInRadians)));
            // Коррекция параллакса
            parallax = (EarthMeanRadius / AstronomicalUnit) * Math.Sin(ZenithAngle);
            ZenithAngle = (ZenithAngle + parallax) / rad;
        }

        //Расчет угла возвышения
        private void ElevCalc()
        {
            Elevation = 90.0 - ZenithAngle;
        }

        // Расчет положения солнца на небе из координат и времени (Параметры:  Широта, долгота, год, месяц, день, часы, минуты, секунды)
        public double[] Calculate(double latitude, double longitude, int year,int month, int day, int hour, int minut, int second)
        {
            ElJuliDate(hour, minut, second, day, month, year);
            EclipticCoordinates();
            CelestialCoordinates();
            AzimuthCalc(longitude, latitude);
            ZenAngCalc(longitude, latitude);
            ElevCalc();
            double[] result = { Elevation, ZenithAngle, Azimuth };
            return result;
        }
    }
}