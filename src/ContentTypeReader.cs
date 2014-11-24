using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinCan
{
    public abstract class ContentTypeReader<T>
    {
        abstract public T Read(ContentReader input, T existingInstance);

    }
}
