using System;
using System.Threading.Tasks;
using System.Threading;

namespace SingleLaneBridge
{

	public abstract class BaseTask
	{
		private Task _task;

		protected BaseTask()
		{
			_task = new Task(() => RunThread());

		}

		public void Start() => _task.Start();
		public void Wait() => _task.Wait();
		//Override in base class
		public abstract void RunThread();
	}

	public class Car : BaseTask
	{
		private string SideOfCar;
		private int TimeToCross, ScenarioNum, CarId;
		private Bridge SingleLaneBridge;
		Random TimeThreshHold = new Random();
		private object BridgeLock;


		public Car(int CarId, string SideOfCar, int TimeToCross, int ScenarioNum, Bridge SingleLaneBridge, object BridgeLock)
		{
			this.SideOfCar = SideOfCar;
			this.TimeToCross = TimeToCross;
			this.ScenarioNum = ScenarioNum;
			this.CarId = CarId;
			this.SingleLaneBridge = SingleLaneBridge;
			this.BridgeLock = BridgeLock;

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
			if(SideOfCar == "Right")
				Console.Write(string.Empty.PadLeft(8, '\t'));
			Console.WriteLine($"{SideOfCar} Car {CarId} arrived at the bridge at Time {Program.Time}");
			Delay();
			Program.Time++;
			if (ScenarioNum == 2)
			{
				lock (BridgeLock)
				{
					WayOfCrossingTheBridgeFunc();
				}
			}
			else
				WayOfCrossingTheBridgeFunc();
			if (SideOfCar == "Left")
				Console.Write(string.Empty.PadLeft(8, '\t'));
			Console.WriteLine($"{SideOfCar} Car {CarId} crossed the bridge at time {Program.Time}");
			Program.Time++;
		}

		private void UnfairlyAndUnsafely()
		{
			Delay();
			if (SideOfCar == "Right")
				Console.Write(string.Empty.PadLeft(8, '\t'));
			Console.WriteLine($"{SideOfCar} Car {CarId} is crossing the bridge at time {Program.Time}");
			Program.Time++;
			Delay();

		}

		private void UnfairlyAndSafely()
		{
			Delay();
			if (SideOfCar == "Right")
				Console.Write(string.Empty.PadLeft(8, '\t'));
			Console.WriteLine($"{SideOfCar} Car {CarId} is crossing the bridge at time {Program.Time}");
			Program.Time++;
			Delay();
		}

		private void Delay()
		{
			int TimeDelay = TimeThreshHold.Next(4) * 1000;
			Thread.Sleep(TimeDelay);
		}
	}
}

	//public abstract class BaseThread
	//{
	//	private Thread _thread;

	//	protected BaseThread()
	//	{
	//		_thread = new Thread(new ThreadStart(RunThread));

	//	}

	//	public void Start() => _thread.Start();
	//	public void Join() => _thread.Join();
	//	public bool IsAlive() => _thread.IsAlive;
	//	public int ManagedThreadId => _thread.ManagedThreadId;
	//	//Override in base class
	//	public abstract void RunThread();
	//}


	//public class Car : BaseThread
	//{
	//	private string SideOfCar;
	//	private int TimeToCross, ScenarioNum, Id;
	//	private Bridge SingleLaneBridge;

	//	public Car(int Id, string SideOfCar, int TimeToCross, int ScenarioNum, Bridge SingleLaneBridge)
	//	{
	//		this.SideOfCar = SideOfCar;
	//		this.TimeToCross = TimeToCross;
	//		this.ScenarioNum = ScenarioNum;
	//		this.Id = Id;
	//		this.SingleLaneBridge = SingleLaneBridge;
	//	}

	//	public override void RunThread()
	//	{
	//		if (!IsAlive())
	//		{
	//			switch (ScenarioNum)
	//			{
	//				case 1:
	//					CrossingTheBridge(UnfairlyAndUnsafely);
	//					break;
	//				case 2:
	//					CrossingTheBridge(UnfairlyAndSafely);
	//					break;
	//				case 3:
	//					Console.WriteLine("Case 3");
	//					break;
	//				case 4:
	//					Console.WriteLine("Case 4");
	//					break;
	//				default:
	//					Console.WriteLine("Wrong Choice");
	//					break;
	//			}
	//		}
	//	}

	//	public Action GetAction()
	//	{
	//		return RunThread;
	//	}

	//	private void CrossingTheBridge(Action WayOfCrossingTheBridgeFunc)
	//	{
	//		Thread.Yield();
	//		Start();
	//		Console.WriteLine($"{SideOfCar} Car {Id} arrived at the bridge at Time {Program.Time}");
	//		Program.Time++;
	//		WayOfCrossingTheBridgeFunc();
	//		Console.WriteLine($"{SideOfCar} Car {Id} crossed the bridge at time {Program.Time}");
	//		Program.Time++;
	//	}

	//	private void UnfairlyAndUnsafely()
	//	{
	//		Console.WriteLine($"{SideOfCar} Car {Id} is crossing the bridge at time {Program.Time}");
	//		Program.Time++;
	//		Random TimeThreshHold = new Random();
	//		int TimeDelay = TimeThreshHold.Next(5) * 1000;
	//		Thread.Sleep(TimeDelay);


	//	}

	//	private void UnfairlyAndSafely()
	//	{
	//		Join();
	//		Console.WriteLine($"{SideOfCar} Car {Id} is crossing the bridge at time {Program.Time}");
	//		Program.Time++;
	//		Thread.Sleep(3000);
	//	}
	//}
