using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Series;

namespace Algorithms.Trees
{
	internal class TreeDisplay<T> where T : IClassified<T>
	{
		private readonly Tree<T> _tree;

		public TreeDisplay(Tree<T> tree)
		{
			_tree = tree;
		}

		private readonly List<Series> _series = new List<Series>();
		private PlotModel _model;

		public PlotModel Display()
		{
			_model = new PlotModel();
			var explorer = TreeExplorer<T>.Get(_tree);
			explorer.ExploreRibs(addSeries);
			foreach (var series in _series)
			{
				_model.Series.Add(series);
			}
			return _model;
		}

		private void addSeries(Rib<T> rib)
		{
			var series = new LineSeries
			{
				Color = OxyColors.Red,
				MarkerSize = 10,
				MarkerStroke = OxyColors.Tomato,
				MarkerType = MarkerType.Plus,
				MarkerFill = OxyColors.Tomato
			};
			series.Points.Add(new DataPoint(rib.FirstNode.Data.FirstMeasurement, rib.FirstNode.Data.SecondMeasurement));
			series.Points.Add(new DataPoint(rib.SecondNode.Data.FirstMeasurement, rib.SecondNode.Data.SecondMeasurement));
			_model.Annotations.Add(new TextAnnotation
			{
				TextPosition = new DataPoint(rib.SecondNode.Data.FirstMeasurement, rib.SecondNode.Data.SecondMeasurement),
				Text = rib.SecondNode.Data.Name,
				Stroke = OxyColors.Transparent
			});
			_series.Add(series);
		}
	}
}
