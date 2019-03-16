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
			int numberOfLeftCars = GetNumber(0);
			int numberOfRightCars = GetNumber(1);
			int leftArrivalTime = GetNumber(2);
			int rightArrivalTime = GetNumber(3);
			int scenarioChoice = GetNumber(4);
			

			Bridge MainBridge = new Bridge(numberOfLeftCars, numberOfRightCars, leftArrivalTime, rightArrivalTime,scenarioChoice);
			MainBridge.StartCars();
			Console.ReadKey();
		}

		private static int GetNumber(int dialogType)
		{
			string msg = "Something went wrong. Please try again";
			switch (dialogType)
			{
				case 0:
					msg = "Insert the amount of Cars that are on the left side of the bridge:";
					break;
				case 1:
					msg = "Insert the amount of Cars that are on the right side of the bridge:";
					break;
				case 2:
					msg = "Pick the seconds that a new car will arrive on the left side of the bridge. Preferably less than 4";
					break;
				case 3:
					msg =
						"Pick the seconds that a new car will arrive on the right side of the bridge. Preferably less than 4";
					break;
				case 4:
					msg = "Pick one of the following scenarios: \n" +
						"Unfair and Unsafe Bridge:   1\n" +
						"Unfair and Safe Brdige:     2\n" +
						"AutoSwitch and Safe Bridge: 3\n" +
						"Fair and Safe Bridge:       4";
					break;
			}

			Console.WriteLine(msg);
			var UserChoiceString = Console.ReadLine();
			if (!int.TryParse(UserChoiceString, out int UserChoice))
			{
				Console.WriteLine("Wrong Input. Try again.");
				return GetNumber(dialogType);
			}
			else
			{
				return UserChoice;
			}
		}
	}
}
