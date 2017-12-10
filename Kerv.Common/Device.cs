using System;
namespace Kerv.Common
{
    public class Device
    {
        public String Name { get; private set; }
        public long ID { get; private set; }
        public String Description { get; private set; }

        public Device(String name, long ID)
        {
            this.Name = name;
            this.ID = ID;
            this.Description = name;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != this.GetType()) {
                return false;
            }
            return ((Device)obj).ID == this.ID;
        }
    }
}
