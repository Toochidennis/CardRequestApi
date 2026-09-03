namespace CardRequestApi.Exceptions;

public class DuplicateCardRequestException : Exception
{
    public DuplicateCardRequestException(string message)
        : base(message)
    {
    }
}