using System.Collections.Generic;

namespace Algorithms.Trees
{
	public class Node<T> where T : IClassified<T>
	{
		public T Data { get; private set; }

		public List<Node<T>> RelatedNodes { get; private set; }

		public Node(T data)
		{
			Data = data;
			RelatedNodes = new List<Node<T>>();
		}

		public void AddNode(Node<T> node)
		{
			if (!RelatedNodes.Contains(node))
			{
				RelatedNodes.Add(node);
			}
			if (!node.RelatedNodes.Contains(this))
			{
				node.RelatedNodes.Add(this);
			}
		}
	}

	public class Rib<T> where T : IClassified<T>
	{
		public Node<T> FirstNode { get; private set; }

		public Node<T> SecondNode { get; private set; }

		public double Distance
		{
			get { return ClassificationSettings<T>.CalculateDistanceStrategy.GetDistance(FirstNode.Data, SecondNode.Data); }
		}

		public Rib(Node<T> firstNode, Node<T> secondNode)
		{
			FirstNode = firstNode;
			SecondNode = secondNode;
		}
	}
}
