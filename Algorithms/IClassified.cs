namespace Algorithms
{
	public interface IClassified<T> where T : IClassified<T>
    {
		string Name { get; }

		double FirstMeasurement { get; }

		double SecondMeasurement { get; }
    }
}
