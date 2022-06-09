using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AurecasLib.Saving {
    [Serializable]
    public class SaveGame {
        [Serializable]
        public class LevelData {
            public bool unlocked;
            public bool finished;
            private bool[] ranking; //3 rankings por padrão
            public Dictionary<string, string> customData;

            public LevelData() {
                ranking = new bool[3];
                customData = new Dictionary<string, string>();
            }

            public void SetRanking(int index, bool value) {
                if (ranking == null) ranking = new bool[3];
                ranking[index] = value;
            }

            public bool GetRanking(int index) {
                if (ranking == null) ranking = new bool[3];
                return ranking[index];
            }

            public int GetRankingCount() {
                return ranking.Where(b => b).Count();
            }

            public void SetData(string key, string data) {
                if (customData == null) customData = new Dictionary<string, string>();
                if (!customData.ContainsKey(key)) {
                    customData.Add(key, data);
                }
                else customData[key] = data;
            }

            public string GetData(string key, string dft = null) {
                if (customData == null) customData = new Dictionary<string, string>();
                if (customData.ContainsKey(key)) return customData[key];
                return dft;
            }
        }

        [Serializable]
        public class InventoryItem {
            public string itemId;
            public int amount;
        }

        public int currency;
        public float timePlayed;
        [JsonConverter(typeof(LevelDataConverter))]
        public ComposedLevelDataList levels;
        public List<InventoryItem> inventory;
        bool dirty = false;

        public int GetDefaultCurrencyAmount() {
            return currency;
        }

        public virtual void AddToDefaultCurrency(int coins) {
            currency += coins;
        }

        public virtual bool SpendDefaultCurrency(int amount) {
            if(currency >= amount) {
                currency -= amount;
                return true;
            }
            else {
                return false;
            }
        }
        public int GetTotalStarsCollected() {
            int count = 0;
            foreach (var l in levels.GetWorlds()) {
                foreach (LevelData ld in l.list) {
                    for (int i = 0; i < 3; i++) {
                        if (ld.GetRanking(i)) {
                            count++;
                        }
                    }
                }
            }
            return count;
        }

        public int GetTotalStarsCollected(int world) {
            if (world >= levels.GetWorldCount()) return 0;
            int count = 0;

            foreach (LevelData ld in levels.GetWorlds()[world].list) {
                for (int i = 0; i < 3; i++) {
                    if (ld.GetRanking(i)) {
                        count++;
                    }
                }
            }
            return count;
        }

        public virtual void AddItemToInventory(string itemId, int amount) {
            for(int i = 0; i < inventory.Count; i++) {
                if(inventory[i].itemId == itemId) {
                    inventory[i].amount += amount;
                    return;
                }
            }

            inventory.Add(new InventoryItem() {
                itemId = itemId,
                amount = amount
            });
            RevalidateInventory();
        }

        public virtual void ConsumeItemInInventory(string itemId) {
            for (int i = 0; i < inventory.Count; i++) {
                InventoryItem item = inventory[i];
                if (item.itemId == itemId) {
                    if (item.amount > 0) {
                        item.amount--;
                        SetDirty();
                        break;
                    }
                }
            }
            RevalidateInventory();
        }

        public virtual int GetItemAmount(string itemId) {
            InventoryItem item = SearchItem(itemId);
            if (item != null) return item.amount;
            else return 0;
        }

        public virtual InventoryItem SearchItem(string itemId) {
            RevalidateInventory();
            foreach(InventoryItem item in inventory) {
                if (item.itemId == itemId) return item;
            }
            return null;
        }

        public void SetDirty() {
            dirty = true;
        }

        private void RevalidateInventory() {
            if (!dirty) return;
            //Faz um merge em todos os items de ID igual;
            Dictionary<string, int> revalidatedInventory = new Dictionary<string, int>();
            foreach(InventoryItem item in inventory) {
                if (!revalidatedInventory.ContainsKey(item.itemId)) {
                    revalidatedInventory.Add(item.itemId, 0);
                }
                revalidatedInventory[item.itemId] += item.amount;
            }

            //Recria a lista de inventário só com os itens q tem amount > 0
            inventory = new List<InventoryItem>();
            foreach(string key in revalidatedInventory.Keys) {
                if(revalidatedInventory[key] > 0) {
                    inventory.Add(new InventoryItem() { 
                        itemId = key,
                        amount = revalidatedInventory[key]
                    });
                }
            }
            dirty = false;
        }

        public void Initialize() {
            if(inventory == null) {
                inventory = new List<InventoryItem>();
            }
            if (levels == null) {
                levels = new ComposedLevelDataList();
                levels.AddWorld();
                levels.AddLevel(0, new LevelData() { unlocked = true });
            }
        }

        public virtual void SetLevelData(int world, int level, LevelData levelData) {
            Initialize();
            while (world >= levels.GetWorldCount()) {
                levels.AddWorld();
            }
            while (level >= levels.GetLevels(world).Count) {
                levels.AddLevel(world, new LevelData());
            }

            levels.SetLevel(world, level, levelData);
        }

        public LevelData GetLevelData(int world, int level) {
            Initialize();
            if (world >= levels.GetWorldCount()) return new LevelData();
            if (level >= levels.GetLevels(world).Count) return new LevelData();

            if (world == 0 && level == 0) {
                levels.GetLevel(world, level).unlocked = true;
            }
            return levels.GetLevel(world, level);
        }

        public void RegisterLevelData(int world, int level) {
            while(world >= levels.GetWorldCount()) { //Se não tem esse mundo, cria
                levels.AddWorld();
            }
            while(level >= levels.GetLevels(world).Count) { //Se não tem esse level, cria
                levels.AddLevel(world, new LevelData());
            }

        }

        public void SetWorldLevelCount(int world, int levelCount) {
            while (levelCount >= levels.GetLevels(world).Count) { //Se não tem esse level, cria
                levels.AddLevel(world, new LevelData());
            }
            //se tem level demais, apaga
            while (levels.GetLevels(world).Count > levelCount) {
                levels.RemoveLevel(world, levels.GetLevels(world).Count - 1);
            }
        }

        public bool GetLastUnlockedLevel(out int world, out int level) {
            Initialize();
            int worldCount = levels.GetWorldCount();
            for (int i = worldCount - 1; i >= 0; i--) {
                int levelCount = levels.GetLevels(i).Count;
                for (int j = levelCount - 1; j >= 0; j--) {
                    LevelData ld = GetLevelData(i, j);
                    if (ld.unlocked && !ld.finished) {
                        world = i;
                        level = j;
                        return true;
                    }
                }
            }
            world = 0;
            level = 0;
            return false;
        }

        public bool GetNextWorldAndLevel(int world, int level, out int nworld, out int nlevel) {
            int nw = world;
            int nl = level + 1;
            if (nl >= levels.GetLevels(nw).Count) {
                nw++;
                nl = 0;
            }
            
            nworld = nw;
            nlevel = nl;

            return nw < levels.GetWorldCount();
        }

        public void UnlockNextLevel(int world, int level) {

            GetNextWorldAndLevel(world, level, out int nw, out int nl);

            if(nw < levels.GetWorldCount()) {
                LevelData ld = GetLevelData(nw, nl);
                ld.unlocked = true;
                SetLevelData(nw, nl, ld);
            }
            else {
                //Acabou
            }

        }

        public override string ToString() {
            Initialize();
            return JsonConvert.SerializeObject(this);
        }

        public string ToJson() {
            return JsonConvert.SerializeObject(this, new SaveGameConverter());
        }

        public static SaveGame FromJson(string json) {
            return JsonConvert.DeserializeObject(json, typeof(SaveGame), new SaveGameConverter()) as SaveGame;
        }

        class SaveGameConverter : JsonConverter {
            public override bool CanConvert(Type objectType) {
                return objectType == typeof(SaveGame);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
                JObject obj = serializer.Deserialize(reader) as JObject;
                SaveGame sg = obj.ToObject<SaveGame>();
                return sg;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
                writer.WriteRaw(JsonConvert.SerializeObject(value));
            }
        }
    }
}