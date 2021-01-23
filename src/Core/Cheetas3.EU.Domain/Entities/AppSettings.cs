using Cheetas3.EU.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cheetas3.EU.Domain.Entities
{
    public class AppSettings : Entity
    {
        /// <summary>
        /// Number of retries container will check the health of the container
        /// inorder to start.
        /// </summary>
        public int RetryCount { get; set; }
        /// <summary>
        /// Number of images that will run concurrently on host platform.
        /// Exception is HostOs, where jobs will run synchronously.
        /// </summary>
        public int MaxConcurrency { get; set; }
        /// <summary>
        /// Length of time in seconds to provision job slices. ie: 600 = 10 minutes
        /// </summary>
        public int SliceTimeSpan { get; set; }
        /// <summary>
        /// Docker Host Url. ie: http://localhost:2375
        /// </summary>
        public string DockerHostUrl { get; set; }
        /// <summary>
        /// Docker Image name and tag.  ie: pguerette/euconverter:latest
        /// </summary>
        public string Image { get; set; }
        /// <summary>
        /// Length of time mock container will sleep
        /// </summary>
        public int DevAttributeContainerLifeDuration { get; set; }
    }
}
