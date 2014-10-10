using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;

namespace Algorithms
{
    public class Spectrum<T> where T : IClassified<T>
    {
        private readonly List<T> _classifiedItems;

        private List<T> _processedItems;

        private List<Tuple<T, double>> _result;

	    public Spectrum(List<T> classifiedItems)
        {
            _classifiedItems = classifiedItems;
        }

        public ClassificationResult Classify()
        {
			_processedItems = new List<T>();
			_result = new List<Tuple<T, double>>();
			

            var firstItem = _classifiedItems.First();
            var currentGroup = new List<T> { firstItem };
            _processedItems.Add(firstItem);
            _result.Add(Tuple.Create(firstItem, 0d));
            while (_processedItems.Count != _classifiedItems.Count)
            {
                var nearestItem = getNearestItem(currentGroup);
				if (nearestItem != null && !nearestItem.Equals(default(T)))
				{
					currentGroup.Add(nearestItem);
					_processedItems.Add(nearestItem);
				}
            }
			_result.RemoveAt(0);
			_result.Insert(0, Tuple.Create(firstItem, _result.Max(e => e.Item2)));
            var sortedResultCopy = _result.ToList().OrderBy(e => e.Item2).ToList();
			sortedResultCopy.RemoveRange(ClassificationSettings<T>.GroupsCount - 1,
				_result.Count - ClassificationSettings<T>.GroupsCount + 1);
            var groups = new List<List<T>>();
	        foreach (var tuple in _result)
            {
				if (tuple.Item1.Equals(firstItem) || sortedResultCopy.Contains(tuple))
                {
					groups.Add(new List<T> { tuple.Item1 });
					continue;
				}
				groups.Last().Add(tuple.Item1);
            }
            return new ClassificationResult
            {
                Spectrum = _result,
                Groups = groups
            };
        }

		public PlotModel GetSpectrumPlot()
		{
			if (_result.Count == 0)
			{
				throw new Exception();
			}
			var result = new PlotModel();
			var spectrum = new LineSeries
			{
				Color = OxyColors.Brown,
				MarkerStroke = OxyColors.DarkGreen,
				MarkerType = MarkerType.Star,
				MarkerFill = OxyColors.Blue
			};
			var counter = 0;
			foreach (var spectrumMember in _result)
			{
				spectrum.Points.Add(new DataPoint(counter, spectrumMember.Item2));
				result.Annotations.Add(new PointAnnotation
				{
					X = counter,
					Y = spectrumMember.Item2,
					Text = spectrumMember.Item1.Name
				});
				counter++;
			}
			result.Series.Add(spectrum);
			return result;
		}

        private double getDistance(T item, List<T> group)
        {
            return 1f / group.Count * group.Sum(e => ClassificationSettings<T>.CalculateDistanceStrategy.GetCoherenceDegree(item, e));
        }

        private T getNearestItem(List<T> group)
        {
            var unprocessed = _classifiedItems.Where(e => !_processedItems.Contains(e)).ToList();
            var distance = 0d;
            var result = default(T);
            foreach (var item in unprocessed)
            {
                var newDistance = getDistance(item, group);
                if (newDistance > distance)
                {
                    result = item;
                    distance = newDistance;
                }
            }
            if (result != null && !result.Equals(default(T)))
            {
                _result.Add(Tuple.Create(result, distance));
            }
            return result;
        }

        public class ClassificationResult
        {
            public List<Tuple<T, double>> Spectrum { get; set; }

            public List<List<T>> Groups { get; set; }
        }
    }
}
