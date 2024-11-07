namespace SpaceBattle.Lib;

public class TrueNode<T> : INode<T>
{
    public bool decision(IList<T> args)
    {
        return true;
    }
}

