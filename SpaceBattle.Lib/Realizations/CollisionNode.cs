namespace SpaceBattle.Lib;

public class CollisionNode<T> : INode<T>
{
    IDictionary<T, INode<T>> nodes;
    int level;
    public CollisionNode(IDictionary<T, INode<T>> l, int lev)
    {
        this.nodes = l;
        this.level = lev;
    }
    public bool decision(IList<T> args)
    {
        return nodes[args[level]].decision(args);
    }
}

