using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Neat.Guitar
{
	[System.Serializable]
	public class TimingMap
	{
		[System.Serializable]
		private class Tick
		{
			int value;
		}

		/// <summary>
		/// use factorials to ensure even divisions
		/// </summary>

		[System.Serializable]
		public class Beat
		{
			/// <summary>
			/// Measure #
			/// </summary>
			public int bar;

			/// <summary>
			/// Which division in the bar
			/// </summary>
			public int div;
		}

		/// <summary>
		/// Contains bpm, has a start time
		/// </summary>
		[System.Serializable]
		public class TimeSignature
		{
			private const long ticksPerSecond = 10000;
			private float _beatsPerMinute;
			private float _beatsPerSecond;
			private float _offset;

			/// <summary>
			/// StartTime in the song
			/// </summary>
			public float offset
			{
				get { return _offset; }
				set { _offset = value; }
			}

			/// <summary>
			/// beats per minute (divisions)
			/// </summary>
			public float beatsPerMinute
			{
				get { return _beatsPerMinute; }
				set
				{
					_beatsPerMinute = value;
					_beatsPerSecond = value * 60f;
				}
			}

			public long ticksPerBeat
			{
				get;
				set;
			}

			/// <summary>
			/// Divisions per second
			/// </summary>
			public float beatsPerSecond
			{
				get { return _beatsPerSecond; }
				set
				{
					_beatsPerSecond = value;
					_beatsPerMinute = value / 60f;
				}
			}

			/// <summary>
			/// Numerator of the time signature
			/// </summary>
			public int num { get; set; }

			/// <summary>
			/// Denominator of the time signature
			/// </summary>
			public int denom { get; set; }

			/// <summary>
			/// divisions are per beat
			/// div = 3 
			/// </summary>
			public int DivisionsPerMeasure(int div)
			{
				// EXAMPLES

				// divs = num * (div) / denom
				// 6/8 and 1/2 divs → 6 * 2 / 8 = 1.5 divs
				// 2/2 →

				// num * denom / div
				// 6/8 2s → 6 * 8 / 2 = 24

				// div * 4 / denom
				// 6 * 2 * 4 / 8

				// (num / denom) * div
				// (6 / 8) * (4 * 2) = 6
				// (2 / 2) * (4 * 3) = 12

				return (num / denom) * (4 * div);
			}
			public float DivisionsPerSecond(float div)
			{
				// beatsPerSecond is technically quarter notes (1 div) per scond
				return beatsPerSecond * div;
			}

			public int TimePerMeasure()
			{
				// EXAMPLES
				// num/dem → (num * (4 / denom)) * quarterNotesPerSecond
				// 4/4 -> (4 * (4 / 4)) = 4 seconds
				// 3/8 -> (3 * (4 / 8)) = 1.5
				// 2/2 -> (2 * (4 / 2)) = 4
				return (num * (4 / denom)) * (int)beatsPerSecond;
			}

			public List<Beat> GetBeatsBetween(long a, long b, long dps)
			{
				throw new System.NotImplementedException();
			}

			/// <summary>
			/// 
			/// </summary>
			public float GetTimeOfBeat(int beatNum, int div)
			{
				return offset + beatNum * div;
			}

			public float GetNextBeatTime(float time, int div)
			{
				// find how many beats of this div are in the timespan
				float timeSpan = time - offset;

				// div 13.3 → 14
				var dps = DivisionsPerSecond(div);
				float divs = dps * timeSpan;
				int next = (int)divs + 1;
				return next * dps;

			}

			public float GetLastBeatTime(float time, int div)
			{
				throw new System.NotImplementedException();
			}

			public Vector2 GetAdjacentBeatTimes(float time, int div)
			{
				throw new System.NotImplementedException();
			}

			public long GetTimeOfBar(int bar)
			{
				//return offset + bps
				throw new System.NotImplementedException();
			}
		}


		public List<TimeSignature> timeSignatures;
		float duration;

		public TimeSignature GetTimeSignature(float time)
		{
			TimeSignature toReturn = null;

			foreach (var ts in timeSignatures)
			{
				// if the timeSignature's start has been passed,
				// flag it for return
				if (time >= ts.offset)
					toReturn = ts;

				// else the next time signature has not been passed
				// return what was flagged
				else
					return ts;
			}

			return toReturn;
		}
		public TimeSignature GetTimeSignature(int i)
		{
			if (i >= 0 && i < timeSignatures.Count)
				return timeSignatures[i];
			else
				return null;
		}
		public void GetAdjacentSignatures(float time, out TimeSignature currentSignature, out TimeSignature nextSignature)
		{
			int idx = IndexOfSignatureAtTime(time);

			currentSignature = GetTimeSignature(idx);
			nextSignature = GetTimeSignature(idx + 1);
		}

		public int IndexOfSignatureAtTime(float time)
		{
			int toReturn = -1;
			for (int i = 0; i < timeSignatures.Count; i++)
			{
				if (time >= timeSignatures[i].offset)
					toReturn = i;

				else
					return toReturn;
			}

			return toReturn;
		}
		public void InsertTimeSignature(TimeSignature s, float time)
		{
			int idxToInsert = IndexOfSignatureAtTime(time) + 1;
			timeSignatures.Insert(idxToInsert, s);
		}

		public float GetNextBeatTime(float time, int div)
		{
			GetAdjacentSignatures(time, out var currentSignature, out var nextSignature);

			if (currentSignature == null)
			{
				// no beats found
				if (nextSignature == null)
					return -1f;
				else
					return nextSignature.offset;
			}
			else
			{
				float beatTime = currentSignature.GetNextBeatTime(time, div);

				// there is no next time signature
				// could also return the end of the chart
				if (nextSignature == null)
					return beatTime;
				else
					return Mathf.Min(beatTime, nextSignature.offset);
			}
		}



		List<Beat> AllBars()
		{
			throw new System.NotImplementedException();
			//foreach (var t in timeSignatures)
			//{
			//	var time = t.GetTimeOfBeat()
			//}
		}
		List<Beat> AllBeats(int div)
		{
			throw new System.NotImplementedException();
		}
		void GetBarsBetween(float a, float b, int division) { }
	}
}
