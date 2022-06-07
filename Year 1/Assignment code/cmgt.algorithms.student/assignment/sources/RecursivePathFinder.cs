using System.Collections.Generic;

internal class RecursivePathFinder : PathFinder
{
    public RecursivePathFinder(NodeGraph pGraph) : base(pGraph)
    {
    }

    protected override List<Node> generate(Node pFrom, Node pTo)
    {
        throw new System.NotImplementedException();
    }
}