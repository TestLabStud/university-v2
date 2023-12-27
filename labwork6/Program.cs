using System;
using System.Text.Json;
using System.Collections.Generic;

public class Program
{
    static void Main(string[] args)
    {
        FlightInformationSystem flightSystem = new();

        List<Flight> flightsList = flightSystem.DeserializeJSON();
        Console.WriteLine("Amount flights in list: " + flightSystem.FlightsCount());

        var newFlight = new Flight
        {
            FlightNumber = "AB961",
            Airline = "WizAir",
            Destination = "Kyiv",
            DepartureTime = DateTime.Now.AddDays(-10),
            ArrivalTime = DateTime.Now.AddDays(-9),
            Status = FlightStatus.Boarding,
            Duration = TimeSpan.FromHours(5),
            AircraftType = "Airbus A320",
            Terminal = "1"
        };
        flightSystem.AddFlights(newFlight);
        Console.WriteLine("Adding element... Amount flights in list now: " + flightSystem.FlightsCount());

        flightSystem.RemoveFlights(5);
        Console.WriteLine("Removing element... Amount flights in list now: " + flightSystem.FlightsCount()+"\n");

        flightSystem.Request1_ByAirlane("MAU");

        flightSystem.Request2_ByStatus(FlightStatus.Delayed);

        DateTime beginsDay = new DateTime(2023, 09, 14);
        flightSystem.Request3_ByBeginsDay(beginsDay);

        DateTime beginsDayTI = new DateTime(2023,05,01,00,00,01);
        DateTime endsDayTI = new DateTime(2023,05,31,23,59,59);
        flightSystem.Request4_ByTimeInterval(beginsDayTI, endsDayTI);

        DateTime beginsTime = new DateTime(2023, 05, 01, 00, 00, 01);
        DateTime endsTime = new DateTime(2023, 05, 31, 23, 59, 59);
        flightSystem.Request5_ByEndsTI(beginsTime, endsTime);
    }
}
