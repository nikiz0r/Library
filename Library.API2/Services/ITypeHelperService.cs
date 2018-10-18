namespace Library.API2.Services
{
    public interface ITypeHelperService
    {
         bool TypeHasProperties<T>(string fields);
    }
}