using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubemapFileGenerator
{
    public interface ICubemapConverter
    {
        byte[] Convert(Cubemap cubemap);
    }
}
