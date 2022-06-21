using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;

class OffGraphWayPointAgent : NodeGraphAgent
{
	//Current target to move towards
	protected Node targetNode = null;
	protected Node currentNode = null;
	protected bool isMoving;
	protected Node lastAddedNode = null;

	public OffGraphWayPointAgent(NodeGraph pNodeGraph) : base(pNodeGraph)
	{
		SetOrigin(width / 2, height / 2);

		//position ourselves on a random node
		if (pNodeGraph.nodes.Count > 0)
		{
			List<Node> allNodes = pNodeGraph.nodes.Values.ToList();

			Node randomNode = allNodes[Utils.Random(0, allNodes.Count)];
			currentNode = randomNode;
			lastAddedNode = currentNode;
			// jumpToNode(pNodeGraph.nodes[Utils.Random(0, pNodeGraph.nodes.Count)]);
			jumpToNode(randomNode);
		}

		//listen to nodeclicks
		pNodeGraph.OnNodeLeftClicked += onNodeClickHandler;
	}

	protected virtual void onNodeClickHandler(Node pNode)
	{
		if (lastAddedNode != null)
		{
			if (!lastAddedNode.connections.Contains(pNode)) return;
		}
		nodePath.Enqueue(pNode);
		lastAddedNode = pNode;
	}

	protected override void Update()
	{
		if (targetNode != null && currentNode != null)
			if (!currentNode.connections.Contains(targetNode)) return;

		if (nodePath.Count > 0)
		{
			if (targetNode == null)
			{
				targetNode = nodePath.Peek();
			}
		}
		else return;

		//Console.WriteLine($"Target node: {targetNode.id}. Nodes to go: {nodePath.Count}");

		//Move towards the first node in the queue, if we reached it, clear the target and take the next first node in the queue until empty.
		if (moveTowardsNode(targetNode))
		{
			currentNode = targetNode;
			targetNode = null;
			nodePath.Dequeue();
		}
		//else Console.WriteLine($"Should still be moving towards {targetNode.id}");
	}
}