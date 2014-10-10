namespace Algorithms
{
	public static class ClassificationSettings<T> where T : IClassified<T>
	{
		public static ICalculateDistanceStrategy<T> CalculateDistanceStrategy { get; set; }

		// TODO
		public static int GroupsCount { get; set; }

		static ClassificationSettings()
		{
			CalculateDistanceStrategy = new DistanceStrategy<T>();
			GroupsCount = 3;
		}
	}
}
