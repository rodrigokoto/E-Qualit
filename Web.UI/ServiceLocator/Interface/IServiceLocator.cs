namespace Web.UI.ServiceLocator.Interface
{
    public interface IServiceLocator
    {
        T GetService<T>();
    }
}
