using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.Web.Mvc;

namespace NReport
{
    public class SettingsHelper
    {
        static Action<MVCxFormLayoutItem> formLayoutItemSettingsMethod;
        public static Action<MVCxFormLayoutItem> FormLayoutItemSettingsMethod
        {
            get
            {
                if (formLayoutItemSettingsMethod == null)
                    formLayoutItemSettingsMethod = CreateFormLayoutItemSettingsMethod();
                return formLayoutItemSettingsMethod;
            }
        }
        static Action<MVCxFormLayoutItem> CreateFormLayoutItemSettingsMethod()
        {
            return itemSettings => {
                itemSettings.Width = Unit.Percentage(100);
                dynamic editorSettings = itemSettings.NestedExtensionSettings;
                editorSettings.Properties.ValidationSettings.ErrorDisplayMode = ErrorDisplayMode.ImageWithText;
                editorSettings.Properties.ValidationSettings.ErrorTextPosition = ErrorTextPosition.Bottom;
                editorSettings.Properties.ValidationSettings.Display = Display.Dynamic;
                editorSettings.ShowModelErrors = true;
                editorSettings.Width = Unit.Percentage(100);
            };
        }
    }
}