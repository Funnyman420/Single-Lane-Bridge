using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;

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
					CarList.Add(new Car(i, "Left", 3, ScenarioChoice, this));
				}
			}
			else
			{
				for (int i = 0; i < NumberOfRightCars; i++)
				{
					CarList.Add(new Car(i, "Right", 5, ScenarioChoice, this));
				}
			}
			return CarList;
		}

		public void StartCars()
		{
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
			List<Action> CarActions = new List<Action>();

			LeftCars.ForEach((item) => { CarActions.Add(item.GetAction()); });
			RightCars.ForEach((item) => { CarActions.Add(item.GetAction()); });

			Action[] FinalCarActions = CarActions.ToArray();

			Parallel.Invoke(FinalCarActions);
		}

		private void ScenarioTwo()
		{
			List<Action> CarActions = new List<Action>();

			LeftCars.ForEach((item) => { CarActions.Add(item.GetAction()); });
			RightCars.ForEach((item) => { CarActions.Add(item.GetAction()); });

			Action[] FinalCarActions = CarActions.ToArray();

			Parallel.Invoke(FinalCarActions);
		}
	}
}