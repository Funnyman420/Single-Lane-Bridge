using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SingleLaneBridge
{
	public class Bridge
	{
		private int numberOfLeftCars;
		private int numberOfRightCars;
		private int arrivalTime;
		private int scenarioChoice;
		private List<Car> leftCars;
		private List<Car> rightCars;
		private readonly object bridgeLock = new object();
		private readonly object checkLock = new object();
		private int leftCarsCounter = 0;
		private int rightCarsCounter = 0;
		private string previousSide;

		public Bridge(int NumberOfLeftCars, int NumberOfRightCars, int ArrivalTime, int ScenarioChoice)
		{
			this.numberOfLeftCars = NumberOfLeftCars;
			this.numberOfRightCars = NumberOfRightCars;
			this.arrivalTime = ArrivalTime * 1000;
			this.scenarioChoice = ScenarioChoice;
			leftCars = CreateCarList("Left");
			rightCars = CreateCarList("Right");

		}

		private List<Car> CreateCarList(string SideOfCar)
		{
			List<Car> CarList = new List<Car>();
			if (SideOfCar == "Left")
			{
				for (int i = 0; i < numberOfLeftCars; i++)
				{
					CarList.Add(new Car(i, "Left", 3, scenarioChoice, this, bridgeLock, checkLock));
				}
			}
			else
			{
				for (int i = 0; i < numberOfRightCars; i++)
				{
					CarList.Add(new Car(i, "Right", 5, scenarioChoice, this, bridgeLock, checkLock));
				}
			}
			return CarList;
		}

		public void StartCars()
		{
			Console.WriteLine(string.Empty.PadLeft(6, '\t') + "Starting Simulation");

			switch (scenarioChoice)
			{
				case 1:
					ScenarioOneAndTwo();
					break;
				case 2:
					ScenarioOneAndTwo();
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

		private void ScenarioOneAndTwo()
		{
			List<Task> CarActions = new List<Task>();

			Task StartLeftCars = Task.Factory.StartNew(() => leftCars.ForEach((item) =>
			{
				Thread.Sleep(3000);
				CarActions.Add(Task.Factory.StartNew(() => item.RunThread()));
			}));

			Task StartRightCars = Task.Factory.StartNew(() => rightCars.ForEach((item) =>
			{
				Thread.Sleep(2000);
				CarActions.Add(Task.Factory.StartNew(() => item.RunThread()));
			}));

			Task.WaitAll(StartLeftCars, StartRightCars);

			Task.WaitAll(CarActions.ToArray());
		}

		public object CheckLock
		{
			get { return checkLock; }
		}

		public object BridgeLock
		{
			get { return bridgeLock; }
		}

		public int LeftCarsCounter
		{
			get { return leftCarsCounter; }
			set { leftCarsCounter = value; }
		}
		public int RightCarsCounter
		{
			get { return rightCarsCounter; }
			set { rightCarsCounter = value; }
		}
		public string PreviousSide
		{
			get { return previousSide; }
		}
	}
}