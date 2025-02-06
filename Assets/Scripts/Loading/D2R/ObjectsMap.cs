using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace Diablo2Editor
{
    /*
     * Mapping <id, jsonKey> for objects/monsters
     * Mapping is relative to act, we can't use object of act2 in other acts
     * 
     */
    public class ObjectIndex
    {
        public Dictionary<long, string> data = new Dictionary<long, string>();

    }
    /*
     * Maps object id and type to preset name for D2R either from objects.json or monsters.json
     * Uses monpreset.txt, objpreset.txt as input data
     */
    public class ObjectsMap
    {
        ObjectIndex[] objectPresets = new ObjectIndex[DS1Consts.ACT_MAX];
        ObjectIndex[] monsterPresets = new ObjectIndex[DS1Consts.ACT_MAX];

        public ObjectsMap()
        {
            Init();
        }
        public string FindObjectPresetName(int act, long type, long index)
        {
            if (act >= 0 && act < DS1Consts.ACT_MAX)
            {
                Dictionary<long, string> indexData = null;
                string fileToSearch = null;
                var pathMapper = EditorMain.Settings().paths;
                if (type == 1) // npc or enemy
                {
                    fileToSearch = pathMapper.GetMonstersPath();
                    indexData = monsterPresets[act].data;
                }
                if (type == 2) // object
                {
                    fileToSearch = pathMapper.GetObjectsPath();
                    indexData = objectPresets[act].data;
                }

                if (indexData.ContainsKey(index) && File.Exists(fileToSearch))
                {
                    var content = File.ReadAllText(fileToSearch);
                    JSONNode json = JSONNode.Parse(content);
                    var key = indexData[index];
                    if (json[key] != null) 
                    {
                        return json[key].ToString();
                    }
                }
            }
            return "";
        }
        private void Init()
        {
            InitObjectPresets();
            InitMonsterPresets();
        }

        private void InitMonsterPresets()
        {
            PrepareArray(monsterPresets);
            
            var pathMapper = EditorMain.Settings().paths;
            // Read files of interest
            string[][] superuniques = CSVReader.ReadFile(pathMapper.GetSuperUniques());
            string[][] monpresets = CSVReader.ReadFile(pathMapper.GetMonPreset());
            string[][] monstats = CSVReader.ReadFile(pathMapper.GetMonStats());

            // Find type for superuniques
            Dictionary<string, string> uniqueTypes = new Dictionary<string, string>();
            if (superuniques != null)
            {
                for (int i = 1; i < superuniques.Length; i++)
                {
                    string name = superuniques[i][0];
                    string uniqueType = superuniques[i][2];
                    uniqueTypes[name] = uniqueType;
                }
            }
            // Now traversing the monpresets
            if (monpresets != null)
            {
                int prev_act = 1;
                long objIndex = 0;
                for (int i = 1; i < monpresets.Length; i++)
                {
                    int act = int.Parse(monpresets[i][0]);
                    // if act changed - we need to reset index 'cause indexes are relative to act
                    if (prev_act != act)
                    {
                        objIndex = 0;
                    }
                    // init with empty string for debug purposes
                    var indexDictionary = monsterPresets[act].data;

                    indexDictionary[objIndex] = "";
                    string name = monpresets[i][1];
                    string type = "";
                    // Did we find any ID 
                    bool found = false;
                    if (uniqueTypes[name] != null)
                    {
                        // it's superUnique - get type from Dictionary
                        type = uniqueTypes[name];
                        found = true;
                    }
                    else
                    {
                        // legacy ids, need some processing
                        if (name.StartsWith("place_"))
                        {
                            if (name == "place_nothing")
                            {
                                // empty id, skip it
                                continue;
                            }
                            // cut out 'place_' prefix
                            string id = name.Replace("place_", "");
                            // try to find something meaningful
                            found = TryFindStatsID(id, out type, monstats);
                        }
                        else
                        {
                            // if it's not legacy and not superunique - it may be npc
                            // We 'll look for exact match in monstats
                            if (SearchMonStatsID(name, monstats))
                            {
                                // if npc is found - his type == name
                                found = true;
                                type = name;
                            }
                        }

                        // all searches done
                        // need to do some final checks
                        if (found)
                        {
                            if (SearchMonStatsID(type, monstats))
                            {
                                // if type found and it is in monstats
                                // write it to final map
                                indexDictionary[objIndex] = type;
                            }    
                        }

                    }

                }
            }

        }

        private void InitObjectPresets()
        {
            PrepareArray(objectPresets);
            var pathMapper = EditorMain.Settings().paths;
            // Read files of interest
            string[][] objpresets = CSVReader.ReadFile(pathMapper.GetObjPreset());
            var monstersJsonFile = pathMapper.GetMonstersPath();
            if (File.Exists(monstersJsonFile))
            {
                var jsonContent = File.ReadAllText(pathMapper.GetMonstersPath());
                JSONNode monstersJson = JSON.Parse(jsonContent);
                if (objpresets != null && monstersJson != null)
                {
                    // first of all read key fields from json to use later
                    string[]keys = new string[monstersJson.Count];
                    int index = 0;
                    foreach (string k in monstersJson.Keys)
                    {
                        keys[index] = k;
                    }

                    for (int i = 1; i < objpresets.Length; i++)
                    {
                        int act = int.Parse(objpresets[i][1]);
                        long objIndex = long.Parse(objpresets[i][0]);
                        string name = keys[objIndex];
                        // just write jsonKey for proper index and act
                        objectPresets[act].data[index] = name;
                    }
                }

            }
        }

        private void PrepareArray(ObjectIndex[] objectIndices)
        {
            for (int i = 0; i < objectIndices.Length; i++)
            {
                if (objectIndices[i] == null)
                {
                    objectIndices[i] = new ObjectIndex();
                }
                else
                {
                    objectIndices[i].data.Clear();
                }
            }
        }

        /*
         * Search monstats for specific ID. If found - we're good and ID is valid
         * If not - data is inconsistent, we need to warn user
         */
        private bool SearchMonStatsID(string id, string[][] monstats)
        {
            for (int i = 1; i < monstats.Length; i++)
            {
                string row_id = monstats[i][0];
                if (row_id == id)
                {
                    return true;
                }
            }
            return false;
        }

        /*
         * Search monstats for some ID. monpreset can 
         * have some legacy place_<something> IDs. Some
         * of them are meaningful, others are not. We can try to find match but 
         * it's ok to fail
         */
        private bool TryFindStatsID(string id, out string result, string[][] monstats)
        {
            // try to find exact match first
            if (SearchMonStatsID(id, monstats))
            {
                // got one, return id as result
                result = id;
                return true;
            }

            for (int i = 1; i < monstats.Length; i++)
            {
                string row_id = monstats[i][0];
                if (row_id.StartsWith(id))
                {
                    // found some match. Good enough
                    result = row_id;
                    return true;
                }
            }

            result = "";
            return false;
        }


    }

}

