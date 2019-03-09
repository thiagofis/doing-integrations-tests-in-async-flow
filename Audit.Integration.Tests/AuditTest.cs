using System;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;

namespace Audit.Integration.Tests
{
    public class AuditTest : IDisposable
    {
        private readonly string path = "C:/target";

        public AuditTest()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public void Dispose()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
        }

        [Fact]
        public void The_1st_attempt()
        {
            // Assemble
            var expectedMessage = "** This directory has 14 file(s). **";

            CreateFiles(10);

            // Act
            Thread.Sleep(TimeSpan.FromSeconds(3));
            var lines = File.ReadLines($"{path}/audit-summary.txt");

            // Assert
            Assert.Equal(expectedMessage, lines.First());
        }

        private void CreateFiles(int numberOfFiles)
        {
            var index = 1;

            foreach (var item in new string[numberOfFiles])
            {
                File.Create($"{path}/file-test-{index}.txt")
                .Dispose();

                index++;
            }
        }
    }
}