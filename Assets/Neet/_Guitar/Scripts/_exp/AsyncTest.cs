using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AsyncTest : MonoBehaviour
{
	public int iterations;

	CancellationTokenSource tokenSource;

	private void Start()
	{
		tokenSource = new CancellationTokenSource();
	}

	bool calculationOver;
	private async void DoSomethingAsync()
	{
		Stopwatch watch = new Stopwatch();
		watch.Start();
		calculationOver = false;


		var result = await Task.Run(() =>
		{
			// do something here

			return false;
		}, tokenSource.Token);

		if (tokenSource.IsCancellationRequested)
		{
			print("Cancelled");
		}
	}

	void DoSomething()
	{
		for (int i = 0; i < iterations; i++)
		{
			Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere));
		}
	}
}
