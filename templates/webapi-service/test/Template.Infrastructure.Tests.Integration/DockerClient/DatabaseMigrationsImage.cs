namespace Template.Infrastructure.Tests.Integration.DockerClient
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Docker.DotNet;
    using Docker.DotNet.Models;
    using ICSharpCode.SharpZipLib.Tar;

    public class DatabaseMigrationsImage
    {
        private readonly string imageName;
        private readonly string tag;
        private readonly string context;
        private readonly string dockerfile;

        public DatabaseMigrationsImage(string imageName, string tag, string context = ".", string dockerfile = "Dockerfile")
        {
            this.imageName = imageName;
            this.tag = tag;
            this.context = context;
            this.dockerfile = dockerfile;
        }

        public async Task BuildAsync(IDockerClient client)
        {
            using (var tarball = CreateTarball(context))
            {
                var responseStream = await client.Images.BuildImageFromDockerfileAsync(
                    tarball,
                    new ImageBuildParameters
                    {
                        Dockerfile = this.dockerfile,
                        Tags = new string[] { $"{this.imageName}:{this.tag}" },
                    });

                await this.WaitUntil(async () =>
                {
                    var images = await client.Images.ListImagesAsync(new ImagesListParameters
                    { MatchName = this.imageName });
                    return images.Count > 0;
                }, 250, -1);
            }
        }

        public async Task DeleteAsync(IDockerClient client)
        {
            await client.Images.DeleteImageAsync($"{this.imageName}:{this.tag}", new ImageDeleteParameters { Force = true });
        }

        private Stream CreateTarball(string directory)
        {
            var tarball = new MemoryStream();
            var files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

            using var archive = new TarOutputStream(tarball)
            {
                //Prevent the TarOutputStream from closing the underlying memory stream when done
                IsStreamOwner = false
            };

            foreach (var file in files)
            {
                //Replacing slashes as KyleGobel suggested and removing leading /
                string tarName = file.Substring(directory.Length).Replace('\\', '/').TrimStart('/');

                //Let's create the entry header
                var entry = TarEntry.CreateTarEntry(tarName);
                using var fileStream = File.OpenRead(file);
                entry.Size = fileStream.Length;
                archive.PutNextEntry(entry);

                //Now write the bytes of data
                byte[] localBuffer = new byte[32 * 1024];
                while (true)
                {
                    int numRead = fileStream.Read(localBuffer, 0, localBuffer.Length);
                    if (numRead <= 0)
                        break;

                    archive.Write(localBuffer, 0, numRead);
                }

                //Nothing more to do with this entry
                archive.CloseEntry();
            }
            archive.Close();

            //Reset the stream and return it, so it can be used by the caller
            tarball.Position = 0;
            return tarball;
        }

        private async Task WaitUntil(Func<Task<bool>> isReady, int frequency, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (!await isReady())
                {
                    await Task.Delay(frequency);
                };
            });

            await RethrowPotentialException(await Task.WhenAny(waitTask, Task.Delay(timeout)), waitTask);
        }

        private static Task RethrowPotentialException(Task completedTask, Task waitTask)
        {
            return completedTask == waitTask ? completedTask : throw new TimeoutException();
        }
    }
}
