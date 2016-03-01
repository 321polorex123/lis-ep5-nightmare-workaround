using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LifeIsStrangeSaveEditor.Ep5Fix
{
    internal static class MsgBox
    {
        public static void ShowInfo(string msg)
        {
            ShowMessageBox(msg, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ShowWarning(string msg)
        {
            ShowMessageBox(msg, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static bool ShowQuestion(string msg)
        {
            var rslt = ShowMessageBox(msg, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return rslt == MessageBoxResult.Yes;
        }

        private static MessageBoxResult ShowMessageBox(string msg, MessageBoxButton btns, MessageBoxImage icon)
        {
            return MessageBox.Show(
                msg,
                "Life is Strange Episode 5 Nightmare Fix",
                btns,
                icon
                );
        }
    }
}
