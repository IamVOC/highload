namespace SpaceBattle.Lib;

using Hwdtech;

public class CollisionCheckStrategy : IStrategy
{
    public object run_strategy(params object[] args)
    {
        var l = new List<int>();
        foreach (Vector arg in args)
        {
            foreach (int coord in arg.getCoords())
            {
                l.Add(coord);
            }
        }
        var tree = IoC.Resolve<INode<int>>("Game.Trees.Collision");
        return tree.decision(l);
    }
}

