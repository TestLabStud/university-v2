
using System;
using System.Collections.Generic;

public class TableReservationApp
{
    static void Main(string[] args)
    {
        ReservationManagerClass m = new ReservationManagerClass();
        m.AddRestaurantMethod("A", 10);
        m.AddRestaurantMethod("B", 5);

        Console.WriteLine(m.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
        Console.WriteLine(m.BookTable("A", new DateTime(2023, 12, 25), 3)); // False
    }
}

public class ReservationManagerClass
{
    // res
    public List<RestaurantClass> res;

    public ReservationManagerClass()
    {
        res = new List<RestaurantClass>();
    }

    public void AddRestaurantMethod(string n, int t)
    {
        try
        {
            RestaurantClass r = new RestaurantClass();
            r.name = n;
            r.table = new RestaurantTableClass[t];
            for (int i = 0; i < t; i++)
            {
                r.table[i] = new RestaurantTableClass();
            }
            res.Add(r);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    public List<string> FindAllFreeTables(DateTime dt)
    {
        try
        { 
            List<string> free = new List<string>();
            foreach (var r in res)
            {
                for (int i = 0; i < r.table.Length; i++)
                {
                    if (!r.table[i].IsBooked(dt))
                    {
                        free.Add($"{r.name} - Table {i + 1}");
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

    public bool BookTable(string rName, DateTime d, int tNumber)
    {
        foreach (var r in res)
        {
            if (r.name == rName)
            {
                if (tNumber < 0 || tNumber >= r.table.Length)
                {
                    throw new Exception(null); //Invalid table number
                }

                return r.table[tNumber].Book(d);
            }
        }

        throw new Exception(null); //Restaurant not found
    }

    public void SortRestaurantsByAvailabilityForUsersMethod(DateTime dt)
    {
        try
        { 
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < res.Count - 1; i++)
                {
                    int avTc = CountAvailableTablesForRestaurantClassAndDateTimeMethod(res[i], dt); // available tables current
                    int avTn = CountAvailableTablesForRestaurantClassAndDateTimeMethod(res[i + 1], dt); // available tables next

                    if (avTc < avTn)
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

    public int CountAvailableTablesForRestaurantClassAndDateTimeMethod(RestaurantClass r, DateTime dt)
    {
        try
        {
            int count = 0;
            foreach (var t in r.table)
            {
                if (!t.IsBooked(dt))
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
