namespace SpaceBattle.Lib;

public interface IReciever
{
    ICommand Recieve();
    bool isEmpty();
}

