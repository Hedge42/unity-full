using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Neat.Audio.Music;

namespace Neat.Profiling
{
    public static class AsyncRunner
    {
        // ?????
        //Profiling.AsyncRunner.Time(delegate
        //{
        //    Debug.Log("hello");
        //})
        //.Wait();
        public static async Task Time(Action a)
        {
            Stopwatch watch = new Stopwatch();
            await Task.Run(a);
            watch.Stop();
            var elapsed = watch.ElapsedMilliseconds;
            UnityEngine.Debug.Log(a.Method.Name + " took " + elapsed + "ms");
        }
        public static async void Time(Action a, CancellationToken token)
        {
            Stopwatch watch = new Stopwatch();
            await Task.Run(a, token);
            watch.Stop();
            var elapsed = watch.ElapsedMilliseconds;

            if (token.IsCancellationRequested)
                UnityEngine.Debug.Log("Cancelled " + a.Method.Name);
            else
                UnityEngine.Debug.Log(a.Method.Name + " took " + elapsed + "ms");
        }
    }
    public static class CoroutineRunner
    {
        public static void Wait(YieldInstruction y, Action a)
        {
            var ca = new GameObject("Coroutine runner").AddComponent<CoroutineAgent>();
            ca.Wait(y, a);
        }
    }
    public class CoroutineAgent : MonoBehaviour
    {
        public void Wait(YieldInstruction y, Action a)
        {
            StartCoroutine(_Wait(y, a));
        }
        private IEnumerator _Wait(YieldInstruction y, Action a)
        {
            yield return y;
            a.Invoke();

            Destroyer.Destroy(gameObject);
        }
    }
}