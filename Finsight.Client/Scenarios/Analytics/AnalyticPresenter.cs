using Finsight.Client.AppFrame;

namespace Finsight.Client.Scenarios.Analytics
{
    internal class AnalyticPresenter : PresenterBase<AnalyticView, AnalyticViewModel>
    {
        public AnalyticPresenter() : base(new AnalyticView(), new AnalyticViewModel(), "Аналитика")
        {
            
        }

        protected override Task OnActivateAsync(bool isInitialActivation)
        {
            throw new NotImplementedException();
        }
    }
}
