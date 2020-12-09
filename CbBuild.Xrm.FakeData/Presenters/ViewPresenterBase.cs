using CbBuild.Xrm.FakeData.Views;

namespace CbBuild.Xrm.FakeData.Presenters
{
    public interface IViewPresenterBase<T>
        where T : IControlView
    {
        T View { get; }
    }

    public abstract class ViewPresenterBase<T> : IViewPresenterBase<T>
        where T : IControlView
    {
        public T View { get; private set; }

        protected ViewPresenterBase(T view)
        {
            View = view;
        }
    }
}