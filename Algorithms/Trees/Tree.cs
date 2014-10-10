using System;
using System.Collections.Generic;
using OxyPlot;

namespace Algorithms.Trees
{
	public class Tree<T> where T : IClassified<T>
	{
		public Tree(Node<T> root)
		{
			Root = root;
		}

		public Node<T> Root { get; private set; }

		public Tuple<Tree<T>, Tree<T>> Split(Node<T> node1, Node<T> node2)
		{
			return TreeSplitter<T>.Split(this, node1, node2);
		}

		public PlotModel Display()
		{
			return new TreeDisplay<T>(this).Display();
		}

		public double GetRibsSum()
		{
			var explorer = TreeExplorer<T>.Get(this);
			var sum = 0d;
			explorer.ExploreRibs(rib => sum += ClassificationSettings<T>.CalculateDistanceStrategy.GetDistance(rib.FirstNode.Data, rib.SecondNode.Data));
			return sum;
		}

		public int GetNodesCount()
		{
			var explorer = TreeExplorer<T>.Get(this);
			var sum = 0;
			explorer.ExploreNodes(node => sum++);
			return sum;
		}

		public List<Node<T>> GetNodes()
		{
			var explorer = TreeExplorer<T>.Get(this);
			var nodes = new List<Node<T>>();
			explorer.ExploreNodes(nodes.Add);
			return nodes;
		}

		public Rib<T> GetMaxRib()
		{
			var explorer = TreeExplorer<T>.Get(this);
			var maxRibLength = 0d;
			Rib<T> maxRib = null;
			explorer.ExploreRibs(rib =>
			{
				if (rib.Distance > maxRibLength)
				{
					maxRib = rib;
					maxRibLength = rib.Distance;
				}
			});
			return maxRib;
		} 
	}
}
