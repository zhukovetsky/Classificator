using System;

namespace Algorithms
{
	public class CosineStrategy<T> : ICalculateDistanceStrategy<T> where T : IClassified<T>
	{
		public double GetDistance(T first, T second)
		{
			return 1d / GetCoherenceDegree(first, second);
		}

		public double GetCoherenceDegree(T first, T second)
		{
			return (first.FirstMeasurement * second.FirstMeasurement + first.SecondMeasurement * second.SecondMeasurement) /
					(getDistance(first) * getDistance(second));
		}

		private double getDistance(T item)
		{
			return Math.Sqrt(Math.Pow(item.FirstMeasurement, 2) + Math.Pow(item.SecondMeasurement, 2));
		}
	}
}