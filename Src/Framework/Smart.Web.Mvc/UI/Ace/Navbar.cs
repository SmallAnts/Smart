using Smart.Core;
using System;
using System.Web.Mvc;

namespace Smart.Web.Mvc.UI.Ace
{
    public class Navbar : DisposableObject
    {
        private readonly ViewContext _viewContext;

        public Navbar(ViewContext viewContext)
        {
            if (viewContext == null)
            {
                throw new ArgumentNullException("viewContext");
            }
            this._viewContext = viewContext;
        }

        public void EndNavbar()
        {
            this.Dispose(true);
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                base.Dispose(disposing);
                _viewContext.Writer.Write("</div>");
            }
        }

        public ToggleButtons BeginToggleButtons()
        {
            return new ToggleButtons();
        }
        public Header BeginHeader()
        {
            return new Header();
        }
        public Menus BeginMenus()
        {
            return new Menus();
        }
        public Buttons BeginButtons()
        {
            return new Buttons();
        }

        public class ToggleButtons : IDisposable
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
        public class Header : IDisposable
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
        public class Menus : IDisposable
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
        public class Buttons : IDisposable
        {
            public void Dispose()
            {
                throw new NotImplementedException();
            }
        }
    }
}
