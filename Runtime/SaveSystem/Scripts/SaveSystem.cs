using Rotslib.Surrogate;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
namespace Rotslib.Saving {

    public class SaveSystem {
        public static bool logFilePath;

        static string persistentDataPath;
        static bool debugBuild;
        public static void Initialize()
        {
            persistentDataPath = Application.persistentDataPath;
            debugBuild = Debug.isDebugBuild;
        }

        public static void SaveGame<T>(T data) {
            BinaryFormatter bf = new BinaryFormatter();
            bf.SurrogateSelector = GetSurrogates();

            if (debugBuild && logFilePath) {
                Debug.Log("Save path: " + persistentDataPath);
            }
            FileStream file = File.Open(persistentDataPath + "/save.dat", FileMode.OpenOrCreate);
            bf.Serialize(file, data);
            file.Close();
        }

        public static async Task SaveGameAsync<T>(T data)
        {
            await Task.Run(() => { SaveGame(data); });
        }

        public static bool LoadGame<T>(out T data) {
            try {
                BinaryFormatter bf = new BinaryFormatter();
                bf.SurrogateSelector = GetSurrogates();

                using (FileStream file = File.Open(persistentDataPath + "/save.dat", FileMode.OpenOrCreate)) {
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

        public static async Task<T> LoadGameAsync<T>()
        {
            T data = default;
            await Task.Run(() => {
                LoadGame(out data);
            });
            return data;
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