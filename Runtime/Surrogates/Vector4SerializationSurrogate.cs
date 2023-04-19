using UnityEngine;
using System.Runtime.Serialization;
using System.Collections;
namespace Rotslib.Surrogate
{
    public class Vector4SerializationSurrogate : ISerializationSurrogate
    {

        public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext context)
        {

            Vector4 v3 = (Vector4)obj;
            info.AddValue("x", v3.x);
            info.AddValue("y", v3.y);
            info.AddValue("z", v3.z);
            info.AddValue("w", v3.w);
        }

        public System.Object SetObjectData(System.Object obj, SerializationInfo info,
                                           StreamingContext context, ISurrogateSelector selector)
        {

            Vector4 v3 = (Vector4)obj;
            v3.x = (float)info.GetValue("x", typeof(float));
            v3.y = (float)info.GetValue("y", typeof(float));
            v3.z = (float)info.GetValue("z", typeof(float));
            v3.w = (float)info.GetValue("w", typeof(float));
            obj = v3;
            return obj;
        }
    }
}