using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;

/**
 * Save data
 * Tạo class lưu trữ với 2 phương thức Save và Load(static)
 * Class sẽ chứa các thuộc tính muốn lưu, vd: level, gold...
 * Sử dụng Load để đọc data, dữ liệu trả về là object được tạo từ class lưu trữ
 * vd: PlayerData data = PlayerData.Load();
 * Để chỉnh sửa dự liệu, sử dụng như chỉnh sửa thuộc tính 1 một object thông thường
 * Sau đó sử dụng Save để lưu thay đổi lên file trong máy
 * vd:
 *  data.testing = 345;
 *  data.Save();
 */

public class SaveSystem<T> where T : new()
{
    private static string EncryptKey= "$2a$12$bFGaHUjLQsB12SXjTPGzIuh1Gqc93EA4Wow0qwg4451dfsvytu81y";
    private static byte[] salt = new byte[] { 0x29, 0x6f, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x41, 0x76, 0x65, 0x64, 0x55, 0x12 };
    private static string directory = Application.persistentDataPath + "/Saves";

    public static void Save(T saveData, bool useEncrypt = true) {
        if(!Directory.Exists(directory)) {
            Directory.CreateDirectory(directory);
        }

        string jsonData = JsonUtility.ToJson(saveData);
        string path = directory + $"/{typeof(T).Name}.sav";

        if(useEncrypt) {
            jsonData = Encrypt(jsonData);
        }

        File.WriteAllText(path, jsonData);
    }

    public static T Load(bool useEncrypt = true) {
        string path = directory + $"/{typeof(T).Name}.sav";
        if(File.Exists(path)) {
            string jsonData = File.ReadAllText(path);
            if(useEncrypt) {
                jsonData = Decrypt(jsonData);
            }

            return JsonUtility.FromJson<T>(jsonData);
        } else {
            T newData = new T();
            Save(newData, useEncrypt);
            return newData;
        }
    }

    private static string Encrypt(string stringData) {
        byte[] clearBytes = Encoding.Unicode.GetBytes(stringData);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptKey, salt);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                stringData = Convert.ToBase64String(ms.ToArray());
            }
        }
        return stringData;
    }

    private static string Decrypt(string stringData)
    {
        byte[] cipherBytes = Convert.FromBase64String(stringData);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptKey, salt);
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                stringData = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return stringData;
    }
}

[Serializable]
public class PlayerData
{
    public bool firstTime;
    public int coin;
    public int selectedCharacter;
    public int selectedWeapon;
    public List<int> characters, weapons;
    public int energy;
    public string energyUpdateDateTimeJson;
    public DateTime energyUpdateDateTime {
        get {
            return Convert.ToDateTime(energyUpdateDateTimeJson);
        }
        set {
            energyUpdateDateTimeJson = value.ToString();
        }
    }

    public PlayerData() {
        firstTime = true;
        coin = 0;
        selectedCharacter = 0;
        characters = new List<int>();
        weapons = new List<int>();
        characters.Add(0);
        weapons.Add(0);
        energy = 0;
        energyUpdateDateTimeJson = DateTime.Now.ToString();
    }

    public static PlayerData Load()
    {
        return SaveSystem<PlayerData>.Load(false);
    }

    public void Save()
    {
        if(this.firstTime) {
            this.firstTime = false;
        }
        SaveSystem<PlayerData>.Save(this,false);
    }
}

public class SettingData{
    public bool mute;
    public float resolutionScale;
    public int fps;
    public SettingData(){
        mute = false;
        fps = 60;
        resolutionScale = 0.7f;
    }

    public static SettingData Load(){
        return SaveSystem<SettingData>.Load(false);
    }

    public void Save(){
        SaveSystem<SettingData>.Save(this, false);
    }
}
