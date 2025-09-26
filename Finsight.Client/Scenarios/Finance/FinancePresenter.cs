using Finsight.Client.AppFrame;

namespace Finsight.Client.Scenarios.Finance
{
    internal class FinancePresenter : PresenterBase<FinanceView, FinanceViewModel>
    {
        public FinancePresenter() : base(new FinanceView(), new FinanceViewModel(), "Финансы")
        {
            
        }

        protected override Task OnActivateAsync(bool isInitialActivation)
        {
            throw new NotImplementedException();
        }
    }
}
