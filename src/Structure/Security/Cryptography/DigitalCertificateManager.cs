using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace Structure.Security.Cryptography
{
    public class DigitalCertificateManager : IDisposable
    {
        private readonly X509Store store;
        private bool isDisposed;

        public DigitalCertificateManager(StoreName storeName, StoreLocation location, OpenFlags openFlags)
        {
            store = new X509Store(storeName, location);
            store.Open(OpenFlags.OpenExistingOnly);
        }

        public DigitalCertificateManager() : this(StoreName.My, StoreLocation.LocalMachine, OpenFlags.OpenExistingOnly)
        { }

        public X509Certificate2 GetBySubject(string subject)
        {
            foreach (var item in store.Certificates)
            {
                if (item.Subject == subject)
                {
                    return item;
                }
            }

            return null;
        }

        public X509Certificate2 GetBySerialNumber(string serialNumber)
        {
            return store.Certificates.OfType<X509Certificate2>().FirstOrDefault(c => c.SerialNumber.ToLower() == serialNumber.ToLower());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                store.Close();
                store.Dispose();
            }

            isDisposed = true;
        }

        public void Add(X509Certificate2 certificate)
        {
            store.Add(certificate);
        }

        public static X509Certificate2 Open(byte[] bytes, string password)
        {
            return new X509Certificate2(bytes, password, X509KeyStorageFlags.MachineKeySet);
        }

        public static X509Certificate2 Open(string fileName, string password)
        {
            return Open(File.ReadAllBytes(fileName), password);
        }

        public static string GetIssuerName(X509Certificate2 certificate)
        {
            return Regex.Match(certificate.Issuer, "CN=(.*?),").Groups[1].Value;
        }

        public static string GetSubjectNameName(X509Certificate2 certificate)
        {
            return Regex.Match(certificate.SubjectName.Name, "CN=(.*?),").Groups[1].Value;
        }
    }
}
