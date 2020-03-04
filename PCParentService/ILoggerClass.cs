namespace PCParentServiceApp
{
    public interface ILoggerClass
    {
        void CreateNewEventViewerLog();
        void WriteLoginToEventViewer();
        void WriteTransactionToEventViewer(string transaction);
        void WriteExceptionToEventViewer(string exception);
        void WriteLogoffToEventViewer();
    }
}