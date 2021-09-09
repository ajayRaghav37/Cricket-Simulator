using System;

namespace Repositories.TableEntities
{
    public class BaseEntity
    {
        public string id { get; set; }
        public DateTime __createdAt { get; set; }
        public DateTime __updatedAt { get; set; }
        public byte[] __version { get; set; }
        public bool __deleted { get; set; }
    }
}
