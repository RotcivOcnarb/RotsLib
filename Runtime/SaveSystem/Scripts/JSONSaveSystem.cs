using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;
namespace Rotslib.Saving {

    public class JSONSaveSystem {
        public static void SaveGame<T>(T data) {
            string json = JsonConvert.SerializeObject(data,
                new Vec2Conv(),
                new Vec3Conv(),
                new Vec4Conv(),
                new QuatConv()
                );

            //byte[] encrypted = Criptography.Encrypt(json);
            //FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.OpenOrCreate);
            //for (int i = 0; i < encrypted.Length; i++) {
            //    file.WriteByte(encrypted[i]);
            //}
            //file.Close();

            File.WriteAllText(Application.persistentDataPath + "/save.dat", json);
        }

        public static bool LoadGame<T>(out T data) {
            try {

                string json = File.ReadAllText(Application.persistentDataPath + "/save.dat");

                //byte[] read = File.ReadAllBytes(Application.persistentDataPath + "/save.dat");
                //string json = Criptography.Decrypt(read);
                data = JsonConvert.DeserializeObject<T>(json,
                        new Vec2Conv(),
                        new Vec3Conv(),
                        new Vec4Conv(),
                        new QuatConv()
                    );

                return true;
            }
            catch (Exception e){
                Debug.LogError(e.Message);
                Debug.LogError("Could not load savegame type " + typeof(T) + ", returning new empty");
                data = default;
                return false;
            }
        }

        public class Vec4Conv : JsonConverter {
            public override bool CanConvert(Type objectType) {
                if (objectType == typeof(Vector4)) {
                    return true;
                }
                return false;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
                var t = serializer.Deserialize(reader);
                var iv = JsonConvert.DeserializeObject<Vector4>(t.ToString());
                return iv;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
                Vector4 v = (Vector4)value;

                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(v.x);
                writer.WritePropertyName("y");
                writer.WriteValue(v.y);
                writer.WritePropertyName("z");
                writer.WriteValue(v.z);
                writer.WritePropertyName("w");
                writer.WriteValue(v.w);
                writer.WriteEndObject();
            }
        }

        public class Vec3Conv : JsonConverter {
            public override bool CanConvert(Type objectType) {
                if (objectType == typeof(Vector3)) {
                    return true;
                }
                return false;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
                var t = serializer.Deserialize(reader);
                var iv = JsonConvert.DeserializeObject<Vector3>(t.ToString());
                return iv;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
                Vector3 v = (Vector3)value;

                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(v.x);
                writer.WritePropertyName("y");
                writer.WriteValue(v.y);
                writer.WritePropertyName("z");
                writer.WriteValue(v.z);
                writer.WriteEndObject();
            }
        }

        public class Vec2Conv : JsonConverter {
            public override bool CanConvert(Type objectType) {
                if (objectType == typeof(Vector2)) {
                    return true;
                }
                return false;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
                var t = serializer.Deserialize(reader);
                var iv = JsonConvert.DeserializeObject<Vector2>(t.ToString());
                return iv;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
                Vector2 v = (Vector2)value;

                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(v.x);
                writer.WritePropertyName("y");
                writer.WriteValue(v.y);
                writer.WriteEndObject();
            }
        }

        public class QuatConv : JsonConverter {
            public override bool CanConvert(Type objectType) {
                if (objectType == typeof(Quaternion)) {
                    return true;
                }
                return false;
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
                var t = serializer.Deserialize(reader);
                var iv = JsonConvert.DeserializeObject<Quaternion>(t.ToString());
                return iv;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
                Quaternion v = (Quaternion)value;

                writer.WriteStartObject();
                writer.WritePropertyName("x");
                writer.WriteValue(v.x);
                writer.WritePropertyName("y");
                writer.WriteValue(v.y);
                writer.WritePropertyName("z");
                writer.WriteValue(v.z);
                writer.WritePropertyName("w");
                writer.WriteValue(v.w);
                writer.WriteEndObject();
            }
        }

    }

}