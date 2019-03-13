using System;
using System.Threading;

namespace SingleLaneBridge
{
	public abstract class BaseThread
	{
		private Thread _thread;

		protected BaseThread()
		{
			_thread = new Thread(new ThreadStart(RunThread));
		
		}

		public void Start() => _thread.Start();
		public void Join() => _thread.Join();
		public bool IsAlive() => _thread.IsAlive;
		public int ManagedThreadId => _thread.ManagedThreadId;
		//Override in base class
		public abstract void RunThread();
	}


	public class Car : BaseThread
	{
		private string SideOfCar;
		private int TimeToCross, ScenarioNum, Id;
		private Bridge SingleLaneBridge;

		public Car(int Id, string SideOfCar, int TimeToCross, int ScenarioNum, Bridge SingleLaneBridge)
		{
			this.SideOfCar = SideOfCar;
			this.TimeToCross = TimeToCross;
			this.ScenarioNum = ScenarioNum;
			this.Id = Id;
			this.SingleLaneBridge = SingleLaneBridge;
		}

		public override void RunThread()
		{
			if (!IsAlive())
			{
				switch (ScenarioNum)
				{
					case 1:
						CrossingTheBridge(UnfairlyAndUnsafely);
						break;
					case 2:
						CrossingTheBridge(UnfairlyAndSafely);
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
		}

		public Action GetAction()
		{
			return RunThread;
		}

		private void CrossingTheBridge(Action WayOfCrossingTheBridgeFunc)
		{
			Thread.Yield();
			Start();
			Console.WriteLine($"{SideOfCar} Car {Id} arrived at the bridge at Time {Program.Time}");
			Program.Time++;
			WayOfCrossingTheBridgeFunc();
			Console.WriteLine($"{SideOfCar} Car {Id} crossed the bridge at time {Program.Time}");
			Program.Time++;
		}

		private void UnfairlyAndUnsafely()
		{
			Console.WriteLine($"{SideOfCar} Car {Id} is crossing the bridge at time {Program.Time}");
			Program.Time++;
			Random TimeThreshHold = new Random();
			int TimeDelay = TimeThreshHold.Next(5) * 1000;
			Thread.Sleep(TimeDelay);
			

		}

		private void UnfairlyAndSafely()
		{
			Join();
			Console.WriteLine($"{SideOfCar} Car {Id} is crossing the bridge at time {Program.Time}");
			Program.Time++;
			Thread.Sleep(3000);
		}
	}
}