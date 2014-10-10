using System;

namespace Algorithms
{
	public interface ICalculateDistanceStrategy<T> where T : IClassified<T>
	{
		double GetDistance(T first, T second);

		double GetCoherenceDegree(T first, T second);
	}
}
