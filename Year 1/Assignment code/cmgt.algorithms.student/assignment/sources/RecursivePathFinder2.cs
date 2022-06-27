using System;
using System.Collections.Generic;

internal class RecursivePathFinder2 : PathFinder
{
    public RecursivePathFinder2(NodeGraph pGraph, NodeGraphAgent pAgent) : base(pGraph, pAgent)
    {
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        List<Node> historyNodes = new List<Node>();
        List<Node> path = new List<Node>();
        int depth = 0;

        path.Add(pFrom);

        //Base case.
        if (pFrom == pTo) return null;

        //Begin with pFrom.
        findPath(pFrom, pTo, historyNodes, path, depth);

        return path;
    }

    private void findPath(Node pGivenNode, Node pEndNode, List<Node> pPath, List<Node> pHistory, int pDepth)
    {
        Console.WriteLine($"{Indent(pDepth)}Investigating node {pGivenNode.id} at depth {pDepth}");

        pPath.Add(pGivenNode);

        if (pGivenNode == pEndNode)
        {
            //Found path!
            Console.WriteLine($"{Indent(pDepth)}Found pEndNode at depth {pDepth}");
            return;
        }

        pDepth++;

        //If this point is reached, we know pGivenNode is not the same as pEndNode.
        //Loop over connections of pGivenNode
        loopOverConnections(pGivenNode, pEndNode, pPath, pHistory, pDepth);
    }

    private void loopOverConnections(Node pGivenNode, Node pEndNode, List<Node> pPath, List<Node> pHistory, int pDepth)
    {
        Node selectedNode = null;

        Console.WriteLine($"{Indent(pDepth)}Looping through connections of {pGivenNode.id} at depth {pDepth}");
        //Foreach loop
        foreach (Node connection in pGivenNode.connections)
        {
            Console.WriteLine($"{Indent(pDepth)}Checking out connection node {connection.id} at depth {pDepth}");
            ////Check if connection exists in pHistory AND if exists in pNode.alreadyVisited.
            ///If true, skip
            if (pHistory.Contains(connection)) continue;

            Console.WriteLine($"{Indent(pDepth)}Selected {connection.id} at depth {pDepth}. Adding to history.");
            //////If false, add to both history collections and run findPath(connection).
            pHistory.Add(connection);
            pGivenNode.alreadyVisited.Contains(connection);
            selectedNode = connection;
            break;
            ///Do not try to visit the other connections. 
        }

        //If no unvisited child was found, the recursion at this level ends.
        if (selectedNode != null) findPath(selectedNode, pEndNode, pPath, pHistory, pDepth);
    }

    private static string Indent(int pDepth = 0) => new string('\t', pDepth);
}
