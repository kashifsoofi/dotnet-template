namespace Template.Infrastructure.Tests.Integration.DockerClient
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Docker.DotNet;
    using Docker.DotNet.Models;

    public abstract class Container
    {
        public string ImageName { get; }

        public string Tag { get; }

        public string ContainerName { get; }

        private string id;

        public Container(string imageName, string tag, string containerName)
        {
            this.ImageName = imageName;
            this.Tag = tag;
            this.ContainerName = containerName;
        }

        public async Task StartAsync(IDockerClient client)
        {
            var images = await client.Images.ListImagesAsync(new ImagesListParameters { MatchName = this.ImageName });
            if (images.Count == 0)
            {
                await client.Images.CreateImageAsync(
                    new ImagesCreateParameters { FromImage = this.ImageName, Tag = this.Tag },
                    null,
                    new Progress<JSONMessage>());
            }

            var list = await client.Containers.ListContainersAsync(new ContainersListParameters { All = true });
            var container = list.FirstOrDefault(x => x.Names.Contains(this.ContainerName));
            if (container == null)
            {
                await CreateContainer(client);
            }
            else
            {
                if (container.State == "running")
                {
                    return;
                }
            }

            var started = await client.Containers.StartContainerAsync(this.ContainerName, new ContainerStartParameters());
            if (!started)
            {
                throw new InvalidOperationException($"Container '{ContainerName}' did not start!!!!");
            }

            await WaitUntil(IsReady, 250, -1);
        }

        public async Task WaitAsync(IDockerClient client)
        {
            await client.Containers.WaitContainerAsync(this.id);
        }

        public async Task StopAsync(IDockerClient client)
        {
            await client.Containers.StopContainerAsync(this.ContainerName, new ContainerStopParameters());
        }

        public Task RemoveAsync(IDockerClient client)
        {
            return client.Containers.RemoveContainerAsync(this.ContainerName, new ContainerRemoveParameters { Force = true });
        }

        private async Task CreateContainer(IDockerClient client)
        {
            var hostConfig = HostConfig();
            var config = Config();

            var createContainerResponse = await client.Containers.CreateContainerAsync(new CreateContainerParameters(config)
            {
                Image = $"{this.ImageName}:{this.Tag}",
                Name = ContainerName,
                Tty = true,
                HostConfig = hostConfig,
            });
            this.id = createContainerResponse.ID;
        }

        protected abstract Task<bool> IsReady();

        public abstract HostConfig HostConfig();

        public abstract Config Config();

        public override string ToString()
        {
            return $"{nameof(ImageName)}: {ImageName}, {nameof(ContainerName)}: {ContainerName}";
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
