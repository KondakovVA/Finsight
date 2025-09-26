using Finsight.Client.AppFrame;

namespace Finsight.Client.Services
{
    public interface IRoleViewManager
    {
        List<IPresenter> GetMenuItems();
        string GetCurrentUsername();
    }
}
