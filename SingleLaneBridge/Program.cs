using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleLaneBridge
{
	class Program
	{
		public static int Time;

		static void Main(string[] args)
		{
			int NumberOfLeftCars = GetNumber(0);
			int NumberOfRightCars = GetNumber(1);
			int ArrivalTime = GetNumber(2);
			int ScenarioChoice = GetNumber(3);
			

			Bridge MainBridge = new Bridge(NumberOfLeftCars, NumberOfRightCars, ArrivalTime, ScenarioChoice);
			MainBridge.StartCars();
			Console.ReadKey();
		}

		private static int GetNumber(int DialogType)
		{
			string msg = "Something went wrong. Please try again";
			switch (DialogType)
			{
				case 0:
					msg = "Insert the amount of Cars that are on the left side of the bridge:";
					break;
				case 1:
					msg = "Insert the amount of Cars that are on the right side of the bridge:";
					break;
				case 2:
					msg = "Pick the seconds that a new car will arrive on the bridge";
					break;
				case 3:
					msg = "Pick one of the following scenarios:\n" +
						"Unfair and Unsafe Bridge\n" +
						"Unfair and Safe Brdige\n" +
						"AutoSwitch and Safe Bridge\n" +
						"Fair and Safe Bridge";
					break;


			}

			Console.WriteLine(msg);
			var UserChoiceString = Console.ReadLine();
			if (!int.TryParse(UserChoiceString, out int UserChoice))
			{
				Console.WriteLine("Wrong Input. Try again.");
				return GetNumber(DialogType);
			}
			else
			{
				return UserChoice;
			}
		}
	}
}
