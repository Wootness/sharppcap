using System;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace SharpPcap.Borrowed
{
#if Windows
    internal static class Interop
    {
        [DllImport("iphlpapi.dll")]
        internal static extern uint GetAdaptersAddresses(AddressFamily family, uint flags, IntPtr pReserved, SafeLocalAllocHandle adapterAddresses, ref uint outBufLen);

        [DllImport("iphlpapi.dll")]
        internal static extern uint GetNetworkParams(SafeLocalAllocHandle pFixedInfo, ref uint pOutBufLen);
        public struct FIXED_INFO
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
            public string hostName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 132)]
            public string domainName;
            public UInt32 currentDnsServer;
            public Borrowed.Interop.IP_ADDR_STRING DnsServerList;
            public uint nodeType;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string scopeId;
            public bool enableRouting;
            public bool enableProxy;
            public bool enableDns;
        }

        public struct IP_ADDR_STRING
        {
            public IntPtr Next;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string IpAddress;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            public string IpMask;
            public uint Context;
        }

        [Flags]
        internal enum AdapterFlags
        {
            DnsEnabled = 1,
            RegisterAdapterSuffix = 2,
            DhcpEnabled = 4,
            ReceiveOnly = 8,
            NoMulticast = 16,
            Ipv6OtherStatefulConfig = 32,
            NetBiosOverTcp = 64,
            IPv4Enabled = 128,
            IPv6Enabled = 256,
            IPv6ManagedAddressConfigurationSupported = 512,
        }

        [Flags]
        internal enum AdapterAddressFlags
        {
            DnsEligible = 1,
            Transient = 2,
        }

        [Flags]
        internal enum GetAdaptersAddressesFlags
        {
            SkipUnicast = 1,
            SkipAnycast = 2,
            SkipMulticast = 4,
            SkipDnsServer = 8,
            IncludePrefix = 16,
            SkipFriendlyName = 32,
            IncludeWins = 64,
            IncludeGateways = 128,
            IncludeAllInterfaces = 256,
            IncludeAllCompartments = 512,
            IncludeTunnelBindingOrder = 1024,
        }

        internal struct IpSocketAddress
        {
            internal IntPtr address;
            internal int addressLength;

        }

        internal struct IpAdapterAddress
        {
            internal uint length;
            internal Borrowed.Interop.AdapterAddressFlags flags;
            internal IntPtr next;
            internal Borrowed.Interop.IpSocketAddress address;

        }

        internal struct IpAdapterUnicastAddress
        {
            internal uint length;
            internal Borrowed.Interop.AdapterAddressFlags flags;
            internal IntPtr next;
            internal Borrowed.Interop.IpSocketAddress address;
            internal PrefixOrigin prefixOrigin;
            internal SuffixOrigin suffixOrigin;
            internal DuplicateAddressDetectionState dadState;
            internal uint validLifetime;
            internal uint preferredLifetime;
            internal uint leaseLifetime;
            internal byte prefixLength;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct IpAdapterAddresses
        {
            internal uint length;
            internal uint index;
            internal IntPtr next;
            [MarshalAs(UnmanagedType.LPStr)]
            internal string AdapterName;
            internal IntPtr firstUnicastAddress;
            internal IntPtr firstAnycastAddress;
            internal IntPtr firstMulticastAddress;
            internal IntPtr firstDnsServerAddress;
            internal string dnsSuffix;
            internal string description;
            internal string friendlyName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            internal byte[] address;
            internal uint addressLength;
            internal Borrowed.Interop.AdapterFlags flags;
            internal uint mtu;
            internal NetworkInterfaceType type;
            internal OperationalStatus operStatus;
            internal uint ipv6Index;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            internal uint[] zoneIndices;
            internal IntPtr firstPrefix;
            internal ulong transmitLinkSpeed;
            internal ulong receiveLinkSpeed;
            internal IntPtr firstWinsServerAddress;
            internal IntPtr firstGatewayAddress;
            internal uint ipv4Metric;
            internal uint ipv6Metric;
            internal ulong luid;
            internal Borrowed.Interop.IpSocketAddress dhcpv4Server;
            internal uint compartmentId;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            internal byte[] networkGuid;
            internal Borrowed.Interop.InterfaceConnectionType connectionType;
            internal Borrowed.Interop.InterfaceTunnelType tunnelType;
            internal Borrowed.Interop.IpSocketAddress dhcpv6Server;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 130)]
            internal byte[] dhcpv6ClientDuid;
            internal uint dhcpv6ClientDuidLength;
            internal uint dhcpV6Iaid;
        }

        internal enum InterfaceConnectionType
        {
            Dedicated = 1,
            Passive = 2,
            Demand = 3,
            Maximum = 4,
        }

        internal enum InterfaceTunnelType
        {
            None = 0,
            Other = 1,
            Direct = 2,
            SixToFour = 11,
            Isatap = 13,
            Teredo = 14,
            IpHttps = 15,
        }

        internal struct IpPerAdapterInfo
        {
            internal bool autoconfigEnabled;
            internal bool autoconfigActive;
            internal IntPtr currentDnsServer;
            internal Borrowed.Interop.IpAddrString dnsServerList;
        }

        internal struct IpAddrString
        {
            internal IntPtr Next;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            internal string IpAddress;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
            internal string IpMask;
            internal uint Context;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct MibIfRow2
        {
            internal ulong interfaceLuid;
            internal uint interfaceIndex;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            internal byte[] interfaceGuid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 257)]
            internal char[] alias;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 257)]
            internal char[] description;
            internal uint physicalAddressLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            internal byte[] physicalAddress;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            internal byte[] permanentPhysicalAddress;
            internal uint mtu;
            internal NetworkInterfaceType type;
            internal Borrowed.Interop.InterfaceTunnelType tunnelType;
            internal uint mediaType;
            internal uint physicalMediumType;
            internal uint accessType;
            internal uint directionType;
            internal byte interfaceAndOperStatusFlags;
            internal OperationalStatus operStatus;
            internal uint adminStatus;
            internal uint mediaConnectState;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            internal byte[] networkGuid;
            internal Borrowed.Interop.InterfaceConnectionType connectionType;
            internal ulong transmitLinkSpeed;
            internal ulong receiveLinkSpeed;
            internal ulong inOctets;
            internal ulong inUcastPkts;
            internal ulong inNUcastPkts;
            internal ulong inDiscards;
            internal ulong inErrors;
            internal ulong inUnknownProtos;
            internal ulong inUcastOctets;
            internal ulong inMulticastOctets;
            internal ulong inBroadcastOctets;
            internal ulong outOctets;
            internal ulong outUcastPkts;
            internal ulong outNUcastPkts;
            internal ulong outDiscards;
            internal ulong outErrors;
            internal ulong outUcastOctets;
            internal ulong outMulticastOctets;
            internal ulong outBroadcastOctets;
            internal ulong outQLen;
        }

        internal struct MibUdpStats
        {
            internal uint datagramsReceived;
            internal uint incomingDatagramsDiscarded;
            internal uint incomingDatagramsWithErrors;
            internal uint datagramsSent;
            internal uint udpListeners;
        }

        internal struct MibTcpStats
        {
            internal uint reTransmissionAlgorithm;
            internal uint minimumRetransmissionTimeOut;
            internal uint maximumRetransmissionTimeOut;
            internal uint maximumConnections;
            internal uint activeOpens;
            internal uint passiveOpens;
            internal uint failedConnectionAttempts;
            internal uint resetConnections;
            internal uint currentConnections;
            internal uint segmentsReceived;
            internal uint segmentsSent;
            internal uint segmentsResent;
            internal uint errorsReceived;
            internal uint segmentsSentWithReset;
            internal uint cumulativeConnections;
        }

        internal struct MibIpStats
        {
            internal bool forwardingEnabled;
            internal uint defaultTtl;
            internal uint packetsReceived;
            internal uint receivedPacketsWithHeaderErrors;
            internal uint receivedPacketsWithAddressErrors;
            internal uint packetsForwarded;
            internal uint receivedPacketsWithUnknownProtocols;
            internal uint receivedPacketsDiscarded;
            internal uint receivedPacketsDelivered;
            internal uint packetOutputRequests;
            internal uint outputPacketRoutingDiscards;
            internal uint outputPacketsDiscarded;
            internal uint outputPacketsWithNoRoute;
            internal uint packetReassemblyTimeout;
            internal uint packetsReassemblyRequired;
            internal uint packetsReassembled;
            internal uint packetsReassemblyFailed;
            internal uint packetsFragmented;
            internal uint packetsFragmentFailed;
            internal uint packetsFragmentCreated;
            internal uint interfaces;
            internal uint ipAddresses;
            internal uint routes;
        }

        internal struct MibIcmpInfo
        {
            internal Borrowed.Interop.MibIcmpStats inStats;
            internal Borrowed.Interop.MibIcmpStats outStats;
        }

        internal struct MibIcmpStats
        {
            internal uint messages;
            internal uint errors;
            internal uint destinationUnreachables;
            internal uint timeExceeds;
            internal uint parameterProblems;
            internal uint sourceQuenches;
            internal uint redirects;
            internal uint echoRequests;
            internal uint echoReplies;
            internal uint timestampRequests;
            internal uint timestampReplies;
            internal uint addressMaskRequests;
            internal uint addressMaskReplies;
        }

        internal struct MibIcmpInfoEx
        {
            internal Borrowed.Interop.MibIcmpStatsEx inStats;
            internal Borrowed.Interop.MibIcmpStatsEx outStats;
        }

        internal struct MibIcmpStatsEx
        {
            internal uint dwMsgs;
            internal uint dwErrors;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            internal uint[] rgdwTypeCount;
        }

        internal struct MibTcpTable
        {
            internal uint numberOfEntries;
        }

        internal struct MibTcpRow
        {
            internal TcpState state;
            internal uint localAddr;
            internal byte localPort1;
            internal byte localPort2;
            internal byte ignoreLocalPort3;
            internal byte ignoreLocalPort4;
            internal uint remoteAddr;
            internal byte remotePort1;
            internal byte remotePort2;
            internal byte ignoreRemotePort3;
            internal byte ignoreRemotePort4;
        }

        internal struct MibTcp6TableOwnerPid
        {
            internal uint numberOfEntries;
        }

        internal struct MibTcp6RowOwnerPid
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            internal byte[] localAddr;
            internal uint localScopeId;
            internal byte localPort1;
            internal byte localPort2;
            internal byte ignoreLocalPort3;
            internal byte ignoreLocalPort4;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            internal byte[] remoteAddr;
            internal uint remoteScopeId;
            internal byte remotePort1;
            internal byte remotePort2;
            internal byte ignoreRemotePort3;
            internal byte ignoreRemotePort4;
            internal TcpState state;
            internal uint owningPid;
        }

        internal enum TcpTableClass
        {
            TcpTableBasicListener,
            TcpTableBasicConnections,
            TcpTableBasicAll,
            TcpTableOwnerPidListener,
            TcpTableOwnerPidConnections,
            TcpTableOwnerPidAll,
            TcpTableOwnerModuleListener,
            TcpTableOwnerModuleConnections,
            TcpTableOwnerModuleAll,
        }

        internal struct MibUdpTable
        {
            internal uint numberOfEntries;
        }

        internal struct MibUdpRow
        {
            internal uint localAddr;
            internal byte localPort1;
            internal byte localPort2;
            internal byte ignoreLocalPort3;
            internal byte ignoreLocalPort4;
        }

        internal enum UdpTableClass
        {
            UdpTableBasic,
            UdpTableOwnerPid,
            UdpTableOwnerModule,
        }

        internal struct MibUdp6TableOwnerPid
        {
            internal uint numberOfEntries;
        }

        internal struct MibUdp6RowOwnerPid
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            internal byte[] localAddr;
            internal uint localScopeId;
            internal byte localPort1;
            internal byte localPort2;
            internal byte ignoreLocalPort3;
            internal byte ignoreLocalPort4;
            internal uint owningPid;
        }

        internal delegate void StableUnicastIpAddressTableDelegate(IntPtr context, IntPtr table);
    }

    internal static class Winsock
    {
        internal struct Linger
        {
            internal ushort OnOff;
            internal ushort Time;
        }

        [Flags]
        internal enum AsyncEventBits
        {
            FdNone = 0,
            FdRead = 1,
            FdWrite = 2,
            FdOob = 4,
            FdAccept = 8,
            FdConnect = 16,
            FdClose = 32,
            FdQos = 64,
            FdGroupQos = 128,
            FdRoutingInterfaceChange = 256,
            FdAddressListChange = 512,
            FdAllEvents = FdAddressListChange | FdRoutingInterfaceChange | FdGroupQos | FdQos | FdClose | FdConnect | FdAccept | FdOob | FdWrite | FdRead,
        }

        [Flags]
        internal enum SocketConstructorFlags
        {
            WSA_FLAG_OVERLAPPED = 1,
            WSA_FLAG_MULTIPOINT_C_ROOT = 2,
            WSA_FLAG_MULTIPOINT_C_LEAF = 4,
            WSA_FLAG_MULTIPOINT_D_ROOT = 8,
            WSA_FLAG_MULTIPOINT_D_LEAF = 16,
        }
    }
#endif
}
