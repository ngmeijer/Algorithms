using System.Collections.Generic;

internal class BreadthFirstPathFinder : PathFinder
{
    public BreadthFirstPathFinder(NodeGraph pGraph) : base(pGraph)
    {
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        List<Node> path = new List<Node>();
        Queue<Node> nodeQ = new Queue<Node>();
        List<Node> visitedNodes = new List<Node>();

        nodeQ.Enqueue(pFrom);

        while (nodeQ.Count > 0)
        {
            Node currentNode = nodeQ.Peek();
            nodeQ.Dequeue();
            foreach (Node childNode in currentNode.connections)
            {
                if (visitedNodes.Contains(childNode)) continue;

                visitedNodes.Add(childNode);
                if (childNode == pTo)
                {

                    break;
                }
            }
        }

        return path;
    }
}