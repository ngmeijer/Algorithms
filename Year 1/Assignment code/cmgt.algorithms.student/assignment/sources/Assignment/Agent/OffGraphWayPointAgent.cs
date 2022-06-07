class OffGraphWayPointAgent : SampleNodeGraphAgent
{
	public OffGraphWayPointAgent(NodeGraph pNodeGraph) : base(pNodeGraph)
	{
		
	}

    protected override void Update()
    {
        if (targetNode != null && currentNode != null)
            if(!currentNode.connections.Contains(targetNode)) return;

        base.Update();
    } 
}