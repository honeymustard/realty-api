namespace Honeymustard
{
    public interface IParser<E>
    {
        E Parse(string blob);
    }
}