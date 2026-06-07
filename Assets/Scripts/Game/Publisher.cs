public interface Publisher
{
    public void AddSubscriber(Subscriber subscriber);
    public void RemoveSubscriber(Subscriber subscriber);
}
