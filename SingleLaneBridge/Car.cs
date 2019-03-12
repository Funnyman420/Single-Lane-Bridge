using System;
using System.Runtime.Remoting.Channels;
using System.Threading;

namespace SingleLaneBridge
{
	public abstract class BaseThread
	{
		private Thread _thread;

		protected BaseThread()
		{
			_thread = new Thread(new ThreadStart(this.RunThread));
		}

		public void Start() => _thread.Start();
		public void Join() => _thread.Join();
		public bool IsAlive() => _thread.IsAlive;
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
			RunThread();
		}

		public override void RunThread()
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

		private void CrossingTheBridge(Action WayOfCrossingTheBridgeFunc)
		{
			Start();
			Console.WriteLine("{} Car {} arrived at the bridge at Time {}", SideOfCar, Id, Program.Time);
			Program.Time++;
			WayOfCrossingTheBridgeFunc();
			Console.WriteLine("{} Car {} crossed the bridge at time {}", SideOfCar, Id, Program.Time);
		}

		private void UnfairlyAndUnsafely()
		{
			Console.WriteLine("{} Car {} is crossing the bridge at time {}", SideOfCar, Id, Program.Time);
			Program.Time++;
			Thread.Sleep(3000);

		}

		private void UnfairlyAndSafely()
		{
			Join();
			Console.WriteLine("{} Car {} is crossing the bridge at time {}", SideOfCar, Id, Program.Time);
			Program.Time++;
			Thread.Sleep(3000);
		}
	}
}