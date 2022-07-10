using System.Collections.Generic;

internal class BreadthFirstPathFinder : PathFinder
{
    public BreadthFirstPathFinder(NodeGraph pGraph, NodeGraphAgent pAgent) : base(pGraph, pAgent)
    {
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        List<Node> path = new List<Node>();
        Queue<Node> nodeQ = new Queue<Node>();
        List<Node> visitedNodes = new List<Node>();

        nodeQ.Enqueue(pFrom);

        findPath(pTo, nodeQ, visitedNodes);

        path = retracePath(pTo, pFrom);

        return path;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //									    void findPath()
    //------------------------------------------------------------------------------------------------------------------------
    /// Breath first search pathfinding algorithm. Kind of like a circle, getting bigger and bigger in all directions until the target node has been found.
    /// Never visits a node twice (so no backtracking). Keeps track of the path by setting a node-instance owned variable.
    /// </summary>
    private void findPath(Node pTo, Queue<Node> nodeQ, List<Node> visitedNodes)
    {
        while (nodeQ.Count > 0)
        {
            Node currentNode = nodeQ.Peek();
            nodeQ.Dequeue();
            if (currentNode == pTo) break;

            foreach (Node connection in currentNode.connections)
            {
                if (visitedNodes.Contains(connection)) continue;

                nodeQ.Enqueue(connection);
                visitedNodes.Add(connection);
                connection.cameFromNode = currentNode;
            }
        }
    }
}