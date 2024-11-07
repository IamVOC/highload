namespace SpaceBattle.Lib;

public class FalseNode<T> : INode<T>
{
    public bool decision(IList<T> args)
    {
        return false;
    }
}

