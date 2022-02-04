using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FileExtensionHandler.Shared
{
    public class External
    {
        // A slightly modified variant of the code
        // https://stackoverflow.com/a/17969470
        public static bool IsBindingValid(DependencyObject parent)
        {
            if (Validation.GetHasError(parent)) return false;
            for (int i = 0; i != VisualTreeHelper.GetChildrenCount(parent); ++i)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (!IsBindingValid(child)) return false;
            }
            return true;
        }

        // A direct copy of the code
        // https://coderedirect.com/questions/678281/reset-wpf-datagrid-scrollbar-position
        public static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
