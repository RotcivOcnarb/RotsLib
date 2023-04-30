using System;
using UnityEngine;

namespace Rotslib.Saving {
    public sealed class SaveGameManager : MonoBehaviour {

        public enum SaveType {
            BinaryFormatter,
            JSON
        }

        public static SaveGameManager Instance;
        public SaveType saveType;
        private SaveGame saveGame;
        float lastTimeSaved;

        float time;

        void Awake() {
            if (Instance == null) {
                Instance = this;
            }
            else {
                if (Instance != this) {
                    Destroy(gameObject);
                    return;
                }
            }
            DontDestroyOnLoad(gameObject);
            SaveSystem.Initialize();
            JSONSaveSystem.Initialize();
        }

        private void Update()
        {
            time = Time.time;
        }

        public T GetSaveGame<T>() where T : SaveGame {
            if (saveGame == null) {
                LoadGame<T>();
            }
            return saveGame as T;
        }
        public void LoadGame<T>(Action<T> afterLoad = null) where T : SaveGame {
            T sg = default;
            switch (saveType) {
                case SaveType.BinaryFormatter:
                    if (!SaveSystem.LoadGame(out sg)) {
                        ResetSave<T>();
                    }
                    break;
                case SaveType.JSON:
                    if (!JSONSaveSystem.LoadGame(out sg)) {
                        ResetSave<T>();
                    }
                    break;
            }
            saveGame = sg;

            afterLoad?.Invoke(saveGame as T);
        }

        public void SaveGame(Action afterSave = null) {
            saveGame.timePlayed += time - lastTimeSaved;
            lastTimeSaved = time;
            switch (saveType) {
                case SaveType.BinaryFormatter:
                    SaveSystem.SaveGame(saveGame);
                    break;
                case SaveType.JSON:
                    JSONSaveSystem.SaveGame(saveGame);
                    break;
            }

            afterSave?.Invoke();
        }

        public void ResetSave<T>(Action afterReset = null) where T : SaveGame {
            saveGame = Activator.CreateInstance(typeof(T)) as T;
            saveGame.Initialize();
            SaveGame(afterReset);
        }
    }

}