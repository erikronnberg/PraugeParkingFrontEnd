using System;

namespace PraugeParkingFrontEnd
{
	abstract class Vehicle
	{
		private readonly string regNr;
		private readonly DateTime entryTime;
		public string RegNr { get { return regNr; } }
		public DateTime EntryTime { get { return entryTime; } }
		public virtual int Size { get { return 0; } }
		public virtual string Type { get { return "Vehicle"; } }
		public virtual int PricePerHour { get { return 0; } }//hej
		public Vehicle(string regNr)
		{
			this.regNr = regNr;
			this.entryTime = DateTime.Now;
		}
		public Vehicle(string regNr, DateTime entryTime)
		{
			this.regNr = regNr;
			this.entryTime = entryTime;
		}
		public Vehicle(Vehicle vehicle)
		{
			this.regNr = vehicle.RegNr;
			this.entryTime = vehicle.EntryTime;
		}
		public override string ToString()
		{
			return $"{this.RegNr} - {this.EntryTime}";
		}

		public bool Equals(string other)
		{
			return RegNr.Equals(other);
		}
	}

	class Car : Vehicle
	{
		public override int Size { get { return 100; } }
		public override string Type { get { return "CAR"; } }
		public override int PricePerHour { get { return 20; } }
		public Car(string regNr) : base(regNr)
		{

		}
		public Car(string regNr, DateTime entryTime) : base(regNr, entryTime)
		{

		}
		public Car(Car car) : base(car)
		{

		}
		public override string ToString()
		{
			return $"{this.Type}: {base.ToString()}";
		}
	}

	class Motorcycle : Vehicle
	{
		public override int Size { get { return 50; } }
		public override string Type { get { return "MC"; } }
		public override int PricePerHour { get { return 10; } }
		public Motorcycle(string regNr) : base(regNr)
		{

		}
		public Motorcycle(string regNr, DateTime entryTime) : base(regNr, entryTime)
		{

		}
		public Motorcycle(Motorcycle motorcycle) : base(motorcycle)
		{

		}
		public override string ToString()
		{
			return $"{this.Type}: {base.ToString()}";
		}
	}
}
