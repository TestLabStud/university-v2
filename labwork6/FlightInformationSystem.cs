using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Net.NetworkInformation;


public class FlightInformationSystem
{
    private List<Flight> flights = new List<Flight>();

    public class FlightData
    {
        public List<Flight> flights { get; set; }
    }


    public List<Flight> DeserializeJSON(string jsonName = "flights_data.json")
    {
        string jsonPath = "C:/Users/Volko/university-v2/labwork6/" + jsonName;
        string jsonData = File.ReadAllText(jsonPath);
        FlightData flightsData = new();
        try
        {
            flightsData = JsonConvert.DeserializeObject<FlightData>(jsonData);

            if (flightsData != null && flightsData.flights != null)
            {
                int i = 1;
                foreach (var flight in flightsData.flights)
                {
                    if (IsCorrect(flight))
                        AddFlights(flight);
                    else
                    {
                        Console.WriteLine(i + " skipped flights.");
                        i++;
                    }
                }
                Console.WriteLine("Deserializing was successfull.");
                PrintListFlights(flights, true);
                return flights;
            }
            else
            {
                Console.WriteLine("JSON convert was failed or don't contains correct data.");
                return flights;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return flights;
        }
    }


    public void SerializeJSON(List<Flight> flightsSer, string fileName = "result.json")
    {
        string jsonData = JsonConvert.SerializeObject(flightsSer, Formatting.Indented);
        string filePath = "C:/Users/Volko/university-v2/labwork6/" + fileName;

        using FileStream fileStream = File.Open(filePath, FileMode.Create);
        using StreamWriter output = new StreamWriter(fileStream);
        output.Write(jsonData);
        Console.WriteLine("Serializing was successful.\n");
       
    }


    public bool IsCorrect(Flight flight)
    {
        if (flight == null) return false;

        else if(!(flight.FlightNumber.Length == 5 && char.IsLetter(flight.FlightNumber[0]) 
                && char.IsLetter(flight.FlightNumber[1]) && char.IsDigit(flight.FlightNumber[2]) 
                && char.IsDigit(flight.FlightNumber[3]) && char.IsDigit(flight.FlightNumber[4]))) return false;

        else if (string.IsNullOrEmpty(flight.Airline)) return false;
        else if (string.IsNullOrEmpty(flight.Destination)) return false;

        else if (flight.DepartureTime == null) return false; // datetime
        else if (flight.ArrivalTime == null) return false; // datetime

        else if (!(flight.Status >= FlightStatus.OnTime &&
                 flight.Status <= FlightStatus.InFlight)) return false; // enum

        else if (flight.Duration == null) return false; // timespan

        else if (string.IsNullOrEmpty(flight.AircraftType)) return false;

        else if (!(flight.Terminal.Length == 1 && char.IsDigit(flight.Terminal[0])
                 && (!string.IsNullOrEmpty(flight.Terminal)))) return false;

       else return true;
    }


    public void PrintFlights(Flight flight)
    {
        Console.WriteLine(flight.FlightNumber + "; " + flight.Airline + "; " + flight.Destination + "; " +
            flight.DepartureTime + " - " + flight.ArrivalTime + "; " + flight.Status + "; " +
            flight.Duration + "; " + flight.AircraftType + "; Terminal " + flight.Terminal);
    } 

    public void PrintListFlights(List<Flight> flightsPr, bool isWithMenu = false)
    {
        int selectedMenuItem = 1;
        if (isWithMenu == true)
        {
            Console.WriteLine("Select option:\n1. Print in console.\n2. Save in JSON file.\n3. Do nothing.");
            var input = Console.ReadLine();

            for (; !(int.TryParse(input, out selectedMenuItem)) || selectedMenuItem < 1 || selectedMenuItem > 3;)
            {
                Console.Write("Incorrect input, try again: ");
                input = Console.ReadLine();
            }
        }

        switch (selectedMenuItem)
        {
            case 1:
                foreach (var fl in flightsPr)
                {
                    Console.Write("№: " + (flightsPr.IndexOf(fl) + 1) + "; ");
                    PrintFlights(fl);
                }
                Console.Write("\n");
                break;

            case 2:
                string jsonName;
                Console.Write("Enter the JSON file name: ");
                jsonName = Console.ReadLine();

                while (string.IsNullOrEmpty(jsonName))
                {
                    Console.Write("Incorrect input, try again: ");
                    jsonName = Console.ReadLine();
                }

                SerializeJSON(flightsPr, jsonName);
                break;

            case 3:
                Console.WriteLine("Nothing? Okay.\n");
                break;
        }
    }

    public void AddFlights(Flight flight)
    {
        if (IsCorrect(flight))
            flights.Add(flight);

        else Console.Write("Adding was failed. ");
    }

    public void RemoveFlights(int flightNumber)
    {
        if (flightNumber >= 0 && flightNumber <= flights.Count)
            flights.RemoveAt(flightNumber);

        else Console.WriteLine("Removing was failed - argument is out of range.");
    }

    public void RemoveFlights(Flight flight)
    {
        if (flights.Contains(flight))
            flights.Remove(flight);

        else Console.WriteLine("Removing was failed - invalid flight.");
    }

    public int FlightsCount()
    {
        BubbleSort(flights, SortingOption.DepartureTime);
        return flights.Count;
    }

     public List<Flight> BubbleSort(List<Flight> listForSorting, SortingOption option)
    {
        int length = listForSorting.Count;
        for (int i = 1; i < length; i++)
        {
            for (var j = 0; j < (length-i); j++)
            {
                bool inIf = false;
                switch ((int)option)
                {
                    case 0:
                        inIf = listForSorting[j].DepartureTime > listForSorting[j + 1].DepartureTime;
                        break;
                    case 1:
                        inIf = listForSorting[j].ArrivalTime > listForSorting[j + 1].ArrivalTime;
                        break;
                    case 2:
                        inIf = listForSorting[j].Status > listForSorting[j + 1].Status;
                        break;
                    case 3:
                        inIf = listForSorting[j].Duration > listForSorting[j + 1].Duration;
                        break;
                }

                if (inIf)
                {
                    var temp = listForSorting[j];
                    listForSorting[j] = listForSorting[j + 1];
                    listForSorting[j + 1] = temp;
                }
            }
        }
        return listForSorting;
    }



    public List<Flight> Request1_ByAirlane(string airline)
    { // 1. Повернути всі рейси, які здійснюються певною авіакомпанією. Рейси повинні бути відсортовані по часу вильоту.
        List<Flight> listWithAirlane = new();

        foreach (var airlineElem in flights)
        {
            if (airlineElem.Airline == airline)
                listWithAirlane.Add(airlineElem);
        }

       listWithAirlane = BubbleSort(listWithAirlane, SortingOption.DepartureTime);

        Console.WriteLine("Request 1 was successfull: List contains flights by airlane.");
        PrintListFlights(listWithAirlane, true);
        return listWithAirlane;
    }

    public List<Flight> Request2_ByStatus(FlightStatus status)
    { // 2. Повернути всі рейси, які на даний момент затримуються (Status == FlightStatus.Delayed). Рейси повинні бути відсортовані по часу затримки.
        List<Flight> listWithStatus = new();

        foreach (var statusElem in flights)
        {
            if (statusElem.Status == status)
                listWithStatus.Add(statusElem);
        }

        listWithStatus = BubbleSort(listWithStatus, SortingOption.DepartureTime);

        Console.WriteLine("Request 2 was successfull: List contains flights by status.");
        PrintListFlights(listWithStatus, true);
        return listWithStatus;
    }

    public List<Flight> Request3_ByBeginsDay(DateTime beginsDay)
    { // 3. Повернути всі рейси, які вилітають в певний день. Рейси повинні бути відсортовані по часу вильоту.
        List<Flight> listInBeginsDay = new();

        string begDay = beginsDay.ToString("dd-MM-yyyy");
        foreach (var begdayElem in flights)
        {
            if (begDay == begdayElem.DepartureTime.ToString("dd-MM-yyyy"))
                listInBeginsDay.Add(begdayElem);
        }

        listInBeginsDay = BubbleSort(listInBeginsDay, SortingOption.DepartureTime);

        Console.WriteLine("Request 3 was successfull: List contains departing flights on a certain day.");
        PrintListFlights(listInBeginsDay, true);
        return listInBeginsDay;
    }

    public List<Flight> Request4_ByTimeInterval(DateTime beginsTime, DateTime endsTime)
    { // 4. Повернути всі рейси, які вилітають та прибувають у вказаний проміжок часу (Наприклад: з 2023-05-1T00:00:01 до 2023-05-31T23:59:59) та мають вказаний пункт призначення. Рейси повинні бути відсортовані по часу вильоту.
        List<Flight> listInInterval = new();

        foreach (var TIElem in flights)
        {
            if (TIElem.DepartureTime >= beginsTime && TIElem.ArrivalTime <= endsTime)
                listInInterval.Add(TIElem);
        }

        listInInterval = BubbleSort(listInInterval, SortingOption.DepartureTime);

        Console.WriteLine("Request 4 was successfull: List contains departing and arriving flights in a certain time interval.");
        PrintListFlights(listInInterval, true);
        return listInInterval;
    }

    public List<Flight> Request5_ByEndsTI(DateTime beginsTime, DateTime endsTime)
    { // 5. Повернути всі рейси, які прибули за останню годину або за вказаний проміжок часу. Рейси повинні бути відсортовані по часу прибуття.
        List<Flight> listInEndsTI = new();

        foreach (var endTIElem in flights)
        {
            if (endTIElem.ArrivalTime >= beginsTime && endTIElem.ArrivalTime <= endsTime)
                listInEndsTI.Add(endTIElem);
        }

        listInEndsTI = BubbleSort(listInEndsTI, SortingOption.ArrivalTime);

        Console.WriteLine("Request 5 was successfull. List contains arriving flights in a certain time interval.");
        PrintListFlights(listInEndsTI, true);
        return listInEndsTI;
    }
}