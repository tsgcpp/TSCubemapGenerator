using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TSCubemapGenerator
{
    public interface ICubemapConverter
    {
        string FileExtension { get; }

        byte[] ConvertFrom(Cubemap cubemap);
    }
}
