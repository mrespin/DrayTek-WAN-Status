using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrayTek_WAN_Status {
    public class ApplicationSettings {

        public ApplicationSettings() {
            DisableConsoleOutput = false;
            OutputRawData = true;

            QueryOptions = new QueryOptions {
                Option = QueryOption.Telnet,
                Udp = new UdpOptions
                {
                    ListeningPort = 51400,
                    Ip = "10.11.200.1"
                },
                Telnet = new TelnetOptions
                {
                    Ip = "10.0.200.1",
                    User = "admin",
                    Password = "pass",
                    QueryIntervalSeconds = 30,
                    Port = 23
                }
            };

            StorageProvider = new StorageProvider {
                StorageProviderOption = StorageProviderOption.CSV,
                InfluxDb = new InfluxDbSettings {
                    Version = CustomInfluxDbVersion.Latest,
                    Url = "http://192.168.0.5:8086",
                    User = "username",
                    Password = "password",
                    DatabaseName = "database"
                },
                Csv = new CsvSettings {
                    Delimiter = ";"
                }
            };
        }

        public bool DisableConsoleOutput { get; set; }

        public bool OutputRawData { get; set; }

        public QueryOptions QueryOptions { get; set; }

        public StorageProvider StorageProvider { get; set; }  
    }

    public class CsvSettings {
        public string Delimiter { get; set; }
    }

    public class TelnetOptions {
        public string User { get; set; }
        public string Password { get; set; }
        public int QueryIntervalSeconds { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
}

public class UdpOptions {                          
        public int ListeningPort { get; set; }
        public string Ip { get; set; }
    }

    public class QueryOptions {
        public QueryOption Option { get; set; }
        public UdpOptions Udp { get; set; }
        public TelnetOptions Telnet { get; set; }
    }

    public class StorageProvider {
        public StorageProviderOption StorageProviderOption { get; set; }
        public InfluxDbSettings InfluxDb { get; set; }
        public CsvSettings Csv { get; set; }
    }

    public class DrayTekSettings {
        public int ListeningPort { get; set; }
        public string Ip { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }

    public class InfluxDbSettings {
        public CustomInfluxDbVersion Version { get; set; }
        public string Url { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum QueryOption {
        UDP = 0,
        Telnet = 1
    }
}
