using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;

namespace SharpPcap.Borrowed
{
#if Windows
    internal class SystemNetworkInterface : NetworkInterface
    {
        private readonly string _name;
        private readonly string _id;
        private readonly string _description;
        private readonly byte[] _physicalAddress;
        private readonly uint _addressLength;
        private readonly NetworkInterfaceType _type;
        private readonly OperationalStatus _operStatus;
        private readonly long _speed;
        private readonly uint _index;
        private readonly uint _ipv6Index;
        private readonly Interop.AdapterFlags _adapterFlags;
        private IPInterfaceProperties interfaceProperties;


        internal SystemNetworkInterface(Interop.FIXED_INFO fixedInfo, Interop.IpAdapterAddresses ipAdapterAddresses)
        {
            this._id = ipAdapterAddresses.AdapterName;
            this._name = ipAdapterAddresses.friendlyName;
            this._description = ipAdapterAddresses.description;
            this._index = ipAdapterAddresses.index;
            this._physicalAddress = ipAdapterAddresses.address;
            this._addressLength = ipAdapterAddresses.addressLength;
            this._type = ipAdapterAddresses.type;
            this._operStatus = ipAdapterAddresses.operStatus;
            this._speed = (long)ipAdapterAddresses.receiveLinkSpeed;
            this._ipv6Index = ipAdapterAddresses.ipv6Index;
            this._adapterFlags = ipAdapterAddresses.flags;

            return;
            Type typeInAssem = typeof(System.Net.NetworkInformation.DuplicateAddressDetectionState);
            Assembly assem = typeInAssem.Assembly;
            Type fixedInfType = assem.GetType("System.Net.NetworkInformation.FIXED_INFO");
            var fixedInfInst = Activator.CreateInstance(fixedInfType);
            var fields = fixedInfType.GetFields((BindingFlags) 0xffff);
            Type myFixedInfoType = fixedInfo.GetType();
            FieldInfo[] myFields = myFixedInfoType.GetFields((BindingFlags) 0xffff);
            foreach (FieldInfo fieldInfo in fields)
            {
                var match = myFields.Single(info => info.Name == fieldInfo.Name);
                fieldInfo.SetValue(fixedInfInst,match.GetValue(fixedInfo));
            }

            Type res = assem.GetType("System.Net.NetworkInformation.SystemIPInterfaceProperties");
            var consts = res.GetConstructors((BindingFlags) 0xffff);
            var item = consts[0].Invoke(new object[]{fixedInfo, ipAdapterAddresses});
            //this.interfaceProperties = new SystemIPInterfaceProperties(fixedInfo, ipAdapterAddresses);
        }

        public override string Id
        {
            get
            {
                return this._id;
            }
        }

        public override string Name
        {
            get
            {
                return this._name;
            }
        }

        public override string Description
        {
            get
            {
                return this._description;
            }
        }

        public override PhysicalAddress GetPhysicalAddress()
        {
            byte[] address = new byte[(int)this._addressLength];
            Buffer.BlockCopy((Array)this._physicalAddress, 0, (Array)address, 0, checked((int)this._addressLength));
            return new PhysicalAddress(address);
        }

        public override NetworkInterfaceType NetworkInterfaceType
        {
            get
            {
                return this._type;
            }
        }

        public override IPInterfaceProperties GetIPProperties()
        {
            return (IPInterfaceProperties)null;
        }

        public override IPv4InterfaceStatistics GetIPv4Statistics()
        {
            return (IPv4InterfaceStatistics)null;
        }

        public override IPInterfaceStatistics GetIPStatistics()
        {
            return (IPInterfaceStatistics)null;
        }

        public override bool Supports(NetworkInterfaceComponent networkInterfaceComponent)
        {
            return networkInterfaceComponent == NetworkInterfaceComponent.IPv6 && (this._adapterFlags & Interop.AdapterFlags.IPv6Enabled) != (Interop.AdapterFlags)0 || networkInterfaceComponent == NetworkInterfaceComponent.IPv4 && (this._adapterFlags & Interop.AdapterFlags.IPv4Enabled) != (Interop.AdapterFlags)0;
        }

        public override OperationalStatus OperationalStatus
        {
            get
            {
                return this._operStatus;
            }
        }

        public override long Speed
        {
            get
            {
                return this._speed;
            }
        }

        public override bool IsReceiveOnly
        {
            get
            {
                return (this._adapterFlags & Interop.AdapterFlags.ReceiveOnly) > (Interop.AdapterFlags)0;
            }
        }

        public override bool SupportsMulticast
        {
            get
            {
                return (this._adapterFlags & Interop.AdapterFlags.NoMulticast) == (Interop.AdapterFlags)0;
            }
        }
    }
#endif
}