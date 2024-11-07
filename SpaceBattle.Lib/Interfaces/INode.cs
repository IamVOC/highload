namespace SpaceBattle.Lib;

public interface INode<T>
{
    public bool decision(IList<T> args);
}

