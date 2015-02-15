using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinCan
{
    public interface IContentManager
    {
        T Load<T>(string p);
    }
}
