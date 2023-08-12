using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

namespace LS
{
    public class SaveFileDataWrite
    {
        public string saveDataDirectoryPath = "";
        public string saveFileName = "";

        public bool CheckToSeeIfFileExists()
        {
            if(File.Exists(Path.Combine(saveDataDirectoryPath, saveFileName)))
            {
                return true;
            }else
            {
                return false;
            }
        }
   
        public void DeleteSaveFile()
        {
            File.Delete(Path.Combine(saveDataDirectoryPath, saveFileName));
        }

        public void CreateNewCharacterSaveFile(CharacterSaveData characterData)
        {
            string savePath = Path.Combine(saveDataDirectoryPath, saveFileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
                Debug.Log("CREATING SAVE FILE, AT SAVE PATH: " + savePath);

                string dataToStore = JsonUtility.ToJson(characterData, true);

                using (FileStream stream = new(savePath, FileMode.Create))
                {
                    using (StreamWriter fileWriter = new(stream))
                    {
                        fileWriter.Write(dataToStore);
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.LogError("ERROR WHILST TRYING TO SAVE CHARACTER DATA");
            }
        }
   
        public CharacterSaveData LoadSaveFile()
        {
            CharacterSaveData characterData = null;
            string loadPath = Path.Combine(saveDataDirectoryPath, saveFileName);
            if (File.Exists(loadPath))
            {
                try
                {
                    string dataToload = "";
                    using (FileStream stream = new(loadPath, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataToload = reader.ReadToEnd();
                        }
                    }

                    characterData = JsonUtility.FromJson<CharacterSaveData>(dataToload);
                }
                catch (Exception ex)
                {
                    Debug.LogError("ERROR WHILST TRYING TO LOAD CHARACTER DATA");
                }
            }
            return characterData;
        }
    
    }
}
