using System;

namespace Algorithms
{
	public class DistanceStrategy<T> : ICalculateDistanceStrategy<T> where T : IClassified<T>
	{
		public double GetDistance(T first, T second)
		{
			return GetDistance(Tuple.Create(first.FirstMeasurement, first.SecondMeasurement), second);
		}

		public double GetCoherenceDegree(T first, T second)
		{
			return 1d / GetDistance(first, second);
		}

		internal static double GetDistance(Tuple<double, double> firstItem, T secondItem)
		{
			return Math.Sqrt(Math.Pow(firstItem.Item1 - secondItem.FirstMeasurement, 2) +
							 Math.Pow(firstItem.Item2 - secondItem.SecondMeasurement, 2));
		}
	}
}
