using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Neet.Guitar
{
    public class LoopTickGenerator
    {
        public event EventHandler TickGenerated;

        private readonly Thread _thread;
        private bool _disposed;

        public LoopTickGenerator()
        {
            _thread = new Thread(() =>
            {
                for (var i = 0; ; i++)
                {
                    if (i % 1000 == 0)
                        TickGenerated?.Invoke(this, EventArgs.Empty);
                }
            });
        }

        public void TryStart()
        {
            if (_thread.IsAlive)
                return;

            _thread.Start();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _thread.Abort();
            }

            _disposed = true;
        }
    }
}
