using System;

namespace Algorithms.Trees
{
	internal class TreeSplitter<T> where T : IClassified<T>
	{
		private readonly Node<T> _node1;
		private readonly Node<T> _node2;

		private Tree<T> _firstSubtree;

		private Tree<T> _secondSubtree;

		private TreeSplitter(Node<T> node1, Node<T> node2)
		{
			_node1 = node1;
			_node2 = node2;
		}

		public static Tuple<Tree<T>, Tree<T>> Split(Tree<T> tree, Node<T> node1, Node<T> node2)
		{
			return new TreeSplitter<T>(node1, node2).split(tree.Root);
		}

		private Tuple<Tree<T>, Tree<T>> split(Node<T> startNode)
		{
			_firstSubtree = new Tree<T>(new Node<T>(startNode.Data));
			loop(startNode, _firstSubtree.Root, null);
			return Tuple.Create(_firstSubtree, _secondSubtree);
		}

		private void loop(Node<T> sourceTreeStartNode, Node<T> targetTreeStartNode, Node<T> excludedNode)
		{
			var canSplit = sourceTreeStartNode == _node1 || sourceTreeStartNode == _node2;
			foreach (var node in sourceTreeStartNode.RelatedNodes)
			{
				if (node == excludedNode)
				{
					continue;
				}
				if (canSplit)
				{
					if (node == _node1 || node == _node2)
					{
						_secondSubtree = new Tree<T>(new Node<T>(node.Data));
						loop(node, _secondSubtree.Root, sourceTreeStartNode);
						continue;
					}
				}
				var newNode = new Node<T>(node.Data);
				targetTreeStartNode.AddNode(newNode);
				loop(node, newNode, sourceTreeStartNode);
			}
		}
	}
}
