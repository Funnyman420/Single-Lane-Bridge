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
		private int leftArrivalTime;
		private int rightArrivaltime;
		private int scenarioChoice;
		private List<Car> leftCars;
		private List<Car> rightCars;
		private readonly object bridgeLock = new object();
		private readonly object checkLock = new object();
		private int leftCarsCounter = 0;
		private int rightCarsCounter = 0;
		private string previousSide = "";

		public Bridge(int numberOfLeftCars, int numberOfRightCars, int leftArrivalTime, int rightArrivaltime ,int scenarioChoice)
		{
			this.numberOfLeftCars = numberOfLeftCars;
			this.numberOfRightCars = numberOfRightCars;
			this.leftArrivalTime = leftArrivalTime * 1000;
			this.rightArrivaltime = rightArrivaltime * 1000;
			this.scenarioChoice = scenarioChoice;
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
					CarList.Add(new Car(i, "Left", 3, scenarioChoice, this));
				}
			}
			else
			{
				for (int i = 0; i < numberOfRightCars; i++)
				{
					CarList.Add(new Car(i, "Right", 5, scenarioChoice, this));
				}
			}
			return CarList;
		}

		public void StartCars()
		{
			Console.WriteLine(string.Empty.PadLeft(6, '\t') + "Starting Simulation");
			RunCars();
		}

		private void RunCars()
		{
			var carActions = new List<Task>();

			Task startLeftCars = GetCarTask(leftCars, carActions, leftArrivalTime);

			Task startRightCars = GetCarTask(rightCars, carActions, rightArrivaltime);

			Task.WaitAll(startLeftCars, startRightCars);					//Waits for Tasks to finish

			Task.WaitAll(carActions.ToArray());								//Waits for Tasks to finish
		}

		/*
		 * To be able to understand the following code, we must understand what task is.
		 * A Thread in C# uses an actual core of Windows. For that reason, Microsoft and
		 * C# devs decided to make it hard for us to manipulate threads, because they
		 * probably did a better job from us at designing them. So for that reason, there
		 * is the Task class, that allows you to pass a function and run it asyncronously.
		 */

		private Task GetCarTask(List<Car> carList, List<Task> carTasks, int timeToDelay)
		{
			//Creates a new Task
			Task _task = Task.Factory.StartNew(() =>									
				carList.ForEach((car) =>												
				{
					//Delaying the time, as requested in the exercise
					Thread.Sleep(timeToDelay);											
					/*
					 * This is the tricky part. After some tests I realized that the cars
					 * run asyncrously during the middle to the end of the simulation but
					 * not quite at the start. The program put more emphasis in some side
					 * of the bridge, one way or another. So, I decided, not only to run
					 * them asynchronously, but also add them to the list of Tasks
					 * asynchronously. Task.WaitAll is blocking so there is no chance of
					 * a stack overflow.
					 */
					carTasks.Add(Task.Factory.StartNew(() => {car.RunThread();})
						/*
						 * After some tests, I realized that some of the cars, because of
						 * their inability to take the lock, they ended, without going to
						 * their critical section. For that reason, I added ContinueWith
						 * which basically tells the task that if it's completed, run a
						 * certain function. And that's why I'm running the Critical Section
						 * of the cars again.
						 */
						.ContinueWith(t =>
						{
							if(scenarioChoice == 3)
								if(car.TaskFinished == false)
									car.WithAutoswitchAndSafely();
							if(scenarioChoice == 4)
								if(car.TaskFinished == false)
									car.FairlyAndSafely();
						}));
				}));
			return _task;
		}

		//Getters and Setters
		public int NumberOfLeftCars
		{
			get { return numberOfLeftCars; }
		}

		public int NumberOfRightCars
		{
			get { return numberOfRightCars; }
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
			set { previousSide = value; }
		}
	}
}