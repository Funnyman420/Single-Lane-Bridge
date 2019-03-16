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
		private bool taskFinished;

		public Car(int carId, string sideOfCar, int timeToCross, int scenarioNum, Bridge singleLaneBridge)
		{
			this.sideOfCar = sideOfCar;
			this.timeToCross = timeToCross;
			this.scenarioNum = scenarioNum;
			this.carId = carId;
			this.singleLaneBridge = singleLaneBridge;
			taskFinished = false;
		}

		public void RunThread()
		{

			switch (scenarioNum)
			{
				case 1:
					CrossingTheBridge(UnfairlyAndUnsafely);         //1st Scenario
					break;
				case 2:
					CrossingTheBridge(UnfairlyAndSafely);           //2nd Scenario
					break;
				case 3:
					CrossingTheBridge(WithAutoswitchAndSafely);     //3rd Scenario
					break;
				case 4:
					CrossingTheBridge(FairlyAndSafely);             //4th Scenario
					break;
				default:
					Console.WriteLine("Wrong Choice");				//Unreachable code... Probably
					break;
			}
		}

		/*
		 * Prints that the car arrived and then proceeds to the critical section of
		 * the car.
		 */

		private void CrossingTheBridge(Action WayOfCrossingTheBridge)
		{
			if (sideOfCar == "Right")
				Console.Write(string.Empty.PadLeft(8, '\t'));
			Console.WriteLine($"{sideOfCar} Car {carId} arrived at the bridge at Time {Program.Time}");
			Delay();
			Program.Time++;
			WayOfCrossingTheBridge();
		}

		/*
		 * The 1st Scenario. This part of code just prints that the car crossed the bridge
		 */

		public void UnfairlyAndUnsafely()
		{
			GetToTheOtherSide();
			if (sideOfCar == "Left")
				Console.Write(string.Empty.PadLeft(8, '\t'));
			Console.WriteLine($"{sideOfCar} Car {carId} crossed the bridge at time {Program.Time}");
			Program.Time++;
			taskFinished = true;
		}

		/*
		 * The 2nd Scenario. If a car finds the lock open then it enters the bridge.
		 */

		public void UnfairlyAndSafely()
		{
			lock (singleLaneBridge.BridgeLock)
			{
				GetToTheOtherSide();
				if (sideOfCar == "Left")
					Console.Write(string.Empty.PadLeft(8, '\t'));
				Console.WriteLine($"{sideOfCar} Car {carId} crossed the bridge at time {Program.Time}");
				Program.Time++;
				taskFinished = true;
			}
		}

		/*
		 * The 3rd Scenario. In this scenario, as in the next one, I decided to lock the bridge
		 * before the program tests if the car should pass, because there was a chance that each
		 * car would reach the if statement at the same time and then the result would not be the
		 * desired one.
		 */

		public void WithAutoswitchAndSafely()
		{
			Delay();
			lock (singleLaneBridge.CheckLock)
			{
				if (CheckCarsForAutoswitch())
				{
					lock (singleLaneBridge.BridgeLock)
					{
						GetToTheOtherSide();
						if (sideOfCar == "Left")
							Console.Write(string.Empty.PadLeft(8, '\t'));
						Console.WriteLine($"{sideOfCar} Car {carId} crossed the bridge at time {Program.Time}");
					}
					taskFinished = true;
					Program.Time++;
				}
			}
		}

		/*
		 * The 4th Scenario. Go to Check For Fairness for examplanation
		 */

		public void FairlyAndSafely()
		{
			Delay();
			lock (singleLaneBridge.CheckLock)
			{
				if (CheckCarsForFairness())
				{
					lock (singleLaneBridge.BridgeLock)
					{
						GetToTheOtherSide();
						if (sideOfCar == "Left")
							Console.Write(string.Empty.PadLeft(8, '\t'));
						Console.WriteLine($"{sideOfCar} Car {carId} crossed the bridge at time {Program.Time}");
						if (sideOfCar == "Left")
							singleLaneBridge.LeftCarsCounter++;
						else
							singleLaneBridge.RightCarsCounter++;
						taskFinished = true;
						Program.Time++;
					}

				}
			}
		}
		/*
		 * A function that is used by all scenarios. It simply prints that the car passed the bridge and delays time a little bit
		 */
		private void GetToTheOtherSide()
		{
			Delay();
			if (sideOfCar == "Right")
				Console.Write(string.Empty.PadLeft(8, '\t'));
			Console.WriteLine($"{sideOfCar} Car {carId} is crossing the bridge at time {Program.Time}");
			Program.Time++;
			Delay();

		}

		/*
		 * Checks the content of the two list and decides, with the previous car passed as criteria, which car
		 * will pass the bridge
		 */

		private bool CheckCarsForAutoswitch()
		{
			bool startFlag = false;
			if (singleLaneBridge.LeftCarsCounter == 0 && singleLaneBridge.RightCarsCounter == 0 && sideOfCar == "Right")
			{
				singleLaneBridge.RightCarsCounter++;
				singleLaneBridge.PreviousSide = "Right";
				startFlag = true;
			}

			if (singleLaneBridge.RightCarsCounter < singleLaneBridge.NumberOfRightCars &&
				singleLaneBridge.PreviousSide == "Left" && sideOfCar == "Right")
			{
				singleLaneBridge.RightCarsCounter++;
				singleLaneBridge.PreviousSide = "Right";
				startFlag = true;
			}

			if (singleLaneBridge.LeftCarsCounter < singleLaneBridge.NumberOfLeftCars &&
				singleLaneBridge.PreviousSide == "Right" && sideOfCar == "Left")
			{
				singleLaneBridge.LeftCarsCounter++;
				singleLaneBridge.PreviousSide = "Left";
				startFlag = true;
			}

			if (singleLaneBridge.LeftCarsCounter == singleLaneBridge.NumberOfLeftCars && sideOfCar == "Right")
			{
				singleLaneBridge.RightCarsCounter++;
				singleLaneBridge.PreviousSide = "Right";
				startFlag = true;
			}

			if (singleLaneBridge.RightCarsCounter == singleLaneBridge.NumberOfRightCars && sideOfCar == "Left")
			{
				singleLaneBridge.LeftCarsCounter++;
				singleLaneBridge.PreviousSide = "Left";
				startFlag = true;
			}
			return startFlag;
		}

		/*
		 * Checks the given car if it's fair or not to pass. The function achieves this by simply
		 * calculating the div between the number of opposing cars and then creating a threshhold
		 * of how many cars that will pass, according to that div. For example, if there are 4 cars
		 * in the left side and 2 cars on the right side, the div of the cars in the opposing sides
		 * of the bridge is 2 so it will let two of the left cars to pass and then one of the right cars
		 */

		private bool CheckCarsForFairness()
		{
			int carThreshHold;
			bool startFlag = false;
			if (singleLaneBridge.NumberOfLeftCars == singleLaneBridge.NumberOfRightCars)
				startFlag = CheckCarsForAutoswitch();
			if (singleLaneBridge.NumberOfLeftCars < singleLaneBridge.NumberOfRightCars)
			{
				carThreshHold = singleLaneBridge.NumberOfRightCars / singleLaneBridge.NumberOfLeftCars;
				if (singleLaneBridge.RightCarsCounter < carThreshHold && sideOfCar == "Right")
				{
					startFlag = true;
				}
				if (singleLaneBridge.RightCarsCounter == carThreshHold && sideOfCar == "Left")
				{
					startFlag = true;
					singleLaneBridge.RightCarsCounter = 0;
				}
			}
			if (singleLaneBridge.NumberOfLeftCars > singleLaneBridge.NumberOfRightCars)
			{
				carThreshHold = singleLaneBridge.NumberOfLeftCars / singleLaneBridge.NumberOfRightCars;
				if (singleLaneBridge.LeftCarsCounter < carThreshHold && sideOfCar == "Left")
				{
					startFlag = true;
				}
				if (singleLaneBridge.LeftCarsCounter == carThreshHold && sideOfCar == "Right")
				{
					startFlag = true;
					singleLaneBridge.LeftCarsCounter = 0;
				}
			}
			return startFlag;
		}


		private void Delay()
		{
			int TimeDelay = TimeThreshHold.Next(4) * 1000;
			Thread.Sleep(TimeDelay);
		}

		public bool TaskFinished
		{
			get { return taskFinished; }
			set { taskFinished = value; }
		}

		public int CarId
		{
			get { return carId; }
		}

		public string SideOfCar
		{
			get { return sideOfCar; }
		}
	}
}
