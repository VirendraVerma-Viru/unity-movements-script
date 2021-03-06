using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;


public class saveload : MonoBehaviour
{

    

    public static string accountID = " ";
    public static string playerName = " ";

    public static string weaponsid = "";
    public static string weaponGOName = "";

    public static string clothtopid = "";
    public static string clothTopGOname = "";
    public static string clothbottomid = "";
    public static string clothbottomGOname = "";


    public static string current_filename = "info.dat";

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + current_filename);
        CharacterData data = new CharacterData();


        data.AccountID = accountID;
        data.PlayerName = Encrypt(playerName);

        data.WeaponID = weaponsid;

        data.ClothTopId = clothtopid;
        data.ClothBottomId = clothbottomid;

        bf.Serialize(file, data);
        file.Close();
    }

    public static void Load()
    {

        if (File.Exists(Application.persistentDataPath + "/" + current_filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + current_filename, FileMode.Open);/* */
            CharacterData data = (CharacterData)bf.Deserialize(file);

            accountID=data.AccountID;
            playerName=Decrypt(data.PlayerName);


            weaponsid = data.WeaponID;

            clothtopid = data.ClothTopId;
            clothbottomid = data.ClothBottomId;

            file.Close();

        }
        else
        {
            current_filename = "info.dat";
            accountID = " ";
            saveload.Save();

        }
    }

    private static string hash="9452@abc";

    public static string Encrypt(string input)
    {
        byte[] data = UTF8Encoding.UTF8.GetBytes(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB,Padding= PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateEncryptor();
                byte[] results = tr.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(results, 0, results.Length);
            }
        }
    }

    public static string Decrypt(string input)
    {
        byte[] data = Convert.FromBase64String(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB,Padding= PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateDecryptor();
                byte[] results = tr.TransformFinalBlock(data, 0, data.Length);
                return UTF8Encoding.UTF8.GetString(results);
            }
        }
    }

   
}


[Serializable]
class CharacterData
{
    public  string AccountID;
    public  string PlayerName;

    public string WeaponID;
    public string WeaponName;

    public string ClothTopId;
    public string ClothTopName;
    public string ClothBottomId;
    public string ClothBottomName;
}