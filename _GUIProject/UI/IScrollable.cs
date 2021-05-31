using System;
using System.Collections.Generic;
using System.Text;

namespace _GUIProject.UI
{
    interface IScrollable
    {
        int MaxNumberOfLines { get; set; }
        int NumberOfLines { get; }
        void ApplyScroll();
    }
}
