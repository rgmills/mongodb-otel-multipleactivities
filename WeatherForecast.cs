using System;
using MongoDB.Bson;

namespace sample
{
    public class WeatherForecast
    {
        public ObjectId Id { get; set; } = new ObjectId();

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
