using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class Timer_UnitTests
{
	[Test]
	public void Timer_Constructor()
	{
		Timer timer;
		Internal_EventsCalls = 0;

		timer = new Timer(1);
		Assert.AreEqual(1.0f, timer.Duration, 0.0001f);
		Assert.AreEqual(0.0f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(1.0f, timer.Remaining, 0.0001f);
		Assert.AreEqual(0, timer.LoopsCount);
		Assert.AreEqual(0, timer.LoopsTotal);
		Assert.AreEqual(false, timer.IsRunning);

		timer = new Timer(10, 10);
		Assert.AreEqual(10.0f, timer.Duration, 0.0001f);
		Assert.AreEqual(0.0f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(10.0f, timer.Remaining, 0.0001f);
		Assert.AreEqual(0, timer.LoopsCount);
		Assert.AreEqual(10, timer.LoopsTotal);
		Assert.AreEqual(false, timer.IsRunning);
	}

	[Test]
	public void Timer_Start()
	{
		Timer timer;
		Internal_EventsCalls = 0;

		timer = new Timer(1.0f);
		timer.OnStarted += Internal_EventCallback;
		timer.Start();

		timer.Start();
		Assert.AreEqual(true, timer.IsRunning);
		Assert.AreEqual(1, Internal_EventsCalls);

		timer.Start();
		Assert.AreEqual(true, timer.IsRunning);
		Assert.AreEqual(1, Internal_EventsCalls);

		timer.Stop();
		timer.Start();
		Assert.AreEqual(true, timer.IsRunning);
		Assert.AreEqual(2, Internal_EventsCalls);
	}

	[Test]
	public void Timer_Stop()
	{
		Timer timer;
		Internal_EventsCalls = 0;

		timer = new Timer(1.0f);
		timer.OnStopped += Internal_EventCallback;

		timer.Stop();
		Assert.AreEqual(false, timer.IsRunning);
		Assert.AreEqual(0, Internal_EventsCalls);

		timer.Start();
		timer.Stop();
		Assert.AreEqual(false, timer.IsRunning);
		Assert.AreEqual(1, Internal_EventsCalls);

		timer.Stop();
		Assert.AreEqual(false, timer.IsRunning);
		Assert.AreEqual(1, Internal_EventsCalls);
	}

	[Test]
	public void Timer_Restart()
	{
		Timer timer;
		Internal_EventsCalls = 0;

		timer = new Timer(1.0f);
		timer.OnRestarted += Internal_EventCallback;
		timer.Start();

		timer.Restart();
		Assert.AreEqual(0.0f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(1.0f, timer.Remaining, 0.0001f);
		Assert.AreEqual(0, timer.LoopsCount);
		Assert.AreEqual(true, timer.IsRunning);
		Assert.AreEqual(1, Internal_EventsCalls);

		timer.Restart();
		timer.Restart();
		Assert.AreEqual(true, timer.IsRunning);
		Assert.AreEqual(3, Internal_EventsCalls);
	}

	[Test]
	public void Timer_Scrub()
	{
		Timer timer;
		Internal_EventsCalls = 0;

		timer = new Timer(1.0f);
		timer.Start();

		timer.Scrub(0.5f);
		Assert.AreEqual(0.5f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(0.5f, timer.Remaining, 0.0001f);

		timer.Stop();
		timer.Scrub(1.0f);
		Assert.AreEqual(1.0f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(0.0f, timer.Remaining, 0.0001f);

		timer.Update(1.0f);
		timer.Scrub(0.0f);
		Assert.AreEqual(0.0f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(1.0f, timer.Remaining, 0.0001f);
	}

	[Test]
	public void Timer_Update()
	{
		Timer timer;
		Internal_EventsCalls = 0;

		timer = new Timer(1.0f);
		timer.OnUpdated += Internal_EventCallback;
		timer.Start();

		timer.Update(0.5f);
		Assert.AreEqual(0.5f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(0.5f, timer.Remaining, 0.0001f);
		Assert.AreEqual(1, Internal_EventsCalls);

		timer.Update(0.5f);
		Assert.AreEqual(1.0f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(0.0f, timer.Remaining, 0.0001f);
		Assert.AreEqual(1, Internal_EventsCalls);

		timer.Restart();
		timer.Update(0.1f);
		timer.Update(0.1f);
		timer.Update(0.1f);
		Assert.AreEqual(0.3f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(0.7f, timer.Remaining, 0.0001f);
		Assert.AreEqual(4, Internal_EventsCalls);

		timer.Restart();
		timer.Stop();
		timer.Update(0.5f);
		Assert.AreEqual(0.0f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(1.0f, timer.Remaining, 0.0001f);
		Assert.AreEqual(4, Internal_EventsCalls);

		timer.Restart();
		timer.Update(1.0f);
		timer.Update(1.0f);
		Assert.AreEqual(1.0f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(0.0f, timer.Remaining, 0.0001f);
		Assert.AreEqual(4, Internal_EventsCalls);
	}

	[Test]
	public void Timer_Loop()
	{
		Timer timer;
		Internal_EventsCalls = 0;

		timer = new Timer(1.0f, 1);
		timer.OnLooped += Internal_EventCallback;
		timer.Start();

		timer.Update(1.0f);
		Assert.AreEqual(0.0f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(1.0f, timer.Remaining, 0.0001f);
		Assert.AreEqual(1, timer.LoopsCount);
		Assert.AreEqual(1, timer.LoopsTotal);
		Assert.AreEqual(1, Internal_EventsCalls);

		timer.Restart();
		timer.Update(1.0f);
		timer.Update(1.0f);
		Assert.AreEqual(1.0f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(0.0f, timer.Remaining, 0.0001f);
		Assert.AreEqual(1, timer.LoopsCount);
		Assert.AreEqual(1, timer.LoopsTotal);
		Assert.AreEqual(2, Internal_EventsCalls);

		timer.Restart();
		timer.Update(3.0f);
		Assert.AreEqual(1.0f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(0.0f, timer.Remaining, 0.0001f);
		Assert.AreEqual(1, timer.LoopsCount);
		Assert.AreEqual(1, timer.LoopsTotal);
		Assert.AreEqual(3, Internal_EventsCalls);

		timer = new Timer(1.0f, -1);
		timer.OnLooped += Internal_EventCallback;
		timer.Start();

		timer.Update(10.0f);
		Assert.AreEqual(0.0f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(1.0f, timer.Remaining, 0.0001f);
		Assert.AreEqual(10, timer.LoopsCount);
		Assert.AreEqual(-1, timer.LoopsTotal);
		Assert.AreEqual(13, Internal_EventsCalls);
	}

	[Test]
	public void Timer_Complete()
	{
		Timer timer;
		Internal_EventsCalls = 0;

		timer = new Timer(1.0f, 1);
		timer.OnCompleted += Internal_EventCallback;
		timer.Start();

		timer.Update(10.0f);
		Assert.AreEqual(1.0f, timer.Elapsed, 0.0001f);
		Assert.AreEqual(0.0f, timer.Remaining, 0.0001f);
		Assert.AreEqual(false, timer.IsRunning);
		Assert.AreEqual(1, Internal_EventsCalls);
	}

	int Internal_EventsCalls = 0;
	private void Internal_EventCallback()
	{
		Internal_EventsCalls++;
	}
}
