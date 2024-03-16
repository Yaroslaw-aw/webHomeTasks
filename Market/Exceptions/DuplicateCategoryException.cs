namespace Market.Exceptions
{
    public class DuplicateCategoryException : Exception
    {
        public DuplicateCategoryException()
        {
        }

        public DuplicateCategoryException(string message)
            : base(message)
        {
        }

        public DuplicateCategoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
