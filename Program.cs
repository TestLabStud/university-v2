
using System;
using System.Collections.Generic;

public class TableReservationApp
{
    static void Main(string[] args)
    {
        ReservationManagerClass manager = new ReservationManagerClass();
        manager.AddRestaurantMethod("A", 10);
        manager.AddRestaurantMethod("B", 5);

        Console.WriteLine(manager.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
        Console.WriteLine(manager.BookTable("A", new DateTime(2023, 12, 25), 3)); // False
    }
}

public class ReservationManagerClass
{
    public List<RestaurantClass> res;

    public ReservationManagerClass()
    {
        res = new List<RestaurantClass>();
    }

    public void AddRestaurantMethod(string name, int table)
    {
        try
        {
            RestaurantClass restoraunt = new RestaurantClass();
            restoraunt.name = name;
            restoraunt.table = new RestaurantTableClass[table];
            for (int i = 0; i < table; i++)
            {
                restoraunt.table[i] = new RestaurantTableClass();
            }
            res.Add(restoraunt);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    public List<string> FindAllFreeTables(DateTime dateTime)
    {
        try
        { 
            List<string> free = new List<string>();
            foreach (var restoraunt in res)
            {
                for (int i = 0; i < restoraunt.table.Length; i++)
                {
                    if (!restoraunt.table[i].IsBooked(dateTime))
                    {
                        free.Add($"{restoraunt.name} - Table {i + 1}");
                    }
                }
            }
            return free;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return new List<string>();
        }
    }

    public bool BookTable(string restorauntName, DateTime date, int tableNumber)
    {
        foreach (var restoraunt in res)
        {
            if (restoraunt.name == restorauntName)
            {
                if (tableNumber < 0 || tableNumber >= restoraunt.table.Length)
                {
                    throw new Exception(null); //Invalid table number
                }

                return restoraunt.table[tableNumber].Book(date);
            }
        }

        throw new Exception(null); //Restaurant not found
    }

    public void SortByAvailability(DateTime dateTime)
    {
        try
        { 
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < res.Count - 1; i++)
                {
                    int currentAvialableTables = CountAvailableTables(res[i], dateTime);
                    int nextAvailableTables = CountAvailableTables(res[i + 1], dateTime);

                    if (currentAvialableTables < nextAvailableTables)
                    {
                        // Swap restaurants
                        var temp = res[i];
                        res[i] = res[i + 1];
                        res[i + 1] = temp;
                        swapped = true;
                    }
                }
            } while (swapped);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    public int CountAvailableTables(RestaurantClass restaurant, DateTime dateTime)
    {
        try
        {
            int count = 0;
            foreach (var t in restaurant.table)
            {
                if (!t.IsBooked(dateTime))
                {
                    count++;
                }
            }
            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return 0;
        }
    }
}

public class RestaurantClass
{
    public string name;
    public RestaurantTableClass[] table;
}

public class RestaurantTableClass
{
    private List<DateTime> bookedDates;

    public RestaurantTableClass()
    {
        bookedDates = new List<DateTime>();
    }

    public bool IsBooked(DateTime date)
    {
        return bookedDates.Contains(date);
    }

    public bool Book(DateTime date)
    {
        try
        { 
            if (IsBooked(date))
            {
                return false;
            }
  
            bookedDates.Add(date);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return false;
        }
    }

}
