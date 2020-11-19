using System;
namespace QuickFox.Providers
{
    public interface IFilesystemProvider
    {
        public byte[] ReadResource(string resourceUri); 
    }
}
