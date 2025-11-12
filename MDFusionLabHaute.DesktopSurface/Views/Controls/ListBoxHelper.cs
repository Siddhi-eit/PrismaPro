using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace MDFusionLabHaute.DesktopSurface.Views.Controls
{
    public static class ListBoxHelper
    {
        public static readonly DependencyProperty IsSmoothScrollEnabledProperty =
            DependencyProperty.RegisterAttached("IsSmoothScrollEnabled", typeof(bool), typeof(ListBoxHelper), new PropertyMetadata(false, OnIsSmoothScrollEnabledChanged));

        public static bool GetIsSmoothScrollEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSmoothScrollEnabledProperty);
        }

        public static void SetIsSmoothScrollEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSmoothScrollEnabledProperty, value);
        }

        private static void OnIsSmoothScrollEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ScrollViewer scrollViewer)
            {
                if ((bool)e.NewValue)
                {
                    scrollViewer.PreviewMouseWheel += ScrollViewer_PreviewMouseWheel;
                }
                else
                {
                    scrollViewer.PreviewMouseWheel -= ScrollViewer_PreviewMouseWheel;
                }
            }
        }

        private static void ScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = (ScrollViewer)sender;
            double change = e.Delta > 0 ? -10 : 10; // Change this value for the smoothness you desire
            scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + change);
            e.Handled = true;
        }
    }
}
