using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radek.SimpleShell
{
    public interface ISimpleNavigationHost : IView
    {
    }

    public class SimpleNavigationHost : View, ISimpleNavigationHost
    {
    }
}
