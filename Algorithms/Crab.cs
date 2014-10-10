using System;
using System.Collections.Generic;
using System.Linq;
using Algorithms.Trees;

namespace Algorithms
{
    public class Crab<T> where T : IClassified<T>
    {
        private readonly List<T> _items;

		public ICalculateDistanceStrategy<T> CalculateDistanceStrategy { get; set; }

        public Crab(List<T> items)
        {
            _items = items;
	        CalculateDistanceStrategy = new DistanceStrategy<T>();
        }

		public Tree<T> BuildTree()
		{
			return TreeBuilder.BuildTree(_items);
		}

        public List<List<T>> Classify()
        {
            var sourceTree = BuildTree();
			double oldQuality;
	        double newQuality = double.MinValue;
			var trees = new Dictionary<Tree<T>, Rib<T>> { { sourceTree, null } };
	        Dictionary<Tree<T>, Rib<T>>  previousStepTrees;
			do
			{
				previousStepTrees = trees;
		        oldQuality = newQuality;
		        Rib<T> maxRib = null;
				Tree<T> maxRibTree = null;
				double maxRibLength = 0d;
				foreach (var tree in trees)
				{
					var rib = tree.Key.GetMaxRib();
					if (rib != null && rib.Distance > maxRibLength)
					{
						maxRib = rib;
						maxRibTree = tree.Key;
						maxRibLength = rib.Distance;
					}
				}
				if (maxRib == null)
				{
					break;
				}
				var t = new Dictionary<Tree<T>, Rib<T>> ();
				foreach (var tree in trees)
				{
					if (tree.Key == maxRibTree)
					{
						var splittedTrees = tree.Key.Split(maxRib.FirstNode, maxRib.SecondNode);
						t.Add(splittedTrees.Item1, null);
						t.Add(splittedTrees.Item2, maxRib);
						continue;
					}
					t.Add(tree.Key, tree.Value);
				}
		        trees = t;
				newQuality = new QualityFuncCalculator<T>(trees).Calculate();
			} while (oldQuality < newQuality);
	        return previousStepTrees.Select(tree => tree.Key.GetNodes().Select(node => node.Data).ToList()).ToList();
        }

	    private class TreeBuilder
	    {
		    private readonly List<T> _items;

		    private readonly List<Node<T>> _usedNodes = new List<Node<T>>();

		    private Tree<T> _resultTree;

		    private TreeBuilder(List<T> items)
		    {
			    _items = items;
		    }

		    public static Tree<T> BuildTree(List<T> items)
		    {
			    return new TreeBuilder(items).buildTree();
		    }

		    private Tree<T> buildTree()
		    {
			    var startNodeData = _items.First();
			    var rootNode = new Node<T>(startNodeData);
			    _usedNodes.Add(rootNode);
			    _resultTree = new Tree<T>(rootNode);
			    while (_usedNodes.Count != _items.Count)
			    {
				    createNearestRib();
			    }
			    return _resultTree;
		    }

		    private void createNearestRib()
		    {
			    Node<T> existingNode = null;
			    T newNodeData = default(T);
			    double distance = double.MaxValue;
			    foreach (var usedNode in _usedNodes)
			    {
				    foreach (var nodeData in _items)
				    {
						if (_usedNodes.Any(e => e.Data.Equals(nodeData)))
					    {
						    continue;
					    }
					    var newDistance = ClassificationSettings<T>.CalculateDistanceStrategy.GetDistance(usedNode.Data, nodeData);
					    if (newDistance < distance)
					    {
						    distance = newDistance;
						    newNodeData = nodeData;
						    existingNode = usedNode;
					    }
				    }
			    }
			    if (existingNode == null || newNodeData.Equals(default(T)))
			    {
				    throw new NotSupportedException();
			    }
			    var newNode = new Node<T>(newNodeData);
			    existingNode.AddNode(newNode);
			    _usedNodes.Add(newNode);
		    }
	    }
    }
}
