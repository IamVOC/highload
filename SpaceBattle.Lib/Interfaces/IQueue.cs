namespace SpaceBattle.Lib;

public interface IQueue<T>
{
    void Push(T obj);
    T Pop();
}

