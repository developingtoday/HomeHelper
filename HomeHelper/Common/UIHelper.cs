using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace HomeHelper.Common
{
    public class UiHelper
    {
        /// <summary>
        /// Metoda este luata de la http://blogs.msdn.com/b/eternalcoding/archive/2012/07/03/tips-and-tricks-for-c-metro-developers-the-flyout-control.aspx
        /// </summary>
        /// <param name="element"></param>
        /// <param name="control"></param>
        /// <returns></returns>
        public static Popup ShowPopup(FrameworkElement element, UserControl control)
        {
            var flyout = new Popup();
            var bounds = Window.Current.Bounds;
            var rootVisual = Window.Current.Content;
            var gt = element.TransformToVisual(rootVisual);
            var absolutePosition = gt.TransformPoint(new Point(0, 0));
            control.Measure(new Size(double.PositiveInfinity,double.PositiveInfinity));
            flyout.VerticalOffset = bounds.Height - control.Height - 120;
            flyout.HorizontalOffset = absolutePosition.X + element.ActualWidth/2 - control.Width/2;
            flyout.Child = control;
            flyout.IsLightDismissEnabled = true;
            var transition = new TransitionCollection();
            transition.Add(new PopupThemeTransition()
                               {
                                   FromHorizontalOffset = 0, FromVerticalOffset = 100
                               });
            flyout.ChildTransitions = transition;
            flyout.IsOpen = true;
            //tastatura
            var flyoutOffset = 0;
            Windows.UI.ViewManagement.InputPane.GetForCurrentView().Showing += (s, e) =>
                                                                                   {
                                                                                       flyoutOffset =
                                                                                           (int)e.OccludedRect.Height;
                                                                                       flyout.VerticalOffset -=
                                                                                           flyoutOffset;
                                                                                   };
            Windows.UI.ViewManagement.InputPane.GetForCurrentView().Hiding += (s, e) =>
                                                                                  {
                                                                                      flyout.VerticalOffset +=
                                                                                          flyoutOffset;
                                                                                  };
            return flyout;
        }
    }
}
