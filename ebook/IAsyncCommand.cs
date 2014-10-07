using System.Threading.Tasks;
using System.Windows.Input;

namespace ebook
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
