using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;

internal class PathFindingAgent : NodeGraphAgent
{
	private PathFinder pathFinder;

	public PathFindingAgent(NodeGraph pGraph, PathFinder pPathFinder) : base(pGraph)
    {
		pathFinder = pPathFinder;

		//position ourselves on a random node
		if (pGraph.nodes.Count > 0)
		{
			List<Node> allNodes = pGraph.nodes.Values.ToList();

			Node randomNode = allNodes[Utils.Random(0, allNodes.Count)];
			currentNode = randomNode;
			jumpToNode(randomNode);
		}

		pGraph.OnNodeLeftClicked += onNodeClickHandler;
	}

	protected virtual void onNodeClickHandler(Node pNode)
	{
		List<Node> nodes = pathFinder.Generate(currentNode, pNode);
		nodePath = new Queue<Node>(nodes);
	}

	protected override void Update()
	{
		if (nodePath.Count > 0)
		{
			if (targetNode == null)
			{
				targetNode = nodePath.Peek();
			}
		}
		else return;

		if (moveTowardsNode(targetNode))
		{
			currentNode = targetNode;
			targetNode = null;
			nodePath.Dequeue();
		}
	}
}