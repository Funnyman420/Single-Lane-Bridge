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
			this.ArrivalTime = ArrivalTime;
			this.ScenarioChoice = ScenarioChoice;
		}

		private void CreateCars()
		{

		}
	}
}