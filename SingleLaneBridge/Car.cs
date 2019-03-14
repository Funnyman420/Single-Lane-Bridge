using System;
using System.Threading;

namespace SingleLaneBridge
{
	public class Car
	{
		private string sideOfCar;
		private int timeToCross, scenarioNum, carId;
		private Bridge singleLaneBridge;
		Random TimeThreshHold = new Random();


		public Car(int carId, string sideOfCar, int timeToCross, int scenarioNum, Bridge singleLaneBridge)
		{
			this.sideOfCar = sideOfCar;
			this.timeToCross = timeToCross;
			this.scenarioNum = scenarioNum;
			this.carId = carId;
			this.singleLaneBridge = singleLaneBridge;


		}

		public void RunThread()
		{

			switch (scenarioNum)
			{
				case 1:
					UnfairlyAndUnsafely();
					break;
				case 2:
					UnfairlyAndSafely();
					break;
				case 3:
					Console.WriteLine("Case 3");
					break;
				case 4:
					Console.WriteLine("Case 4");
					break;
				default:
					Console.WriteLine("Wrong Choice");
					break;
			}
		}


		private void UnfairlyAndUnsafely()
		{
			if (sideOfCar == "Right")
				Console.Write(string.Empty.PadLeft(8, '\t'));
			Console.WriteLine($"{sideOfCar} Car {carId} arrived at the bridge at Time {Program.Time}");
			Delay();
			Program.Time++;
			CrossTheBridge();
			if (sideOfCar == "Left")
				Console.Write(string.Empty.PadLeft(8, '\t'));
			Console.WriteLine($"{sideOfCar} Car {carId} crossed the bridge at time {Program.Time}");
			Program.Time++;
		}

		private void UnfairlyAndSafely()
		{
			if (sideOfCar == "Right")
				Console.Write(string.Empty.PadLeft(8, '\t'));
			Console.WriteLine($"{sideOfCar} Car {carId} arrived at the bridge at Time {Program.Time}");
			Delay();
			Program.Time++;
			lock (singleLaneBridge.BridgeLock)
			{
				CrossTheBridge();
				if (sideOfCar == "Left")
					Console.Write(string.Empty.PadLeft(8, '\t'));
				Console.WriteLine($"{sideOfCar} Car {carId} crossed the bridge at time {Program.Time}");
			}
			Program.Time++;
		}

		private void AutoswitchAndSafely()
		{
			if (sideOfCar == "Right")
				Console.Write(string.Empty.PadLeft(8, '\t'));
			Console.WriteLine($"{sideOfCar} Car {carId} arrived at the bridge at Time {Program.Time}");
			Delay();
			Program.Time++;
			lock (singleLaneBridge.CheckLock)
			{

				lock (singleLaneBridge.BridgeLock)
				{
					CrossTheBridge();
					if (sideOfCar == "Left")
						Console.Write(string.Empty.PadLeft(8, '\t'));
					Console.WriteLine($"{sideOfCar} Car {carId} crossed the bridge at time {Program.Time}");
				}
			}
			Program.Time++;
		}

		private void CrossTheBridge()
		{
			Delay();
			if (sideOfCar == "Right")
				Console.Write(string.Empty.PadLeft(8, '\t'));
			Console.WriteLine($"{sideOfCar} Car {carId} is crossing the bridge at time {Program.Time}");
			Program.Time++;
			Delay();

		}

		private bool CheckCars()
		{
			return true;
		}

		private void Delay()
		{
			int TimeDelay = TimeThreshHold.Next(4) * 1000;
			Thread.Sleep(TimeDelay);
		}
	}
}
