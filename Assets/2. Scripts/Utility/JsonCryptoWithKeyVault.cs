using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using UnityEngine;

//AES_GCM 기반 JSOn 암복호화 유틸
public static class JsonCrypto
{
    // JSON 문자열을 Base64 블롭으로 암호화합니다.
    public static string EncryptJson(string json, byte[] key)
    {
        byte[] plain = Encoding.UTF8.GetBytes(json);
        byte[] nonce = new byte[12];
        RandomNumberGenerator.Fill(nonce);

        byte[] cipher = new byte[plain.Length];
        byte[] tag    = new byte[16];

        using (AesGcm gcm = new(key))
        {
            gcm.Encrypt(nonce, plain, cipher, tag);
        }

        byte[] blob = new byte[nonce.Length + cipher.Length + tag.Length];
        Buffer.BlockCopy(nonce, 0, blob, 0, nonce.Length);
        Buffer.BlockCopy(cipher, 0, blob, nonce.Length, cipher.Length);
        Buffer.BlockCopy(tag, 0, blob, nonce.Length + cipher.Length, tag.Length);
        return Convert.ToBase64String(blob);
    }

    // Base64 블롭을 JSON 문자열로 복호화합니다.
    public static string DecryptJson(string blob, byte[] key)
    {
        byte[] data      = Convert.FromBase64String(blob);
        byte[] nonce     = new byte[12];
        byte[] tag       = new byte[16];
        int    cipherLen = data.Length - nonce.Length - tag.Length;
        if (cipherLen < 0)
        {
            throw new ArgumentException("Invalid blob.");
        }

        byte[] cipher = new byte[cipherLen];
        Buffer.BlockCopy(data, 0, nonce, 0, nonce.Length);
        Buffer.BlockCopy(data, nonce.Length, cipher, 0, cipherLen);
        Buffer.BlockCopy(data, nonce.Length + cipherLen, tag, 0, tag.Length);

        byte[] plain = new byte[cipherLen];
        using (AesGcm gcm = new(key))
        {
            gcm.Decrypt(nonce, cipher, tag, plain);
        }

        return Encoding.UTF8.GetString(plain);
    }
}

public interface IKeyVault
{
    byte[] GetOrCreateKey(string alias);
}

public static class KeyVaultFactory
{
    public static IKeyVault Create()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return new AndroidKeyVault();
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        return null;
#else
        return new FileVault(); // 단순 파일 보관(임시 대안).
#endif
    }
}

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
// Windows: DPAPI로 키를 보호하여 디스크에 저장합니다.
// public sealed class WindowsKeyVault : IKeyVault
// {
//     private readonly string rootPath = Path.Combine(Application.persistentDataPath, "kv");
//
//     public byte[] GetOrCreateKey(string alias)
//     {
//         Directory.CreateDirectory(rootPath);
//         string path = Path.Combine(rootPath, alias + ".bin");
//         if (File.Exists(path))
//         {
//             byte[] protectedBytes = File.ReadAllBytes(path);
//             return ProtectedData.Unprotect(protectedBytes, null, DataProtectionScope.CurrentUser);
//         }
//
//         byte[] key = new byte[32];
//         RandomNumberGenerator.Fill(key);
//         byte[] protectedKey = ProtectedData.Protect(key, null, DataProtectionScope.CurrentUser);
//         File.WriteAllBytes(path, protectedKey);
//         return key;
//     }
// }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
// Android: Keystore의 RSA 키쌍으로 32바이트 키를 래핑하여 저장합니다.
public sealed class AndroidKeyVault : IKeyVault
{
    private readonly string rootPath = Path.Combine(Application.persistentDataPath, "kv");
    private const string BridgeClass = "com.yourcompany.security.KeystoreBridge";

    public byte[] GetOrCreateKey(string alias)
    {
        Directory.CreateDirectory(rootPath);
        string path = Path.Combine(rootPath, alias + "_wrapped.bin");

        using (AndroidJavaClass bridge = new(BridgeClass))
        {
            bool ok = bridge.CallStatic<bool>("ensureKeyPair", alias);
            if (!ok)
            {
                throw new Exception("Failed to ensure Android Keystore key pair.");
            }

            if (File.Exists(path))
            {
                byte[] wrapped = File.ReadAllBytes(path);
                return bridge.CallStatic<byte[]>("unwrap", alias, wrapped);
            }
            else
            {
                byte[] key = new byte[32];
                RandomNumberGenerator.Fill(key);
                byte[] wrapped = bridge.CallStatic<byte[]>("wrap", alias, key);
                File.WriteAllBytes(path, wrapped);
                return key;
            }
        }
    }
}
#endif
// 기타 플랫폼 임시 대안: 암호화 없이 파일에 저장(개발용). 실제 릴리즈에선 사용 금지.
public sealed class FileVault : IKeyVault
{
    private readonly string rootPath = Path.Combine(Application.persistentDataPath, "kv");

    public byte[] GetOrCreateKey(string _alias)
    {
        Directory.CreateDirectory(rootPath);
        string path = Path.Combine(rootPath, _alias + ".raw");
        if (File.Exists(path))
        {
            return File.ReadAllBytes(path);
        }

        byte[] key = new byte[32];
        RandomNumberGenerator.Fill(key);
        File.WriteAllBytes(path, key);
        return key;
    }
}

public sealed class JsonSecureStore
{
    // JSON을 암호화하여 파일에 저장합니다.
    public static void Save(string alias, string json)
    {
        IKeyVault vault = KeyVaultFactory.Create();
        byte[]    key   = vault.GetOrCreateKey(alias);
        string    blob  = JsonCrypto.EncryptJson(json, key);
        string    path  = Path.Combine(Application.persistentDataPath, alias + ".jenc");
        File.WriteAllText(path, blob, Encoding.UTF8);
    }

    // 파일을 복호화하여 JSON을 반환합니다.
    public static string Load(string alias)
    {
        string path = Path.Combine(Application.persistentDataPath, alias + ".jenc");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException(path);
        }

        IKeyVault vault = KeyVaultFactory.Create();
        byte[]    key   = vault.GetOrCreateKey(alias);
        string    blob  = File.ReadAllText(path, Encoding.UTF8);
        return JsonCrypto.DecryptJson(blob, key);
    }
}