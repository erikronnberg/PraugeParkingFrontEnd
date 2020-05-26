using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PraugeParkingFrontEnd
{
    class Parking : IEnumerable
    {
        private ParkingSquare[] squares;
        public ParkingSquare this[int index]
        {
            get
            {
                if (index < 0 && index >= this.squares.Length)
                    throw new IndexOutOfRangeException($"{index} is not within the range.");
                return this.squares[index];
            }
            /*
			set
			{
				if (index < 0 && index >= squares.Length)
					throw new IndexOutOfRangeException($"{index} is not within the range.");
				this.squares[index] = value;
			}
			*/
        }
        public Parking()
        {
            squares = new ParkingSquare[100];
            for (int i = 0; i < 100; i++)
            {
                squares[i] = new ParkingSquare();
            }
        }
        public Parking(int squareAmount)
        {
            squares = new ParkingSquare[squareAmount];
            for (int i = 0; i < squareAmount; i++)
            {
                squares[i] = new ParkingSquare();
            }
        }
        public Parking(string filePath)
        {
            squares = new ParkingSquare[100];
            ImportFromFile(filePath);
        }
        public Parking(string filePath, int squareAmount)
        {
            squares = new ParkingSquare[squareAmount];
            ImportFromFile(filePath);
        }
        public int SquaresWithVehicles
        {
            get
            {
                int count = 0;
                foreach (ParkingSquare square in squares)
                {
                    if (square.CapacityRemaining == square.Capacity)
                        count++;
                }
                return count;
            }
        }
        public int NumberOfSquares
        {
            get
            {
                return squares.Length;
            }
        }
        public void ClearArray()
        {
            for (int i = 0; i < squares.Length; i++)
            {
                squares[i] = null;
            }
        }
        public int AddVehicle(Vehicle vehicle)
        {
            if (SearchVehicle(vehicle.RegNr) == -1)
            {
                for (int i = 0; i < squares.Length; i++)
                {
                    if (squares[i].Add(vehicle))
                        return i;
                }
            }
            return -1;
        }
        public int AddVehicle(Vehicle vehicle, int index)
        {
            if (SearchVehicle(vehicle.RegNr) == -1)
            {
                if (squares[index].Add(vehicle))
                    return index;
            }
            return -1;
        }
        public int RemoveVehicle(string regNr, out Vehicle vehicle)
        {
            vehicle = null;
            int originalPosition = SearchVehicle(regNr);
            if (originalPosition != -1)
            {
                vehicle = squares[originalPosition].Remove(regNr);
            }
            return originalPosition;
        }
        public int MoveVehicle(string regNr, int index)
        {
            //public int AddVehicle(Vehicle vehicle) letar plats automatiskt
            int originalPosition = SearchVehicle(regNr);
            if (originalPosition != -1)
            {
                Vehicle vehicle = squares[originalPosition].Remove(regNr);
                try
                {
                    if (squares[index].Add(vehicle))
                    {
                        return originalPosition;
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    squares[originalPosition].Add(vehicle);
                    throw e;
                }
                squares[originalPosition].Add(vehicle);
            }
            originalPosition = -1;
            return originalPosition;
        }
        public int SearchVehicle(string regNr)
        {
            for (int i = 0; i < squares.Length; i++)
            {
                if (squares[i].Exists(regNr))
                    return i;
            }
            return -1;
        }
        public IEnumerator GetEnumerator()
        {
            return this.squares.GetEnumerator();
        }
        /// <summary>
        /// Parses and saves the parking lot to a readable txt file
        /// </summary>
        /// <param name="filePath"></param>
        public void ExportToFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                    File.Delete(filePath);
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    for (int i = 0; i < squares.Length; i++)
                    {
                        if (squares[i].CapacityRemaining != squares[i].Capacity)
                        {
                            foreach (Vehicle vehicle in squares[i])
                            {
                                sw.WriteLine($"{i}  {vehicle.Type}  {vehicle.RegNr}  {vehicle.EntryTime.ToString()}");
                            }
                        }
                    }
                    sw.Flush();
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// Imports a text file and parses it into the parking class
        /// </summary>
        /// <param name="filePath"></param>
        public void ImportFromFile(string filePath)
        {
            for (int i = 0; i < squares.Length; i++)
            {
                squares[i] = new ParkingSquare();
            }
            StreamReader sr;
            try
            {
                using (sr = new StreamReader(filePath))
                {
                    string line = string.Empty;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] values = line.Split(' '); //index, Type, RegNr, entryTime
                        if (values[2] == "CAR")
                        {
                            string dateTime = values[6] + " " + values[7];
                            squares[int.Parse(values[0])].Add(new Car(values[4], DateTime.Parse(dateTime)));
                        }
                        else if (values[2] == "MC")
                        {
                            string dateTime = values[6] + " " + values[7];
                            squares[int.Parse(values[0])].Add(new Motorcycle(values[4], DateTime.Parse(dateTime)));
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        public int CalculateCost(Vehicle vehicle, DateTime outTime)
        {
            try
            {
                int timeParked = (int)outTime.Subtract(vehicle.EntryTime).TotalMinutes - 5;
                if (timeParked >= 0)
                {
                    if (timeParked >= 120)
                    {
                        return vehicle.PricePerHour * (timeParked / 60);
                    }
                    return vehicle.PricePerHour * 2;
                }
                return 0;
            }
            catch (Exception)
            {
            }
            return 0;
        }
    }

    class ParkingSquare : IEnumerable
    {
        private readonly int capacity;
        private List<Vehicle> vehicles;
        /*
		public Vehicle this[int index]
		{
			get
			{
				if (index < 0 && index >= vehicles.Count)
					throw new IndexOutOfRangeException($"{index} is not within the range.");
				return vehicles[index];
			}
			set
			{
				if (index < 0 && index >= vehicles.Count)
					throw new IndexOutOfRangeException($"{index} is not within the range.");
				this.vehicles[index] = value;
			}
		}
		*/
        public int Capacity { get { return this.capacity; } /*private set { this.Capacity = value; }*/ }
        public int CapacityRemaining
        {
            get
            {
                int returnValue = this.Capacity;
                foreach (Vehicle vehicle in vehicles)
                {
                    returnValue -= vehicle.Size;
                }
                return returnValue;
            }
        }
        public ParkingSquare()
        {
            this.capacity = 100;
            this.vehicles = new List<Vehicle>();
        }
        public bool Add(Vehicle vehicle)
        {
            if (this.CapacityRemaining >= vehicle.Size)
            {
                vehicles.Add(vehicle);
                return true;
            }
            else return false;
        }
        public Vehicle Remove(string regNr)
        {
            for (int i = 0; i < this.vehicles.Count; i++)
            {
                if (this.vehicles[i].RegNr == regNr)
                {
                    Vehicle vehicle = vehicles[i];
                    vehicles.RemoveAt(i);
                    return vehicle;
                }
            }
            throw new Exception($"{regNr} does not exist in this square");
        }
        public bool Exists(string regNr)
        {
            return vehicles.Exists(x => x.RegNr == regNr);
        }
        public bool IsEmpty()
        {
            if (vehicles.Count == 0)
                return true;
            return false;
        }

        public IEnumerator GetEnumerator()
        {
            return this.vehicles.GetEnumerator();
        }
    }
}
