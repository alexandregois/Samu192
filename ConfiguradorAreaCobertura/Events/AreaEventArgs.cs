using System;
using SAMU192Core.DTO;

namespace ConfiguradorAreaCobertura.Events
{
    public class AreaEventArgs:EventArgs
    {

        private AreaDTO area;
        public AreaDTO Area { get { return area; } }

        public AreaEventArgs(AreaDTO area)
        {
            this.area = area;
        }

    }
}
