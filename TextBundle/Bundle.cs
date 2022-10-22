using System.Collections.Generic;

namespace TextBundle
{
    public class BundleInfo
    {
        public uint index_;
        public string id_;
        public uint size_;
        public uint crc_;

        public BundleInfo() { }
        public BundleInfo(uint index, string id, uint size, uint crc)
        {
            index_ = index;
            id_ = id;
            size_ = size;
            crc_ = crc;
        }
    }

    public class Bundle
    {
        public int magic_number_;
        public int file_number_;
        public List<BundleInfo> file_info_ = new List<BundleInfo>();
    }
}
