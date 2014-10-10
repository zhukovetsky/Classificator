using System;
using System.Collections.Generic;

namespace Algorithms.Trees
{
	public class TreeExplorer<T> where T : IClassified<T>
	{
		private readonly Tree<T> _tree;

		private static readonly Dictionary<Tree<T>, TreeExplorer<T>> _treeExplorers; 

		private TreeExplorer(Tree<T> tree)
		{
			_tree = tree;
		}

		static TreeExplorer()
		{
			_treeExplorers = new Dictionary<Tree<T>, TreeExplorer<T>>();
		}

		public static TreeExplorer<T> Get(Tree<T> tree)
		{
			TreeExplorer<T> explorer;
			if (!_treeExplorers.TryGetValue(tree, out explorer))
			{
				explorer = new TreeExplorer<T>(tree);
				_treeExplorers[tree] = explorer;
			}
			return explorer;
		}

		public void ExploreNodes(Action<Node<T>> nodeAction)
		{
			nodeAction(_tree.Root);
			loop(_tree.Root, null, nodeAction, null);
		}

		public void ExploreRibs(Action<Rib<T>> ribAction)
		{
			loop(_tree.Root, null, null, ribAction);
		}

		private void loop(Node<T> startNode, Node<T> excludedNode, Action<Node<T>> nodeAction, Action<Rib<T>> ribAction)
		{
			foreach (var node in startNode.RelatedNodes)
			{
				if (node == excludedNode)
				{
					continue;
				}
				if (nodeAction != null)
				{
					nodeAction(node);
				}
				if (ribAction != null)
				{
					ribAction(new Rib<T>(startNode, node));
				}
				loop(node, startNode, nodeAction, ribAction);
			}
		}
	}
}
