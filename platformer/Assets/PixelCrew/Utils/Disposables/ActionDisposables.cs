using System;

namespace PixelCrew.Utils.Disposables
{
    public class ActionDisposables : IDisposable
    {
        private Action _onDispose;
        public ActionDisposables(Action onDispose)
        {
            _onDispose = onDispose;
        }
        public void Dispose()
        {
            _onDispose?.Invoke();
            _onDispose = null;
        }
    }
}