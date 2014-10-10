using System.Collections.Generic;
using System.Text;
using System.Windows;
using Algorithms;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Classification
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Spectrum<WebProject> _backingSpectrum;

		private Spectrum<WebProject> _spectrum
		{
			get
			{
				if (_backingSpectrum == null)
				{
					_backingSpectrum = new Spectrum<WebProject>(WebProject.Projects);
					_backingSpectrum.Classify();
				}
				return _backingSpectrum;
			}
		}

		private Crab<WebProject> _backingCrab;

		private Crab<WebProject> _crab
		{
			get { return _backingCrab ?? (_backingCrab = new Crab<WebProject>(WebProject.Projects)); }
		}

		public MainWindow()
		{
			InitializeComponent();
			_groupsCount.ItemsSource = new List<int> { 2, 3, 4, 5, 6 };
			_groupsCount.SelectedValue = ClassificationSettings<WebProject>.GroupsCount;
			_groupsCount.SelectionChanged += (sender, args) =>
			{
				ClassificationSettings<WebProject>.GroupsCount = (int) _groupsCount.SelectedValue;
			};
			Loaded += onLoaded;
		}

		private void onLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
	        _plotView.Model = getDefaultPlotModel();
			setDefaultPlotModelParameters();
			var itemsListBuilder = new StringBuilder();
			foreach (var webProject in WebProject.Projects)
			{
				itemsListBuilder.AppendLine(webProject.Name);
			}
			_classificationResult.Text = itemsListBuilder.ToString();
        }

		private PlotModel getDefaultPlotModel()
		{
			return getPlotModel(new List<List<WebProject>> { WebProject.Projects });
		}

		private PlotModel getPlotModel(List<List<WebProject>> projects)
		{
			var plotModel = new PlotModel();
			foreach (var projectList in projects)
			{
				var s = new LineSeries
				{
					Color = OxyColors.Transparent,
					MarkerStroke = Tools.GetRandomColor(),
					MarkerType = MarkerType.Star,
					MarkerFill = Tools.GetRandomColor(),
					MarkerSize = 8
				};
				foreach (var webProject in projectList)
				{
					s.Points.Add(new DataPoint(webProject.UsersPerDay, webProject.UsersPerMonth));
					plotModel.Annotations.Add(new TextAnnotation
					{
						TextPosition = new DataPoint(webProject.UsersPerDay, webProject.UsersPerMonth),
						Text = webProject.Name,
						Stroke = OxyColors.Transparent
					});
				}
				plotModel.Series.Add(s);
			}
			return plotModel;
		}

		private void showSpectrumClick(object sender, RoutedEventArgs e)
		{
			_plotView.Model = _spectrum.GetSpectrumPlot();
			setDefaultPlotModelParameters();
		}

		private void displaySpectrumClassificationClick(object sender, RoutedEventArgs e)
		{
			afterClassification(_spectrum.Classify().Groups);
		}

	    private void displayTreeClick(object sender, RoutedEventArgs e)
	    {
			_plotView.Model = _crab.BuildTree().Display();
			setDefaultPlotModelParameters();
	    }

	    private void displayCrabClassificationClick(object sender, RoutedEventArgs e)
	    {
		    afterClassification(_crab.Classify());
	    }

		private void afterClassification(List<List<WebProject>> groups)
		{
			_plotView.Model = getPlotModel(groups);
			setDefaultPlotModelParameters();
			_classificationResult.Text = getClassificationRepresentation(groups);
		}

	    private string getClassificationRepresentation(List<List<WebProject>> groups)
	    {
		    var result = new StringBuilder();
		    for (int i = 0; i < groups.Count; i++)
		    {
			    result.AppendFormat("Group #{0}\n", i);
			    var group = groups[i];
			    foreach (var webProject in group)
			    {
				    result.AppendLine(webProject.Name);
			    }
			    result.AppendLine();
		    }
		    return result.ToString();
	    }

	    private void setDefaultPlotModelParameters()
		{
			_plotView.Model.Title = "Social Networks";
			_plotView.Model.Axes.Add(new LinearAxis
			{
				Position = AxisPosition.Bottom,
				Title = "Daily unique visitors count * 1000"
			});
			_plotView.Model.Axes.Add(new LinearAxis
			{
				Position = AxisPosition.Left,
				Title = "Monthly unique visitors count * 1000"
			});
		}

	    private void classifyTroutClick(object sender, RoutedEventArgs e)
	    {
			// TODO
		    var t = new FormalElement<WebProject>(WebProject.Projects);
			afterClassification(t.Classify());
	    }

	    private void setDistanceCoherenceDegreeStrategy(object sender, RoutedEventArgs e)
	    {
		    ClassificationSettings<WebProject>.CalculateDistanceStrategy = new DistanceStrategy<WebProject>();
	    }

	    private void setCosineCoherenceDegreeStrategy(object sender, RoutedEventArgs e)
	    {
			ClassificationSettings<WebProject>.CalculateDistanceStrategy = new CosineStrategy<WebProject>();
	    }
    }
}
