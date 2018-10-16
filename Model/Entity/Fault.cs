using System;

namespace Model.Entity
{
    public class Fault
    {
        public Guid PictureID { get; set; }

        public Guid RecordID { get; set; }

        public string FaultInfo { get; set; }

        public string Timestamp { get; set; }

        public string FaultTypeName { get; set; }
    }
}
