using System;
using System.Collections.Generic;
using System.Linq;
using Algorithms.Trees;

namespace Algorithms
{
	internal class QualityFuncCalculator<T> where T : IClassified<T>
	{
		private readonly Dictionary<Tree<T>, Rib<T>> _trees;
		
		public QualityFuncCalculator(Dictionary<Tree<T>, Rib<T>> trees)
		{
			_trees = trees;
		}

		public double Calculate()
		{
			return Math.Log((getDValue() * getHValue()) / (getGValue() * getRValue()));
		}

		private double getRValue()
		{
			return _trees.Sum(tree => tree.Key.GetRibsSum() / tree.Key.GetNodesCount()) / _trees.Count;
		}

		private double getGValue()
		{
			return _trees.Values.Sum(rib => rib == null ? 0 : rib.Distance) / (_trees.Count - 1);
		}

		private double getHValue()
		{
			var nearestRibs = new List<double>();
			foreach (var tree in _trees)
			{
				if (tree.Value == null)
				{
					continue;
				}
				var nearestRib = getNearestGroupRib(tree);
				if (nearestRib != null)
				{
					nearestRibs.Add(nearestRib.Item2 / tree.Value.Distance);
				}
			}
			return nearestRibs.Sum() / (_trees.Count - 1);
		}

		private Tuple<Node<T>, double> getNearestGroupRib(KeyValuePair<Tree<T>, Rib<T>> tree)
		{
			var minDistance = double.MaxValue;
			Node<T> nearestNode = null;
			m(tree.Value.FirstNode, tree.Value.SecondNode, ref nearestNode, ref minDistance);
			m(tree.Value.SecondNode, tree.Value.FirstNode, ref nearestNode, ref minDistance);
			return nearestNode == null ? null : Tuple.Create(nearestNode, minDistance);
		}

		private void m(Node<T> selfNode, Node<T> oppositeNode , ref Node<T> nearestNode, ref double distance)
		{
			foreach (var node in selfNode.RelatedNodes)
			{
				if (node == oppositeNode || _trees.Any(t =>  t.Value != null && (t.Value.FirstNode == node || t.Value.SecondNode == node)))
				{
					continue;
				}
				var newMinDistance = ClassificationSettings<T>.CalculateDistanceStrategy.GetDistance(selfNode.Data, node.Data);
				if (newMinDistance < distance)
				{
					distance = newMinDistance;
					nearestNode = node;
				}
			}
		}

		private double getDValue()
		{
			var result = 1d;
			var perfectGroupMembersCount = _trees.Sum(tree => tree.Key.GetNodesCount()) / _trees.Count;
			foreach (var tree in _trees)
			{
				result *= Math.Abs(perfectGroupMembersCount - tree.Key.GetNodesCount() / perfectGroupMembersCount);
			}
			return result;
		}
	}
}
