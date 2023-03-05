using System;
namespace OpenWeather_io
{
	public class Report
	{

		public string Temperature { get; set; }
		public string Weather { get; set; }

        public Report(string _temp, string _weather){ Temperature = _temp; Weather = _weather;}
    }
}


