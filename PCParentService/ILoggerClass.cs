namespace PCParentServiceApp
{
    public interface ILoggerClass
    {
        void WriteLoginToEventViewer();
        void WriteTransactionToEventViewer();
        void WriteExceptionToEventViewer(string exception);
        void WriteLogoffToEventViewer();
    }
}