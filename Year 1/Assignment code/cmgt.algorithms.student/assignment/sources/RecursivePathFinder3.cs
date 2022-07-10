using System;
using System.Collections.Generic;

internal class RecursivePathFinder3 : PathFinder
{
    private int shortestLength = int.MaxValue;

    public RecursivePathFinder3(NodeGraph pGraph, NodeGraphAgent pAgent) : base(pGraph, pAgent)
    {
    }


    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        List<Node> path = new List<Node>();
        int depth = -1;
        shortestLength = int.MaxValue;

        if (pFrom == pTo) return path;

        findPath(pFrom, pTo, depth, 0);
        path = retracePath(pTo, pFrom);

        return path;
    }

    //------------------------------------------------------------------------------------------------------------------------
    //									    void findPath()
    //------------------------------------------------------------------------------------------------------------------------
    /// Recursive pathfinding algorithm (Depth First Search).
    /// Is the current path longer than the existing shortest path? No point in wasting more resources to trying to find a path, so stop the search.
    /// Did we find the end node? Set the shortest path variable to the current length and return.
    /// When returning (or if the findPath function reaches the last line, we retrace to the last node. We also set the current node back to "unvisited", so future shortest paths aren't blocked off. Repeated visits relative from the same node are prevented because from the current standpoint, the connection has already been visited (and is set back to unvisited before retracing), but seeing the node has already passed in the foreach loop, we won't visit it again from this node.
    /// </summary>
    private void findPath(Node pNode, Node pEndNode, int pDepth, int pCurrentLength)
    {
        pDepth += 1;
        pCurrentLength += 1;

        //1st base case
        if (pCurrentLength > shortestLength)
        {
            Console.WriteLine($"{indent(pDepth)}Current path (length {pCurrentLength}) is already longer than shortest path (length {shortestLength}). Returning.");
            return;
        }

        Console.WriteLine($"{indent(pDepth)}Current node {pNode.id}");

        //2nd base case
        if (pNode == pEndNode)
        {
            Console.WriteLine($"{indent(pDepth)}FOUND PATH to node {pNode.id} Current length: {pCurrentLength}, shortest length: {shortestLength}");
            shortestLength = pCurrentLength;
            return;
        }

        pNode.visited = true;

        Console.WriteLine($"{indent(pDepth)}Looping through connections of node {pNode.id}");
        foreach (Node connection in pNode.connections)
        {
            Console.WriteLine($"{indent(pDepth)}Current connection of node {pNode.id} - {connection.id} -, already visited? {connection.visited}");
            if (connection.visited || connection == pNode) continue;
              
            Console.WriteLine($"{indent(pDepth)}Setting previous node of connection {connection.id} to node {pNode.id}");
            connection.cameFromNode = pNode;

            findPath(connection, pEndNode, pDepth, pCurrentLength);
        }

        pNode.visited = false;
        Console.WriteLine($"{indent(pDepth)}Tracing back from node {pNode.id}");
    }

    //To see how many lines of code I actually have without white space/comments/Console.WriteLine's. (spoiler: it's 16)
    //private void findPath(Node pNode, Node pEndNode, int pDepth, int pCurrentLength)
    //{
    //    pDepth += 1;
    //    pCurrentLength += 1;
    //    if (pCurrentLength > shortestLength) return;
    //    if (pNode == pEndNode)
    //    {
    //        shortestLength = pCurrentLength;
    //        return;
    //    }
    //    pNode.visited = true;
    //    foreach (Node connection in pNode.connections)
    //    {
    //        if (connection.visited || connection == pNode) continue;
    //        connection.cameFromNode = pNode;
    //        findPath(connection, pEndNode, pDepth, pCurrentLength);
    //    }
    //    pNode.visited = false;
    //}
}