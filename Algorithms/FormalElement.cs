using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms
{
	public class FormalElement<T> where T : IClassified<T>
	{
		private const double _randomConst = 3;

		private const double _epsilon = 0.0001;

		private readonly List<T> _items;

		public FormalElement(List<T> items)
		{
			_items = items;
		}

		public List<List<T>> Classify()
		{
			var startPoint = getRandomPoint(_items);
			var center = Tuple.Create(startPoint.FirstMeasurement, startPoint.SecondMeasurement);
			Tuple<double, double> previousCenter;
			var radius = getDefaultRadius() / _randomConst;
			var groups = new List<List<T>>();
			while (true)
			{
				List<T> group;
				do
				{
					previousCenter = center;
					group = getItemsNear(center, radius, groups);
					center = getCenter(group);
				} while (Math.Abs(previousCenter.Item1 - center.Item1) >= _epsilon && Math.Abs(previousCenter.Item2 - center.Item2) >= _epsilon);
				groups.Add(group);
				var availableItems = getAvailableItems(groups);
				if (availableItems.Count == 0)
				{
					break;
				}
				var newStartPoint = getRandomPoint(availableItems);
				center = Tuple.Create(newStartPoint.FirstMeasurement, newStartPoint.SecondMeasurement);
			}
			return groups;
		}

		private Tuple<double, double> getCenter(List<T> group)
		{
			return Tuple.Create(group.Sum(item => item.FirstMeasurement) / group.Count,
								group.Sum(item => item.SecondMeasurement) / group.Count);
		}

		private List<T> getAvailableItems(List<List<T>> groups)
		{
			return _items.Where(item => !getItemsFromGroups(groups).Contains(item)).ToList();
		}

		private List<T> getItemsFromGroups(List<List<T>> groups)
		{
			return groups.SelectMany(e => e).ToList();
		} 

		private double getDefaultRadius()
		{
			double maxDistance = 0d;
			foreach (var item in _items)
			{
				foreach (var innerItem in _items)
				{
					var distance = new DistanceStrategy<T>().GetDistance(item, innerItem);
					if (distance > maxDistance)
					{
						maxDistance = distance;
					}
				}
			}
			return maxDistance;
		}

		private T getRandomPoint(List<T> points)
		{
			if (points.Count == 0)
			{
				throw new ArgumentException();
			}
			var random = new Random();
			var probability = 1d / points.Count;
			while (true)
			{
				foreach (var item in points)
				{
					var randomNumber = random.NextDouble();
					if (randomNumber < probability)
					{
						return item;
					}
				}
			}
		}

		private List<T> getItemsNear(Tuple<double, double> point, double radius, List<List<T>> excludeGroups)
		{
			var excludedItems = getItemsFromGroups(excludeGroups);
			var result = new List<T>();
			foreach (var item in _items)
			{
				if (excludedItems.Contains(item))
				{
					continue;
				}
				if (DistanceStrategy<T>.GetDistance(point, item) <= radius)
				{
					result.Add(item);
				}
			}
			return result;
		}
	}
}