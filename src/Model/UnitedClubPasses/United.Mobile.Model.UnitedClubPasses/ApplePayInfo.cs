﻿using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace United.Mobile.Model.UnitedClubPasses
{
    public class ApplePayInfo
    {
        public byte[] merchantIdentifierField { get; set; }
        public ECPrivateKeyParameters merchantPrivateKey { get; set; }
        public ECPublicKeyParameters ephemeralPublicKey { get; set; }

        public byte[] sharedSecret { get; set; }
        public byte[] symmetricKey { get; set; }

        public byte[] Iv = new byte[16];

        public ApplePayInfo Init(string base64PubKeyStr, string path, string passWord)
        {
            GetReady(base64PubKeyStr, path, passWord);

            return this;
        }

        private void GetReady(string base64PubKeyStr, string path, string passWord)
        {
            GetPubKey(base64PubKeyStr);

            GetPrivateKeyFromP12(path, passWord);

            GetShardSecret();

            GetSymmetricKey();
        }

        private void GetPubKey(string base64PubKeyStr)
        {
            var keyInBytes = Convert.FromBase64String(base64PubKeyStr);

            this.ephemeralPublicKey = (ECPublicKeyParameters)PublicKeyFactory.CreateKey(keyInBytes);
        }

        private void GetPrivateKeyFromP12(string path, string passWord)
        {
            //this.mKeyStoreEntities = PKCSUtils.extractEntities(privateKeyFilePath, privateKeyPassword);
            /**
             * Java has a minimum requirement for keystore password lengths.  Utilities like KeyChain will
             * allow you to specify less but then you will receive an obscure java error when trying to
             * load the keystore.  Check for it here and throw a meaningful error
             */

            //X509Certificate certificate;
            //ECPrivateKey privateKey;

            using (Stream fs = Assembly.GetExecutingAssembly().GetManifestResourceStream("United.Mobile.FOP." + path))
            {
                //var fs = reader.BaseStream;
                GetMerchantIdentifierField(fs, passWord);

                fs.Position = 0;
                GetPrivateKey(fs, passWord);
            }
        }

        private void GetPrivateKey(Stream fs, string passWord)
        {
            Pkcs12Store store = new Pkcs12Store(fs, passWord.ToCharArray());

            foreach (string n in store.Aliases)
            {
                if (store.IsKeyEntry(n))
                {
                    AsymmetricKeyEntry asymmetricKey = store.GetKey(n);

                    if (asymmetricKey.Key.IsPrivate)
                    {
                        this.merchantPrivateKey = asymmetricKey.Key as ECPrivateKeyParameters;
                    }
                }
            }
        }

        private void GetMerchantIdentifierField(Stream fs, string passWord)
        {
            byte[] rawTmp = new byte[fs.Length];
            fs.Read(rawTmp, 0, (Int32)fs.Length);

            System.Security.Cryptography.X509Certificates.X509Certificate2 x509cert =
            new X509Certificate2(rawTmp, passWord);
            rawTmp = null;

            rawTmp = x509cert.Extensions[7].RawData;

            using (var stream = new MemoryStream(64))
            using (var sr = new BinaryWriter(stream))
            {
                sr.Write(rawTmp, 2, 64);
                sr.Flush();
                rawTmp = null;

                //var streamTmp = stream.GetBuffer();

                var ascStr = Encoding.ASCII.GetString(stream.GetBuffer());

                rawTmp = new byte[ascStr.Length / 2];
                for (int i = 0; i < rawTmp.Length; i++)
                {
                    rawTmp[i] = Convert.ToByte(ascStr.Substring(i * 2, 2), 16);
                }

                this.merchantIdentifierField = rawTmp;
            }
        }


        public string DecryptedByAES256GCM(string encryptedStr)
        {
            var encryptedData = Convert.FromBase64String(encryptedStr);
            var cipher = WrapperUtilities.GetWrapper("AES/GCM/NoPadding");
            cipher.Init(false, new ParametersWithIV(new KeyParameter(this.symmetricKey), this.Iv));

            var deCryptedData = cipher.Unwrap(encryptedData, 0, encryptedData.Length);
            return Encoding.ASCII.GetString(deCryptedData);
        }

        private void GetShardSecret()
        {
            //IBasicAgreement aKeyAgree = AgreementUtilities.GetBasicAgreement("ECDH");
            //aKeyAgree.Init(this.merchantPrivateKey);
            //BigInteger SharedSecret = aKeyAgree.CalculateAgreement(ephemeralPublicKey);
            //this.sharedSecret = SharedSecret.ToByteArrayUnsigned();
        }

        private void GetSymmetricKey()
        {
            var infoToWrite = GetInfoReady(this.merchantIdentifierField);

            IDigest digest = new Sha256Digest();
            this.symmetricKey = new byte[digest.GetDigestSize()];
            digest.Update((byte)(1 >> 24));
            digest.Update((byte)(1 >> 16));
            digest.Update((byte)(1 >> 8));
            digest.Update((byte)1);
            digest.BlockUpdate(this.sharedSecret, 0, this.sharedSecret.Length);
            digest.BlockUpdate(infoToWrite, 0, infoToWrite.Length);
            digest.DoFinal(this.symmetricKey, 0);
        }
        private byte[] GetInfoReady(byte[] mid)
        {
            byte[] algorithmId = Encoding.ASCII.GetBytes(((char)0x0D + "id-aes256-GCM"));//.getBytes("ASCII");
            byte[] partyUInfo = Encoding.ASCII.GetBytes("Apple");//.getBytes("ASCII");
            byte[] partyVInfo = mid;// extractMerchantIdFromCertificateOid("1.2.840.113635.100.6.32");

            //byte[] otherInfo = new byte[algorithmId.Length + partyUInfo.Length + partyVInfo.Length];
            var stream = new MemoryStream(algorithmId.Length + partyUInfo.Length + partyVInfo.Length);
            var sr = new BinaryWriter(stream);
            sr.Write(algorithmId);
            sr.Flush();
            sr.Write(partyUInfo);
            sr.Flush();
            sr.Write(partyVInfo);
            sr.Flush();
            stream.Position = 0;
            var o2 = stream.GetBuffer();
            return o2;
        }
    }
}
