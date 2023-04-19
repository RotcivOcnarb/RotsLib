using Rotslib.Surrogate;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
namespace Rotslib.Saving {

    public class SaveSystem {
        public static bool logFilePath;
        public static void SaveGame<T>(T data) {
            BinaryFormatter bf = new BinaryFormatter();
            bf.SurrogateSelector = GetSurrogates();

            if (Debug.isDebugBuild && logFilePath) {
                Debug.Log("Save path: " + Application.persistentDataPath);
            }
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.OpenOrCreate);
            bf.Serialize(file, data);
            file.Close();
        }

        public static bool LoadGame<T>(out T data) {
            try {
                BinaryFormatter bf = new BinaryFormatter();
                bf.SurrogateSelector = GetSurrogates();

                using (FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.OpenOrCreate)) {
                    data = (T)bf.Deserialize(file);
                }
                return true;
            }
            catch (SerializationException e) {
                Debug.Log(e);
                data = default;
                return false;
            }
        }

        private static SurrogateSelector GetSurrogates()
        {
            SurrogateSelector surrogateSelector = new SurrogateSelector();

            //Vector2 surrogate
            Vector2SerializationSurrogate vector2SS = new Vector2SerializationSurrogate();
            surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext(StreamingContextStates.All), vector2SS);

            //Vector2Int surrogate
            Vector2IntSerializationSurrogate vector2intSS = new Vector2IntSerializationSurrogate();
            surrogateSelector.AddSurrogate(typeof(Vector2Int), new StreamingContext(StreamingContextStates.All), vector2intSS);

            //Vector3 surrogate
            Vector3SerializationSurrogate vector3SS = new Vector3SerializationSurrogate();
            surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3SS);
            
            //Vector3Int surrogate
            Vector3IntSerializationSurrogate vector3intSS = new Vector3IntSerializationSurrogate();
            surrogateSelector.AddSurrogate(typeof(Vector3Int), new StreamingContext(StreamingContextStates.All), vector3intSS);

            //Vector4 surrogate
            Vector4SerializationSurrogate vector4SS = new Vector4SerializationSurrogate();
            surrogateSelector.AddSurrogate(typeof(Vector4), new StreamingContext(StreamingContextStates.All), vector4SS);

            //Quaternion surrogate
            QuaternionSerializationSurrogate quaternionSS = new QuaternionSerializationSurrogate();
            surrogateSelector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSS);

            return surrogateSelector;
        }

    }
}