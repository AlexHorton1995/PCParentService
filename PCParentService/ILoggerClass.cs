namespace PCParentServiceApp
{
    public interface ILoggerClass
    {
        void WriteExceptionToEventViewer(string exception);
        void WriteLoginToEventViewer();
    }
}