using CbBuild.Xrm.FakeData.Views;

namespace CbBuild.Xrm.FakeData.Presenters
{
    public abstract class ViewPresenterBase<T>
        where T : IControlView
    {
        protected T View { get; private set; }

        protected ViewPresenterBase(T view)
        {
            View = view;
        }
    }
}