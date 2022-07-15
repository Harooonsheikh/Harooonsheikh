using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VSI.EDGEAXConnector.UI.Helpers
{
    public static  class ExceptionHelper
    {
        public static void ShowMessage(Exception ex)
        {
            string innerMessage = ex != null
                ? string.Format("Errors: {0} {1}" + Environment.NewLine,
                ex.Message,
                ex.InnerException != null ? ex.InnerException.Message : ""):"Exception is null";
            MessageBox.Show(innerMessage);
        }
    }
}
