using ModernWpf.Controls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExtensionHandler.Settings.Model
{
    public class GUI
    {
        [JsonProperty("navbar_paneDisplayMode")]
        public NavigationViewPaneDisplayMode Navbar_PaneDisplayMode;
    }
}
