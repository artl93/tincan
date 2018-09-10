using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinCan;

namespace TinCan
{
    public class SampleFileReader : ContentTypeReader<SampleFile>
    {
        public override SampleFile Read(ContentReader input, SampleFile existingInstance)
        {
            return SampleFile.OpenWAVFile(input.BaseStream);
        }
    }
}
