using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SingleLaneBridge
{
	public class Bridge
	{
		private int NumberOfLeftCars;
		private int NumberOfRightCars;
		private int ArrivalTime;
		private int ScenarioChoice;
		private List<Car> LeftCars;
		private List<Car> RightCars;
		private readonly object BridgeLock = new object();

		public Bridge(int NumberOfLeftCars, int NumberOfRightCars, int ArrivalTime, int ScenarioChoice)
		{
			this.NumberOfLeftCars = NumberOfLeftCars;
			this.NumberOfRightCars = NumberOfRightCars;
			this.ArrivalTime = ArrivalTime * 1000;
			this.ScenarioChoice = ScenarioChoice;
			LeftCars = CreateCarList("Left");
			RightCars = CreateCarList("Right");

		}

		private List<Car> CreateCarList(string SideOfCar)
		{
			List<Car> CarList = new List<Car>();
			if (SideOfCar == "Left")
			{
				for (int i = 0; i < NumberOfLeftCars; i++)
				{
					CarList.Add(new Car(i, "Left", 3, ScenarioChoice, this, BridgeLock));
				}
			}
			else
			{
				for (int i = 0; i < NumberOfRightCars; i++)
				{
					CarList.Add(new Car(i, "Right", 5, ScenarioChoice, this, BridgeLock));
				}
			}
			return CarList;
		}

		public void StartCars()
		{
			Console.WriteLine(string.Empty.PadLeft(6, '\t') + "Starting Simulation");

			switch (ScenarioChoice)
			{
				case 1:
					ScenarioOne();
					break;
				case 2:
					ScenarioTwo();
					break;
				case 3:
					Console.WriteLine("Scenario 3");
					break;
				case 4:
					Console.WriteLine("Scenario 4");
					break;
				default:
					Console.WriteLine("Wrong Choice");
					break;
			}
		}

		private void ScenarioOne()
		{
			List<Task> CarActions = new List<Task>();

			Task StartLeftCars = Task.Factory.StartNew(() => LeftCars.ForEach((item) =>
			{
				Thread.Sleep(3000);
				CarActions.Add(Task.Factory.StartNew(() => item.RunThread()));
			}));

			Task StartRightCars = Task.Factory.StartNew(() => RightCars.ForEach((item) =>
			{
				Thread.Sleep(2000);
				CarActions.Add(Task.Factory.StartNew(() => item.RunThread()));
			}));

			Task.WaitAll(StartLeftCars, StartRightCars);

			Task.WaitAll(CarActions.ToArray());
		}

		private void ScenarioTwo()
		{
			List<Task> CarActions = new List<Task>();

			Task StartLeftCars = Task.Factory.StartNew(() => LeftCars.ForEach((item) =>
			{
				Thread.Sleep(3000);
				CarActions.Add(Task.Factory.StartNew(() => item.RunThread()));
			}));

			Task StartRightCars = Task.Factory.StartNew(() => RightCars.ForEach((item) =>
			{
				Thread.Sleep(2000);
				CarActions.Add(Task.Factory.StartNew(() => item.RunThread()));
			}));

			Task.WaitAll(StartLeftCars, StartRightCars);

			Task.WaitAll(CarActions.ToArray());
		}
	}
}