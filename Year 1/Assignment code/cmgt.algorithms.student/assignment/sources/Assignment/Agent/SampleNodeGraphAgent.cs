using System;
using System.Collections.Generic;
using System.Linq;
using GXPEngine;

/**
 * Very simple example of a nodegraphagent that walks directly to the node you clicked on,
 * ignoring walls, connections etc.
 */
class SampleNodeGraphAgent : NodeGraphAgent
{
	//Current target to move towards
	protected Node targetNode = null;
	protected Node currentNode = null;
	protected Queue<Node> nodePath = new Queue<Node>();
	protected bool isMoving;
	protected Node lastAddedNode = null;

	public SampleNodeGraphAgent(NodeGraph pNodeGraph) : base(pNodeGraph)
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
		if(lastAddedNode != null)
        {
			if (!lastAddedNode.connections.Contains(pNode)) return;
        }
		nodePath.Enqueue(pNode);
		lastAddedNode = pNode;
	}

	protected override void Update()
	{
		if (nodePath.Count > 0)
		{
			if (targetNode == null)
			{
				targetNode = nodePath.Peek();
				Console.WriteLine($"New target node: {targetNode.location}");
			}
		}
		else return;

		//Move towards the first node in the queue, if we reached it, clear the target and take the next first node in the queue until empty.
		if (moveTowardsNode(targetNode))
		{
			currentNode = targetNode;
			targetNode = null;
			nodePath.Dequeue();
		}
	}
}
