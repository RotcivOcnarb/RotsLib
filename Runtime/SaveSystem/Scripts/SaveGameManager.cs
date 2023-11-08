using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Rotslib.Saving
{
    public sealed class SaveGameManager : MonoBehaviour
    {

        public enum SaveType
        {
            BinaryFormatter,
            JSON
        }

        public static SaveGameManager Instance;
        public SaveType saveType;
        private SaveGame saveGame;
        float lastTimeSaved;

        float time;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if (Instance != this)
                {
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

        public T GetSaveGame<T>() where T : SaveGame
        {
            if (saveGame == null)
            {
                LoadGame<T>();
            }
            return saveGame as T;
        }
        public T LoadGame<T>() where T : SaveGame
        {
            T sg = default;
            switch (saveType)
            {
                case SaveType.BinaryFormatter:
                    if (!SaveSystem.LoadGame(out sg))
                    {
                        ResetSave<T>();
                    }
                    break;
                case SaveType.JSON:
                    if (!JSONSaveSystem.LoadGame(out sg))
                    {
                        ResetSave<T>();
                    }
                    break;
            }
            saveGame = sg;
            return saveGame as T;
        }

        public async Task<T> LoadGameAsync<T>() where T : SaveGame
        {
            T result = default;
            await Task.Run(() =>
            {
                result = LoadGame<T>();
            });
            return result;
        }

        public void SaveGame()
        {
            saveGame.timePlayed += time - lastTimeSaved;
            lastTimeSaved = time;
            switch (saveType)
            {
                case SaveType.BinaryFormatter:
                    SaveSystem.SaveGame(saveGame);
                    break;
                case SaveType.JSON:
                    JSONSaveSystem.SaveGame(saveGame);
                    break;
            }
        }

        public async Task SaveGameAsync()
        {
            await Task.Run(() =>
            {
                SaveGame();
            });
        }

        public void ResetSave<T>() where T : SaveGame
        {
            saveGame = Activator.CreateInstance(typeof(T)) as T;
            saveGame.Initialize();
            SaveGame();
        }

        public async Task ResetSaveAsync<T>() where T : SaveGame
        {
            await Task.Run(() =>
            {
                saveGame = Activator.CreateInstance(typeof(T)) as T;
                saveGame.Initialize();
            });
            await SaveGameAsync();
        }
    }

}